using UnityEngine;

namespace CharacterComponents
{
    public class SFXEmittable : MonoBehaviour
    {
        [Header("SFX")]
        [SerializeField] private AudioClip[] movementSfx;
        [SerializeField] private AudioClip[] attackSfx;
        [SerializeField] private AudioClip[] damageSfx;
        [SerializeField] private AudioClip[] deathSfx;
    
        [Header("AudioHolders")]
        [SerializeField] private AudioSource bodyAudio;
        [SerializeField] private AudioSource weaponAudio;
        [SerializeField] private AudioSource footAudio;

        public void PlayRandomMovement()
        {
            if (movementSfx.Length == 0)
            {
                return;
            }
            footAudio.PlayOneShot(movementSfx[Random.Range(0, movementSfx.Length)]);
        }

        public void PlayRandomAttack()
        {
            if (attackSfx.Length == 0)
            {
                return;
            }
            weaponAudio.PlayOneShot(attackSfx[Random.Range(0, attackSfx.Length)]);
        }

        public void PlayRandomDamage()
        {
            if (damageSfx.Length == 0)
            {
                return;
            }
            bodyAudio.PlayOneShot(damageSfx[Random.Range(0, damageSfx.Length)]);
        }

        public void PlayRandomDeath()
        {
            if (deathSfx.Length == 0)
            {
                return;
            }
            bodyAudio.PlayOneShot(deathSfx[Random.Range(0, deathSfx.Length)]);
        }
    }
}
