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
    public float switchControlRadius = 5f;
    [Header("Outline Shader")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    private MaterialPropertyBlock mpb;
    private static readonly int OutlineEnabledID =
    Shader.PropertyToID("_OutlineEnabled");


    protected Rigidbody2D rb;
    private Vector2 movement;
private void SetOutline(bool enabled)
{
    if (spriteRenderer == null) return;

    spriteRenderer.GetPropertyBlock(mpb);
    mpb.SetFloat(OutlineEnabledID, enabled ? 1f : 0f);
    spriteRenderer.SetPropertyBlock(mpb);
}
private void UpdateOutline()
{
    // Never outline the currently controlled player
    if (isControlled)
    {
        SetOutline(false);
        return;
    }

    PlayerController controlled = GetControlledPlayer();
    if (controlled == null)
    {
        SetOutline(false);
        return;
    }

    float dist = Vector2.Distance(
        controlled.transform.position,
        transform.position
    );

    SetOutline(dist <= controlled.switchControlRadius);
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
        if (spriteRenderer == null)
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogWarning("SpriteRenderer component not found on " + gameObject.name);
        }
        else
        {
            mpb = new MaterialPropertyBlock();
            SetOutline(false); // start clean
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
        UpdateOutline();

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

    public bool IsControlled()
    {
        return isControlled;
    }

    public void SetControlled(bool controlled)
    {
        isControlled = controlled;
        
        //I committed this crime CJ is innocent
        if(isControlled)
        {
            GetComponent<BehaviorGraphAgent>().enabled = false;
            GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        }
        else
        {
            GetComponent<BehaviorGraphAgent>().Restart();
            GetComponent<BehaviorGraphAgent>().enabled = true;
            GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
        }

        //FindFirstObjectByType<KillLogic>().PlayerJumped(this);
    }


    public static PlayerController GetControlledPlayer()
    {
        PlayerController[] players =
            FindObjectsByType<PlayerController>(FindObjectsSortMode.None);

        foreach (PlayerController player in players)
        {
            if (player.isControlled)
                return player;
        }

        return null;
    }

    public void MoveToCameraPosition()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        Vector3 cameraPos = cam.transform.position;
        cameraPos.z = 0f;
        transform.position = cameraPos;

        if (rb != null)
            rb.linearVelocity = Vector2.zero;
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

        Camera cam = Camera.main;
        if (cam == null) return;

        Vector2 mouseWorldPos = GetMouseWorldPosition();
        Collider2D hitCollider = Physics2D.OverlapPoint(mouseWorldPos);
        if (hitCollider == null) return;

        PlayerController clickedPlayer =
            hitCollider.GetComponent<PlayerController>() ??
            hitCollider.GetComponentInParent<PlayerController>();

        if (clickedPlayer == null) return;

        PlayerController currentlyControlled = GetControlledPlayer();
        if (currentlyControlled != null && clickedPlayer != currentlyControlled)
        {
            float dist = Vector2.Distance(
                currentlyControlled.transform.position,
                clickedPlayer.transform.position
            );

            if (dist > currentlyControlled.switchControlRadius)
                return;
        }

        foreach (PlayerController player in
            FindObjectsByType<PlayerController>(FindObjectsSortMode.None))
        {
            player.SetControlled(false);
        }

        clickedPlayer.SetControlled(true);
        
    }
}
