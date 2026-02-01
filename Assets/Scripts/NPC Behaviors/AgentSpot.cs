using UnityEngine;

public class AgentSpot : MonoBehaviour
{
    public bool isOccupied { get; protected set; } = false;


    public virtual void ReserveSpot()
    {
        isOccupied = true;
    }
    public void VacateSpot()
    {
        isOccupied = false;
        OnVacate();
    }

    protected virtual void OnVacate()
    {

    }

    // Get animation info for this spot

}
