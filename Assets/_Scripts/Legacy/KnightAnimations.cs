using System.Collections.Generic;
using UnityEngine;

public class KnightAnimations : MonoBehaviour
{
    [SerializeField] private TrailRenderer swordTrail;
    [SerializeField] private ParticleSystem blood;
    [SerializeField] private AudioSource footAudio;
    [SerializeField] private AudioSource swordAudio;
    [SerializeField] private AudioSource bodyAudio;
    [Header("SFX")]
    [SerializeField] private AudioClip[] footsteps;
    [SerializeField] private AudioClip[] slashes;
    [SerializeField] private AudioClip[] damages;

    private CharacterController characterController;
    private Knight knight;
    private Animator anim;
    private void Awake()
    {
        characterController = GetComponentInParent<CharacterController>();
        anim = GetComponent<Animator>();
        knight = GetComponentInParent<Knight>();
    }

    private void Start()
    {
        knight.onAttack.AddListener(Attack);
        knight.onEarnDamage.AddListener(EarnDamage);
        knight.onDie.AddListener(EnableRagDoll);

        knight.onShieldBlock.AddListener(ShieldBlock);
        knight.onRemoveShieldBlock.AddListener(RemoveBlock);
    }

    private void Update()
    {
        if (characterController == null) return;
        anim.SetFloat("Velocity", new Vector3(characterController.velocity.x, 0, characterController.velocity.z).magnitude);
    }

    private void EnableRagDoll()
    {
        transform.parent = null;
        anim.enabled = false;
    }

    private void Attack()
    {
        knight.IsAttacking = true;
        anim.SetTrigger("Attack");
    }

    private void AllowAttack() 
    {
        knight.IsAttacking = false;
    }

    private void ShieldBlock()
    {
        anim.SetLayerWeight(1, 1);
    }

    private void RemoveBlock()
    {
        anim.SetLayerWeight(1, 0);
    }

    private void EarnDamage()
    {
        anim.SetBool("isDamage", true);
        blood.Emit(10);
        bodyAudio.PlayOneShot(damages[Random.Range(0, damages.Length)]);
        StartCoroutine(DisableDamage());
    }

    private IEnumerator<WaitForSeconds> DisableDamage()
    {
        yield return new WaitForSeconds(0.1f);
        anim.SetBool("isDamage", false);
    }

    /// <summary>
    /// KnightAttack event
    /// </summary>
    public void AE_EnableDamageZone()
    {
        swordTrail.enabled = true;
        knight.CurrentWeapon.StartAttack();
    }

    /// <summary>
    /// KnightAttack event
    /// </summary>
    public void AE_DisableDamageZone()
    {
        knight.CurrentWeapon.EndAttack();
        swordTrail.enabled = false;
    }

    /// <summary>
    /// KnightWalk event
    /// </summary>
    public void AE_PlayFootStep()
    {
        footAudio.PlayOneShot(footsteps[Random.Range(0, footsteps.Length)]);
    }

    /// <summary>
    /// KnightAttack event
    /// </summary>
    public void AE_PlaySwordSlash()
    {
        footAudio.PlayOneShot(slashes[Random.Range(0, slashes.Length)]);
    }
}
