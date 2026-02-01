using UnityEngine;
using UnityEngine.SceneManagement;

public class UIAudioManager : MonoBehaviour
{
    [Header("Setup")]
    [Tooltip("The AudioSource used for UI sounds. If empty, uses AudioManager's source.")]
    [SerializeField] private AudioSource uiSource;

    void Awake()
    {
        // Ensure we have a source to play from immediately
        if (uiSource == null && AudioManager.Instance != null)
        {
            uiSource = AudioManager.Instance.GetComponent<AudioSource>();
        }
    }

    // --- PUBLIC FUNCTIONS FOR INSPECTOR ---

    public void PlayStandardClick()
    {
        Play("UI_Button_v2");
    }

    public void PlayGameStart()
    {
        Play("UI_Button_GameStart");
    }

    public void PlayMurderButton()
    {
        Play("UI_Button_Murder");
    }

    public void PlayGenericButton()
    {
        Play("UI_Button");
    }

    public void PlayLevelCorrectGuess()
    {
        // Determines Lvl 1, 2, or 3 based on scene name
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName.Contains("1")) Play("UI_CorrectGuess_1");
        else if (sceneName.Contains("2")) Play("UI_CorrectGuess_2");
        else if (sceneName.Contains("3")) Play("UI_CorrectGuess_3");
        else PlayIncorrectGuess();
    }

    public void PlayIncorrectGuess()
    {
        Play("UI_IncorrectGuess");
    }

    // --- INTERNAL HELPER ---

    private void Play(string key)
    {
        if (AudioManager.Instance != null && uiSource != null)
        {
            AudioManager.Instance.PlaySound(key, uiSource);
        }
    }
}