using UnityEngine;

public class MainMenu : MonoBehaviour
{
  public GameObject CreditsScreen;

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    CreditsScreen.SetActive(false);
	}

  // Update is called once per frame
  void Update()
  {

  }

	public void Play()
  {
    UnityEngine.SceneManagement.SceneManager.LoadScene("LevelOne");
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
