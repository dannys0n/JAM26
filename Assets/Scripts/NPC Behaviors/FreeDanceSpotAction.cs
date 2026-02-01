using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Free ActivitySpot", story: "Free [ActivitySpot]", category: "Action", id: "4e752bb5eef4d7d5c30843090a2fb2d1")]
public partial class FreeActivitySpotAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> ActivitySpot;

    protected override Status OnStart()
    {
        if (ActivitySpot.Value == null)
        {
            //Debug.LogError("AgentSpot is null in FreeActivitySpotAction.");
            return Status.Success;
        }
        AgentSpot spotComponent = ActivitySpot.Value.GetComponent<AgentSpot>();
        if (spotComponent == null)
        {
            Debug.LogError("AgentSpotSpot component not found on the provided GameObject.");
            return Status.Failure;
        }
        spotComponent.VacateSpot();

        return Status.Success;
    }
}

