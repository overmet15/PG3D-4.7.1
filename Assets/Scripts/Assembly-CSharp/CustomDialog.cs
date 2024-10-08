using System;
using UnityEngine;

internal sealed class CustomDialog : MonoBehaviour
{
	public Action yesPressed;

	public Action noPressed;

	public string message = string.Empty;

	public GUIStyle noButton;

	public GUIStyle yesButton;

	public GUIStyle transparentButton;

	public Texture shadowTexture;

	public Texture window;

	private void OnGUI()
	{
		DrawWindow();
	}

	private void DrawWindow()
	{
		GUI.depth = -100;
		float coef = Defs.Coef;
		if (shadowTexture != null)
		{
			GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), shadowTexture, ScaleMode.StretchToFill);
		}
		Rect position = new Rect(0.5f * ((float)Screen.width - coef * (float)window.width), 0.5f * ((float)Screen.height - coef * (float)window.height), coef * (float)window.width, coef * (float)window.height);
		GUI.DrawTexture(position, window, ScaleMode.StretchToFill);
		GUI.BeginGroup(position);
		Rect position2 = new Rect(0.5f * (position.width - coef * (float)yesButton.normal.background.width), 318f * coef, (float)yesButton.normal.background.width * coef, (float)yesButton.normal.background.height * coef);
		if (GUI.Button(position2, string.Empty, yesButton))
		{
			if (yesPressed != null)
			{
				yesPressed();
			}
			_Remove();
		}
		Rect position3 = new Rect(position.width - coef * (float)noButton.normal.background.width, 0f, (float)noButton.normal.background.width * coef, (float)noButton.normal.background.height * coef);
		if (GUI.Button(position3, string.Empty, noButton))
		{
			if (noPressed != null)
			{
				noPressed();
			}
			_Remove();
		}
		GUI.EndGroup();
		GUI.Button(new Rect(0f, 0f, Screen.width, Screen.height), string.Empty, transparentButton);
	}

	private void _Remove()
	{
		yesPressed = null;
		noPressed = null;
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
