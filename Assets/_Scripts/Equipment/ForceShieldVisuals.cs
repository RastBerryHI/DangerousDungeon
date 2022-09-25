using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ForceShieldVisuals : MonoBehaviour
{
    [SerializeField] private float step;

    private Renderer material;
    private MaterialPropertyBlock propBlock;

    private Coroutine currentCoroutine;
    private Tween currentTween;

    private void Awake()
    {
        material = GetComponent<Renderer>();
        propBlock = new MaterialPropertyBlock();
    }

    public void EnableShield()
    {
        material.SetPropertyBlock(propBlock);
        transform.localScale = Vector3.zero;

        currentCoroutine = StartCoroutine(UnpackShield());
        currentTween = transform.DOScale(new Vector3(2,2,2), 2);
    }

    public void DisableShield()
    {
        if (currentCoroutine != null) StopCoroutine(currentCoroutine);
        if (currentTween.IsActive() == true) currentTween.Kill();
        
        StartCoroutine(PackShield());
        transform.DOScale(Vector3.zero, 2);
    }

    private IEnumerator<WaitForSeconds> UnpackShield()
    {
        while (step < 0.5)
        {
            step += 0.005f;
            propBlock.SetFloat("_Offset", step);
            material.SetPropertyBlock(propBlock);
            yield return new WaitForSeconds(0.001f);
        }
    }
    private IEnumerator<WaitForSeconds> PackShield()
    {
        while (step > -2)
        {
            step -= 0.005f;
            propBlock.SetFloat("_Offset", step);
            material.SetPropertyBlock(propBlock);
            yield return new WaitForSeconds(0.001f);
        }
    }

}
