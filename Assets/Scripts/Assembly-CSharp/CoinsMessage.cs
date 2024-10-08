using System;
using UnityEngine;

public sealed class CoinsMessage : MonoBehaviour
{
	public GUIStyle labelStyle;

	public Rect rect = Player_move_c.SuccessMessageRect();

	public string message = "Purchases restored";

	public Texture texture;

	public int depth = -2;

	public bool singleMessage;

	public Texture youveGotCoin;

	public Texture passNextLevels;

	private int coinsToShow;

	private int coinsForNextLevels;

	private double startTime;

	private float _time = 2f;

	public Texture plashka;

	public static event Action CoinsLabelDisappeared;

	public static void FireCoinsAddedEvent()
	{
		if (CoinsMessage.CoinsLabelDisappeared != null)
		{
			CoinsMessage.CoinsLabelDisappeared();
		}
	}

	private void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		coinsToShow = Storager.getInt(Defs.EarnedCoins, false);
		Storager.setInt(Defs.EarnedCoins, 0, false);
		if (coinsToShow > 1)
		{
			plashka = Resources.Load(ResPath.Combine("CoinsIndicationSystem", "got_prize")) as Texture;
		}
		else
		{
			plashka = Resources.Load(ResPath.Combine("CoinsIndicationSystem", "got_coin")) as Texture;
		}
		startTime = Time.realtimeSinceStartup;
	}

	private void Remove()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void OnGUI()
	{
		if ((double)Time.realtimeSinceStartup - startTime >= (double)_time)
		{
			if (!((double)Time.realtimeSinceStartup - startTime >= (double)(_time + 0.3f)))
			{
				return;
			}
			if (coinsToShow > 0)
			{
				int @int = Storager.getInt(Defs.Coins, false);
				Storager.setInt(Defs.Coins, @int + coinsToShow, false);
				for (int i = 0; i < coinsToShow; i++)
				{
					FlurryPluginWrapper.LogCoinEarned();
				}
				coinsToShow = 0;
			}
			if (CoinsMessage.CoinsLabelDisappeared != null)
			{
				CoinsMessage.CoinsLabelDisappeared();
			}
			Remove();
		}
		else
		{
			GUI.depth = depth;
			float num = 488f * Defs.Coef;
			float num2 = 93f * Defs.Coef;
			GUI.DrawTexture(new Rect(((float)Screen.width - num) / 2f, (float)Screen.height / 4f - num2 / 2f, num, num2), plashka, ScaleMode.StretchToFill);
		}
	}
}
