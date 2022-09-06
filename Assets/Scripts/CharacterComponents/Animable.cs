using UnityEngine;
using Cysharp.Threading.Tasks;

namespace CharacterComponents
{
    public class Animable : MonoBehaviour
    {
        [SerializeField] private Animator anim;
        
        [Header("Character Components")]
        [SerializeField] private MovementControllable movable;
        [SerializeField] private ItemsHoldable itemsHoldable;
        [SerializeField] private SFXEmittable sfx;

        private void Update()
        {
            if (!movable)
            {
                return;
            }
            
            anim.SetFloat("Velocity", movable.HorizontalVelocity);
        }

        private async void Trigger(string state)
        {
            anim.SetTrigger(state);
            await UniTask.NextFrame();
            anim.ResetTrigger(state);
        }

        public void Attack()
        {
            Trigger("Attack");
        }
        
        public void EarnDamage()
        {
           Trigger("Damage");
        }

        public void PlayFootStep()
        {
            sfx.PlayRandomMovement();
        }

        public void ActivateDamageZone()
        {
            itemsHoldable.ActivateDamageZone();
            sfx.PlayRandomAttack();
        }

        public void DisableDamageZone()
        {
            itemsHoldable.DisableDamageZone();
        }
    }
}
