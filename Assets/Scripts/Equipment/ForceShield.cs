using System.Collections.Generic;
using UnityEngine;

public class ForceShield : MonoBehaviour
{
    [SerializeField] private float shieldDuration;
    [SerializeField] private float shieldReload;

    private Character owner;
    private ForceShieldVisuals shieldFX;
    private bool bIsSheildUsed;

    private void Awake()
    {
        owner = GetComponentInParent<Character>();
        shieldFX = GetComponent<ForceShieldVisuals>();
    }

    private void Start()
    {
        owner.onEarnDamage.AddListener(OnEarnDamage);
    }

    private void OnEarnDamage()
    {
        if((owner.Health <= owner.MaxHealth / 2) && !bIsSheildUsed)
        {
            owner.IsImmortal = true;
            StartCoroutine(SupportShield());
            shieldFX.EnableShield();
            bIsSheildUsed = true;
        }
    }

    private IEnumerator<WaitForSeconds> SupportShield()
    {
        yield return new WaitForSeconds(shieldDuration);

        owner.IsImmortal = false;
        shieldFX.DisableShield();
        StartCoroutine(ReloadShield());
    }

    private IEnumerator<WaitForSeconds> ReloadShield()
    {
        yield return new WaitForSeconds(shieldReload);
        bIsSheildUsed = false;
    }

}
