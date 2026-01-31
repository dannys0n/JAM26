using UnityEngine;

/// <summary>
/// Camera controller for isometric games.
/// Follows a target GameObject and supports zoom with scroll wheel.
/// Can switch between different targets at runtime.
/// </summary>
public class CameraController : MonoBehaviour
{
    [Header("Target Settings")]
    [Tooltip("The GameObject the camera should follow")]
    [SerializeField] private Transform target;
    
    [Tooltip("Automatically find and follow the PlayerController with isControlled = true")]
    [SerializeField] private bool autoFindControlledPlayer = true;
    
    [Header("Follow Settings")]
    [Tooltip("How smoothly the camera follows the target (0 = instant, 1 = very smooth)")]
    [SerializeField] private float followSmoothness = 0.1f;
    
    [Tooltip("Offset from the target position")]
    [SerializeField] private Vector3 offset = Vector3.zero;
    
    [Tooltip("How much the camera lerps toward the mouse position (0 = none, 1 = full)")]
    [SerializeField] [Range(0f, 1f)] private float mouseLookAhead = 0.3f;
    
    [Tooltip("Max distance from target that mouse can pull the camera (world units)")]
    [SerializeField] private float mouseLookAheadMaxDistance = 5f;
    
    [Header("Zoom Settings")]
    [Tooltip("Current zoom level (orthographic size for 2D cameras)")]
    [SerializeField] private float zoomLevel = 5f;
    
    [Tooltip("Minimum zoom level")]
    [SerializeField] private float minZoom = 2f;
    
    [Tooltip("Maximum zoom level")]
    [SerializeField] private float maxZoom = 10f;
    
    [Tooltip("How fast the zoom changes with scroll wheel")]
    [SerializeField] private float zoomSpeed = 2f;
    
    [Tooltip("How smoothly the zoom transitions")]
    [SerializeField] private float zoomSmoothness = 0.1f;
    
    // Reference to the Camera component
    private Camera cam;
    
    // Target zoom level (for smooth zooming)
    private float targetZoom;
    
    // Current velocity for smooth damping
    private Vector3 velocity = Vector3.zero;

    /// <summary>
    /// Called once at the start of the game.
    /// Initializes camera references and sets initial zoom.
    /// </summary>
    void Start()
    {
        // Get the Camera component attached to this GameObject
        cam = GetComponent<Camera>();
        
        // If no Camera is found, log a warning
        if (cam == null)
        {
            Debug.LogWarning("Camera component not found on " + gameObject.name);
        }
        
        // Initialize target zoom to current zoom level
        targetZoom = zoomLevel;
        
        // Set initial camera orthographic size (for 2D/isometric cameras)
        if (cam != null && cam.orthographic)
        {
            cam.orthographicSize = zoomLevel;
        }
    }

    /// <summary>
    /// Called every frame after Update.
    /// Handles camera following and zoom input.
    /// </summary>
    void LateUpdate()
    {
        // Handle zoom input from scroll wheel
        HandleZoom();
        
        // If auto-find is enabled, check for controlled player
        if (autoFindControlledPlayer)
        {
            UpdateControlledPlayerTarget();
        }
        
        // Follow the target if one is assigned
        if (target != null)
        {
            FollowTarget();
        }
    }

    /// <summary>
    /// Handles zoom input from the mouse scroll wheel.
    /// Updates the target zoom level within min/max bounds.
    /// </summary>
    private void HandleZoom()
    {
        // Get scroll wheel input
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        
        // If scroll input is detected, adjust target zoom
        if (scrollInput != 0f)
        {
            // Decrease zoom (zoom in) when scrolling up
            // Increase zoom (zoom out) when scrolling down
            targetZoom -= scrollInput * zoomSpeed;
            
            // Clamp zoom between min and max values
            targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
        }
        
        // Smoothly interpolate current zoom to target zoom
        zoomLevel = Mathf.Lerp(zoomLevel, targetZoom, zoomSmoothness * 10f * Time.deltaTime);
        
        // Apply zoom to camera (for orthographic cameras)
        if (cam != null && cam.orthographic)
        {
            cam.orthographicSize = zoomLevel;
        }
    }

    /// <summary>
    /// Smoothly follows the target GameObject, lerping in the direction of the mouse.
    /// Uses SmoothDamp for smooth camera movement.
    /// </summary>
    private void FollowTarget()
    {
        Vector3 desiredPosition = target.position + offset;
        
        // Lerp camera toward mouse position
        if (cam != null && mouseLookAhead > 0.001f)
        {
            Vector2 mouseWorld = GetMouseWorldPosition();
            Vector2 targetFlat = new Vector2(target.position.x, target.position.y);
            Vector2 toMouse = mouseWorld - targetFlat;
            float dist = toMouse.magnitude;
            if (dist > 0.001f)
            {
                float pull = Mathf.Min(dist, mouseLookAheadMaxDistance) * mouseLookAhead;
                desiredPosition.x += toMouse.normalized.x * pull;
                desiredPosition.y += toMouse.normalized.y * pull;
            }
        }
        
        desiredPosition.z = transform.position.z;
        
        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref velocity,
            followSmoothness
        );
    }

    /// <summary>
    /// Gets the mouse position in 2D world space using the camera.
    /// </summary>
    private Vector2 GetMouseWorldPosition()
    {
        if (cam == null) return new Vector2(transform.position.x, transform.position.y);
        Vector3 screen = Input.mousePosition;
        screen.z = -cam.transform.position.z;
        Vector3 world = cam.ScreenToWorldPoint(screen);
        return new Vector2(world.x, world.y);
    }

    /// <summary>
    /// Sets a new target for the camera to follow.
    /// Can be called from other scripts to switch targets.
    /// </summary>
    /// <param name="newTarget">The new GameObject to follow</param>
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    /// <summary>
    /// Sets a new target for the camera to follow using GameObject.
    /// Can be called from other scripts to switch targets.
    /// </summary>
    /// <param name="newTarget">The new GameObject to follow</param>
    public void SetTarget(GameObject newTarget)
    {
        if (newTarget != null)
        {
            target = newTarget.transform;
        }
    }

    /// <summary>
    /// Gets the current target the camera is following.
    /// </summary>
    /// <returns>The current target Transform, or null if no target is set</returns>
    public Transform GetTarget()
    {
        return target;
    }

    /// <summary>
    /// Sets the zoom level directly.
    /// </summary>
    /// <param name="zoom">The new zoom level (will be clamped to min/max)</param>
    public void SetZoom(float zoom)
    {
        targetZoom = Mathf.Clamp(zoom, minZoom, maxZoom);
    }

    /// <summary>
    /// Gets the current zoom level.
    /// </summary>
    /// <returns>The current zoom level</returns>
    public float GetZoom()
    {
        return zoomLevel;
    }

    /// <summary>
    /// Checks for a PlayerController with isControlled = true and switches to it.
    /// Called automatically if autoFindControlledPlayer is enabled.
    /// </summary>
    private void UpdateControlledPlayerTarget()
    {
        // Find the currently controlled player
        PlayerController controlledPlayer = PlayerController.GetControlledPlayer();
        
        // If a controlled player is found and it's different from current target
        if (controlledPlayer != null)
        {
            // Check if we need to switch targets
            if (target != controlledPlayer.transform)
            {
                // Switch to the controlled player
                target = controlledPlayer.transform;
            }
        }
        // If no controlled player is found and we have a target, keep following it
        // (This allows manual target assignment to still work)
    }

}
