using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Rigidbody rb;
    private new Renderer renderer;
    private MaterialPropertyBlock propBlock;

    [Tooltip("Step of changing dissolve between -1...1 every 0.02 seconds.")]
    [SerializeField] private float dissolveStep;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        renderer = GetComponent<Renderer>();
        propBlock = new MaterialPropertyBlock();

        rb.isKinematic = true;
        LevelGenerator.s_levelGenerator.onUnitCleared.AddListener(ActivateDoor);
    }

    private void ActivateDoor()
    {
        Destroy(GetComponent<Collider>());
        Destroy(GetComponent<Rigidbody>());

        StartCoroutine(StartDissolve());
        Destroy(gameObject, 5f);
    }

    private IEnumerator<WaitForSeconds> StartDissolve()
    {
        float treshold = -1f;
        Debug.Log(treshold);

        propBlock.SetFloat("_AlphaClipThreshold", treshold += 0.1f);
        renderer.SetPropertyBlock(propBlock);

        while(treshold < 1)
        {
            yield return new WaitForSeconds(0.02f);
            propBlock.SetFloat("_AlphaClipThreshold", treshold += dissolveStep);
            renderer.SetPropertyBlock(propBlock);
        }
    }
}
