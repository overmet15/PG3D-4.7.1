using UnityEngine;

public sealed class coinsPlashka : MonoBehaviour
{
	public static coinsPlashka thisScript;

	public static bool hideButtonCoins;

	private float kfSize = (float)Screen.height / 768f;

	public Texture txFonCoins;

	public GUIStyle stButCoins;

	public GUIStyle stLabelCoins;

	public Rect rectButCoins;

	public Rect rectLabelCoins;

	private int tekKolCoins;

	private float lastTImeFetchedeychain;

	private Font f;

	public static Rect symmetricRect
	{
		get
		{
			Rect result = new Rect(thisScript.rectLabelCoins.x, thisScript.rectButCoins.y, thisScript.rectButCoins.width, thisScript.rectButCoins.height);
			result.x = (float)Screen.width - result.x - result.width;
			return result;
		}
	}

	private void Awake()
	{
		thisScript = base.gameObject.GetComponent<coinsPlashka>();
		hidePlashka();
		rectButCoins = new Rect((float)Screen.width - 21f * kfSize - (float)stButCoins.normal.background.width * kfSize, 21f * kfSize, (float)stButCoins.normal.background.width * kfSize, (float)stButCoins.normal.background.height * kfSize);
		rectLabelCoins = new Rect(rectButCoins.x + 78f * kfSize, rectButCoins.y, 85f * kfSize, rectButCoins.height - 5f * kfSize);
		stLabelCoins.fontSize = Mathf.RoundToInt(21f * kfSize);
		f = stLabelCoins.font;
		tekKolCoins = Storager.getInt(Defs.Coins, false);
		lastTImeFetchedeychain = Time.realtimeSinceStartup;
	}

	public static void showPlashka()
	{
		if (thisScript != null)
		{
			thisScript.enabled = true;
		}
	}

	public static void hidePlashka()
	{
		if (thisScript != null)
		{
			thisScript.enabled = false;
		}
	}

	private void OnGUI()
	{
		GUI.depth = -3;
		bool flag = GUI.enabled;
		GUI.enabled = !Defs.IsTraining;
		if (Time.realtimeSinceStartup - lastTImeFetchedeychain > 1f)
		{
			tekKolCoins = Storager.getInt(Defs.Coins, false);
			lastTImeFetchedeychain = Time.realtimeSinceStartup;
		}
		if (!hideButtonCoins)
		{
			if (GUI.Button(rectButCoins, string.Empty, stButCoins))
			{
				coinsShop.showCoinsShop();
			}
		}
		else
		{
			GUI.DrawTexture(rectButCoins, txFonCoins);
		}
		string text = tekKolCoins.ToString();
		if (text.Length >= 5)
		{
			text = string.Format("{0}..{1}", text[0], text[text.Length - 1]);
		}
		GUI.Label(rectLabelCoins, (!Defs.IsTraining) ? text : CoinsUpdater.trainCoinsStub, stLabelCoins);
		GUI.enabled = flag;
	}
}
