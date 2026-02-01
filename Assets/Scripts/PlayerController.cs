using UnityEngine;
using Unity.Behavior;
using System.Collections;         // Required for IEnumerator
using System.Collections.Generic; // Required for List<>
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
    [Header("Audio Settings")]
    [Tooltip("The key for the initial body transfer sound in AudioManager")]
    [SerializeField] private string transferSfxKey = "SFX_TransferBody";

    [Tooltip("List of keys for reaction sounds in AudioManager")]
    [SerializeField] private string[] reactionSfxKeys;

    [Header("Footstep Settings")]
    [SerializeField] private string[] footstepSfxKeys;
    [SerializeField] private float footstepInterval = 0.4f; // Time between steps
    private float footstepTimer;

    private static List<int> footstepHistory = new List<int>();
    private const int FOOTSTEP_HISTORY_LIMIT = 2; // Don't repeat last 2 steps
    private static List<int> reactionHistory = new List<int>();
    private const int HISTORY_LIMIT = 3; // Avoid repeating the last 3 sounds
    private void HandleFootsteps()
    {
        // Only play footsteps if we are actually moving and controlled
        if (!isControlled || movement == Vector2.zero) return;

        footstepTimer -= Time.deltaTime;

        if (footstepTimer <= 0)
        {
            PlayRandomFootstep();
            // Adjust interval based on speed if you want (e.g., footstepInterval / moveSpeed)
            footstepTimer = footstepInterval;
        }
    }

    private void PlayRandomFootstep()
    {
        if (footstepSfxKeys == null || footstepSfxKeys.Length == 0 || AudioManager.Instance == null) return;

        int index;
        // Prevent repeating the last 2 sounds
        do
        {
            index = Random.Range(0, footstepSfxKeys.Length);
        } while (footstepHistory.Contains(index));

        footstepHistory.Add(index);
        if (footstepHistory.Count > FOOTSTEP_HISTORY_LIMIT) footstepHistory.RemoveAt(0);

        AudioSource mySource = GetComponent<AudioSource>();
        if (mySource != null)
        {
            // Use the AudioManager to play the selected footstep key
            AudioManager.Instance.PlaySound(footstepSfxKeys[index], mySource);
        }
    }
    private void PlayPossessionAudio()
    {
        if (AudioManager.Instance == null) return;

        AudioSource mySource = GetComponent<AudioSource>();
        if (mySource == null) return;

        // 1. Play the transfer sound immediately
        AudioManager.Instance.PlaySound(transferSfxKey, mySource);

        // 2. Pick a random reaction that hasn't played recently
        if (reactionSfxKeys != null && reactionSfxKeys.Length > 0)
        {
            string randomReaction = GetRandomReactionKey();

            // Use a Coroutine or Invoke if you want a slight delay between 
            // the transfer and the reaction, otherwise play sequence:
            StartCoroutine(PlayReactionSequence(mySource, randomReaction));
        }
    }

    private string GetRandomReactionKey()
    {
        if (reactionSfxKeys.Length <= HISTORY_LIMIT) return reactionSfxKeys[Random.Range(0, reactionSfxKeys.Length)];

        int index;
        do
        {
            index = Random.Range(0, reactionSfxKeys.Length);
        } while (reactionHistory.Contains(index));

        reactionHistory.Add(index);
        if (reactionHistory.Count > HISTORY_LIMIT) reactionHistory.RemoveAt(0);

        return reactionSfxKeys[index];
    }

    // Use System.Collections.IEnumerator to avoid the "requires 1 type arguments" error
    private System.Collections.IEnumerator PlayReactionSequence(AudioSource source, string reactionKey)
    {
        // Increase this to 0.5f or 1.0f depending on how long SFX_TransferBody is
        yield return new WaitForSeconds(0.6f);

        if (AudioManager.Instance != null && source != null)
        {
            AudioManager.Instance.PlaySound(reactionKey, source);
        }
    }
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
        HandleFootsteps();
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
                main.startColor = Color.cyan;
                selectionParticles.Play();

                // CALL THE AUDIO LOGIC HERE:
                PlayPossessionAudio();
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
            if (player.isControlled)
            {
                player.SetControlled(false);
            }
        }

        clickedPlayer.SetControlled(true);
    }
}