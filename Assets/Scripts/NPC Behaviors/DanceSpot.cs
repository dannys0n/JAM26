using UnityEngine;

public class DanceSpot : AgentSpot
{
    public DanceFloor parentFloor;

    protected override void OnVacate()
    {
        parentFloor.NotifySpotVacated();
    }

}
