using UnityEngine;

public class SkipTrainNOPresser : SkipTrainingButton
{
	public GameObject skipButton;

	private new void OnClick()
	{
		base.gameObject.transform.parent.gameObject.SetActive(false);
		skipButton.SetActive(true);
		base.OnClick();
		GameObject gameObject = GameObject.FindGameObjectWithTag("PlayerGun");
		if ((bool)gameObject && gameObject != null)
		{
			Transform child = gameObject.transform.GetChild(0);
			if ((bool)child && child != null)
			{
				child.gameObject.SetActive(true);
			}
		}
		TrainingController.CancelSkipTraining();
	}
}
