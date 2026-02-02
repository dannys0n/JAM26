using UnityEngine;

public class ProximityPlayer : MonoBehaviour
{
    [SerializeField] private string soundKey = "SFX_Fire_Loop";
    private AudioSource mySource;

    void Start()
    {
        mySource = GetComponent<AudioSource>();

        // Wait for the end of the frame to ensure AudioManager is initialized
        Invoke("PlayOnStart", 0.1f);
    }

    void PlayOnStart()
    {
        if (AudioManager.Instance != null && mySource != null)
        {
            AudioManager.Instance.PlaySound(soundKey, mySource);
        }
    }
}