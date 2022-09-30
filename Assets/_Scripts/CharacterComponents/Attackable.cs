using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace CharacterComponents
{
    public class Attackable : MonoBehaviour
    {
        protected bool attackAllowed = true;

        public bool AttackAllowed
        {
            set => attackAllowed = value;
            get => attackAllowed;
        }
        
        public UnityEvent onAttack;
        public UnityEvent onSecondaryAction;
        public UnityEvent onEndAction;
        public UnityEvent onEndSecondAction;
    }
}
