using UnityEngine;

namespace CharacterComponents
{
    public class MovementControllable : MonoBehaviour
    {
        protected float horizontalVelocity;
        protected bool isMotionBanned;
        
        public float HorizontalVelocity => horizontalVelocity;
        
        public bool IsMotionBanned
        {
            set => isMotionBanned = value;
            get => isMotionBanned;
        }
    }
}