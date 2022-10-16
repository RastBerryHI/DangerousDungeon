using CharacterComponents;
using UnityEngine;

public class CacheBehaviour : StateMachineBehaviour
{
    private Attackable attackable;
    private Movable movable;

    protected Attackable Attackable => attackable;
    protected Movable Movable => movable;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!attackable)
        {
            attackable = animator.GetComponentInParent<Attackable>();
        }

        if (!movable)
        {
            movable = animator.GetComponentInParent<Movable>();
        }
    }
}
