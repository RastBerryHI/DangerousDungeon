using UnityEngine;

public class PhysicsPusher : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddExplosionForce(1700, transform.position, 5);
        }
    }
}
