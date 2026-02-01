using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
  public TextMeshProUGUI TimerText;
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
  {
  }

  // Update is called once per frame
  void Update()
  {
		TimerText.text = "Time Elapsed: ";

		TimerText.text += Time.timeSinceLevelLoad.ToString("F2");  
	}
}
