using UnityEngine;

public class ItemsHoldable : MonoBehaviour
{
    [SerializeField] private DamageZone damageZone;
    [SerializeField] private GameObject weaponSocket;
    [SerializeField] private Weapon weapon;
    
    public DamageZone DamageZone => damageZone;
    public GameObject WeaponSocket => weaponSocket;
    public Weapon Weapon => weapon;

    public void ActivateDamageZone()
    {
        damageZone.gameObject.SetActive(true);
    }

    public void DisableDamageZone()
    {
        damageZone.gameObject.SetActive(false);
    }
}
