using CharacterComponents;
using UnityEngine;

namespace StateMachines
{
    public class AttackBehaviour : CacheBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Movable.IsMotionBanned = true;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Attackable?.onEndAction?.Invoke();
        
            Movable.IsMotionBanned = false;
        }
    }
}
