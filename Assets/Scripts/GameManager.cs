using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private AudioSource myAudioSource;
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        myAudioSource = GetComponent<AudioSource>();

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
        AudioManager.Instance.PlaySound("Music", myAudioSource);
    }
    private void OnEnable()
    {
        // This tells Unity: "When a scene loads, please run my OnSceneLoaded method"
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // This runs every time a scene finishes loading
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // If the music isn't playing (which happens after StopAll or a Reset), play it!
        if (myAudioSource != null && !myAudioSource.isPlaying)
        {
            AudioManager.Instance.PlaySound("Music", myAudioSource);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("CJTestScene2");
        }
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            ResetScene();
        }
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            AudioManager.Instance.StopAllSounds();
        }
        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            AudioManager.Instance.PlaySound("Test", myAudioSource);
        }
    }
    public void ResetScene()
    {
        // Gets the index of the currently active scene and reloads it
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
