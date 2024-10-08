using UnityEngine;

public class YesPresser : SkipTrainingButton
{
	public GameObject noButton;

	private new void OnClick()
	{
		noButton.GetComponent<UIButton>().enabled = false;
		base.enabled = false;
		GotToNextLevel.GoToNextLevel();
	}
}
