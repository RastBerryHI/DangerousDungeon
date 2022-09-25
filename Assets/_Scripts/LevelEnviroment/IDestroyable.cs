using UnityEngine;

public interface IDestroyable
{
    public float Strength { get;}

    public void EarnDamage(float damage, GameObject sender);

    public void FallApart(); 
}
