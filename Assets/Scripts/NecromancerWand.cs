using System.Collections.Generic;
using UnityEngine;

public class NecromancerWand : Weapon
{
    [SerializeField] private int damageBonus;
    [SerializeField] private WeaponType weaponType;

    [SerializeField] private Character[] characters;
    [SerializeField] private Necromancer owner;
    [SerializeField] private GhostEater ghost;
    [SerializeField] private Transform ghostDir;

    [SerializeField] private float damageCullDown;
    [SerializeField] private float ghostSummonCullDown;
    [SerializeField] private float devastationRange;

    public override int DamageBonus 
    { 
        get => damageBonus; 
        set => damageBonus = value; 
    }
    public override WeaponType WeaponType => weaponType;


    private void Awake()
    {
        owner = GetComponentInParent<Necromancer>();
    }

    public override void EndAttack()
    {
        System.Array.Clear(characters, 0, characters.Length);
    }

    public override void StartAttack()
    {
        GhostEater currentGhost = Instantiate(ghost, ghostDir.position, Quaternion.identity);
        currentGhost.master = owner;
        currentGhost.direction = transform.forward;
        currentGhost.transform.LookAt(ghostDir.transform);
    }

    public void StartAttackAlt()
    {
        characters = FindObjectsOfType<Character>();
        if(characters == null || characters.Length == 0)
        {
            return;
        }
        
        for (int i = 0; i < characters.Length; i++)
        {
            if(characters[i] != null)
            {
                if (characters[i].name.Contains("Boss") == false && characters[i].transform.tag != "Player" && Vector3.Distance(transform.position, characters[i].transform.position) <= devastationRange)
                {
                    characters[i].EarnDamage(characters[i].MaxHealth, gameObject);
                }
                else if(characters[i].name.Contains("Boss") == true && characters[i].transform.tag != "Player" && Vector3.Distance(transform.position, characters[i].transform.position) <= devastationRange)
                {
                    characters[i].EarnDamage(owner.Damage + DamageBonus, gameObject);
                }
            }
        }

        System.Array.Clear(characters, 0, characters.Length);
        characters = null;
        owner.EarnDamage(owner.MaxHealth / 5, gameObject);
    }
}
