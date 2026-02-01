using UnityEngine;
using Unity.Behavior;

/// <summary>
/// Simple player controller for isometric games using Rigidbody2D.
/// Controlled entity always moves towards the mouse.
/// Can only switch control to entities within switch radius.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Speed at which the player moves when possessed")]
    [SerializeField] private float moveSpeed = 1.0f;

    [Tooltip("Distance from mouse within which the entity stops moving")]
    public float mouseTargetTolerance = 0.5f;

    [Header("Control Settings")]
    [Tooltip("Is this character currently being controlled by the player?")]
    [SerializeField] protected bool isControlled = false;

    [Tooltip("Maximum distance from current controlled entity at which you can switch to another entity")]
    public float switchControlRadius = 2.5f;

    [Header("Feedback Settings")]
    [SerializeField] private ParticleSystem selectionParticles; // Drag child particles here

    protected Rigidbody2D rb;
    private Vector2 movement;

    private void UpdateFeedback()
    {
        // Never show "nearby" feedback for the currently controlled player
        if (isControlled) return;

        PlayerController controlled = GetControlledPlayer();

        // IF NO ONE IS SELECTED: Stop particles and exit
        if (controlled == null)
        {
            if (selectionParticles != null && selectionParticles.isPlaying)
                selectionParticles.Stop();
            return;
        }

        if (selectionParticles == null) return;

        float dist = Vector2.Distance(controlled.transform.position, transform.position);
        bool inRange = dist <= controlled.switchControlRadius;

        // Toggle Particles for proximity (Green for nearby)
        if (inRange && !selectionParticles.isPlaying)
        {
            var main = selectionParticles.main;
            main.startColor = Color.green;
            selectionParticles.Play();
        }
        else if (!inRange && selectionParticles.isPlaying)
        {
            selectionParticles.Stop();
        }
    }

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogWarning("Rigidbody2D component not found on " + gameObject.name);
        }
        else
        {
            ConfigureRigidbody2D();
        }
    }

    private void ConfigureRigidbody2D()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
        rb.linearDamping = 0f;
        rb.simulated = true;
    }

    void Update()
    {
        HandleClickSelection();
        UpdateFeedback();

        if (!isControlled)
        {
            movement = Vector2.zero;
            return;
        }

        Vector2 mouseWorldPos = GetMouseWorldPosition();
        if (mouseWorldPos == Vector2.zero)
        {
            movement = Vector2.zero;
            return;
        }

        Vector2 myPos = transform.position;
        float distanceToMouse = Vector2.Distance(myPos, mouseWorldPos);

        movement = distanceToMouse <= mouseTargetTolerance
            ? Vector2.zero
            : (mouseWorldPos - myPos).normalized;
    }

    void FixedUpdate()
    {
        if (rb != null)
            rb.linearVelocity = movement * moveSpeed;
    }

    public bool IsControlled() => isControlled;

    public void SetControlled(bool controlled)
    {
        isControlled = controlled;

        // Feedback: Particles
        if (selectionParticles != null)
        {
            var main = selectionParticles.main;
            if (controlled)
            {
                main.startColor = Color.cyan; // Active player color
                selectionParticles.Play();

                // HARDCODE YOUR AUDIO LOGIC HERE
            }
            else
            {
                selectionParticles.Stop();
            }
        }

        // Component Management (Possession Logic)
        var behaviorAgent = GetComponent<BehaviorGraphAgent>();
        var navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        if (isControlled)
        {
            if (behaviorAgent != null)
            {
                behaviorAgent.enabled = false;
                behaviorAgent.Restart();
            }
            if (navAgent != null) navAgent.enabled = false;
        }
        else
        {
            if (behaviorAgent != null)
            {
                behaviorAgent.enabled = true;
            }
            if (navAgent != null) navAgent.enabled = true;
        }

        var killLogic = FindFirstObjectByType<KillLogic>();
        if (killLogic != null) killLogic.PlayerJumped(this);
    }

    public static PlayerController GetControlledPlayer()
    {
        PlayerController[] players = FindObjectsByType<PlayerController>(FindObjectsSortMode.None);
        foreach (PlayerController player in players)
        {
            if (player.isControlled) return player;
        }
        return null;
    }

    private static Vector2 GetMouseWorldPosition()
    {
        Camera cam = Camera.main;
        if (cam == null) return Vector2.zero;

        Vector3 screen = Input.mousePosition;
        screen.z = -cam.transform.position.z;
        Vector3 world = cam.ScreenToWorldPoint(screen);
        return new Vector2(world.x, world.y);
    }

    public static void HandleClickSelection()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        Vector2 mouseWorldPos = GetMouseWorldPosition();
        Collider2D hitCollider = Physics2D.OverlapPoint(mouseWorldPos);
        if (hitCollider == null) return;

        PlayerController clickedPlayer = hitCollider.GetComponentInParent<PlayerController>();
        if (clickedPlayer == null) return;

        PlayerController currentlyControlled = GetControlledPlayer();

        // Only allow switching if we are in range of the current possessor
        if (currentlyControlled != null && clickedPlayer != currentlyControlled)
        {
            float dist = Vector2.Distance(currentlyControlled.transform.position, clickedPlayer.transform.position);
            if (dist > currentlyControlled.switchControlRadius) return;
        }

        // De-select everyone else
        foreach (PlayerController player in FindObjectsByType<PlayerController>(FindObjectsSortMode.None))
        {
            player.SetControlled(false);
        }

        clickedPlayer.SetControlled(true);
    }
}