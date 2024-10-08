using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class coinsShop : MonoBehaviour
{
	public static coinsShop thisScript;

	public static bool showPlashkuPriExit;

	public Action onReturnAction;

	private float kfSize = (float)Screen.height / 768f;

	private float reSizeWidth;

	public GameObject _purchaseActivityIndicator;

	private bool productPurchased;

	private float _timeWhenPurchShown;

	public GUIStyle labelStyle;

	public GUIStyle notEnoughCoinsStyle;

	public bool notEnoughCoins;

	public Texture txFon;

	public Texture txTitle;

	public GUIStyle stButBack;

	public GUIStyle noInternetStyle;

	private Rect[] rects = new Rect[7];

	private GUIStyle[] styles = new GUIStyle[7];

	private bool coinsBought;

	private Rect rectFon;

	private Rect rectBut1;

	private Rect rectBut2;

	private Rect rectBut3;

	private Rect rectBut4;

	private Rect rectBut5;

	private Rect rectButBack;

	private Rect rectTitle;

	private bool productsReceived;

	private void HandleQueryInventorySucceededEvent(List<GooglePurchase> purchases, List<GoogleSkuInfo> skus)
	{
		if (!skus.Any((GoogleSkuInfo s) => s.productId == "skinsmaker"))
		{
			string[] value = skus.Select((GoogleSkuInfo sku) => sku.productId).ToArray();
			string arg = string.Join(", ", value);
			string message = string.Format("Google: Query inventory succeeded;\tPurchases count: {0}, Skus: [{1}]", purchases.Count, arg);
			Debug.Log(message);
			productsReceived = true;
		}
	}

	private void HandleItemDataRequestFinishedEvent(List<string> unavailableSkus, List<AmazonItem> availableItems)
	{
		productsReceived = true;
	}

	private void OnEnable()
	{
		if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
		{
			AmazonIAPManager.itemDataRequestFinishedEvent += HandleItemDataRequestFinishedEvent;
			AmazonIAPManager.purchaseSuccessfulEvent += HandlePurchaseSuccessfulEvent;
		}
		else
		{
			GoogleIABManager.queryInventorySucceededEvent += HandleQueryInventorySucceededEvent;
			GoogleIABManager.purchaseSucceededEvent += HandlePurchaseSucceededEvent;
		}
		GameObject gameObject = GameObject.FindGameObjectWithTag("InAppGameObject");
		StoreKitEventListener component = gameObject.GetComponent<StoreKitEventListener>();
		if (component.HasCoins())
		{
			productsReceived = true;
			Debug.Log(string.Format("productsReceived == {0}", productsReceived));
		}
		_purchaseActivityIndicator = StoreKitEventListener.purchaseActivityInd;
		if (_purchaseActivityIndicator == null)
		{
			Debug.LogWarning("_purchaseActivityIndicator == null");
		}
		else
		{
			_purchaseActivityIndicator.SetActive(false);
		}
		coinsBought = false;
	}

	private void OnDisable()
	{
		if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
		{
			AmazonIAPManager.itemDataRequestFinishedEvent -= HandleItemDataRequestFinishedEvent;
			AmazonIAPManager.purchaseSuccessfulEvent -= HandlePurchaseSuccessfulEvent;
		}
		else
		{
			GoogleIABManager.queryInventorySucceededEvent -= HandleQueryInventorySucceededEvent;
			GoogleIABManager.purchaseSucceededEvent -= HandlePurchaseSucceededEvent;
		}
		if (_purchaseActivityIndicator != null)
		{
			_purchaseActivityIndicator.SetActive(false);
		}
		coinsBought = false;
	}

	private void HandlePurchaseSuccessfullCore()
	{
		try
		{
			if (coinsBought)
			{
				productPurchased = true;
				_timeWhenPurchShown = Time.realtimeSinceStartup;
			}
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
	}

	private void HandlePurchaseSucceededEvent(GooglePurchase purchase)
	{
		HandlePurchaseSuccessfullCore();
	}

	private void HandlePurchaseSuccessfulEvent(AmazonReceipt receipt)
	{
		HandlePurchaseSuccessfullCore();
	}

	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		thisScript = base.gameObject.GetComponent<coinsShop>();
		hideCoinsShop();
		rectFon = new Rect((float)Screen.width * 0.5f - 683f * kfSize, 0f, 1366f * kfSize, 768f * kfSize);
		float num = (float)Screen.width / 45f;
		float num2 = 1f;
		float num3 = ((float)Screen.width - ((float)StoreKitEventListener.coinIds.Length + 1f) * num) / (float)StoreKitEventListener.coinIds.Length * num2;
		for (int i = 0; i < StoreKitEventListener.coinIds.Length; i++)
		{
			rects[i] = new Rect((float)(i + 1) * num + (float)i * num3, 175f * kfSize, num3, num3 * 2.7041886f);
		}
		rectButBack = new Rect(21f * kfSize, (float)Screen.height - (21f + (float)stButBack.normal.background.height) * kfSize, (float)stButBack.normal.background.width * kfSize, (float)stButBack.normal.background.height * kfSize);
		float num4 = 0.76f;
		float num5 = (float)txTitle.width * kfSize * num4;
		float height = (float)txTitle.height * kfSize * num4;
		rectTitle = new Rect((float)Screen.width * 0.5f - num5 * 0.5f, Defs.BottomOffs * Defs.Coef, num5, height);
	}

	private static void _LoadCoinStyle(GUIStyle st, string nm)
	{
		string a = "Coins";
		string path = ResPath.Combine(a, nm);
		st.normal.background = Resources.Load(path) as Texture2D;
		st.active.background = Resources.Load(ResPath.Combine(a, nm + "_n")) as Texture2D;
	}

	public static void showCoinsShop()
	{
		if (thisScript.styles[0] == null)
		{
			for (int i = 0; i < thisScript.styles.Length; i++)
			{
				thisScript.styles[i] = new GUIStyle();
			}
		}
		_LoadCoinStyle(thisScript.styles[0], "coins10");
		_LoadCoinStyle(thisScript.styles[1], "coins45");
		_LoadCoinStyle(thisScript.styles[2], "coins50");
		_LoadCoinStyle(thisScript.styles[3], "coins120");
		_LoadCoinStyle(thisScript.styles[4], "coins250");
		_LoadCoinStyle(thisScript.styles[5], "coins650");
		_LoadCoinStyle(thisScript.styles[6], "coins2000");
		thisScript.enabled = true;
		coinsPlashka.hideButtonCoins = true;
		coinsPlashka.showPlashka();
	}

	public static void hideCoinsShop()
	{
		if (!(thisScript != null))
		{
			return;
		}
		thisScript.enabled = false;
		GUIStyle[] array = thisScript.styles;
		foreach (GUIStyle gUIStyle in array)
		{
			if (gUIStyle != null)
			{
				gUIStyle.normal.background = null;
				gUIStyle.active.background = null;
			}
		}
		Resources.UnloadUnusedAssets();
	}

	private void OnGUI()
	{
		GUI.depth = -2;
		GUI.enabled = !StoreKitEventListener.purchaseInProcess;
		if (notEnoughCoins)
		{
			float num = 2f * (float)Screen.width / 5f;
			float num2 = 2f * (float)Screen.height / 10f;
			Rect position = new Rect((float)Screen.width / 2f - num / 2f, coinsPlashka.thisScript.rectLabelCoins.y + coinsPlashka.thisScript.rectLabelCoins.height / 2f - num2 / 2f, num, num2);
			notEnoughCoinsStyle.fontSize = Mathf.RoundToInt(30f * Defs.Coef);
			GUI.Box(position, "Not enough coins...", notEnoughCoinsStyle);
		}
		if (Time.realtimeSinceStartup - _timeWhenPurchShown >= GUIHelper.Int)
		{
			productPurchased = false;
		}
		if (productPurchased)
		{
			labelStyle.fontSize = Player_move_c.FontSizeForMessages;
			GUI.Label(Player_move_c.SuccessMessageRect(), "Purchase was successful", labelStyle);
		}
		_purchaseActivityIndicator.SetActive(StoreKitEventListener.purchaseInProcess);
		GUI.DrawTexture(rectFon, txFon);
		GUI.DrawTexture(rectTitle, txTitle);
		guiButtonCoins();
		if (GUI.Button(rectButBack, string.Empty, stButBack))
		{
			ExitFromShop(true);
		}
		GUI.enabled = true;
	}

	public static void ExitFromShop(bool performOnExitActs)
	{
		hideCoinsShop();
		if (showPlashkuPriExit)
		{
			coinsPlashka.hidePlashka();
		}
		coinsPlashka.hideButtonCoins = false;
		if (performOnExitActs)
		{
			if (thisScript.onReturnAction != null && thisScript.coinsBought)
			{
				thisScript.coinsBought = false;
				thisScript.onReturnAction();
			}
			else
			{
				thisScript.onReturnAction = null;
			}
		}
	}

	private void guiButtonCoins()
	{
		bool flag = !Application.isEditor;
		flag = StoreKitEventListener.billingSupported;
		if (!productsReceived || !flag)
		{
			float num = (float)Screen.width / 2f;
			noInternetStyle.fontSize = Mathf.RoundToInt(30f * (float)Screen.height / 768f);
			string text = ((Application.platform == RuntimePlatform.IPhonePlayer) ? "No connection to App Store..." : ((Application.platform != RuntimePlatform.Android) ? "No connection to Store..." : ((Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon) ? "No connection to Market..." : "No connection to store...")));
			GUI.Box(new Rect((float)Screen.width / 2f - num / 2f, (float)Screen.height / 4f, num, (float)Screen.height / 2f), text, noInternetStyle);
			return;
		}
		for (int i = 0; i < StoreKitEventListener.coinIds.Length; i++)
		{
			if (GUI.Button(rects[i], string.Empty, styles[i]))
			{
				coinsBought = true;
				StoreKitEventListener.purchaseInProcess = true;
				string sku = StoreKitEventListener.coinIds[i];
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
				{
					AmazonIAP.initiatePurchaseRequest(sku);
				}
				else
				{
					GoogleIAB.purchaseProduct(sku);
				}
			}
		}
	}
}
