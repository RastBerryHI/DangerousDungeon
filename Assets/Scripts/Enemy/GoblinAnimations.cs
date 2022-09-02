using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoblinAnimations : MonoBehaviour
{
    [SerializeField] private AudioSource footAudio;
    [SerializeField] private AudioSource swordAudio;
    [SerializeField] private AudioSource bodyAudio;
    [Header("SFX")]
    [SerializeField] private AudioClip[] footsteps;
    [SerializeField] private AudioClip[] slashes;
    [SerializeField] private AudioClip[] hitSFX;

    private NavMeshAgent agent;
    private Goblin goblin;
    private Animator anim;

    private void Awake()
    {
        agent = GetComponentInParent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        goblin = GetComponentInParent<Goblin>();
    }

    private void Start()
    {
        goblin.onAttack.AddListener(Attack);
        goblin.onEarnDamage.AddListener(EarnDamage);
    }

    private void Update()
    {
        if (agent == null) return;
        anim.SetFloat("Velocity", goblin.horizontalVelocity); 
    }

    private void Attack()
    {
        anim.SetBool("isAttack", true);
        StartCoroutine(DelayTurnoff("isAttack"));
    }

    private void EarnDamage()
    {
        anim.SetBool("isDamage", true);
        StartCoroutine(DelayTurnoff("isDamage"));
        bodyAudio.PlayOneShot(hitSFX[Random.Range(0, hitSFX.Length)]);
    }

    private IEnumerator<WaitForSeconds> DelayTurnoff(string parameter)
    {
        yield return new WaitForSeconds(0.1f);
        anim.SetBool(parameter, false);
    }

    public void AE_EnableDamageZone()
    {
        goblin.CurrentWeapon.StartAttack();
    }

    public void AE_DisableDamageZone()
    {
        goblin.CurrentWeapon.EndAttack();
    }

    public void AE_AllowMotion()
    {
        goblin.ChangeMotionStatus(false);
    }

    public void AE_PlayFootStep()
    {
        footAudio.PlayOneShot(footsteps[Random.Range(0, footsteps.Length)]);
    }

    public void AE_PlaySwordSlash()
    {
        footAudio.PlayOneShot(slashes[Random.Range(0, slashes.Length)]);
    }
}
