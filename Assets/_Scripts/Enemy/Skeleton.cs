using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Skeleton : Character
{
    [SerializeField] private int health;
    [SerializeField] private int damage;
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float groundDistance;
    [SerializeField] private int minGold;
    [SerializeField] private int maxGold;
    [SerializeField] private float goldChance;
    [SerializeField] private GoldBehavior[] gold;

    [SerializeField] private ParticleSystem bonePieces;
    [SerializeField] private Rigidbody[] bones;
    [SerializeField] private ProgressBarCircle healthCircle;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform weaponSocket;

    private int maxHealth;
    private float baseSpeed;

    private bool bIsAlive;
    private bool bIsMotionBanned;
    private bool bIsGrounded;
    private bool bIsAttack;
    private bool bIsImmortal;
    private bool bIsSlowed;

    private NavMeshAgent agent;
    private Transform target;
    private FieldOfView fow;
    private Weapon currentWeapon;
    private DamageZone damageZone;
    private Vector3 velocity;

    [HideInInspector] public float horizontalVelocity;

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
    public DamageZone DamageZone => damageZone;

    public override Weapon CurrentWeapon => currentWeapon;

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

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        currentWeapon = weaponSocket.GetComponentInChildren<Weapon>();
        damageZone = GetComponentInChildren<DamageZone>();
        fow = GetComponent<FieldOfView>();

        baseSpeed = Speed;
        maxHealth = health;
        bIsAlive = true;
    }

    private void Start()
    {
        // If skeleton does not have bones, or has no health - kill him
        if (bones.Length == 0 || health <= 1)
        {
            LevelGenerator.s_levelGenerator.DecrementEnemyCount(gameObject);
            Destroy(gameObject);
        }

        healthCircle.maxHealth = Health;
        healthCircle.BarValue = Health;
        healthCircle.Alert = Health / 3;

        healthCircle.gameObject.SetActive(false);
        // Set weapon
        ((MeleeWeapon)currentWeapon).DamageZone = damageZone;
        PickupSword(currentWeapon);
    }

    private void Update()
    {
        // Support gravity during skeleton's lifetime
        ApplyGravity();

        if (bIsAttack == true)
        {
            onAttack.Invoke();
        }

        if (bIsMotionBanned == false)
        {
            if (fow.visibleTargets.Count > 0)
            {
                target = GetClosestTarget();
            }
            if (target != null)
            {
                Move(Speed);
                // Rotate enemy in direction of target
                Vector3 dir = (target.position - transform.position).normalized;
                RotateToDirection(new Vector3(dir.x, 0, dir.z));
            }
        }
    }

    // Functionality to set weapon
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

    // Find closest from visible targets
    private Transform GetClosestTarget()
    {
        float minDistance = 100f;
        Transform target = fow.visibleTargets.First<Transform>();

        foreach (Transform t in fow.visibleTargets)
        {
            if (t == null)
            {
                return null;
            }

            // Distance to target
            float currentDistance = Vector3.Distance(transform.position, t.position);
            if (currentDistance < minDistance)
            {
                minDistance = currentDistance;
                target = t;
            }
        }
        return target;
    }

    public override void EarnDamage(int damage, GameObject sender)
    {   
        // Does not earns damage if skeleton is immortal
        if (bIsImmortal == true) return;
        // Spawn bone pieces on earning damage
        bonePieces.Emit(50);

        Health -= damage;
        healthCircle.BarValue = Health;

        if (Health <= 0)
        {
            // Gold spawn chance on death
            if (Random.value > goldChance && gold.Length > 0)
            {
                GoldBehavior piece = Instantiate<GoldBehavior>(gold[Random.Range(0, gold.Length)], transform.position, Quaternion.identity);
                piece.GoldAmount = Random.Range(minGold, maxGold + 1);
            }

            Die();
            return;
        }

        onEarnDamage.Invoke();
        ChangeMotionStatus(true);

        Debug.LogWarning($"{this.name} took {damage} dmg.");
    }

    protected override void Die()
    {
        bIsAlive = false;
        LevelGenerator.s_levelGenerator.DecrementEnemyCount(gameObject);

        Debug.Log($"{this.name} is dead.");

        foreach (Rigidbody bone in bones)
        {
            bone.transform.parent = null;
            bone.gameObject.SetActive(true);
        }

        // If currently is boss fight, remove killed skeleton from boss summoned collection
        if (SkeletonBoss.s_instance != null)
        {
            SkeletonBoss.s_instance.SummonedSkeletons.Remove(this);
        }

        Destroy(this.gameObject);
    }

    protected override void Move(float speed)
    {
        // Getting distance to target
        float disTarget = Vector3.Distance(transform.position, target.position);

        // State for stopping
        if (disTarget <= agent.stoppingDistance)
        {
            // Stop skeleton
            bIsAttack = true;
            ChangeMotionStatus(true);
            horizontalVelocity = 0;
            agent.isStopped = true;
        }
        else
        {
            // Continue movement
            agent.isStopped = false;
            bIsAttack = false;
            horizontalVelocity = 1;
            agent.speed = speed;
            agent.SetDestination(target.position);
        }
    }

    private void RotateToDirection(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            // Rotate to direction to target around Y axis
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void ApplyGravity()
    {
        // Cast sphere underneath skeleton
        bIsGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (bIsGrounded == true && velocity.y < 0)
        {
            velocity.y = -2f;
            agent.Move(velocity * Time.deltaTime);
            return;
        }

        velocity.y += -0.00001f * Time.deltaTime;
        agent.Move(velocity);
    }

    public void ChangeMotionStatus(bool status)
    {
        bIsMotionBanned = status;
        agent.isStopped = status;
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

    public override void PutEffect()
    {
        bIsSlowed = true;
    }
    public override void RemoveEffect()
    {
        bIsSlowed = false;
    }
}
