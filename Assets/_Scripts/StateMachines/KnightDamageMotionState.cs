using UnityEngine;

public class KnightDamageMotionState : KnightEntryMotionState
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        knight.IsAttacking = false;
    }
}
