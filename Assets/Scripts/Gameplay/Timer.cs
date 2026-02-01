using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
  public TextMeshProUGUI TimerText;
  public bool isRunning = false;
  float time = 0;

  // Update is called once per frame
  void Update()
  {
    if (isRunning == false)
      return;

    time += Time.deltaTime;
		TimerText.text = "Time Elapsed: ";
		TimerText.text += time.ToString("F2");  
	}

  public void StartTimer()
  {
    isRunning = true;
	}

	public void StopTimer(bool winConditionMet)
  {
    isRunning = false;

		if (winConditionMet)
    {
      PlayerRating.AddTimeSpent(Time.timeSinceLevelLoad);
      PlayerRating.IncrementCorrectGuess();
		}
	}
}
