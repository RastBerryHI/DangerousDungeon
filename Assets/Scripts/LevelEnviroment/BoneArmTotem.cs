using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class BoneArmTotem : MonoBehaviour, IDestroyable
{
    [SerializeField] private Rigidbody totemHead;
    [SerializeField] private Rigidbody totemLeg;
    [SerializeField] private GameObject arm;
    [SerializeField] private float verticalOffset;
    [SerializeField] private float attackDelay;
    [SerializeField] private float strength;

    private bool shouldSpawn = true;

    private Vector3 spawnPos;
    private Vector3 initalPos;

    private GameObject instantiatedArm;
    private Tween completedTween;
    private Coroutine attackCoroutine;

    private WaitForSeconds wait;

    public float Strength => strength;
    public UnityEvent onFallApart;

    private void Awake()
    {
        wait = new WaitForSeconds(attackDelay);
    }

    private void Update()
    {
        if (shouldSpawn)
        {
            attackCoroutine = StartCoroutine(DelayAttack());
            shouldSpawn = false;
        }

        if (completedTween == null)
        {
            return;
        }

        if (!completedTween.active && instantiatedArm != null)
        {
            instantiatedArm.transform.DOMove(spawnPos, 1);
        }
    }

    private IEnumerator<WaitForSeconds> DelayAttack()
    {
        yield return wait;
        AttackTarget();
        shouldSpawn = true;
    }

    private void AttackTarget()
    {
        initalPos = PlayerSummon.s_instance.Character.transform.position; ;
        spawnPos = initalPos;
        spawnPos = new Vector3(spawnPos.x, spawnPos.y - verticalOffset, spawnPos.z);

        instantiatedArm = Instantiate<GameObject>(arm, spawnPos, arm.transform.transform.rotation);
        completedTween = instantiatedArm.transform.DOMove(new Vector3(spawnPos.x, spawnPos.y + verticalOffset, spawnPos.z), 1);

        Destroy(instantiatedArm, 3);
    }

    public void FallApart()
    {
        onFallApart.Invoke();

        totemHead.isKinematic = false;
        totemLeg.isKinematic = false;

        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            Destroy(instantiatedArm);
        }

        Destroy(gameObject, 3);
    }

    public void EarnDamage(float damage, GameObject sender)
    {
        strength -= damage;

        if (strength <= 0)
        {
            FallApart();
        }
    }
}
