using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponEffect
{
    public GameObject EffectFX { get; set; }
    public bool IsEffectDamages { get;}
    public bool IsEffectSlows { get;}
    public int EffectDamage { get; set; }
    public float EffectSlowKoeficient { get; set; }
    public void PutEffectOn(Transform target);
}
