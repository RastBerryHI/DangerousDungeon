using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon, IWeaponEffect
{
    [SerializeField] private GameObject effectFX;
    [SerializeField] private int damageBonus;
    [SerializeField] private int effectDamage;
    [SerializeField] private float effectSlowKoeficient;
    [SerializeField] private float effectDuration;
    [SerializeField] private float effectCullDown;
    [SerializeField] private float effectChance;
    [SerializeField] private bool bIsEffectDamages;
    [SerializeField] private bool bIsEffectSlows;
    [SerializeField] private DamageZone damageZone;

    private BoxCollider pickupVoulme;
    private WeaponType weaponType;
    
    public bool bIsUnInterractable;
    public bool IsEffectDamages => bIsEffectDamages;
    public bool IsEffectSlows => bIsEffectSlows;

    public int EffectDamage 
    { 
        get => effectDamage;
        set
        {
            if(bIsEffectDamages == true) effectDamage = value;
        }
    }

    public float EffectSlowKoeficient
    {
        get => effectSlowKoeficient;
        set 
        {
            if (value <= 0) effectSlowKoeficient = 0.001f;
            if(bIsEffectSlows == true) effectSlowKoeficient = value;
        }
    }

    public float EffectDuration => effectDuration;
    public DamageZone DamageZone { set => damageZone = value; }
    public override int Damage { get => damageBonus; set => damageBonus = value; }
    public override WeaponType WeaponType => weaponType;
    public GameObject EffectFX { get => effectFX; set => effectFX = value; }

    private void Awake()
    {
        pickupVoulme = GetComponent<BoxCollider>();
    } 

    private void OnTriggerEnter(Collider other)
    {
        if(bIsUnInterractable == true) return;
        if(other.tag == "Player" && other.transform.name.Contains("Knight") == true)
        {
            Knight knight = other.GetComponent<Knight>();
            knight.PickupSword(this); 
            damageZone = knight.DamageZone;

            //damageZone.weapon = this;
            pickupVoulme.enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player" && other.transform.name.Contains("Knight") == true)
        {
            bIsUnInterractable = false;
        }
    }

    public override void EndAttack()
    {
        //damageZone.GetComponent<BoxCollider>().enabled = false;  
    }

    public void PutEffectOn(Transform target)
    {
        if (effectFX == null) return;
        if (Random.value <= effectChance) return;
        
        Character character = target.GetComponent<Character>();
        if (character == null) return;
          
        GameObject effect = new GameObject();

        if(effectFX.name == "Ice")
        {
            effect = Instantiate(effectFX, target.position, Quaternion.identity);
            effect.transform.SetParent(target);
        }
        else
        {
            effect = Instantiate(effectFX, target.position, Quaternion.identity, target);
        }
        
        StartCoroutine(DelayTurnOffFX(effect, character));
    }

    public override void StartAttack()
    {
        //damageZone.GetComponent<BoxCollider>().enabled = true;
    }

    private IEnumerator<WaitForSeconds> DelayTurnOffFX(GameObject fx, Character target)
    {
        Animator characterAnim = target.GetComponentInChildren<Animator>();
        float baseSpeed, baseRotationSpeed, baseAnimSpeed;
    
        target.PutEffect();
        baseSpeed = target.Speed;
        baseRotationSpeed = target.RotationSpeed;
        baseAnimSpeed = characterAnim.speed;

        if (bIsEffectSlows == true && characterAnim != null)
        {
            target.Speed *= effectSlowKoeficient;
            target.RotationSpeed *= effectSlowKoeficient;

            characterAnim.speed = effectSlowKoeficient;
        }

        for (int i = -1; i < effectDuration; i++)
        {
            // pass first iteration to avoid double damage from spawn effect + sword hit
            if (i > -1)
            {
                if (bIsEffectDamages == true)
                {
                    target.EarnDamage(effectDamage, gameObject);
                }
            }
            yield return new WaitForSeconds(effectCullDown);
        }

        if (bIsEffectSlows == true && characterAnim != null)
        {
            target.Speed = baseSpeed;
            target.RotationSpeed = baseRotationSpeed;

            characterAnim.speed = baseAnimSpeed;
        }

        characterAnim = null;
        Destroy(fx.gameObject);
        target.RemoveEffect();
    }
}
