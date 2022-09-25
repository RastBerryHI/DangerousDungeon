using UnityEngine;

public class VitalityCurse : Curse
{
    private static bool isCursed;
    public override void ImposeACurse(Character character)
    {
        if(!isCursed)
        {
            character.MaxHealth /= 2;
            character.EarnDamage(0, gameObject);
            
            isCursed = true;
        }
    }

    public override void DispelTheCurse(Character character)
    {
        if(isCursed && character != null)
        {
            character.MaxHealth *= 2;
            character.EarnDamage(0, gameObject);

            isCursed = false;
        }
    }
}
