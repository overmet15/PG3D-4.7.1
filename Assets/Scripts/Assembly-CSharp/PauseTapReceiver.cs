using System;
using UnityEngine;

public class PauseTapReceiver : MonoBehaviour
{
	public static event Action PauseClicked;

	private void Start()
	{
		if (Defs.IsTraining)
		{
			base.gameObject.SetActive(false);
		}
	}

	private void OnPress(bool isDown)
	{
		if (!isDown && PauseTapReceiver.PauseClicked != null)
		{
			PauseTapReceiver.PauseClicked();
		}
	}
}
