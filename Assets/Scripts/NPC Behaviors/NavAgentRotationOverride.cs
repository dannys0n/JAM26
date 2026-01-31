using UnityEngine;
using UnityEngine.AI;

public class NavAgentRotationOverride : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }
        else
        {
            Debug.LogWarning("NavMeshAgent component not found on this GameObject.");
        }
    }
}
