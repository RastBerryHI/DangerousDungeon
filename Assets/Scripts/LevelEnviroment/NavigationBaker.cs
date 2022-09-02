using UnityEngine.AI;
using UnityEngine;

public class NavigationBaker : MonoBehaviour
{
    public NavMeshSurface surface;

    [ContextMenu("BakeNavMesh")]
    public void BakeNavMesh()
    {
        surface.BuildNavMesh();
    }
}
