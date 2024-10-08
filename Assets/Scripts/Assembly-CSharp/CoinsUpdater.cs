using UnityEngine;

internal sealed class CoinsUpdater : MonoBehaviour
{
	public static readonly string trainCoinsStub = "999";

	private UILabel coinsLabel;

	private string _trainingMsg = "0";

	private void Start()
	{
		coinsLabel = GetComponent<UILabel>();
		GlobalGameController.fontHolder = coinsLabel.font.dynamicFont;
		CoinsMessage.CoinsLabelDisappeared += _ReplaceMsgForTraining;
	}

	private void _ReplaceMsgForTraining()
	{
		if (Defs.IsTraining)
		{
			_trainingMsg = trainCoinsStub;
		}
	}

	private void Update()
	{
		string text = Storager.getInt(Defs.Coins, false).ToString();
		if (text.Length >= 5)
		{
			text = string.Format("{0}..{1}", text[0], text[text.Length - 1]);
		}
		coinsLabel.text = ((!Defs.IsTraining) ? text : _trainingMsg);
	}

	private void OnDestroy()
	{
		CoinsMessage.CoinsLabelDisappeared -= _ReplaceMsgForTraining;
	}
}
