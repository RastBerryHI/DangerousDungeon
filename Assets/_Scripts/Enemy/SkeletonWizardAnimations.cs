using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonWizardAnimations : MonoBehaviour
{
    [SerializeField] private AudioSource footAudio;
    [SerializeField] private AudioSource swordAudio;
    [SerializeField] private AudioSource bodyAudio;
    [Header("SFX")]
    [SerializeField] private AudioClip[] attacks;
    [SerializeField] private AudioClip[] bones;

    private SkeletonWizard skeleton;
    private Animator anim; 

    private void Awake()
    {
        skeleton = GetComponentInParent<SkeletonWizard>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        skeleton.onAttack.AddListener(Attack);
        skeleton.onEarnDamage.AddListener(EarnDamage);
    }

    private void Update()
    {
        anim.SetFloat("Velocity", skeleton.horizontalVelocity);
    }

    private void Attack()
    {
        anim.SetBool("isAttack", true);
        StartCoroutine(DelayTurnOff("isAttack"));
    }

    private void EarnDamage()
    {
        anim.SetBool("isDamage", true);
        StartCoroutine(DelayTurnOff("isDamage"));
        bodyAudio.PlayOneShot(bones[Random.Range(0, bones.Length)]);
    }

    /// <summary>
    /// Wizard attack event
    /// </summary>
    public void AE_EnableDamageZone()
    {
        skeleton.StartAttack();
    }

    /// <summary>
    /// Wizard attack event
    /// </summary>
    public void AE_DisableDamageZone()
    {
        skeleton.EndAttack();
    }
    /// <summary>
    ///  Wizard movement event
    /// </summary>
    public void AE_AllowMotionSkeleton()
    {
        skeleton.ChangeMotionStatus(false);
    }

    private IEnumerator<WaitForSeconds> DelayTurnOff(string parameter)
    {
        yield return new WaitForSeconds(0.1f);
        anim.SetBool(parameter, false);
    }

}
