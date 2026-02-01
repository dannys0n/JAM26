using TMPro;
using UnityEngine;
using System.Collections;

/// <summary>
/// Displays gameplay text popups.
/// </summary>
public class PopupManager : MonoBehaviour
{
	[SerializeField] private TMP_Text textField;
	[SerializeField] private CanvasGroup canvasGroup;

	private Coroutine activePopup;

	private void OnEnable()
	{
		PopupEvents.OnPopupRequested += Show;
	}

	private void OnDisable()
	{
		PopupEvents.OnPopupRequested -= Show;
	}

	private void Show(string text, float duration)
	{
		if (activePopup != null)
			StopCoroutine(activePopup);

		activePopup = StartCoroutine(ShowRoutine(text, duration));
	}

	private IEnumerator ShowRoutine(string text, float duration)
	{
		textField.text = text;
		canvasGroup.alpha = 1f;

		yield return new WaitForSeconds(duration);

		canvasGroup.alpha = 0f;
		activePopup = null;
	}
}
