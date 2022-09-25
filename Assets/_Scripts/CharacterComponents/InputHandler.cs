using UnityEngine;
using UnityEngine.Events;

namespace CharacterComponents
{
    public class InputHandler : MonoBehaviour
    {
        [SerializeField] private KeyCode keyHold;
        [SerializeField] private KeyCode keyDown;
        
        public UnityEvent<bool> onKeyHold;
        public UnityEvent<bool> onKeyDown;
        
        private void Update()
        {
            onKeyHold?.Invoke(Input.GetKey(keyHold));   
            onKeyDown?.Invoke(Input.GetKeyDown(keyDown));
        }
    }
}