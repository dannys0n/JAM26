using UnityEngine;

public class DialogueTextTest : MonoBehaviour
{
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			PopupEvents.Request("Hello, this is a test dialogue popup!", 2f);

			//GetComponent<DialogueTrigger>()?.Trigger();
		}
	}
}