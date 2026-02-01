using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Navigate To Location 2D", story: "[Agent] Navigates to [Location]", category: "Action/Navigation", id: "44b6b3ffeb10b5a693f55b87daa47d34")]
public partial class NavigateToLocation2DAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<Vector2> Location;
    [SerializeReference] public BlackboardVariable<float> Speed = new BlackboardVariable<float>(1.0f);
    [SerializeReference] public BlackboardVariable<float> DistanceThreshold = new BlackboardVariable<float>(0.2f);
    [SerializeReference] public BlackboardVariable<string> AnimatorSpeedParam = new BlackboardVariable<string>("SpeedMagnitude");

    // This will only be used in movement without a navigation agent.
    [SerializeReference] public BlackboardVariable<float> SlowDownDistance = new BlackboardVariable<float>(1.0f);

    private UnityEngine.AI.NavMeshAgent m_NavMeshAgent;
    private Animator m_Animator;
    [CreateProperty] private float m_OriginalStoppingDistance = -1f;
    [CreateProperty] private float m_OriginalSpeed = -1f;
    private float m_CurrentSpeed;

    protected override Status OnStart()
    {
        if (Agent.Value == null || Location.Value == null)
        {
            return Status.Failure;
        }

        return Initialize();
    }

    protected override Status OnUpdate()
    {
        if (Agent.Value == null || Location.Value == null)
        {
            return Status.Failure;
        }

        Vector3 agentPosition, locationPosition;
        float distance = GetDistanceToLocation(out agentPosition, out locationPosition);
        bool destinationReached = distance <= DistanceThreshold;

        if (destinationReached && (m_NavMeshAgent == null || !m_NavMeshAgent.pathPending))
        {
            return Status.Success;
        }
        else if (m_NavMeshAgent == null) // transform-based movement
        {
            m_CurrentSpeed = SimpleMoveTowardsLocation(Agent.Value.transform, locationPosition, Speed, distance, SlowDownDistance);
        }

        UpdateAnimatorSpeed();

        return Status.Running;
    }

    protected override void OnEnd()
    {
        UpdateAnimatorSpeed(0f);

        if (m_NavMeshAgent != null)
        {
            if (m_NavMeshAgent.isOnNavMesh)
            {
                m_NavMeshAgent.ResetPath();
            }
            m_NavMeshAgent.speed = m_OriginalSpeed;
            m_NavMeshAgent.stoppingDistance = m_OriginalStoppingDistance;
        }

        m_NavMeshAgent = null;
        m_Animator = null;
    }

    protected override void OnDeserialize()
    {
        // If using a navigation mesh, we need to reset default value before Initialize.
        m_NavMeshAgent = Agent.Value.GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
        if (m_NavMeshAgent != null)
        {
            if (m_OriginalSpeed >= 0f)
                m_NavMeshAgent.speed = m_OriginalSpeed;
            if (m_OriginalStoppingDistance >= 0f)
                m_NavMeshAgent.stoppingDistance = m_OriginalStoppingDistance;

            m_NavMeshAgent.Warp(Agent.Value.transform.position);
        }

        Initialize();
    }

    private Status Initialize()
    {
        if (GetDistanceToLocation(out Vector3 agentPosition, out Vector3 locationPosition) <= DistanceThreshold)
        {
            return Status.Success;
        }

        // If using a navigation mesh, set target position for navigation mesh agent.
        m_NavMeshAgent = Agent.Value.GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
        if (m_NavMeshAgent != null)
        {
            if (m_NavMeshAgent.isOnNavMesh)
            {
                m_NavMeshAgent.ResetPath();
            }

            m_OriginalSpeed = m_NavMeshAgent.speed;
            m_NavMeshAgent.speed = Speed;
            m_OriginalStoppingDistance = m_NavMeshAgent.stoppingDistance;
            m_NavMeshAgent.stoppingDistance = DistanceThreshold;
            m_NavMeshAgent.SetDestination(locationPosition);
        }

        m_Animator = Agent.Value.GetComponentInChildren<Animator>();
        UpdateAnimatorSpeed(0f);

        return Status.Running;
    }

    private float GetDistanceToLocation(out Vector3 agentPosition, out Vector3 locationPosition)
    {
        agentPosition = Agent.Value.transform.position;
        locationPosition = Location.Value;
        return Vector3.Distance(new Vector3(agentPosition.x, agentPosition.y, locationPosition.z), locationPosition);
    }

    private void UpdateAnimatorSpeed(float explicitSpeed = -1)
    {
        UpdateAnimatorSpeed(m_Animator, AnimatorSpeedParam, m_NavMeshAgent, m_CurrentSpeed, explicitSpeed: explicitSpeed);
    }

    private static float SimpleMoveTowardsLocation(Transform agentTransform, Vector3 targetLocation, float speed, float distance, float slowDownDistance = 0.0f,
            float minSpeedRatio = 0.1f)
    {
        if (agentTransform == null)
        {
            return 0f;
        }

        Vector3 agentPosition = agentTransform.position;
        float movementSpeed = speed;

        // Slowdown
        if (slowDownDistance > 0.0f && distance < slowDownDistance)
        {
            float ratio = distance / slowDownDistance;
            movementSpeed = Mathf.Max(speed * minSpeedRatio, speed * ratio);
        }

        Vector3 toDestination = targetLocation - agentPosition;
        toDestination.y = 0.0f;

        if (toDestination.sqrMagnitude > 0.0001f)
        {
            toDestination.Normalize();

            // Apply movement
            agentPosition += toDestination * (movementSpeed * Time.deltaTime);
            agentTransform.position = agentPosition;

            // Look at the target
            agentTransform.forward = toDestination;
        }

        return movementSpeed;
    }

    private static bool UpdateAnimatorSpeed(Animator animator, string speedParameterName, NavMeshAgent navMeshAgent, float currentSpeed, float minSpeedThreshold = 0.1f,
            float explicitSpeed = -1f)
    {
        if (animator == null || string.IsNullOrEmpty(speedParameterName))
        {
            return false;
        }

        float speedValue = 0;
        if (explicitSpeed >= 0)
        {
            speedValue = explicitSpeed;
        }
        else if (navMeshAgent != null)
        {
            speedValue = navMeshAgent.velocity.magnitude;
        }
        else
        {
            speedValue = currentSpeed;
        }

        if (speedValue <= minSpeedThreshold)
        {
            speedValue = 0;
        }

        animator.SetFloat(speedParameterName, speedValue);
        return true;
    }

}

