using UnityEngine;

public class PlayerPockets : MonoBehaviour
{
    [SerializeField] private Transform artifactPocket;
    [SerializeField] private Transform wonderPocket;
    
    public Transform ArtifactPocket => artifactPocket;
    public Transform WonderPocket => wonderPocket;
}
