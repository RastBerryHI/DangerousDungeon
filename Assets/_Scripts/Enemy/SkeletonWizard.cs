using System.Linq;
using CharacterComponents;
using UnityEngine;
using UnityEngine.AI;

public class SkeletonWizard : Character
{
    [SerializeField] private int health;
    [SerializeField] private int damage;

    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float groundDistance;

    [SerializeField] private int minGold;
    [SerializeField] private int maxGold;
    [SerializeField] private int goldChance;

    [SerializeField] private GoldBehavior[] gold;
    [SerializeField] private ParticleSystem bonePieces;
    [SerializeField] private Rigidbody[] bones;
    [SerializeField] private ProgressBarCircle healthCircle;
    [SerializeField] private Transform groundCheck;

    [SerializeField] private LayerMask groundMask;

    [Space]
    [SerializeField] private Curse curse;

    private int maxHealth;
    private bool bIsAlive;
    private bool bIsMotionBanned;
    private bool bIsGrounded;
    private bool bIsAttack;
    private bool bIsImmortal;
    private bool bIsSlowed;

    private NavMeshAgent agent;
    private Transform target;
    private FieldOfView fow;
    private BoxCollider damageZone;

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
    public override bool IsUnderEffect => bIsSlowed;

    public override float RotationSpeed { get => rotationSpeed; set => rotationSpeed = value; }

    private void Awake()
    {
        if (bones.Length == 0)
        {
            LevelGenerator.s_levelGenerator.DecrementEnemyCount(gameObject);
            Destroy(gameObject);
        }

        agent = GetComponent<NavMeshAgent>();
        fow = GetComponent<FieldOfView>();
        damageZone = GetComponentInChildren<BoxCollider>();
        bIsAlive = true;
    }

    private void Start()
    {
        healthCircle.maxHealth = Health;
        healthCircle.BarValue = Health;
        healthCircle.Alert = Health / 3;
        healthCircle.gameObject.SetActive(false);
        maxHealth = health;
        
        // TODO: Refactor curse binding
        //GetComponentInChildren<DamageZone>().OnOverlap += OnHit;
    }

    private void Update()
    {
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
                Vector3 dir = (target.position - transform.position).normalized;
                RotateToDirection(new Vector3(dir.x, 0, dir.z));
            }
        }
    }

    private void OnHit(Damageble damageble)
    {
        Curse[] curses = damageble.GetComponentsInChildren<Curse>();
        if(curses != null)
        {
            foreach(Curse c in curses)
            {
                if(c is VitalityCurse)
                {
                    return;
                }
            }

            Curse _curse = Instantiate<Curse>(curse, damageble.GetComponent<CharacterCached>().Transform.position, Quaternion.identity);
            _curse.transform.parent = damageble.transform;

            System.Array.Clear(curses, 0, curses.Length);
            curses = null;
        }
    } 
    
    private void RotateToDirection(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    public void StartAttack() => damageZone.enabled = true;

    public void EndAttack() => damageZone.enabled = false;

    private void ApplyGravity()
    {
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

    public override void EarnDamage(int damage, GameObject sender)
    {
        if (bIsImmortal == true) return;

        bonePieces.Emit(50);

        Health -= damage;
        healthCircle.BarValue = Health;

        if (Health <= 0)
        {
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

    public override void ShowHealthBar(bool status)
    {
        healthCircle.gameObject.SetActive(status);
    }

    protected override void Die()
    {
        bIsAlive = false;
        LevelGenerator.s_levelGenerator.DecrementEnemyCount(gameObject);

        Debug.LogError($"{this.name} is dead.");
        foreach (Rigidbody bone in bones)
        {
            bone.transform.parent = null;
            bone.gameObject.SetActive(true);
        }

        if (SkeletonBoss.s_instance != null)
        {
            SkeletonBoss.s_instance.SummonedSkeletons.Remove(this);
        }

        Destroy(this.gameObject);
    }

    protected override void Move(float speed)
    {
        float disTarget = Vector3.Distance(transform.position, target.position);

        if (disTarget <= agent.stoppingDistance)
        {
            bIsAttack = true;
            ChangeMotionStatus(true);
            horizontalVelocity = 0;
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
            bIsAttack = false;
            horizontalVelocity = 1;
            agent.speed = speed;
            agent.SetDestination(target.position);
        }
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

    public override void PutEffect()
    {
        bIsSlowed = true;
    }

    public override void RemoveEffect()
    {
        bIsSlowed = false;
    }

}
