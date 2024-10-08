using System;
using UnityEngine;

public class RanksTapReceiver : MonoBehaviour
{
	public static event Action RanksClicked;

	private void Start()
	{
		base.gameObject.SetActive(PlayerPrefs.GetInt("MultyPlayer", 0) == 1 && PlayerPrefs.GetInt("COOP", 0) == 0);
	}

	private void OnPress(bool isDown)
	{
		if (!isDown && RanksTapReceiver.RanksClicked != null)
		{
			RanksTapReceiver.RanksClicked();
		}
	}
}
