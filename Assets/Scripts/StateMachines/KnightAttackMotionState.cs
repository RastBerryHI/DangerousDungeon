using UnityEngine;

public class KnightAttackMotionState : KnightEntryMotionState
{
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       // knight.IsAttacking = false;
    }
}
