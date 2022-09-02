using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBossAnimations : MonoBehaviour
{
    [SerializeField] private TrailRenderer swordTrail;
    [SerializeField] private ParticleSystem armGrowFx;
    [SerializeField] private TossSkull tossSkull;
    [SerializeField] private Transform tossSource;
    [SerializeField] private AudioSource footAudio;
    [SerializeField] private AudioSource swordAudio;
    [SerializeField] private AudioSource bodyAudio;
    [SerializeField] private Transform[] skeletonSummonSpawns;
    [Header("SFX")]
    [SerializeField] private AudioClip[] footsteps;
    [SerializeField] private AudioClip[] slashes;
    [SerializeField] private AudioClip[] bones;

    private CharacterController characterController;
    private SkeletonBoss skeleton;
    private Animator anim;
    private bool bIsSummonBanned;

    private void Awake()
    {
        characterController = GetComponentInParent<CharacterController>();
        anim = GetComponent<Animator>();
        skeleton = GetComponentInParent<SkeletonBoss>();
    }

    private void Start()
    {
        skeleton.onAttack.AddListener(Attack);
    }
    private void FixedUpdate()
    {
        if(skeleton.SummonedSkeletons.Count == 0)
        {
            bIsSummonBanned = false;
        }
    }

    private void Update()
    {
        if (characterController == null) return;
        anim.SetFloat("Velocity", skeleton.horizontalVelocity);
    }

    private void Attack()
    {
        int rand = Random.Range(0, 4);
        Debug.Log(rand);
        switch (rand)
        {
            case 0:
                anim.SetBool("isStepAttack", true);
                StartCoroutine(DisableAttack("isStepAttack"));

                break;
            case 1:
                anim.SetBool("isAttack", true);
                StartCoroutine(DisableAttack("isAttack"));

                break;
            case 2:
                anim.SetBool("isArmGrowAttack", true);
                StartCoroutine(DisableAttack("isArmGrowAttack"));
                 
                break;
            case 3:
                if (bIsSummonBanned == true)
                {
                    anim.SetBool("isAttack", true);
                    StartCoroutine(DisableAttack("isAttack"));
                }
                else
                {
                    anim.SetBool("isToss", true);
                    StartCoroutine(DisableAttack("isToss"));
                    bIsSummonBanned = true;
                }

                break;
        }
    }

    private IEnumerator<WaitForSeconds> DisableAttack(string attack)
    {
        yield return new WaitForSeconds(0.1f);
        anim.SetBool(attack, false);
    }

    public void AE_SummonSkeletons()
    {
        LevelGenerator.s_levelGenerator.SpawnWithChance(LevelGenerator.s_levelGenerator.DungeonUnit);
        skeleton.SummonedSkeletons = FindObjectsOfType<Character>().Where(s => s.transform.tag == "Character").ToList<Character>();
    }

    public void AE_TossSkull()
    {
        TossSkull _tossSkull = Instantiate(tossSkull, tossSource.position, Quaternion.identity);
        _tossSkull.owner = skeleton;
    }

    /// <summary>
    /// KnightAttack event
    /// </summary>
    public void AE_EnableDamageZone()
    {
        swordTrail.enabled = true;
        skeleton.CurrentWeapon.StartAttack();
    }

    /// <summary>
    /// KnightAttack event
    /// </summary>
    public void AE_DisableDamageZone()
    {
        swordTrail.enabled = false;
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

    public void AE_PlayArmGrowFX()
    {
        armGrowFx.Emit(50);
    }
}
