using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Necromancer : Character, ICanTrade
{
    [SerializeField] private int health;
    [SerializeField] private int damage;
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float groundDistance;
    [SerializeField] private int gold;
    [SerializeField] private new Camera camera;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private ProgressBarCircle healthCircle;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask characterMask;

    private bool bYButtonHeld;
    private bool bXButtonHeld;

    private int maxHealth;

    private bool bIsMotionBanned;
    private bool bIsAlive;
    private bool bIsImmortal;
    private bool bIsGrounded;
    private bool bIsSlowed;

    private Vector3 movementDirection;
    private Vector3 velocity;
    private Transform target;
    private Weapon currentWeapon;
    private CharacterController characterController;
    private Joystick joystick;

    private Button xButton;
    private Button yButton;
    public UnityEvent onAttackAlt;
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
            // TODO: damage control
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

    public Camera Camera
    {
        get => camera;
        set => camera = value;
    }

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        currentWeapon = GetComponentInChildren<Weapon>();

        bIsAlive = true;
        maxHealth = health;
    }

    private void Start()
    {
        onDie.AddListener(Die);
        healthCircle.maxHealth = Health;
        healthCircle.BarValue = Health;
        healthCircle.Alert = Health / 3;

        joystick = FindObjectOfType<Joystick>();

        xButton = GameObject.FindGameObjectWithTag("xButton").GetComponent<Button>();
        yButton = GameObject.FindGameObjectWithTag("yButton").GetComponent<Button>();

        xButton.onClick.AddListener(OnXPress);
        yButton.onClick.AddListener(OnYPress);
        onTrade(Gold);
    }

    private void Update()
    {
        ApplyGravity();

        if (bIsMotionBanned == false)
        {
            Move(Speed);
        }

        if (target == null) RotateToDirection(movementDirection);
        else
        {
            movementDirection = (target.position - transform.position).normalized;
            RotateToDirection(new Vector3(movementDirection.x, 0, movementDirection.z));
        }

#if UNITY_ANDROID

        if (bXButtonHeld == true)
        {
            onAttack.Invoke();
            //currentWeapon.StartAttack();
            ChangeMotionStatus(true);
            bXButtonHeld = false;
        }
        else if (bYButtonHeld == true)
        {
            NecromancerWand wand = currentWeapon as NecromancerWand;
            if (wand != null)
            {
                onAttackAlt.Invoke();
                //wand.StartAttackAlt();
                ChangeMotionStatus(true);

            }
            bYButtonHeld = false;
        }

        for(int i = 0; i < Input.touchCount; i++)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                FindTarget(i);
            }
        }
#else
        if (Input.GetKeyDown(KeyCode.Mouse0) == true)
        {
            onAttack.Invoke();
            //currentWeapon.StartAttack();
            //ChangeMotionStatus(true);
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1) == true)
        {
            NecromancerWand wand = currentWeapon as NecromancerWand;
            if (wand != null)
            {
                onAttackAlt.Invoke();
                //wand.StartAttackAlt();
                //ChangeMotionStatus(true);

            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse2) == true)
        {
            FindTarget(0);
        }
#endif
    }
    public override void EarnDamage(int damage, GameObject sender)
    {
        if (!IsAlive) return;
        if (bIsImmortal && damage < 1000) return;

        //ChangeMotionStatus(true);

        Health -= damage;
        healthCircle.BarValue = Health;

        if (Health <= 0)
        {
            onDie.Invoke();
            Destroy(gameObject);
            return;
        }

        onEarnDamage.Invoke();
        Debug.LogWarning($"{this.name} took {damage} dmg.");
    }

    public override void Heal(int healthInput)
    {
        if (bIsAlive == false) return;
        Health += healthInput;
        if (Health >= maxHealth)
        {
            Health = maxHealth;
        }
        healthCircle.BarValue = Health;
    }

    public override void ShowHealthBar(bool isShow)
    {
        if (this != null)
            healthCircle.gameObject.SetActive(isShow);
    }

    protected override void Die()
    {
        bIsAlive = false;
        Debug.Log($"{this.name} is dead.");

        // If single player
        ScenesBehavior.s_instance.RestartGame();
    }

    protected override void Move(float speed)
    {
#if UNITY_ANDROID
        float _x = joystick.Horizontal;
        float _y = joystick.Vertical;
#else
        float _x = Input.GetAxisRaw("Horizontal");
        float _y = Input.GetAxisRaw("Vertical");
#endif

        movementDirection = (new Vector3(-_x, 0, -_y)).normalized;
        characterController.Move(movementDirection * speed * Time.deltaTime);
    }

    public override void PutEffect()
    {
        bIsSlowed = true;
    }
    public override void RemoveEffect()
    {
        bIsSlowed = false;
    }

    public void OnXPress()
    {
        bXButtonHeld = !bXButtonHeld;
    }

    public void OnYPress()
    {
        bYButtonHeld = !bYButtonHeld;
    }

    public void FindTarget(int i)
    {
        RaycastHit hit;
#if UNITY_ANDROID
        Ray ray = camera.ScreenPointToRay(Input.GetTouch(i).position);
#else
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
#endif
        if (Physics.Raycast(ray, out hit, 1000, characterMask) == true && hit.transform.name != transform.name)
        {
            if (hit.transform.tag != "Character") return;

            if (target == null)
            {
                target = hit.transform;
                target.GetComponent<Character>().ShowHealthBar(true);
            }
            else
            {
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

    public void ChangeMotionStatus(bool status)
    {
        bIsMotionBanned = status;
    }

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
    private void ApplyGravity()
    {
        bIsGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        velocity.y += -5f * Time.deltaTime;
        characterController.Move(velocity);
    }

    private void RotateToDirection(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
