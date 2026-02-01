using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Free DanceSpot", story: "Free [DanceSpot]", category: "Action", id: "4e752bb5eef4d7d5c30843090a2fb2d1")]
public partial class FreeDanceSpotAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> DanceSpot;

    protected override Status OnStart()
    {
        if (DanceSpot.Value == null)
        {
            Debug.LogError("DanceSpot is null in FreeDanceSpotAction.");
            return Status.Failure;
        }
        DanceSpot spotComponent = DanceSpot.Value.GetComponent<DanceSpot>();
        if (spotComponent == null)
        {
            Debug.LogError("DanceSpot component not found on the provided GameObject.");
            return Status.Failure;
        }
        spotComponent.VacateSpot();

        return Status.Success;
    }
}

