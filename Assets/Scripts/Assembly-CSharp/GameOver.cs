using System;
using System.Collections.Generic;
using Rilisoft.PixlGun3D;
using UnityEngine;

public sealed class GameOver : MonoBehaviour
{
	private GameObject _purchaseActivityIndicator;

	public Texture elixir;

	public Texture res_tag;

	public Texture noElixir;

	public Texture noElixirNOinet;

	public GUIStyle resurrect;

	public GUIStyle retry;

	public GUIStyle quit;

	public GUIStyle decline;

	public GUIStyle buy;

	public GUIStyle ok;

	private bool haveNoElixirSh;

	private float coef = (float)Screen.height / 768f;

	private GameObject _inAppGameObject;

	public StoreKitEventListener _listener;

	internal ICollection<IMarketProduct> _products = new IMarketProduct[0];

	public bool activeInicator;

	private void Start()
	{
		_inAppGameObject = GameObject.FindGameObjectWithTag("InAppGameObject");
		_listener = _inAppGameObject.GetComponent<StoreKitEventListener>();
		if (_listener == null)
		{
			Debug.LogWarning("_listener is null.");
		}
		_purchaseActivityIndicator = StoreKitEventListener.purchaseActivityInd;
		if (_purchaseActivityIndicator == null)
		{
			Debug.LogWarning("_purchaseActivityIndicator is null.");
		}
		else
		{
			_purchaseActivityIndicator.SetActive(false);
		}
		Invoke("setAppropriateProducts", 0.01f);
		coinsPlashka.thisScript.enabled = true;
	}

	private void setAppropriateProducts()
	{
		_products = _listener._products;
	}

	private void hideActiveInd(string error)
	{
		activeInicator = false;
		Debug.Log("activeInicator=false; " + error);
	}

	private void OnEnable()
	{
		if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
		{
			GoogleIABManager.purchaseSucceededEvent += ElixirBuyAndr;
		}
	}

	private void OnDisable()
	{
		if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
		{
			GoogleIABManager.purchaseSucceededEvent -= ElixirBuyAndr;
		}
	}

	private void OnDestroy()
	{
		coinsPlashka.thisScript.enabled = false;
	}

	public void ElixirBuy()
	{
		activeInicator = false;
		_purchaseActivityIndicator.SetActive(activeInicator);
		_Resurrect();
		string elixirID = StoreKitEventListener.elixirID;
		string eventName = ((!InAppData.inappReadableNames.ContainsKey(elixirID)) ? elixirID : InAppData.inappReadableNames[elixirID]);
		FlurryAndroid.logEvent(eventName, false);
	}

	private void ElixirBuyAndr(GooglePurchase purchase)
	{
		try
		{
			if (purchase == null)
			{
				Debug.LogWarning("purchase is null.");
			}
			else if (purchase.productId == null)
			{
				Debug.LogWarning("purchase.productId is null.");
			}
			else if (purchase.productId.Equals(StoreKitEventListener.elixirID))
			{
				ElixirBuy();
			}
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
	}

	private void _Resurrect()
	{
		Defs.NumberOfElixirs--;
		WeaponManager component = GameObject.FindGameObjectWithTag("WeaponManager").GetComponent<WeaponManager>();
		foreach (Weapon playerWeapon in component.playerWeapons)
		{
			WeaponSounds component2 = playerWeapon.weaponPrefab.GetComponent<WeaponSounds>();
			if (playerWeapon.currentAmmoInClip + playerWeapon.currentAmmoInBackpack < component2.InitialAmmo + component2.ammoInClip)
			{
				playerWeapon.currentAmmoInClip = component2.ammoInClip;
				playerWeapon.currentAmmoInBackpack = component2.InitialAmmo;
			}
		}
		PlayerPrefs.SetFloat(Defs.CurrentHealthSett, Player_move_c.MaxPlayerHealth);
		PlayerPrefs.SetFloat(Defs.CurrentArmorSett, 0f);
		PlayerPrefs.SetInt(Defs.ArmorType, 0);
		Application.LoadLevel("Loading");
	}

	private void _Retry()
	{
	}

	private void _Buy()
	{
		Defs.NumberOfElixirs++;
		ElixirBuy();
	}

	private void OnGUI()
	{
		int depth = GUI.depth;
		if (_purchaseActivityIndicator == null)
		{
			Debug.LogWarning("_purchaseActivityIndicator is null.");
		}
		else
		{
			_purchaseActivityIndicator.SetActive(activeInicator);
		}
		float num = (float)Screen.width * 0.31f;
		float num2 = num * ((float)resurrect.normal.background.height / (float)resurrect.normal.background.width);
		float num3 = num2 * 0.2f;
		Rect position = new Rect((float)(Screen.width / 2) - num / 2f, (float)Screen.height - num2 * 3f - num3 * 3f, num, num2);
		GUI.enabled = !haveNoElixirSh && !activeInicator;
		GUI.enabled = !haveNoElixirSh && !activeInicator;
		Rect position2 = new Rect((float)(Screen.width / 2) - num / 2f, (float)Screen.height - num2 * 2f - num3 * 2f, num, num2);
		if (GUI.Button(position, string.Empty, retry))
		{
			GUI.enabled = true;
			GlobalGameController.ResetParameters();
			GlobalGameController.Score = 0;
			GameObject.FindGameObjectWithTag("WeaponManager").GetComponent<WeaponManager>().Reset();
			PlayerPrefs.SetFloat(Defs.CurrentHealthSett, Player_move_c.MaxPlayerHealth);
			PlayerPrefs.SetFloat(Defs.CurrentArmorSett, 0f);
			Application.LoadLevel(CurrentCampaignGame.levelSceneName);
		}
		float num4 = num * ((float)quit.normal.background.width / (float)resurrect.normal.background.width);
		float num5 = num4 * ((float)quit.normal.background.height / (float)quit.normal.background.width);
		if (GUI.Button(position2, string.Empty, quit))
		{
			GUI.enabled = true;
			Application.LoadLevel("ChooseLevel");
		}
		float num6 = (float)(elixir.width * Screen.height) / 768f;
		float num7 = (float)(elixir.height * Screen.height) / 768f;
		if (haveNoElixirSh)
		{
			GUI.enabled = !activeInicator;
			float num8 = (float)Screen.width * 0.45f * 1.5f;
			Texture texture = noElixir;
			float num9 = num8 * ((float)texture.height / (float)texture.width);
			float num10 = num8 * 0.27f;
			float num11 = num10 * ((float)buy.normal.background.height / (float)buy.normal.background.width);
			float num12 = num10 / 10f;
			float num13 = num11 * 3f;
			float num14 = Defs.Coef;
			Rect position3 = new Rect(0.5f * ((float)Screen.width - num14 * (float)texture.width), 0.5f * ((float)Screen.height - num14 * (float)texture.height), num14 * (float)texture.width, num14 * (float)texture.height);
			GUI.DrawTexture(position3, texture, ScaleMode.StretchToFill);
			GUI.BeginGroup(position3);
			try
			{
				Rect position4 = new Rect(0.5f * num14 * (float)(texture.width - buy.normal.background.width), (float)(texture.height - buy.normal.background.height - 96) * num14, (float)buy.normal.background.width * num14, (float)buy.normal.background.height * num14);
				if (GUI.Button(position4, string.Empty, buy))
				{
					Action act = null;
					act = delegate
					{
						coinsShop.thisScript.notEnoughCoins = false;
						coinsShop.thisScript.onReturnAction = null;
						int num15 = ((!VirtualCurrencyHelper.prices.ContainsKey(StoreKitEventListener.elixirID)) ? 10 : VirtualCurrencyHelper.prices[StoreKitEventListener.elixirID]);
						int @int = Storager.getInt(Defs.Coins, false);
						int newCoins = @int - num15;
						Action action = delegate
						{
							Storager.setInt(Defs.Coins, newCoins, false);
							_Buy();
						};
						Action<string> showShop = delegate(string pressedbutton)
						{
							if (!pressedbutton.Equals("Cancel"))
							{
								coinsShop.thisScript.notEnoughCoins = true;
								coinsShop.thisScript.onReturnAction = act;
								coinsShop.showCoinsShop();
							}
						};
						if (newCoins >= 0)
						{
							action();
						}
						else
						{
							GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("CustomDialog")) as GameObject;
							if (gameObject == null)
							{
								Debug.LogError("customDialogEntity == null");
							}
							else
							{
								CustomDialog component = gameObject.GetComponent<CustomDialog>();
								component.yesPressed = delegate
								{
									showShop("Yes!");
								};
							}
						}
					};
					act();
				}
				if (haveNoElixirSh && _products.Count > 0)
				{
					Rect position5 = new Rect(num14 * (float)(texture.width - decline.normal.background.width), 0f, (float)decline.normal.background.width * num14, (float)decline.normal.background.height * num14);
					if (GUI.Button(position5, string.Empty, decline))
					{
						haveNoElixirSh = false;
					}
				}
			}
			finally
			{
				GUI.EndGroup();
			}
			GUI.enabled = false;
		}
		GUI.depth = depth;
	}
}
