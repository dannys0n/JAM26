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

        if (activitySpots == null || activitySpots.Length == 0)
            return false;

        // Gather all available spots
        List<AgentSpot> available = new List<AgentSpot>(activitySpots.Length);
        foreach (AgentSpot aspot in activitySpots)
        {
            if (!aspot.isOccupied)
                available.Add(aspot);
        }

        if (available.Count == 0)
            return false;

        // Pick a random available spot and reserve it
        int index = Random.Range(0, available.Count);
        AgentSpot chosen = available[index];
        chosen.ReserveSpot();
        spot = chosen;
        return true;
    }

}
