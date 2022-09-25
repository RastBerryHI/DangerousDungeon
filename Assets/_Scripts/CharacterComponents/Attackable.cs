using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace CharacterComponents
{
    public class Attackable : MonoBehaviour
    {
        public UnityEvent onAttack;
        public UnityEvent onSecondaryAction;
        public UnityEvent onEndAction;
        public UnityEvent onEndSecondAction;
    }
}
