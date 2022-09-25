using System;
using UnityEngine;

public class VFXEmittable : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] damageVFX;
    [SerializeField] private Rigidbody[] deathPieces;
    [SerializeField] private GameObject attackVfx;
    
    public void EmitAll()
    {
        foreach(var vfx in damageVFX)
        {
            vfx.Emit(10);vfx.Emit(10);
        }
    }

    public void EmitById(int id)
    {
        try
        {
            damageVFX[id].Emit(10);
        }
        catch (ArgumentOutOfRangeException rangeEx)
        {
            Debug.LogError("vfx id is out of range");
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    public void EmitAttackVfx()
    {
        attackVfx.SetActive(true);
    }

    public void AbortAttackVfx()
    {
        attackVfx.SetActive(false);
    }
    
    public void OnDie()
    {
        foreach (var part in deathPieces)
        {
            part.transform.parent = null;
            part.gameObject.SetActive(true);
        }
    }
}
