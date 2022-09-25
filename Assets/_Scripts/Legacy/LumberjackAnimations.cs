using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class LumberjackAnimations : MonoBehaviour
{
    [SerializeField] private TrailRenderer swordTrail;
    [SerializeField] private ParticleSystem blood;
    [SerializeField] private AudioSource footAudio;
    [SerializeField] private AudioSource bodyAudio;
    
    [Header("SFX")]
    [SerializeField] private AudioClip[] footsteps;
    [SerializeField] private AudioClip[] damages;
    [SerializeField] private AudioClip[] slashes;

    private NavMeshAgent agent;
    private Lumberjack lumber;
    private Animator anim;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        lumber = GetComponent<Lumberjack>();
    }

    private void Start()
    {
        lumber.onAttack.AddListener(Attack);
        lumber.onEarnDamage.AddListener(EarnDamage);
    }

    private void Update()
    {
        if (agent == null) return;
        anim.SetFloat("Velocity", lumber.horizontalVelocity); 
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
        bodyAudio.PlayOneShot(damages[Random.Range(0, damages.Length)]);
    }

    private IEnumerator<WaitForSeconds> DelayTurnoff(string parameter)
    {
        yield return new WaitForSeconds(0.1f);
        anim.SetBool(parameter, false);
    }

    /// <summary>
    /// KnightAttack event
    /// </summary>
    public void AE_EnableDamageZone()
    {
        swordTrail.enabled = true;
        lumber.CurrentWeapon.StartAttack();
    }

    /// <summary>
    /// KnightAttack event
    /// </summary>
    public void AE_DisableDamageZone()
    {
        swordTrail.enabled = false ;
        lumber.CurrentWeapon.EndAttack();
    }

    /// <summary>
    ///  SkeletonEarnDamage event
    /// </summary>
    public void AE_AllowMotionSkeletion()
    {
        lumber.ChangeMotionStatus(false);
    }

    /// <summary>
    /// SkeletonAttack event
    /// </summary>
    public void AE_AllowMotionSkeletionAttack()
    {
        lumber.ChangeMotionStatus(false);
    }

    /// <summary>
    /// SkeletontWalk event
    /// </summary>
    public void AE_PlayFootStep()
    {
        footAudio.PlayOneShot(footsteps[Random.Range(0, footsteps.Length)]);
    }

    /// <summary>
    /// SkeletontAttack event 
    /// </summary>
    public void AE_PlaySwordSlash()
    {
        footAudio.PlayOneShot(slashes[Random.Range(0, slashes.Length)]);
    }
}
