using UnityEngine;

namespace CharacterComponents
{
    public class MovementControllable : MonoBehaviour
    {
        protected float horizontalVelocity;
        
        public float HorizontalVelocity => horizontalVelocity;
    }
}