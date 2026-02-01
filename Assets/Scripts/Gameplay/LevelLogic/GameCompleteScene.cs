using TMPro;
using UnityEngine;

public class GameCompleteScene : MonoBehaviour
{
  public TextMeshProUGUI gameCompleteText;
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
  {
    DisplayFinalScore();
	}

  void DisplayFinalScore()
  {
    gameCompleteText.text = "Game Complete!\n";
    gameCompleteText.text += "Total Time: " + PlayerRating.GetTotalTimeSpent().ToString("F2") + " seconds\n";
    gameCompleteText.text += "Correct Guesses: " + PlayerRating.GetCorrectGuesses().ToString() + "\n";
    gameCompleteText.text += "Final Score: " + PlayerRating.CalculateScore().ToString("F2") + "\n";
	}
}
