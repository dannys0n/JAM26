using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
  public TextMeshProUGUI TimerText;
  public static Timer instance;
	public bool isRunning = false;
  float time = 0;

	private void Awake()
	{
		instance = this;
	}

	private void OnDestroy()
	{
		if (instance == this)
			instance = null;
	}

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
