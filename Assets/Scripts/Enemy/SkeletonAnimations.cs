using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SkeletonAnimations : MonoBehaviour
{
    [SerializeField] private TrailRenderer swordTrail;
    [SerializeField] private AudioSource footAudio;
    [SerializeField] private AudioSource swordAudio;
    [SerializeField] private AudioSource bodyAudio;
    [Header("SFX")]
    [SerializeField] private AudioClip[] footsteps;
    [SerializeField] private AudioClip[] slashes;
    [SerializeField] private AudioClip[] bones;

    private NavMeshAgent agent;
    private Skeleton skeleton;
    private Animator anim;

    private void Awake()
    {
        agent = GetComponentInParent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        skeleton = GetComponentInParent<Skeleton>();
    }

    private void Start()
    {
        skeleton.onAttack.AddListener(Attack);
        skeleton.onEarnDamage.AddListener(EarnDamage);
    }

    private void Update()
    {
        if (agent == null) return;
        anim.SetFloat("Velocity", skeleton.horizontalVelocity); 
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
        bodyAudio.PlayOneShot(bones[Random.Range(0, bones.Length)]);
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
        if(swordTrail != null)
        {
            swordTrail.enabled = true;
        }
        skeleton.CurrentWeapon.StartAttack();
    }

    /// <summary>
    /// KnightAttack event
    /// </summary>
    public void AE_DisableDamageZone()
    {
        if(swordTrail != null)
        {
            swordTrail.enabled = false;
        }
        skeleton.CurrentWeapon.EndAttack();
    }

    /// <summary>
    ///  SkeletonEarnDamage event
    /// </summary>
    public void AE_AllowMotionSkeletion()
    {
        skeleton.ChangeMotionStatus(false);
    }

    /// <summary>
    /// SkeletonAttack event
    /// </summary>
    public void AE_AllowMotionSkeletionAttack()
    {
        skeleton.ChangeMotionStatus(false);
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
