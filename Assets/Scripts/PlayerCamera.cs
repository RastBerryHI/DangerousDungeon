using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform player;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float step;

    private void LateUpdate()
    {
        if (player == null) return;
        Vector3 pos = player.transform.position;

        transform.position = Vector3.Lerp(transform.position, new Vector3(pos.x + offset.x, pos.y + offset.y, pos.z + offset.z), step);
    }
}
