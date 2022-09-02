using System.Linq;
using UnityEngine.AI;
using UnityEngine;

public class Lumberjack : Character
{
    [SerializeField] private int health;
    [SerializeField] private int damage;
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float lifetime;
    private int maxHealth;
    private bool bIsAlive;
    private bool bIsImmortal;
    private bool bIsSlowed;
    private bool bIsAttack;
    private bool bIsMotionBanned;

    private ParticleSystem blood;
    private NavMeshAgent agent;
    private BoxCollider damageZone;
    private Transform target;
    private FieldOfView fow;
    [SerializeField] private Weapon currentWeapon;

    public int horizontalVelocity;

    public override int Health
    {
        get => health;
        set
        {
            if (health < 0)
            {
                health = 0;
            }
            else
            {
                health = value;
            }
        }
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
    public override bool IsUnderEffect => bIsSlowed;

    public override float RotationSpeed { get => agent.angularSpeed; set => agent.angularSpeed = value; }
    public override Weapon CurrentWeapon => currentWeapon;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        fow = GetComponent<FieldOfView>();
        damageZone = GetComponentInChildren<BoxCollider>();
    }

    private void Start()
    {
        maxHealth = health;

        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
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
            }
        }

        if (target != null)
        {
            Vector3 dir = (target.position - transform.position).normalized;
            RotateToDirection(new Vector3(dir.x, 0, dir.z));
        }
    }

    public override void EarnDamage(int damage, GameObject sender)
    {
        if (bIsImmortal == true) return;

        Health -= damage;

        if (Health <= 0)
        {
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
            return;
        }
    }

    public override void PutEffect() => bIsSlowed = true;

    public override void RemoveEffect() => bIsSlowed = false;

    public override void ShowHealthBar(bool status)
    {
        // Unnecessary 
    }

    protected override void Die()
    {
        Debug.LogError($"{this.name} is dead.");

        Destroy(this.gameObject);
    }

    protected override void Move(float speed)
    {
        float disTarget = Vector3.Distance(transform.position, target.position);

        if (disTarget <= 1)
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

    private void RotateToDirection(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
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
}
