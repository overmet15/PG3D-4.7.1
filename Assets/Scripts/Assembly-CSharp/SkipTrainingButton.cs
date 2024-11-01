using System;
using UnityEngine;

public class SkipTrainingButton : MonoBehaviour
{
	public static event Action SkipTrClosed;

	protected void OnClick()
	{
		if (SkipTrainingButton.SkipTrClosed != null)
		{
			SkipTrainingButton.SkipTrClosed();
		}
		Resources.UnloadUnusedAssets();
	}
}
