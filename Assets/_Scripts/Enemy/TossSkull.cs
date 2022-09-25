using UnityEngine;

public class TossSkull : MonoBehaviour
{
    private Rigidbody rb;
    public SkeletonBoss owner;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.rotation = Random.rotationUniform;
    }

    private void FixedUpdate()
    {
        if (owner == null) return;
        rb.AddForce(owner.transform.forward, ForceMode.Impulse);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log(hit);
    }
}
