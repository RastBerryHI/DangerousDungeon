using DigitalRuby.LightningBolt;
using UnityEngine;

public class LightningFactory : MonoBehaviour
{
    [SerializeField] private LightningBoltScript lightning;

    private void Awake()
    {
        // TODO: change lightning cast system
        
        MeleeWeapon currentWeapon = FindObjectOfType<ItemsHoldable>().Weapon.GetComponent<MeleeWeapon>();
        LightningBoltScript instantiated = Instantiate(lightning, transform.position, Quaternion.identity, transform.parent);

        instantiated.StartObject.transform.parent = transform.parent;
        instantiated.StartObject.transform.position = transform.position;

        instantiated.EndObject.transform.parent = currentWeapon.transform;
        instantiated.EndObject.transform.position = currentWeapon.transform.position;

        Destroy(instantiated.StartObject, currentWeapon.EffectDuration);
        Destroy(instantiated.EndObject, currentWeapon.EffectDuration);
        Destroy(instantiated.gameObject, currentWeapon.EffectDuration);
        Destroy(gameObject, currentWeapon.EffectDuration);
    }

}
