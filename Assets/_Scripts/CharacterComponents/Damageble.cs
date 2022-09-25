using UnityEngine;
using UnityEngine.Events;

public class Damageble : MonoBehaviour
{
    [SerializeField] private float health;
    
    private float maxHealth;
    private bool isImmortal;

    public float Health
    {
        get => health;
        set
        {
            health = value;

            if (health < 0)
            {
                health = 0;
            }
            else if (health > maxHealth)
            {
                health = maxHealth;
            }
        }
    }

    public float MaxHealth => maxHealth;
    
    public bool IsImmortal => isImmortal;
    
    [Space]
    public UnityEvent onEarnDamage;
    public UnityEvent onDie;
    
    public void EarnDamage(int damage)
    {
        if (isImmortal)
        {
            return;
        }

        Health -= damage;

        if (Health == 0)
        {
            Die();
        }
        
        onEarnDamage.Invoke();
    }

    public void BecomeImmortal() => isImmortal = true;
    public void BecomeMortal() => isImmortal = false;
    
    private void Die()
    {
        onDie.Invoke();
        
        // Implement summoned skeleton removing
        // if (SkeletonBoss.s_instance != null)
        // {
        //     SkeletonBoss.s_instance.SummonedSkeletons.Remove(this);
        // }
        
        Destroy(gameObject);
    }
}
