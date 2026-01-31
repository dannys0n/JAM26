using UnityEngine;

/// <summary>
/// Simple player controller for isometric games using Rigidbody2D.
/// Controlled entity always moves towards the mouse. Can only switch control to entities within switch radius.
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
    public float switchControlRadius = 3f;
    
    // Reference to the Rigidbody2D component
    protected Rigidbody2D rb;
    
    // Movement vector (towards mouse when controlled)
    private Vector2 movement;

    /// <summary>
    /// Called once at the start of the game.
    /// Gets the Rigidbody2D component reference and configures it for proper collision.
    /// </summary>
    protected virtual void Start()
    {
        // Get the Rigidbody2D component attached to this GameObject
        rb = GetComponent<Rigidbody2D>();
        
        // If no Rigidbody2D is found, log a warning
        if (rb == null)
        {
            Debug.LogWarning("Rigidbody2D component not found on " + gameObject.name);
        }
        else
        {
            // Configure Rigidbody2D for isometric game with collision
            ConfigureRigidbody2D();
        }
    }

    /// <summary>
    /// Configures the Rigidbody2D settings for proper collision and movement.
    /// Recommended settings for isometric games with player-to-player collision.
    /// </summary>
    private void ConfigureRigidbody2D()
    {
        // Set body type to Dynamic for physics-based collision
        rb.bodyType = RigidbodyType2D.Dynamic;
        
        // Disable gravity for top-down/isometric games
        rb.gravityScale = 0f;
        
        // Freeze rotation to prevent unwanted spinning
        rb.freezeRotation = true;
        
        // Set collision detection (Discrete is fine for most cases, use Continuous for fast objects)
        rb.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
        
        // Set drag to 0 for instant stop when input stops (or use small value for gradual stop)
        rb.linearDamping = 0f;
        
        // Ensure the rigidbody is simulated
        rb.simulated = true;
    }

    /// <summary>
    /// Called every frame.
    /// Moves controlled entity towards mouse position (stops when within tolerance).
    /// </summary>
    void Update()
    {
        // Handle click selection (only needs to be called once, but safe to call from any PlayerController)
        HandleClickSelection();
        
        if (isControlled)
        {
            Vector2 mouseWorldPos = GetMouseWorldPosition();
            if (mouseWorldPos != Vector2.zero)
            {
                Vector2 myPos = transform.position;
                float distanceToMouse = Vector2.Distance(myPos, mouseWorldPos);
                if (distanceToMouse <= mouseTargetTolerance)
                {
                    movement = Vector2.zero;
                }
                else
                {
                    movement = (mouseWorldPos - myPos).normalized;
                }
            }
            else
            {
                movement = Vector2.zero;
            }
        }
        else
        {
            movement = Vector2.zero;
        }
    }

    /// <summary>
    /// Gets the mouse position in 2D world space (Z = 0).
    /// </summary>
    private static Vector2 GetMouseWorldPosition()
    {
        Camera cam = Camera.main;
        if (cam == null) return Vector2.zero;
        Vector3 screen = Input.mousePosition;
        screen.z = -cam.transform.position.z;
        Vector3 world = cam.ScreenToWorldPoint(screen);
        return new Vector2(world.x, world.y);
    }

    /// <summary>
    /// Called at fixed intervals (tied to physics update).
    /// Applies movement to the Rigidbody2D.
    /// </summary>
    void FixedUpdate()
    {
        // Apply movement using velocity
        // This provides smooth physics-based movement
        rb.linearVelocity = movement * moveSpeed;
    }

    /// <summary>
    /// Gets whether this character is currently being controlled.
    /// </summary>
    /// <returns>True if this character is controlled, false otherwise</returns>
    public bool IsControlled()
    {
        return isControlled;
    }

    /// <summary>
    /// Sets whether this character is currently being controlled.
    /// </summary>
    /// <param name="controlled">True to set as controlled, false otherwise</param>
    public void SetControlled(bool controlled)
    {
        isControlled = controlled;
    }

    /// <summary>
    /// Finds the PlayerController that is currently being controlled.
    /// </summary>
    /// <returns>The controlled PlayerController, or null if none is found</returns>
    public static PlayerController GetControlledPlayer()
    {
        // Find all PlayerController instances in the scene
        PlayerController[] players = FindObjectsByType<PlayerController>(FindObjectsSortMode.None);
        
        // Loop through all players to find the one that is controlled
        foreach (PlayerController player in players)
        {
            if (player.isControlled)
            {
                return player;
            }
        }
        
        // No controlled player found
        return null;
    }

    /// <summary>
    /// Moves this character to the camera's position.
    /// </summary>
    public void MoveToCameraPosition()
    {
        Camera cam = Camera.main;
        if (cam != null)
        {
            // Get camera position and set Z to 0 for 2D
            Vector3 cameraPos = cam.transform.position;
            cameraPos.z = 0f;
            
            // Move this character to camera position
            transform.position = cameraPos;
            
            // Reset velocity if rigidbody exists
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
    }


    /// <summary>
    /// Handles mouse clicks to select and control PlayerController objects.
    /// Can only switch to an entity within switchControlRadius of the currently controlled entity.
    /// </summary>
    public static void HandleClickSelection()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        Camera cam = Camera.main;
        if (cam == null) return;

        Vector2 mouseWorldPos = GetMouseWorldPosition();
        Collider2D hitCollider = Physics2D.OverlapPoint(mouseWorldPos);

        if (hitCollider == null) return;

        PlayerController clickedPlayer = hitCollider.GetComponent<PlayerController>();
        if (clickedPlayer == null) return;

        PlayerController currentlyControlled = GetControlledPlayer();
        if (currentlyControlled != null && clickedPlayer != currentlyControlled)
        {
            float dist = Vector2.Distance(currentlyControlled.transform.position, clickedPlayer.transform.position);
            if (dist > currentlyControlled.switchControlRadius)
                return;
        }

        foreach (PlayerController player in FindObjectsByType<PlayerController>(FindObjectsSortMode.None))
        {
            player.SetControlled(false);
        }

        clickedPlayer.SetControlled(true);
    }
}
