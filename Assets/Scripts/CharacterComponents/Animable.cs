using UnityEngine;
using Cysharp.Threading.Tasks;

namespace CharacterComponents
{
    public class Animable : MonoBehaviour
    {
        [SerializeField] private TrailRenderer attackTrail;
        [SerializeField] private AudioSource bodyAudio;
        [SerializeField] private AudioSource footAudio;
        [SerializeField] private AudioSource weaponAudio;
        [SerializeField] private Animator anim;
        
        [Header("Character Components")]
        [SerializeField] private AIMovable movable;
        [SerializeField] private ItemsHoldable itemsHoldable;
        
        [Header("SFX")]
        [SerializeField] private AudioClip[] movementSfx;
        [SerializeField] private AudioClip[] attackSfx;
        [SerializeField] private AudioClip[] deathSfx;

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
            footAudio.PlayOneShot(movementSfx[Random.Range(0, movementSfx.Length)]);
        }

        public void ActivateDamageZone()
        {
            itemsHoldable.ActivateDamageZone();
        }

        public void DisableDamageZone()
        {
            itemsHoldable.DisableDamageZone();
        }
    }
}
