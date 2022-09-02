using UnityEngine;
using UnityEngine.Events;

/// <summary>
///  Base behavior of every character
/// </summary>
public abstract class Character : MonoBehaviour
{
    // HideInInspector - attribute which hides fields 
    [HideInInspector] public UnityEvent onAttack;
    [HideInInspector] public UnityEvent onEarnDamage;
    [HideInInspector] public UnityEvent onDie;

    public abstract int Health { get; set; }
    public abstract int MaxHealth { get; set; }
    public abstract int Damage { get; set; }
    public abstract float Speed { get; set; }
    public abstract bool IsAlive { get;}
    public abstract bool IsImmortal { get; set; }
    public abstract bool IsUnderEffect { get; }
    public abstract float RotationSpeed { get; set; }
    public virtual Weapon CurrentWeapon {get;}
    public abstract void PutEffect();
    public abstract void RemoveEffect();

    protected abstract void Move(float speed);
    public abstract void EarnDamage(int damage, GameObject sender);
    public abstract void Heal(int healthInput);
    public abstract void ShowHealthBar(bool status);
    protected abstract void Die();
}
