using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Knight : Character, ICanTrade
{
    // SerializeField - attribute shows encapsulated field in inspector window
    [SerializeField] private int health;
    [SerializeField] private int damage;
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float groundDistance;
    [SerializeField] private int gold;

    [SerializeField] private new Camera camera;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private ProgressBarCircle healthCircle;
    [SerializeField] private bool bIsMotionBanned;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask characterMask;
    [SerializeField] private Transform weaponSocket;

    private bool bBButtonHeld;
    private bool bYButtonHeld;
    private bool bXButtonHeld;

    private int maxHealth;

    private bool bIsGrounded;
    private bool bIsBlocking;
    private bool bIsAlive;
    private bool bIsImmortal;
    private bool bIsSlowed;
    private bool bIsAttacking;

    private Vector3 movementDirection;
    private Vector3 velocity;
    private DamageZone damageZone;
    private Transform target;
    private Weapon currentWeapon;
    private CharacterController characterController;
    private Joystick joystick;

    private Button xButton;
    private Button yButton;
    private Button bButton;
    public UnityEvent onShieldBlock;
    public UnityEvent onRemoveShieldBlock;
    public event UnityAction<int> onTrade;
    public CharacterController CharacterController
    {
        get => characterController;
    }

    public override int Health
    {
        get => health;
        set
        {
            health = value;

            if (health < 0)
            {
                health = 0;
            }
            if (health > maxHealth)
            {
                health = maxHealth;
            }
        }
    }
    public override int Damage
    {
        get => damage;
        set
        {
            damage = value;
        }
    }
    public override float Speed
    {
        get => speed;
        set
        {
            if (value < 0)
            {
                speed = 0;
            }
            else
            {
                speed = value;
            }
        }
    }

    public override bool IsAlive => bIsAlive;
    public override bool IsImmortal
    {
        get => bIsImmortal;
        set => bIsImmortal = value;
    }
    public override Weapon CurrentWeapon => currentWeapon;
    // Reference to zone, which activates on attck
    public DamageZone DamageZone => damageZone;

    public override int MaxHealth
    {
        get => maxHealth;
        set
        {
            maxHealth = value;
            if (maxHealth <= 0)
            {
                maxHealth = 1;
            }
            if (maxHealth < health)
            {
                health = maxHealth;
            }
        }
    }
    public override bool IsUnderEffect => bIsSlowed;
    public bool IsAttacking { get => bIsAttacking; set => bIsAttacking = value; }
    public override float RotationSpeed { get => rotationSpeed; set => rotationSpeed = value; }
    public int Gold
    {
        get => gold;
        set
        {
            gold = value;
            if (gold < 0)
            {
                gold = 0;
            }
        }
    }
    // Reference to camera, which follows character
    public Camera Camera
    {
        get => camera;
        set => camera = value;
    }

    // Initialization on game awake
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        currentWeapon = weaponSocket.GetComponentInChildren<Weapon>();
        damageZone = GetComponentInChildren<DamageZone>();
        bIsAlive = true;
        maxHealth = health;
    }

    // Initialization on game start, invokes after awake
    private void Start()
    {
        onDie.AddListener(Die);
        healthCircle.maxHealth = Health;
        healthCircle.BarValue = Health;
        healthCircle.Alert = Health / 3;
        
#if UNITY_ANDROID
        joystick = FindObjectOfType<Joystick>();
        xButton = GameObject.FindGameObjectWithTag("xButton").GetComponent<Button>();
        yButton = GameObject.FindGameObjectWithTag("yButton").GetComponent<Button>();
        bButton = GameObject.FindGameObjectWithTag("bButton").GetComponent<Button>();

        xButton.onClick.AddListener(OnXPress);
        yButton.onClick.AddListener(OnYPress);
        bButton.onClick.AddListener(OnBPress);
#endif
        
        ((MeleeWeapon)currentWeapon).DamageZone = damageZone;

        PickupSword(currentWeapon);
        onTrade(Gold);
    }

    // Calls every frame
    private void Update()
    {
        ApplyGravity();

        if (bIsMotionBanned == false)
        {
            Move(Speed);
        }

        // If locked on target - rotate to it's position in world space, else rotate to look direction 
        if (target == null) RotateToDirection(movementDirection);
        else
        {
            // getting direction to locked target
            // subtracting player's position from target's position and getting normal vector of it
            movementDirection = (target.position - transform.position).normalized;
            // Ignoring Y coordinate, to rotate always around Y axis 
            RotateToDirection(new Vector3(movementDirection.x, 0, movementDirection.z));
        }

        // If android platform
#if UNITY_ANDROID
        if(!bIsBlocking && !bIsAttacking)
        {
            if(bXButtonHeld == true && bYButtonHeld == false)
            {
                onAttack.Invoke();
                ChangeMotionStatus(true);
                bXButtonHeld = false;
            }
        }

        if(bYButtonHeld && !bIsAttacking)
        {
            onShieldBlock.Invoke();
            bIsBlocking = true;
        }
        else
        {
            onRemoveShieldBlock.Invoke();
            bIsBlocking = false;
        }

        for (int i = 0; i < Input.touchCount; i++)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                FindTarget(i); 
            }
        }
#else

        // While blocking or while attack is performing, attacking is impossible
        if (!bIsBlocking && !bIsAttacking)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                onAttack.Invoke();
                ChangeMotionStatus(true);
            }
        }

        // Block enabling
        if (Input.GetKey(KeyCode.Mouse1) && !bIsAttacking)
        {
            onShieldBlock.Invoke();
            bIsBlocking = true;
        }
        else
        {
            onRemoveShieldBlock.Invoke();
            bIsBlocking = false;
        }

        // Lock on target 
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            // If not mobile platform - ignoring touch id
            FindTarget(0);
        }
#endif

    }

    // Response call on HUD button press
    public void OnXPress()
    {
        bXButtonHeld = !bXButtonHeld;
    }

    public void OnYPress()
    {
        bYButtonHeld = !bYButtonHeld;
    }

    public void OnBPress()
    {
        bBButtonHeld = !bBButtonHeld;
    }

    // Showing health bar
    public override void ShowHealthBar(bool isShow)
    {
        if (this != null)
            healthCircle.gameObject.SetActive(isShow);
    }

    // Locking on target
    // i - touch id, uses if mobile platofrm
    public void FindTarget(int i)
    {
        // result of raycast hit
        RaycastHit hit;
#if UNITY_ANDROID
        // Getting exact touch on screen via touch id
        Ray ray = camera.ScreenPointToRay(Input.GetTouch(i).position);
#else
        // Sending ray from mouse position on screen
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
#endif
        // if ray hit character, and it isn't player itself - lock on target
        if (Physics.Raycast(ray, out hit, 1000, characterMask) == true && hit.transform.name != transform.name)
        {
            if (hit.transform.tag != "Character") return;

            // if there is no target
            if (target == null)
            {
                // getting target from ray hit result
                target = hit.transform;
                target.GetComponent<Character>().ShowHealthBar(true);
            }
            else
            {
                // if locked on same target - stop focusing on it and hide healthbar
                if (target == hit.transform)
                {
                    target.GetComponent<Character>().ShowHealthBar(false);
                    target = null;
                    return;
                }

                target.GetComponent<Character>().ShowHealthBar(false);
                target = hit.transform;
                target.GetComponent<Character>().ShowHealthBar(true);
            }
        }
    }

    // Picking up weapon
    public void PickupSword(Weapon sword)
    {
        // initialization of starting weapon
        if (currentWeapon != null && currentWeapon.transform.name != "DefaultSword")
        {
            currentWeapon.transform.position = transform.position;
            currentWeapon.transform.parent = null;
            currentWeapon.GetComponent<BoxCollider>().enabled = true;
            currentWeapon.GetComponent<MeleeWeapon>().bIsUnInterractable = true;
        }

        // getting new weapon and dropping previous one
        sword.transform.parent = weaponSocket;
        sword.transform.position = weaponSocket.position;
        sword.transform.rotation = weaponSocket.rotation;
        currentWeapon = sword;
    }

    protected override void Die()
    {
        bIsAlive = false;
        Debug.LogError($"{this.name} is dead.");

        // If single player just restart game from start menu
        ScenesBehavior.s_instance.RestartGame();
    }

    // Handling of detecting if character in from of player
    private bool IsNotInFront(GameObject sender)
    {
        // Direction to target
        Vector3 toTarget = (sender.transform.position - transform.position).normalized;

        // The dot product is a float value equal to the magnitudes of the two vectors multiplied together and then multiplied by the cosine of the angle between them
        if (Vector3.Dot(toTarget, transform.forward) > 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public override void EarnDamage(int damage, GameObject sender)
    {
        // Ingoring getting damage if constraints are not valid
        if (!bIsAlive) return;
        if (bIsImmortal && damage < 1000) return;
        if (bIsBlocking && !IsNotInFront(sender)) return;

        Health -= damage;
        healthCircle.BarValue = Health;

        // Death
        if (Health <= 0)
        {
            onDie.Invoke();
            Destroy(gameObject);
            return;
        }

        onEarnDamage.Invoke();
        Debug.LogWarning($"{this.name} took {damage} dmg.");
    }

    public void Test()
    {
        Debug.Log("Test");
    }

    // Getting health 
    public override void Heal(int healthInput)
    {
        // Ignoring healing if dead already
        if (bIsAlive == false) return;
        Health += healthInput;

        if (Health >= maxHealth)
        {
            Health = maxHealth;
        }
        healthCircle.BarValue = Health;
    }

    protected override void Move(float speed)
    {
        // Getting Input, converting direction axises to float values
#if UNITY_ANDROID
        float _x = joystick.Horizontal;
        float _y = joystick.Vertical;
#else
        float _x = Input.GetAxisRaw("Horizontal");
        float _y = Input.GetAxisRaw("Vertical");
#endif
        movementDirection = (new Vector3(-_x, 0, -_y)).normalized;
        // Casting speed to same for every frame rate, multiplying by Time.deltaTime
        // deltaTime - time interval between previous and last frame
        characterController.Move(movementDirection * speed * Time.deltaTime);
    }

    private void RotateToDirection(Vector3 direction)
    {
        // If look direction does not have coordinates 0,0,0
        if (direction != Vector3.zero)
        {
            // Quaternions used to represent direction, based on euler angles
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    // Gravity apply
    private void ApplyGravity()
    {
        bIsGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        velocity.y += -5f * Time.deltaTime;
        characterController.Move(velocity);
    }

    /// <summary>
    /// Is Motion banned = bool status
    /// </summary>
    /// <param name="status">ban status</param>
    public void ChangeMotionStatus(bool status)
    {
        bIsMotionBanned = status;
    }

    // Ability to trade - ICanTrade
    public bool CanTrade(int price)
    {
        if (price <= gold)
        {
            gold -= price;
            if (onTrade != null)
            {
                OnTrade(gold);
            }

            return true;
        }

        return false;
    }

    public void OnTrade(int gold)
    {
        if (onTrade != null)
        {
            onTrade(gold);
        }
    }

    // Public service for effect puting
    public override void PutEffect()
    {
        bIsSlowed = true;
    }

    public override void RemoveEffect()
    {
        bIsSlowed = false;
    }
}
