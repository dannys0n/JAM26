using System;
using System.Collections.Generic;
using UnityEngine;

public class DanceFloor : MonoBehaviour
{
    public int availableSpots = 0;
    public List<DanceSpot> danceSpots = new List<DanceSpot>();

    void Awake()
    {
        foreach (DanceSpot obj in GetComponentsInChildren<DanceSpot>())
        {
            DanceSpot spot = obj.GetComponent<DanceSpot>();
            if (spot != null)
            {
                danceSpots.Add(spot);
                spot.parentFloor = this;
                ++availableSpots;
            }
        }
    }
    public bool ReserveSpot(out DanceSpot spot)
    {
        spot = null;

        // Check if any spots are available
        if (availableSpots <= 0)
        {
            return false;
        }

        // Find available spot and reserve it
        foreach (DanceSpot danceSpot in danceSpots)
        {
            if (!danceSpot.isOccupied)
            {
                danceSpot.ReserveSpot();
                spot = danceSpot;
                --availableSpots;
                return true;
            }
        }
        return false;
    }

    internal void NotifySpotVacated()
    {
        ++availableSpots;
    }
}
