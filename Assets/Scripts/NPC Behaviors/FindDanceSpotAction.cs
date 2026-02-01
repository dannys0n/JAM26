using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Find Dance Spot", story: "Agent finds [DanceSpot]", category: "Action", id: "932f0d5b71ec82212bd987a8d6819f67")]
public partial class FindDanceSpotAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> DanceSpot;
    protected override Status OnStart()
    {
        DanceFloor danceFloor = GameObject.FindFirstObjectByType<DanceFloor>();
        DanceSpot ds = null;
        if (danceFloor.ReserveSpot(out ds))
        {
            DanceSpot.Value = ds.gameObject;
            return Status.Success;
        }
        return Status.Failure;
    }
}

