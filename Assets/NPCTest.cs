using UnityEngine;

public class NPCTest : MonoBehaviour
{
    [Header("Patrol Settings")]
    public Transform pointA;
    public Transform pointB;
    public float speed = 2.0f;

    private Vector3 targetPosition;
    private PlayerController pc;
    private Rigidbody2D rb;

    void Start()
    {
        pc = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();

        // Start by moving toward Point B
        if (pointB != null) targetPosition = pointB.position;
    }

    void Update()
    {
        // 1. CHECK CONTROL STATE
        // If the player is currently controlling this entity, 
        // we bail out early and let the PlayerController script take over.
        if (pc != null && pc.IsControlled())
        {
            return;
        }

        // 2. NPC LOGIC (Patrol)
        MoveNPC();
    }

    private void MoveNPC()
    {
        if (pointA == null || pointB == null) return;

        // Move the NPC towards the current target
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Switch targets if we reached the destination
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            targetPosition = targetPosition == pointA.position ? pointB.position : pointA.position;
        }
    }
}