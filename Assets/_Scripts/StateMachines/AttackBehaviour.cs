using CharacterComponents;
using UnityEngine;

public class AttackBehaviour : StateMachineBehaviour
{
    private Attackable attackable;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!attackable)
        {
            attackable = animator.GetComponentInParent<Attackable>();
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackable.onEndAction?.Invoke();
    }
}
