using UnityEngine;
using UnityEngine.Events;

public class GameEvents : MonoBehaviour
{
    [Header("Player Events")]
    public UnityEvent PlayerJumped;
    public UnityEvent KillSubmitted;

    [Header("UI Events")]
    public UnityEvent ButtonPressed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
