using UnityEngine;
using Cysharp.Threading.Tasks;

namespace CharacterComponents
{
    public class Animable : MonoBehaviour
    {
        [SerializeField] private TrailRenderer attackTrail;
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

        public async void Attack()
        {
            anim.SetBool("isAttack", true);
            await UniTask.Delay(100);
            if (anim)
            {
                anim.SetBool("isAttack", false);
            }
        }
        
        public async void EarnDamage()
        {
           anim.SetBool("isDamage", true); 
           await UniTask.Delay(100);
           if (anim)
           {
               anim.SetBool("isDamage", false);
           }
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
