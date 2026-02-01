using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Simple, self-contained dialogue box.
/// Displays queued dialogue lines and blocks progression
/// until Space, Escape, or Left Mouse is pressed.
/// Lines can be injected at runtime.
/// </summary>
public class DialogueBox : MonoBehaviour
{
	[Header("UI References")]
	[SerializeField] private GameObject root;
	[SerializeField] private TMP_Text dialogueText;

	public List<string> dialogueQueue = new List<string>();
	private bool isActive;

	private void Awake()
	{
		if (dialogueQueue.Count > 0)
		{
			Open();
		}
		else
			root.SetActive(false);
	}

	private void Update()
	{
		if (!isActive)
		{
			if (dialogueQueue.Count > 0)
			{
				Open();
			}
			else
				return;
		}

		if (InputTriggered())
		{
			Advance();
		}
	}

	/// <summary>
	/// Adds dialogue lines to the queue.
	/// If dialogue is not currently active, starts immediately.
	/// </summary>
	public void Enqueue(params string[] lines)
	{
		if (lines == null || lines.Length == 0)
			return;

		dialogueQueue.AddRange(lines);

		if (!isActive)
		{
			Open();
		}
	}

	private void Open()
	{
		isActive = true;
		root.SetActive(true);
		ShowNext();
	}

	private void Advance()
	{
		ShowNext();
	}

	private void ShowNext()
	{
		if (dialogueQueue.Count == 0)
		{
			Close();
			return;
		}

		// Pop front
		dialogueText.text = dialogueQueue[0];
		dialogueQueue.RemoveAt(0);
	}

	private void Close()
	{
		isActive = false;
		root.SetActive(false);
	}

	private bool InputTriggered()
	{
		return Input.GetKeyDown(KeyCode.Space) ||
					 Input.GetKeyDown(KeyCode.Escape) ||
					 Input.GetMouseButtonDown(0);
	}
}
