using UnityEngine;

public class KnightEntryMotionState : StateMachineBehaviour
{
    protected Knight knight;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (knight != null)
        {
            return;
        }

        knight = animator.GetComponentInParent<Knight>();
    }
}
