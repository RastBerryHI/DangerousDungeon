using UnityEngine;
using UnityEngine.Events;

public class DamageZone : MonoBehaviour
{
    [SerializeField] private WeaponType weaponType;
    [SerializeField] private BoxCollider damageZone;

    [SerializeField] private ItemsHoldable itemsHoldable;
    [SerializeField] private Damageble damageble;
    
    [SerializeField] private Transform trm;
    private IDestroyable destroyable;
    
    public UnityAction<Damageble> OnOverlap;

    private void Start()
    {
        trm = transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (trm.parent != other.transform)
        {
            var damagebale = other.GetComponent<Damageble>();
        
            if (damagebale)
            {
                InteractWithDamageble(damagebale);
            }
        
            // if (destroyable != null)
            // {
            //     InteractWithDestroyable(destroyable);
            // }
        }
    }

    private void InteractWithDamageble(Damageble target)
    {
        if (!itemsHoldable)
        {
            return;
        }

        target.EarnDamage(itemsHoldable.Weapon.Damage);
        
        // TODO: Refactor effects system
        
        // if (weapon && !currentCharacter.IsUnderEffect)
        // {
        //     weapon.PutEffectOn(currentCharacter.transform);
        // }

        OnOverlap(target);
    }

    private void InteractWithDestroyable(IDestroyable destroyObj)
    {
        // TODO: Get rid of destroyables
    }
}
