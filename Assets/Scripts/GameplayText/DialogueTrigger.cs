using UnityEngine;

/// <summary>
/// Emits a text popup request when the player interacts.
/// </summary>
public class DialogueTrigger : MonoBehaviour
{
	[TextArea]
	[SerializeField] private string text;

	[SerializeField] private float duration = 3f;

	private bool hasFired;

	public void Trigger()
	{
		if (hasFired) return;

		hasFired = true;
		PopupEvents.Request(text, duration);
	}
}
