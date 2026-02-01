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

    public void PlayLevel1CorrectGuess()
    {
        Play("UI_CorrectGuess_1");
    }
    public void PlayLevel2CorrectGuess()
    {
        Play("UI_CorrectGuess_2");
    }
    public void PlayLevel3CorrectGuess()
    {
        Play("UI_CorrectGuess_3");
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