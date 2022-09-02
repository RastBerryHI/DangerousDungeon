using UnityEngine;
using UnityEngine.AI;

public class AIAttackable : Attackable
{
    [SerializeField] private FieldOfView fieldOfView;
    [SerializeField] private NavMeshAgent agent;

    private void Update()
    {
        if (agent.isStopped && fieldOfView.ClosestTarget)
        {
            onAttack.Invoke();
        }
    }
}
