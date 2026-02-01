using UnityEngine;

public class PlayerRating : MonoBehaviour
{
  private int correctGuesses = 0;

  public static PlayerRating instance;
  public float timeSpent = 0f;

  private void Awake()
  {
    if (instance == null)
    {
      instance = this;
      DontDestroyOnLoad(gameObject);
    }
    else
    {
      Destroy(gameObject);
    }
  }

  public static void IncrementCorrectGuess()
  {
		instance.correctGuesses++;
  }

  public static int GetCorrectGuesses()
  {
    return instance.correctGuesses;
  }

  public static float GetTotalTimeSpent()
  {
    return instance.timeSpent;
	}

	public static void AddTimeSpent(float _timeSpent)
  {
    instance.timeSpent += _timeSpent;
  }

  public static float CalculateScore()
  {
    if (instance.timeSpent == 0f)
      return 0f;

    float score = 10000000 * (instance.correctGuesses / instance.timeSpent);


		Debug.Log("Final score "+ score);

		return score;
	}
}