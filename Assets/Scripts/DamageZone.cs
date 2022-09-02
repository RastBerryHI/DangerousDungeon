using UnityEngine;
using UnityEngine.Events;
public class DamageZone : MonoBehaviour
{
    [SerializeField] private WeaponType weaponType;
    [SerializeField] private BoxCollider damageZone;

    [SerializeField] private Character character;
    [SerializeField] public MeleeWeapon weaponOwner;

    private Character currentCharacter;
    private IDestroyable destroyable;

    public WeaponType WeaponType => weaponType;
    public UnityAction<Character> onHit;

    private void Awake()
    {
        damageZone = GetComponent<BoxCollider>();
        character = GetComponentInParent<Character>();
        damageZone.enabled = false;
    }

    private void Start()
    {
        damageZone.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name != character.name)
        {
            currentCharacter = other.GetComponent<Character>();
            destroyable = other.GetComponent<IDestroyable>();

            if(currentCharacter != null)
            {
                InterractWithCharacter(currentCharacter);
            }

            if(destroyable != null)
            {
                InterractWithDestroyable(destroyable);
            }

            currentCharacter = null;
            destroyable = null;
        }
    }

    private void InterractWithCharacter(Character character)
    {
        if (character == null)
        {
            return;
        }

        if (weaponType == WeaponType.Melee)
        {
            currentCharacter.EarnDamage(character.Damage + weaponOwner.DamageBonus, gameObject);
        }
        else if (weaponType == WeaponType.Magic)
        {
            currentCharacter.EarnDamage(character.Damage, gameObject);
        }

        if (weaponOwner != null)
        {
            if (currentCharacter.IsUnderEffect == true)
            {
                return;
            }

            weaponOwner.PutEffectOn(currentCharacter.transform);
        }

        onHit(currentCharacter);
    }

    private void InterractWithDestroyable(IDestroyable destroyObj)
    {
        if(destroyObj == null)
        {
            return;
        }

        if (weaponType == WeaponType.Melee)
        {
            destroyObj.EarnDamage(character.Damage + weaponOwner.DamageBonus, gameObject);
        }
        else if (weaponType == WeaponType.Magic)
        {
            destroyObj.EarnDamage(character.Damage, gameObject);
        }
    }
}
