using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SkeletonBoss : Character
{
    public static SkeletonBoss s_instance;

    [SerializeField] private int health;
    [SerializeField] private int damage;
    [SerializeField] private float speed;
    [SerializeField] private float accelerationKoeficient;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float groundDistance;

    [SerializeField] private ParticleSystem bonePieces;
    [SerializeField] private Rigidbody[] bones;
    [SerializeField] private ProgressBarCircle healthCircle;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform weaponSocket;

    [SerializeField] private List<Character> skeletons;

    private int maxHealth;
    private bool bIsAlive;
    private bool bIsImmortal;
    private bool bIsMotionBanned;
    private bool bIsGrounded;
    private bool bIsAttack;
    private bool bIsSlowed;

    private CharacterController controller;
    private Transform target;
    private FieldOfView fow;
    private DamageZone damageZone;
    private Weapon currentWeapon;
    private Vector3 movementDirection;
    private Vector3 velocity;

    public float horizontalVelocity;

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
            if(health > maxHealth)
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
            if(value < 0)
            {
                speed = 0;
            }
            else
            {
                speed = value;
            }
        }
    }

    public Transform Target => target;
    public override Weapon CurrentWeapon => currentWeapon;
    public DamageZone DamageZone => damageZone;
    public List<Character> SummonedSkeletons 
    {
        get => skeletons;
        set
        {
            skeletons = value;
        }
    }
    public override bool IsAlive => bIsAlive;
    public override bool IsImmortal
    {
        get => bIsImmortal;
        set => bIsImmortal = value;
    }
    public override int MaxHealth 
    {
        get => maxHealth;
        set
        {
            maxHealth = value;
            if(maxHealth <= 0)
            {
                maxHealth = 1;
            }
            if(maxHealth < health)
            {
                health = maxHealth;
            }
        }
    }
    public override bool IsUnderEffect => bIsSlowed;
    public override float RotationSpeed { get => rotationSpeed; set => rotationSpeed = value; }

    private void Awake()
    {
        if(s_instance == null)
        {
            s_instance = this;
        }
        else
        {
            Destroy(s_instance.gameObject);
        }

        controller = GetComponent<CharacterController>();
        currentWeapon = weaponSocket.GetComponentInChildren<Weapon>();
        damageZone = GetComponentInChildren<DamageZone>();
        fow = GetComponent<FieldOfView>();
        maxHealth = health;
        skeletons = new List<Character>();
    }

    private void Start()
    {
        if (bones.Length == 0)
        {
            LevelGenerator.s_levelGenerator.DecrementEnemyCount(gameObject);
            Destroy(gameObject);
        }
        healthCircle.maxHealth = Health;
        healthCircle.BarValue = Health;
        healthCircle.Alert = Health / 3;

        healthCircle.gameObject.SetActive(false);
        ((MeleeWeapon)currentWeapon).DamageZone = damageZone;
        PickupSword(currentWeapon);
    }

    private void Update()
    {
        ApplyGravity();

        if (bIsMotionBanned == false)
        {
            if (fow.visibleTargets.Count > 0)
            {
                target = GetClosestTarget();
            }
            if (target != null)
            {
                Move(Speed);
                Vector3 dir = (target.position - transform.position).normalized;
                RotateToDirection(new Vector3(dir.x, 0, dir.z));
            }
        }
        else
        {
            if (bIsAttack == true)
            {
                onAttack.Invoke();
            }
        }
    }

    public void PickupSword(Weapon sword)
    {
        sword.transform.parent = weaponSocket;
        sword.transform.position = weaponSocket.position;
        sword.transform.rotation = weaponSocket.rotation;
        currentWeapon = sword;
    }

    public override void ShowHealthBar(bool isShow)
    {
        if (this != null)
            healthCircle.gameObject.SetActive(isShow);
    }

    private Transform GetClosestTarget()
    {
        float minDistance = 100f;
        Transform target = fow.visibleTargets.First<Transform>();

        foreach (Transform t in fow.visibleTargets)
        {
            float currentDistance = Vector3.Distance(transform.position, t.position);
            if (currentDistance < minDistance)
            {
                minDistance = currentDistance;
                target = t;
            }
        }
        return target;
    }

    protected override void Move(float speed)
    {
        movementDirection = target.position - transform.position;
        float disTarget = Vector3.Distance(transform.position, target.position);

        if (disTarget > 3f || fow.visibleTargets.Count == 0)
        {
            bIsAttack = false;
            movementDirection = movementDirection.normalized * speed;
            controller.Move(movementDirection * Time.deltaTime);
            horizontalVelocity = 1;
        }
        else if(disTarget <= 3f && fow.visibleTargets.Count > 0)
        {
            bIsAttack = true;
            ChangeMotionStatus(true);
            horizontalVelocity = 0;
        }
    }

    private void ApplyGravity()
    {
        bIsGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (bIsGrounded == true && velocity.y < 0)
        {
            velocity.y = -2f;
            controller.Move(velocity * Time.deltaTime);
            return;
        }

        velocity.y += -0.00001f * Time.deltaTime;
        controller.Move(velocity);
    }
    public void ChangeMotionStatus(bool status)
    {
        bIsMotionBanned = status;
    }

    public override void Heal(int healthInput)
    {
        Health += health;
        if (Health >= maxHealth)
        {
            Health = maxHealth;
            healthCircle.BarValue = Health;
            return;
        }
        healthCircle.BarValue = Health;
    }

    private void RotateToDirection(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    public override void EarnDamage(int damage, GameObject sender)
    {
        if (bIsImmortal == true) return;
        bonePieces.Emit(50);

        Health -= damage;
        healthCircle.BarValue = Health;

        if (Health <= 0)
        {
            Die();
            return;
        }

        onEarnDamage.Invoke();

        Debug.LogWarning($"{this.name} took {damage} dmg.");
    }

    protected override void Die()
    {
        LevelGenerator.s_levelGenerator.DecrementEnemyCount(gameObject);

        Debug.LogError($"{this.name} is dead.");
        foreach (Rigidbody bone in bones)
        {
            bone.transform.parent = null;
            bone.gameObject.SetActive(true);
        }

        Destroy(this.gameObject);
    }

    public override void PutEffect()
    {
        bIsSlowed = true;
    }
    public override void RemoveEffect()
    {
        bIsSlowed = false;
    }

    private void OnDestroy()
    {
        SceneManager.LoadScene(0);
    }
}
