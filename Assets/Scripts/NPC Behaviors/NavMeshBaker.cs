using UnityEngine;
using Unity.AI.Navigation;
public class NavMeshBaker : MonoBehaviour
{
    private NavMeshSurface navMeshSurface;

    void Start()
    {
        // Get the NavMeshSurface component attached to this GameObject
        navMeshSurface = GetComponent<NavMeshSurface>();

        if (navMeshSurface != null)
        {
            // Bake the NavMesh at runtime
            BakeNavMesh();
        }
        else
        {
            Debug.LogError("NavMeshSurface component not found!");
        }
    }

    public void BakeNavMesh()
    {
        navMeshSurface.BuildNavMesh();
        Debug.Log("NavMesh baked at runtime.");
    }

}
