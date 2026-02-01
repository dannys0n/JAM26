using UnityEngine;
using System.Collections.Generic;

public class PartyManager : MonoBehaviour
{
    AgentSpot[] activitySpots;
    
    void Awake()
    {
        activitySpots = GameObject.FindObjectsByType<AgentSpot>(FindObjectsSortMode.None);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public bool ReserveSpot(out AgentSpot spot)
    {
        spot = null;

        // Find available spot and reserve it
        foreach (AgentSpot aspot in activitySpots)
        {
            if (!aspot.isOccupied)
            {
                aspot.ReserveSpot();
                spot = aspot;
                return true;
            }
        }
        return false;
    }

}
