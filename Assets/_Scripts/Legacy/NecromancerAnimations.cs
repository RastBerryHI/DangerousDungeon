using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class NecromancerAnimations : MonoBehaviour
{
    [SerializeField] private ParticleSystem blood;
    [SerializeField] private AudioSource footAudio;
    [SerializeField] private AudioSource wandAudio;
    [SerializeField] private AudioSource bodyAudio;
    [Header("SFX")]
    [SerializeField] private AudioClip[] footsteps;
    [SerializeField] private AudioClip[] damages;
    [Header("VFX")]
    [SerializeField] private Transform soulGrabber;
    [SerializeField] private Transform devastationPointsHolder;
    private Transform[] devastationsPoints;

    private CharacterController characterController;
    private Necromancer necromancer;
    private Animator anim;
    private void Awake()
    {
        characterController = GetComponentInParent<CharacterController>();
        anim = GetComponent<Animator>();
        necromancer = GetComponentInParent<Necromancer>();
        devastationsPoints = devastationPointsHolder.GetComponentsInChildren<Transform>();
    }

    private void Start()
    {
        necromancer.onAttack.AddListener(Attack);
        necromancer.onAttackAlt.AddListener(AttackAlt);
        necromancer.onEarnDamage.AddListener(EarnDamage);
        necromancer.onDie.AddListener(EnableRagDoll);
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
        anim.SetBool("isAttack", true);
        StartCoroutine(DisableAttack());
    }

    private void AttackAlt()
    {
        anim.SetBool("isAttackAlt", true);
        StartCoroutine(DisableAttackAlt());
    }

    private void EarnDamage()
    {
        anim.SetBool("isDamage", true);
        blood.Emit(10);
        bodyAudio.PlayOneShot(damages[Random.Range(0, damages.Length)]);
        StartCoroutine(DisableDamage());
    }

    private IEnumerator<WaitForSeconds> DisableAttack()
    {
        yield return new WaitForSeconds(0.1f);
        anim.SetBool("isAttack", false);
    }

    private IEnumerator<WaitForSeconds> DisableAttackAlt()
    {
        yield return new WaitForSeconds(0.1f);
        anim.SetBool("isAttackAlt", false);
    }

    private IEnumerator<WaitForSeconds> DisableDamage()
    {
        yield return new WaitForSeconds(0.1f);
        anim.SetBool("isDamage", false);
    }

    /// <summary>
    /// NecromancerAttack event
    /// </summary>
    public void AE_Devastation()
    {
        NecromancerWand wand = necromancer.CurrentWeapon as NecromancerWand;
        if(wand != null)
        {
            wand.StartAttackAlt();
            for(int i = 1; i < devastationsPoints.Length; i++)
            {
                Transform devastator = Instantiate(soulGrabber, transform.position, Quaternion.identity);
                devastator.DOMove(devastationsPoints[i].position, 0.7f);
                Destroy(devastator.gameObject, 2);
            }
        }
    }
    /// <summary>
    /// NecromancerAttack event
    /// </summary>
    public void AE_SummonGhosts()
    {
        necromancer.CurrentWeapon.StartAttack();
    }
    /// <summary>
    /// NecromancerFootStep event
    /// </summary>
    public void AE_PlayFootStep() 
    {
        footAudio.PlayOneShot(footsteps[Random.Range(0, footsteps.Length)]);
    }
}
