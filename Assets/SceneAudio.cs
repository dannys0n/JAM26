using UnityEngine;

public class SceneAudio : MonoBehaviour
{
    [Header("Music & Ambience Settings")]
    [SerializeField] private string sceneMusicKey;
    [SerializeField] private string ambienceKey = "Ambience_01_Loop";

    [Header("Emitters")]
    [Tooltip("Attach an AudioSource located on this object or the AudioManager")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource ambienceSource;

    void Start()
    {
        if (AudioManager.Instance == null) return;

        // Play the specific music for this scene
        if (!string.IsNullOrEmpty(sceneMusicKey))
        {
            AudioManager.Instance.PlaySound(sceneMusicKey, musicSource);
        }
        else
        {
            print("key didnt work");
        }

        // Play the global ambience
        if (!string.IsNullOrEmpty(ambienceKey))
        {
            AudioManager.Instance.PlaySound(ambienceKey, ambienceSource);
        }
        
    }
}