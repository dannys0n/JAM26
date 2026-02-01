using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject CreditsScreen;
    private AudioSource menuAudioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreditsScreen.SetActive(false);
        // Get the AudioSource component attached to this GameObject
        menuAudioSource = GetComponent<AudioSource>();

        // Call the AudioManager to play the clip
        // Make sure "MainMenuMusic" matches the 'key' in your AudioAsset library
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySound("MainMenuMusic", menuAudioSource);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Play()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        AudioManager.Instance.StopAllSounds();
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

    public void Credits()
    {
        CreditsScreen.SetActive(!CreditsScreen.activeSelf);
    }
}
