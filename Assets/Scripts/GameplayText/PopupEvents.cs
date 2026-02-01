using System;

/// <summary>
/// Central event hub for gameplay popups.
/// </summary>
public static class PopupEvents
{
	public static Action<string, float> OnPopupRequested;

	public static void Request(string text, float duration)
	{
		OnPopupRequested?.Invoke(text, duration);
	}
}
