using UnityEngine;

namespace StateMachines
{
    public class DamageBehaviour : CacheBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Movable.IsMotionBanned = true;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Movable.IsMotionBanned = false;
        }
    }
}
