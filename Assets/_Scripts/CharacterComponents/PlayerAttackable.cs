using UnityEngine;

namespace CharacterComponents
{
    [RequireComponent(typeof(InputHandler))]
    public class PlayerAttackable : Attackable
    {
        public void FirstAction(bool isInvoking)
        {
            if (isInvoking)
            {
                onAttack?.Invoke();
                return;
            }
            
            onEndAction?.Invoke();
        }

        public void SecondaryAction(bool isInvoking)
        {
            if (isInvoking)
            {
                onSecondaryAction?.Invoke();
                return;
            }
            
            onEndSecondAction?.Invoke();
        }
    }
}
