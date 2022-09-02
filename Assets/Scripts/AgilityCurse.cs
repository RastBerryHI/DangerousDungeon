using UnityEngine;

public class AgilityCurse : Curse
{
    private static bool isCursed;
    private Animator animator;
    private float baseAnimSpeed;
    public override void ImposeACurse(Character character)
    {
        if (!isCursed)
        {
            animator = character.GetComponentInChildren<Animator>();
            if (animator == null)
            {
                animator = character.GetComponent<Animator>();
            }

            if (animator == null)
            {
                WarnerMessages.NoComponent("Animator", character);
                return;
            }

            baseAnimSpeed = animator.speed;
            animator.speed = 0.8f;
            character.Speed /= 2;

            isCursed = true;
        }
    }
    public override void DispelTheCurse(Character character)
    {
        if (isCursed)
        {
            animator.speed = baseAnimSpeed;
            character.Speed *= 2;

            isCursed = false;
        }
    }

}
