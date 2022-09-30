using CharacterComponents;
using UnityEngine;

namespace StateMachines
{
    public class AttackBehaviour : CacheBehaviour
    {
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Attackable?.onEndAction?.Invoke();
        }
    }
}
