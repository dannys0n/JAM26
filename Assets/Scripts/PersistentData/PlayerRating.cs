using UnityEngine;

public class PlayerRating : MonoBehaviour
{
  private static int correctGuesses = 0;

  public static PlayerRating instance;
  public static float timeSpent = 0f;

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
    correctGuesses++;
  }

  public static int GetCorrectGuesses()
  {
    return correctGuesses;
  }

  public static void AddTimeSpent(float timeSpent)
  {
    timeSpent += timeSpent;
  }

  public static float CalculateScore()
  {
    if (timeSpent == 0f)
      return 0f;

    return 1000 * (correctGuesses / timeSpent);
	}
}