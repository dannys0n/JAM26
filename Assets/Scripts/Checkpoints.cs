using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoints : MonoBehaviour
{
  static public Checkpoints Instance { get; private set; }
  static private int currentCheckpointScene = 0;

	private void Awake()
	{
	  if (Instance != null && Instance != this)
    {
      Destroy(this.gameObject);
    }
    else
    {
      Instance = this;
      DontDestroyOnLoad(this.gameObject);
    }
	}

  public void SetNewCheckpoint(int sceneIndex)
  {
    currentCheckpointScene = sceneIndex;
	}

  public int GetCurrentCheckpoint()
  {
    return currentCheckpointScene;
  }

  public void TriggerCheckpoint()
  {
    SceneManager.LoadScene(currentCheckpointScene);
	}
}
