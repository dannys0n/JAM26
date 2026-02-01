using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Find Activity Spot", story: "Agent finds [ActivitySpot]", category: "Action", id: "932f0d5b71ec82212bd987a8d6819f67")]
public partial class FindActivitySpotAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> ActivitySpot;
    protected override Status OnStart()
    {
        PartyManager party = GameObject.FindFirstObjectByType<PartyManager>();
        AgentSpot spot = null;
        if (party.ReserveSpot(out spot))
        {
            ActivitySpot.Value = spot.gameObject;
            return Status.Success;
        }
        return Status.Failure;
    }
}

