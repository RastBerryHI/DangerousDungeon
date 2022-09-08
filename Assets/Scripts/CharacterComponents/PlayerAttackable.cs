using UnityEngine;

namespace CharacterComponents
{
    enum ActionStates
    {
        Idle,
        FirstAction,
        SecondaryAction
    }
    
    public class PlayerAttackable : Attackable
    {
        private ActionStates action;
        
        void Update()
        {
            // Action state machine
            if (action != ActionStates.Idle)
            {
                return;
            }
            
            if (Input.GetMouseButton(0))
            {   
                FirstAction();
                action = ActionStates.FirstAction;
            }
            else if (Input.GetMouseButton(1))
            {
                SecondaryAction();
                action = ActionStates.SecondaryAction;
            }
        }
        
        public void FinnishAction()
        {
            action = ActionStates.Idle;
        }

        private void FirstAction()
        {
            onAttack?.Invoke();
        }

        private void SecondaryAction()
        {
            onSecondaryAction?.Invoke();
        }

    }
}
