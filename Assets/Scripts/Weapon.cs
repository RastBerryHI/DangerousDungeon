using UnityEngine;

public enum WeaponType
{
    Melee,
    Range,
    Magic
}

/// <summary>
/// Base class for any weapon
/// </summary>
public abstract class Weapon : MonoBehaviour
{
    public abstract int Damage { get; set; }
    public abstract WeaponType WeaponType { get;}

    public abstract void StartAttack();
    public abstract void EndAttack();

    public Sprite sprite;
}
