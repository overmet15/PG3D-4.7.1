using System;
using UnityEngine;

public class ShopTapReceiver : MonoBehaviour
{
	public static event Action ShopClicked;

	private void OnPress(bool isDown)
	{
		if (!isDown && ShopTapReceiver.ShopClicked != null)
		{
			ShopTapReceiver.ShopClicked();
		}
	}
}
