using UnityEngine;

public class PlayerRating : MonoBehaviour
{
  private static int correctGuesses = 0;

  public static PlayerRating instance;

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
}
