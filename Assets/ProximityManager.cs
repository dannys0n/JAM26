using UnityEngine;

public class ProximityManager : MonoBehaviour
{
    public GameObject activePlayer;
    public float switchRadius = 3.0f;
    public LayerMask npcLayer;
    public ParticleSystem rangeVisualizer; // Drag your circle particles here

    void Update()
    {
        if (activePlayer == null) return;

        // Move the range visualizer to follow the active player
        if (rangeVisualizer != null)
        {
            rangeVisualizer.transform.position = activePlayer.transform.position;
        }

        HandleProximity();
    }

    void HandleProximity()
    {
        // Find all NPCs within range using Physics2D
        Collider2D[] nearby = Physics2D.OverlapCircleAll(activePlayer.transform.position, switchRadius, npcLayer);
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        foreach (var col in nearby)
        {
            if (col.gameObject == activePlayer) continue;

            // Simple Distance/Hover check
            if (col.OverlapPoint(mousePos))
            {
                // TODO: Set feedback to Yellow/Hover
                if (Input.GetMouseButtonDown(0))
                {
                    SwitchTo(col.gameObject);
                }
            }
        }
    }

    void SwitchTo(GameObject newNPC)
    {
        activePlayer = newNPC;
        // Logic for your audio pivot: Play switch SFX here!
    }
}