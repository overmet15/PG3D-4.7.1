using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Prime31;
using Rilisoft.PixlGun3D;
using UnityEngine;

public sealed class MainMenu : MonoBehaviour
{
	private const string SkinsMakerSku = "skinsmaker";

	public GUIStyle survivalStyle;

	public GUIStyle coopStyle;

	public GUIStyle facebookStyle;

	public GUIStyle gamecenterStyle;

	public GUIStyle playMultyStyle;

	public GUIStyle playStyle;

	public GUIStyle skinsMakerStyle;

	public GUIStyle skinsMakerOffStyle;

	public GUIStyle soundStyle;

	public GUIStyle twitterStyle;

	private float horizontal;

	public GUIStyle shopButtonStyle;

	public Texture fon;

	public Texture bottomShadow;

	private bool showFreeCoins;

	private bool isShowSetting;

	public GUIStyle endermanStyle;

	public GUIStyle soundOnOff;

	public GUIStyle restore;

	public GUIStyle sliderStyle;

	public GUIStyle thumbStyle;

	public Texture settingPlashka;

	public Texture settingTitle;

	public Texture slow_fast;

	public Texture polzunok;

	private float mySens;

	private Shop _shopInstance;

	private float _lastTimeSettingsLogged;

	private bool _skinsMakerQuerySucceeded;

	private string _version;

	private GUIStyle _versionLabelStyle;

	public Texture head_worldwide;

	public Texture head_COOP;

	public Texture bossBattle;

	public Texture nickPlashka;

	public Texture head_Subscribe;

	public GUIStyle worldwideBut;

	public GUIStyle localBut;

	public GUIStyle timeSurvivalBut;

	public GUIStyle teamFight;

	public GUIStyle profileBut;

	public GUIStyle f_Subscribe;

	public GUIStyle t_Subscribe;

	private bool isShowDeadMatch;

	private bool isShowCOOP;

	private GameObject _inAppGameObject;

	private StoreKitEventListener _listener;

	private bool musicOld;

	private bool fxOld;

	public Texture inAppFon;

	public GUIStyle puliInApp;

	public GUIStyle healthInApp;

	public GUIStyle pulemetInApp;

	public GUIStyle crystalSwordInapp;

	public GUIStyle elixirInapp;

	public bool isFirstFrame = true;

	public bool isInappWinOpen;

	private bool productPurchased;

	private Dictionary<string, KeyValuePair<Action, GUIStyle>> _actionsForPurchasedItems = new Dictionary<string, KeyValuePair<Action, GUIStyle>>();

	private bool showUnlockDialog;

	private bool isPressFullOnMulty;

	private string[] productIdentifiers = StoreKitEventListener.idsForFull;

	private float _timeWhenPurchShown;

	public Texture plashkaPodScore;

	public Texture playMultyStyleNO;

	public Texture fonFull;

	public Texture fonFree;

	public Texture head_Free;

	public GameObject skinsManagerPrefab;

	public GameObject weaponManagerPrefab;

	public GUIStyle unlockStyle;

	public GUIStyle noStyle;

	public GUIStyle fullVerStyle;

	public GUIStyle bestScoreStyle;

	public GUIStyle labelStyle;

	public GUIStyle f_Free;

	public GUIStyle t_Free;

	public GUIStyle rate_Free;

	public GUIStyle coins_Free;

	public GUIStyle tube_free;

	public GUIStyle backBut;

	private bool showMessagFacebook;

	private bool showMessagTiwtter;

	private GameObject _purchaseActivityIndicator;

	public Texture freeFaceOff;

	public Texture freeTwitterOff;

	public Texture freeTubeOff;

	private ExperienceController expController;

	private bool clickButtonFacebook;

	private bool _canUserUseFacebookComposer;

	private bool chatIsOn;

	private bool invCur;

	private bool _hasPublishPermission;

	private bool _hasPublishActions;

	public static int FontSizeForMessages
	{
		get
		{
			return Mathf.RoundToInt((float)Screen.height * 0.03f);
		}
	}

	public static float iOSVersion
	{
		get
		{
			float result = -1f;
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				string text = SystemInfo.operatingSystem.Replace("iPhone OS ", string.Empty);
				float.TryParse(text.Substring(0, 1), out result);
			}
			return result;
		}
	}

	private void GoToTraining(Action act)
	{
		GUIHelper.DrawLoading();
		PlayerPrefs.SetInt("MultyPlayer", 0);
		PlayerPrefs.SetInt("COOP", 0);
		PlayerPrefs.SetInt("company", 0);
		PlayerPrefs.SetInt(Defs.SurvivalSett, 0);
		GlobalGameController.Score = 0;
		GameObject.FindGameObjectWithTag("WeaponManager").GetComponent<WeaponManager>().Reset();
		PlayerPrefs.SetFloat(Defs.CurrentHealthSett, Player_move_c.MaxPlayerHealth);
		PlayerPrefs.SetFloat(Defs.CurrentArmorSett, 0f);
		Application.LoadLevel("CampaignLoading");
	}

	private static string ReadPrefsFileToString()
	{
		/*if (Application.platform == RuntimePlatform.Android)
		{
			try
			{
				using (StreamReader streamReader = File.OpenText("/data/data/com.P3D.Pixlgun/shared_prefs/com.P3D.Pixlgun.xml"))
				{
					return streamReader.ReadToEnd();
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
		}*/
		return string.Empty;
	}

	private void DrawGameModeButtons(float rightBorder)
	{
		if (_shopInstance != null)
		{
			return;
		}
		float num = (float)Screen.height / 768f;
		float buttonWidth = (float)playStyle.normal.background.width * num;
		float buttonHeight = (float)playStyle.normal.background.height * num;
		float buttonsLeft = rightBorder - buttonWidth;
		float num2 = 30f * num;
		float rowHeight = buttonHeight + num2;
		float buttonsTop = (float)Screen.height * 0.5f - buttonHeight * 2f - num2 * 1.5f + 20f * Defs.Coef;
		Func<int, Rect> func = (int rowIndex) => new Rect(buttonsLeft, buttonsTop + (float)rowIndex * rowHeight, buttonWidth, buttonHeight);
		int arg = 1;
		if (GUI.RepeatButton(func(arg), string.Empty, survivalStyle))
		{
			Action action = delegate
			{
				PlayerPrefs.SetInt("MultyPlayer", 0);
				PlayerPrefs.SetInt("COOP", 0);
				PlayerPrefs.SetInt("company", 0);
				PlayerPrefs.SetInt(Defs.SurvivalSett, 1);
				CurrentCampaignGame.levelSceneName = string.Empty;
				GlobalGameController.Score = 0;
				GameObject.FindGameObjectWithTag("WeaponManager").GetComponent<WeaponManager>().Reset();
				PlayerPrefs.SetFloat(Defs.CurrentHealthSett, Player_move_c.MaxPlayerHealth);
				PlayerPrefs.SetFloat(Defs.CurrentArmorSett, 0f);
				FlurryPluginWrapper.LogTrueSurvivalModePress();
				Application.LoadLevel("CampaignLoading");
			};
			if (PlayerPrefs.GetInt(Defs.TrainingCompleted_4_4_Sett, 0) == 0)
			{
				GoToTraining(action);
				return;
			}
			GUIHelper.DrawLoading();
			action();
		}
		int arg2 = 0;
		if (GUI.RepeatButton(func(arg2), string.Empty, playStyle))
		{
			Action action2 = delegate
			{
				PlayerPrefs.SetInt("MultyPlayer", 0);
				PlayerPrefs.SetInt("COOP", 0);
				PlayerPrefs.SetInt("company", 0);
				PlayerPrefs.SetInt(Defs.SurvivalSett, 0);
				GlobalGameController.Score = 0;
				GameObject.FindGameObjectWithTag("WeaponManager").GetComponent<WeaponManager>().Reset();
				PlayerPrefs.SetFloat(Defs.CurrentHealthSett, Player_move_c.MaxPlayerHealth);
				PlayerPrefs.SetFloat(Defs.CurrentArmorSett, 0f);
				FlurryPluginWrapper.LogCampaignModePress();
				Application.LoadLevel("CampaignChooseBox");
			};
			if (PlayerPrefs.GetInt(Defs.TrainingCompleted_4_4_Sett, 0) == 0)
			{
				GoToTraining(action2);
				return;
			}
			GUIHelper.DrawLoading();
			action2();
		}
		int arg3 = 2;
		if (GUI.RepeatButton(func(arg3), string.Empty, playMultyStyle))
		{
			Action action3 = delegate
			{
				PlayerPrefs.SetInt("MultyPlayer", 1);
				PlayerPrefs.SetInt("COOP", 0);
				PlayerPrefs.SetInt("company", 0);
				PlayerPrefs.SetInt(Defs.SurvivalSett, 0);
				GameObject.FindGameObjectWithTag("WeaponManager").GetComponent<WeaponManager>().Reset();
				FlurryPluginWrapper.LogDeathmatchModePress();
				MenuBackgroundMusic.keepPlaying = true;
				LoadConnectScene.textureToShow = null;
				LoadConnectScene.sceneToLoad = "ConnectScene";
				Application.LoadLevel(Defs.PromSceneName);
			};
			if (PlayerPrefs.GetInt(Defs.TrainingCompleted_4_4_Sett, 0) == 0)
			{
				GoToTraining(action3);
				return;
			}
			GUIHelper.DrawLoading();
			action3();
		}
		int arg4 = 3;
		if (GUI.RepeatButton(func(arg4), string.Empty, coopStyle))
		{
			Action action4 = delegate
			{
				PlayerPrefs.SetString("TypeConnect", "inet");
				PlayerPrefs.SetInt("COOP", 1);
				PlayerPrefs.SetInt("MultyPlayer", 1);
				PlayerPrefs.SetInt("company", 0);
				PlayerPrefs.SetInt(Defs.SurvivalSett, 0);
				GameObject.FindGameObjectWithTag("WeaponManager").GetComponent<WeaponManager>().Reset();
				FlurryPluginWrapper.LogCooperativeModePress();
				MenuBackgroundMusic.keepPlaying = true;
				LoadConnectScene.textureToShow = null;
				LoadConnectScene.sceneToLoad = "ConnectScene";
				Application.LoadLevel(Defs.PromSceneName);
			};
			if (PlayerPrefs.GetInt(Defs.TrainingCompleted_4_4_Sett, 0) == 0)
			{
				GoToTraining(action4);
				return;
			}
			GUIHelper.DrawLoading();
			action4();
		}
	}

	private void HandleShopButton()
	{
		if (_shopInstance == null)
		{
			FlurryPluginWrapper.LogEvent("Shop");
			expController.isShowRanks = false;
			_shopInstance = Shop.sharedShop;
			if (_shopInstance != null)
			{
				_shopInstance.loadShopCategories();
				_shopInstance.resumeAction = HandleResumeFromShop;
			}
			else
			{
				Debug.LogWarning("sharedShop == null");
			}
		}
	}

	private void HandleResumeFromShop()
	{
		if (_shopInstance != null)
		{
			expController.isShowRanks = true;
			_shopInstance.resumeAction = delegate
			{
			};
			_shopInstance.unloadShopCategories();
			_shopInstance = null;
		}
	}

	private void DrawToolbar(float rightBorder)
	{
		float num = (float)Screen.height / 768f;
		float num2 = (float)Screen.width - rightBorder;
		float num3 = (float)soundStyle.normal.background.width * num;
		float num4 = (float)soundStyle.normal.background.height * num;
		float num5 = (float)Screen.height - 21f * num - num4;
		float num6 = 30f * num;
		float num7 = num3 + num6;
		if (_shopInstance != null)
		{
			return;
		}
		if (GUI.Button(new Rect(21f * num, 21f * num, (float)soundStyle.normal.background.width * num, (float)soundStyle.normal.background.height * num), string.Empty, soundStyle) && !isFirstFrame)
		{
			isShowSetting = true;
			expController.isShowRanks = false;
			if (Time.time - _lastTimeSettingsLogged > 2f)
			{
				FlurryPluginWrapper.LogEvent("Settings");
				_lastTimeSettingsLogged = Time.time;
			}
		}
		if (Application.platform == RuntimePlatform.IPhonePlayer && GUI.Button(new Rect(21f * num, (63f + (float)(soundStyle.normal.background.height * 2)) * num, (float)soundStyle.normal.background.width * num, (float)soundStyle.normal.background.height * num), string.Empty, gamecenterStyle))
		{
			FlurryPluginWrapper.LogGamecenter();
			if (Application.isEditor)
			{
			}
		}
		if (GUI.RepeatButton(new Rect(21f * num, (float)Screen.height - (21f + (float)profileBut.normal.background.height) * num, (float)profileBut.normal.background.width * num, (float)profileBut.normal.background.height * num), string.Empty, profileBut) && !isFirstFrame)
		{
			FlurryPluginWrapper.LogEvent("Profile");
			GUIHelper.DrawLoading();
			PlayerPrefs.SetInt(Defs.ProfileEnteredFromMenu, 0);
			GoToProfile();
		}
		float num8 = (float)skinsMakerStyle.normal.background.width * num;
		if (true)
		{
			Rect position = new Rect(rightBorder - num8, (float)Screen.height - (21f + (float)skinsMakerStyle.normal.background.height) * num, num8, (float)skinsMakerStyle.normal.background.height * num);
			bool flag = Defs.IsProEdition || PlayerPrefs.GetInt(Defs.SkinsMakerInMainMenuPurchased) > 0;
			GUIStyle style = skinsMakerStyle;
			if ((Application.platform != RuntimePlatform.Android) ? GUI.RepeatButton(position, string.Empty, skinsMakerStyle) : GUI.Button(position, string.Empty, style))
			{
				if (flag)
				{
					GUIHelper.DrawLoading();
					PlayerPrefs.SetInt(Defs.SkinEditorMode, 0);
					FlurryPluginWrapper.LogSkinsMakerModePress();
					FlurryPluginWrapper.LogSkinsMakerEnteredEvent();
					Application.LoadLevel("SkinEditor");
				}
				else if (_skinsMakerQuerySucceeded)
				{
					if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
					{
						GoogleIABManager.purchaseSucceededEvent += HandleSkinsMakerPurchaseSucceededEvent;
						GoogleIABManager.purchaseFailedEvent += HandleSkinsMakerPurchasePurchaseFailedEvent;
						GoogleIAB.purchaseProduct("skinsmaker");
					}
				}
				else
				{
					Debug.Log("Skins Maker query not performed yet.");
				}
			}
		}
		float width = (float)coins_Free.normal.background.width * num;
		if (true)
		{
			Rect position2 = new Rect((float)Screen.width - (37f + (float)skinsMakerStyle.normal.background.width + (float)coins_Free.normal.background.width) * num, (float)Screen.height - (21f + (float)coins_Free.normal.background.height) * num, width, (float)coins_Free.normal.background.height * num);
			if (GUI.Button(position2, string.Empty, coins_Free))
			{
				showFreeCoins = true;
				expController.isShowRanks = false;
			}
		}
	}

	public void GoToProfile()
	{
		PlayerPrefs.SetInt(Defs.SkinEditorMode, 1);
		Application.LoadLevel("SkinEditor");
	}

	private void PerformSkinsMakerQueryIfLiteEdition()
	{
		if (!Defs.IsProEdition && PlayerPrefs.GetInt(Defs.SkinsMakerInMainMenuPurchased) <= 0 && Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
		{
			GoogleIABManager.queryInventorySucceededEvent += HandleSkinsMakerQueryInventorySucceededEvent;
			GoogleIABManager.queryInventoryFailedEvent += HandleSkinsMakerQueryInventoryFailedEvent;
			GoogleIAB.queryInventory(new string[1] { "skinsmaker" });
		}
	}

	private void HandleSkinsMakerQueryInventorySucceededEvent(List<GooglePurchase> purchases, List<GoogleSkuInfo> skus)
	{
		string arg = string.Join(", ", purchases.Select((GooglePurchase p) => p.productId).ToArray());
		string arg2 = string.Join(", ", skus.Select((GoogleSkuInfo s) => s.productId).ToArray());
		if (skus.Count != 1 || skus.Single().productId != "skinsmaker")
		{
			string message = string.Format("Skins Maker query inventory not performed yet.\n\t[ {0} ]\n\t[ {1} ]", arg, arg2);
			Debug.Log(message);
			return;
		}
		if (purchases.Any((GooglePurchase p) => p.productId == "skinsmaker"))
		{
			PlayerPrefs.SetInt(Defs.SkinsMakerInMainMenuPurchased, 1);
		}
		_skinsMakerQuerySucceeded = true;
		string message2 = string.Format("Skins Maker query inventory succeeded.\n\t[ {0} ]\n\t[ {1} ]", arg, arg2);
		Debug.Log(message2);
		if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
		{
			GoogleIABManager.queryInventorySucceededEvent -= HandleSkinsMakerQueryInventorySucceededEvent;
			GoogleIABManager.queryInventoryFailedEvent -= HandleSkinsMakerQueryInventoryFailedEvent;
		}
	}

	private void HandleSkinsMakerQueryInventoryFailedEvent(string message)
	{
		if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
		{
			GoogleIABManager.queryInventorySucceededEvent -= HandleSkinsMakerQueryInventorySucceededEvent;
			GoogleIABManager.queryInventoryFailedEvent -= HandleSkinsMakerQueryInventoryFailedEvent;
		}
		Debug.LogWarning("Skins Maker query failed.\n\t" + message);
	}

	private void HandleSkinsMakerPurchaseSucceededEvent(GooglePurchase purchase)
	{
		try
		{
			if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon && !(purchase.productId != "skinsmaker"))
			{
				PlayerPrefs.SetInt(Defs.SkinsMakerInMainMenuPurchased, 1);
				Debug.Log("Skins Maker purchase succeeded: " + purchase.productId);
				GoogleIABManager.purchaseSucceededEvent -= HandleSkinsMakerPurchaseSucceededEvent;
				GoogleIABManager.purchaseFailedEvent -= HandleSkinsMakerPurchasePurchaseFailedEvent;
			}
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
	}

	private void HandleSkinsMakerPurchasePurchaseFailedEvent(string message)
	{
		if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
		{
			GoogleIABManager.purchaseSucceededEvent -= HandleSkinsMakerPurchaseSucceededEvent;
			GoogleIABManager.purchaseFailedEvent -= HandleSkinsMakerPurchasePurchaseFailedEvent;
		}
		Debug.LogWarning("Skins Maker purchase failed.\n\t" + message);
	}

	private static string GetEndermanUrl()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.isEditor)
		{
			return "https://itunes.apple.com/us/app/pocket-enderman-skin-uploader/id790181179?mt=8";
		}
		if (Application.platform == RuntimePlatform.Android)
		{
			return (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon) ? "https://play.google.com/store/apps/details?id=com.ender.android" : "http://www.amazon.com/Pocket-Enderman-Virtual-simulator-Minecraft/dp/B00HJY6XJM/ref=sr_1_1";
		}
		return string.Empty;
	}

	private void DrawFreeCoins()
	{
		if (_shopInstance != null)
		{
			return;
		}
		float num = (float)Screen.height / 768f;
		float num2 = 30f * num;
		float num3 = 682f * num;
		Rect rect = new Rect(0.5f * ((float)Screen.width - num * 1366f), 0f, num * 1366f, num * 768f);
		GUI.DrawTexture(new Rect(0f, 290f * num, (float)head_Subscribe.width * num, (float)head_Subscribe.height * num), head_Subscribe);
		GUI.DrawTexture(new Rect(0.5f * ((float)Screen.width - (float)head_Free.width * num), 0f, (float)head_Free.width * num, (float)head_Free.height * num), head_Free);
		if (true)
		{
			if (endermanStyle == null || endermanStyle.normal == null || endermanStyle.normal.background == null)
			{
				Debug.LogWarning("endermanStyle.normal.background == null");
			}
			else
			{
				Rect position = new Rect(21f * num, 21f * num, (float)endermanStyle.normal.background.width * num, (float)endermanStyle.normal.background.height * num);
				if (GUI.Button(position, string.Empty, endermanStyle))
				{
					if (Application.isEditor)
					{
						Debug.Log(GetEndermanUrl());
					}
					else
					{
						Application.OpenURL(GetEndermanUrl());
					}
				}
			}
		}
		if (GUI.Button(new Rect(21f * num, 336f * num, (float)f_Subscribe.normal.background.width * num, (float)f_Subscribe.normal.background.height * num), string.Empty, f_Subscribe))
		{
			Application.OpenURL("https://www.facebook.com/PixelGun3DOfficial");
		}
		if (GUI.Button(new Rect(21f * num, 450f * num, (float)t_Subscribe.normal.background.width * num, (float)t_Subscribe.normal.background.height * num), string.Empty, t_Subscribe))
		{
			Application.OpenURL("https://twitter.com/PixelGun3D");
		}
		if (true)
		{
			if (PlayerPrefs.GetInt("freeFacebook", 0) == 0)
			{
				if (GUI.Button(new Rect((float)Screen.width * 0.74f - (float)f_Free.normal.background.width * 0.5f * num, (float)Screen.height * 0.5f - (float)f_Free.normal.background.height * num * 1.5f - num2, (float)f_Free.normal.background.width * num, (float)f_Free.normal.background.height * num), string.Empty, f_Free))
				{
					FlurryPluginWrapper.LogFacebook();
					FlurryPluginWrapper.LogFreeCoinsFacebook();
					PlayerPrefs.SetInt("freeFacebook", 1);
					if (!Application.isEditor && FacebookSupported())
					{
						int @int = Storager.getInt(Defs.Coins, false);
						Storager.setInt(Defs.Coins, @int + 5, false);
						InitFacebook();
					}
				}
			}
			else
			{
				GUI.DrawTexture(new Rect((float)Screen.width * 0.74f - (float)f_Free.normal.background.width * 0.5f * num, (float)Screen.height * 0.5f - (float)f_Free.normal.background.height * num * 1.5f - num2, (float)f_Free.normal.background.width * num, (float)f_Free.normal.background.height * num), freeFaceOff);
			}
		}
		if (true)
		{
			if (PlayerPrefs.GetInt("freeTwitter", 0) == 0)
			{
				if (GUI.Button(new Rect((float)Screen.width * 0.74f - (float)f_Free.normal.background.width * 0.5f * num, (float)Screen.height * 0.5f - (float)f_Free.normal.background.height * num * 0.5f, (float)f_Free.normal.background.width * num, (float)f_Free.normal.background.height * num), string.Empty, t_Free))
				{
					PlayerPrefs.SetInt("freeTwitter", 1);
					FlurryPluginWrapper.LogTwitter();
					FlurryPluginWrapper.LogFreeCoinsTwitter();
					if (!Application.isEditor)
					{
						int int2 = Storager.getInt(Defs.Coins, false);
						Storager.setInt(Defs.Coins, int2 + 5, false);
						InitTwitter();
					}
				}
			}
			else
			{
				GUI.DrawTexture(new Rect((float)Screen.width * 0.74f - (float)f_Free.normal.background.width * 0.5f * num, (float)Screen.height * 0.5f - (float)f_Free.normal.background.height * num * 0.5f, (float)f_Free.normal.background.width * num, (float)f_Free.normal.background.height * num), freeTwitterOff);
			}
		}
		if (PlayerPrefs.GetInt("freeTube", 0) == 0)
		{
			Rect position2 = new Rect((float)Screen.width * 0.74f - (float)f_Free.normal.background.width * num * 0.5f, (float)Screen.height * 0.5f + (float)f_Free.normal.background.height * num * 0.5f + num2, (float)f_Free.normal.background.width * num, (float)f_Free.normal.background.height * num);
			if (GUI.Button(position2, string.Empty, tube_free))
			{
				FlurryPluginWrapper.LogFreeCoinsYoutube();
				PlayerPrefs.SetInt("freeTube", 1);
				int int3 = Storager.getInt(Defs.Coins, false);
				Storager.setInt(Defs.Coins, int3 + 3, false);
				Application.OpenURL("http://pixelgun3d.com/watch.html");
			}
		}
		else
		{
			GUI.DrawTexture(new Rect((float)Screen.width * 0.74f - (float)f_Free.normal.background.width * 0.5f * num, (float)Screen.height * 0.5f + (float)f_Free.normal.background.height * num * 0.5f + num2, (float)f_Free.normal.background.width * num, (float)f_Free.normal.background.height * num), freeTubeOff);
		}
		if (GUI.Button(new Rect((float)Screen.width * 0.74f - (float)f_Free.normal.background.width * 0.5f * num, (float)Screen.height - (21f + (float)backBut.normal.background.height) * num, (float)rate_Free.normal.background.width * num, (float)rate_Free.normal.background.height * num), string.Empty, rate_Free))
		{
			FlurryPluginWrapper.LogFreeCoinsRateUs();
			string applicationUrl = Defs.ApplicationUrl;
			Application.OpenURL(applicationUrl);
		}
		if (GUI.Button(new Rect(21f * num, (float)Screen.height - (21f + (float)backBut.normal.background.height) * num, (float)backBut.normal.background.width * num, (float)backBut.normal.background.height * num), string.Empty, backBut))
		{
			showFreeCoins = false;
			expController.isShowRanks = true;
		}
	}

	private void DrawDeadMatch()
	{
		if (!(_shopInstance != null))
		{
			float num = (float)Screen.height / 768f;
			float num2 = 30f * num;
			float num3 = 682f * num;
			Rect rect = new Rect(0.5f * ((float)Screen.width - num * 1366f), 0f, num * 1366f, num * 768f);
			GUI.DrawTexture(new Rect(0.5f * ((float)Screen.width - (float)head_worldwide.width * num), 0f, (float)head_worldwide.width * num, (float)head_worldwide.height * num), head_worldwide);
			if (GUI.Button(new Rect((float)Screen.width * 0.74f - (float)worldwideBut.normal.background.width * num * 0.5f, (float)Screen.height * 0.5f - (float)worldwideBut.normal.background.height * num - num2 * 0.5f, (float)worldwideBut.normal.background.width * num, (float)worldwideBut.normal.background.height * num), string.Empty, worldwideBut))
			{
			}
			if (GUI.Button(new Rect((float)Screen.width * 0.74f - (float)localBut.normal.background.width * num * 0.5f, (float)Screen.height * 0.5f + num2 * 0.5f, (float)localBut.normal.background.width * num, (float)localBut.normal.background.height * num), string.Empty, localBut))
			{
			}
			if (GUI.Button(new Rect(21f * num, (float)Screen.height - (21f + (float)backBut.normal.background.height) * num, (float)backBut.normal.background.width * num, (float)backBut.normal.background.height * num), string.Empty, backBut))
			{
				isShowDeadMatch = false;
			}
		}
	}

	private void DrawCOOP()
	{
		if (!(_shopInstance != null))
		{
			float num = (float)Screen.height / 768f;
			float num2 = 30f * num;
			float num3 = 682f * num;
			Rect rect = new Rect(0.5f * ((float)Screen.width - num * 1366f), 0f, num * 1366f, num * 768f);
			GUI.DrawTexture(new Rect(0.5f * ((float)Screen.width - (float)head_worldwide.width * num), 0f, (float)head_worldwide.width * num, (float)head_worldwide.height * num), head_COOP);
			if (GUI.Button(new Rect((float)Screen.width * 0.74f - (float)timeSurvivalBut.normal.background.width * num * 0.5f, (float)Screen.height * 0.5f - (float)timeSurvivalBut.normal.background.height * num * 1.5f - num2, (float)timeSurvivalBut.normal.background.width * num, (float)timeSurvivalBut.normal.background.height * num), string.Empty, timeSurvivalBut))
			{
			}
			if (GUI.Button(new Rect((float)Screen.width * 0.74f - (float)teamFight.normal.background.width * num * 0.5f, (float)Screen.height * 0.5f - (float)teamFight.normal.background.height * num * 0.5f, (float)teamFight.normal.background.width * num, (float)teamFight.normal.background.height * num), string.Empty, teamFight))
			{
			}
			GUI.DrawTexture(new Rect((float)Screen.width * 0.74f - (float)bossBattle.width * num * 0.5f, (float)Screen.height * 0.5f + (float)bossBattle.height * num * 0.5f + num2, (float)bossBattle.width * num, (float)bossBattle.height * num), bossBattle);
			if (GUI.Button(new Rect(21f * num, (float)Screen.height - (21f + (float)backBut.normal.background.height) * num, (float)backBut.normal.background.width * num, (float)backBut.normal.background.height * num), string.Empty, backBut))
			{
				isShowCOOP = false;
			}
		}
	}

	private void DrawVersionLabel()
	{
		if (_version == null)
		{
			_version = Application.version; //.GetExecutingAssembly().GetName().Version.ToString();
		}
		if (_versionLabelStyle == null)
		{
			_versionLabelStyle = new GUIStyle(GUI.skin.label)
			{
				fontSize = Convert.ToInt32(24f * Defs.Coef),
				normal = new GUIStyleState
				{
					textColor = Color.white
				},
				alignment = TextAnchor.UpperRight
			};
			Font font = Resources.Load("Ponderosa") as Font;
			if (font != null)
			{
				_versionLabelStyle.font = font;
			}
		}
		if (_version.StartsWith("0"))
		{
			Debug.LogWarning("Invalid version: " + _version);
		}
		else
		{
			GUI.Label(new Rect(0f, 21f * Defs.Coef, (float)Screen.width - 21f * Defs.Coef, 32f * Defs.Coef), _version, _versionLabelStyle);
		}
	}

	private void showSetting()
	{
		GUI.depth = 2;
		DrawVersionLabel();
		float num = (float)Screen.height / 768f;
		GUI.DrawTexture(new Rect((float)Screen.width - (21f + (float)settingPlashka.width) * num, 160f * num, (float)settingPlashka.width * num, (float)settingPlashka.height * num), settingPlashka);
		GUI.DrawTexture(new Rect((float)Screen.width / 2f - (float)settingTitle.width / 2f * Defs.Coef, (float)Screen.height * 0.02f, (float)settingTitle.width * Defs.Coef, (float)settingTitle.height * Defs.Coef), settingTitle);
		musicOld = Defs.isSoundMusic;
		fxOld = Defs.isSoundFX;
		float left = (float)Screen.width - 262f * Defs.Coef;
		Rect position = new Rect(left, 194f * num, (float)soundOnOff.normal.background.width * num, (float)soundOnOff.normal.background.height * num);
		bool isSoundMusic = Defs.isSoundMusic;
		Defs.isSoundMusic = GUI.Toggle(position, Defs.isSoundMusic, string.Empty, soundOnOff);
		if (isSoundMusic != Defs.isSoundMusic)
		{
			PlayerPrefsX.SetBool(PlayerPrefsX.SoundMusicSetting, Defs.isSoundMusic);
			PlayerPrefs.Save();
		}
		Rect position2 = new Rect(left, 269f * num, (float)soundOnOff.normal.background.width * num, (float)soundOnOff.normal.background.height * num);
		bool isSoundFX = Defs.isSoundFX;
		Defs.isSoundFX = GUI.Toggle(position2, Defs.isSoundFX, string.Empty, soundOnOff);
		if (isSoundFX != Defs.isSoundFX)
		{
			PlayerPrefsX.SetBool(PlayerPrefsX.SoundFXSetting, Defs.isSoundFX);
			PlayerPrefs.Save();
		}
		if (musicOld != Defs.isSoundMusic)
		{
			if (Defs.isSoundMusic)
			{
                FindObjectOfType<MenuBackgroundMusic>().Play();
			}
			else
			{
				FindObjectOfType<MenuBackgroundMusic>().Stop();
			}
		}
		Rect position3 = new Rect(left, 344f * num, (float)soundOnOff.normal.background.width * num, (float)soundOnOff.normal.background.height * num);
		bool flag = GUI.Toggle(position3, chatIsOn, string.Empty, soundOnOff);
		if (flag != chatIsOn)
		{
			chatIsOn = flag;
			PlayerPrefs.SetInt("ChatOn", chatIsOn ? 1 : 0);
			PlayerPrefs.Save();
		}
		Rect position4 = new Rect(left, 524f * num, (float)soundOnOff.normal.background.width * num, (float)soundOnOff.normal.background.height * num);
		bool flag2 = GUI.Toggle(position4, invCur, string.Empty, soundOnOff);
		if (invCur != flag2)
		{
			invCur = flag2;
			PlayerPrefs.SetInt(Defs.InvertCamSN, invCur ? 1 : 0);
			PlayerPrefs.Save();
		}
		if (GUI.Button(new Rect(21f * num, (float)Screen.height - (21f + (float)backBut.normal.background.height) * num, (float)backBut.normal.background.width * num, (float)backBut.normal.background.height * num), string.Empty, backBut))
		{
			isShowSetting = false;
			expController.isShowRanks = true;
		}
		GUI.enabled = !StoreKitEventListener.purchaseInProcess;
		Rect position5 = new Rect((float)Screen.width - (21f + (float)restore.normal.background.width) * num, (float)Screen.height - (21f + (float)restore.normal.background.height) * num, (float)restore.normal.background.width * num, (float)restore.normal.background.height * num);
		if (GUI.Button(position5, string.Empty, restore))
		{
			StoreKitEventListener.purchaseInProcess = true;
		}
		GUI.enabled = true;
		sliderStyle.fixedWidth = (float)slow_fast.width * num;
		sliderStyle.fixedHeight = (float)slow_fast.height * num;
		thumbStyle.fixedWidth = (float)polzunok.width * num;
		thumbStyle.fixedHeight = (float)polzunok.height * num;
		Rect position6 = new Rect((float)Screen.width - (21f + (float)settingPlashka.width * 0.5f) * num - (float)slow_fast.width * 0.5f * num, 469f * num, (float)slow_fast.width * num, (float)slow_fast.height * num);
		float value = GUI.HorizontalSlider(position6, PlayerPrefs.GetFloat("SensitivitySett", 12f), 6f, 18f, sliderStyle, thumbStyle);
		PlayerPrefs.SetFloat("SensitivitySett", value);
	}

	private void OnGUI()
	{
		if (expController == null) expController = FindObjectOfType<ExperienceController>();
		if (expController.isShowAdd)
		{
			GUI.enabled = false;
		}
		if (isInappWinOpen)
		{
			return;
		}
		horizontal = (float)(playStyle.normal.background.width - skinsMakerStyle.normal.background.width * 2) * Defs.Coef;
		float num = (float)Screen.height / 768f;
		if (showFreeCoins)
		{
			DrawFreeCoins();
		}
		else if (isShowDeadMatch)
		{
			DrawDeadMatch();
		}
		else if (isShowCOOP)
		{
			DrawCOOP();
		}
		else if (isShowSetting)
		{
			showSetting();
		}
		else
		{
			Rect position = new Rect(0.5f * ((float)Screen.width - num * (float)fon.width), 0f, num * (float)fon.width, num * (float)fon.height);
			GUI.DrawTexture(position, fon, ScaleMode.StretchToFill);
			float num2 = 133f * num;
			Rect position2 = new Rect(0f, (float)Screen.height - num2, Screen.width, num2);
			GUI.DrawTexture(position2, bottomShadow, ScaleMode.StretchToFill);
			float rightBorder = (float)Screen.width - 21f * num;
			DrawGameModeButtons(rightBorder);
			DrawToolbar(rightBorder);
		}
		bestScoreStyle.fontSize = Mathf.RoundToInt((float)Screen.height * 0.04f);
		labelStyle.fontSize = Mathf.RoundToInt((float)Screen.height * 0.04f);
		if (showMessagFacebook)
		{
			labelStyle.fontSize = Player_move_c.FontSizeForMessages;
			GUI.Label(Player_move_c.SuccessMessageRect(), _SocialSentSuccess("Facebook"), labelStyle);
		}
		if (showMessagTiwtter)
		{
			labelStyle.fontSize = Player_move_c.FontSizeForMessages;
			GUI.Label(Player_move_c.SuccessMessageRect(), _SocialSentSuccess("Twitter"), labelStyle);
		}
		if (Time.realtimeSinceStartup - _timeWhenPurchShown >= 2f)
		{
			productPurchased = false;
		}
		if (productPurchased)
		{
			labelStyle.fontSize = FontSizeForMessages;
			GUI.Label(Player_move_c.SuccessMessageRect(), "Purchase was successful", labelStyle);
		}
		if (shopButtonStyle != null)
		{
			Rect position3 = new Rect(21f * Defs.Coef, (42f + (float)shopButtonStyle.normal.background.height) * Defs.Coef, (float)shopButtonStyle.normal.background.width * Defs.Coef, (float)shopButtonStyle.normal.background.height * Defs.Coef);
			if (!showFreeCoins && !isShowSetting && GUI.Button(position3, string.Empty, shopButtonStyle))
			{
				HandleShopButton();
			}
		}
		else
		{
			Debug.LogWarning("shopButtonStyle == null");
		}
		if (_shopInstance != null)
		{
			_shopInstance.SetHatsAndCapesEnabled(true);
			_shopInstance.SetGearCatEnabled(false);
			_shopInstance.ShowShop(true);
		}
		GUI.enabled = true;
	}

	private string _SocialMessage()
	{
		string applicationUrl = Defs.ApplicationUrl;
		return "Come and play with me in epic multiplayer shooter - Pixel Gun 3D! " + applicationUrl;
	}

	private string _SocialSentSuccess(string SocialName)
	{
		return "Your best score was sent to " + SocialName;
	}

	private void completionHandler(string error, object result)
	{
		if (error != null)
		{
			Debug.LogError(error);
		}
		else
		{
			Utils.logObject(result);
		}
	}

	private void facebookGraphReqCompl(object result)
	{
		Utils.logObject(result);
	}

	private void facebookSessionOpened()
	{
		_hasPublishPermission = ServiceLocator.FacebookFacade.GetSessionPermissions().Contains("publish_stream");
		_hasPublishActions = ServiceLocator.FacebookFacade.GetSessionPermissions().Contains("publish_actions");
	}

	private void facebookreauthorizationSucceededEvent()
	{
		_hasPublishPermission = ServiceLocator.FacebookFacade.GetSessionPermissions().Contains("publish_stream");
		_hasPublishActions = ServiceLocator.FacebookFacade.GetSessionPermissions().Contains("publish_actions");
	}

	private void addExp()
	{
		GameObject.FindGameObjectWithTag("ExperienceController").GetComponent<ExperienceController>().addExperience(20);
		Invoke("addExp", 10f);
	}

	private void Start()
	{
		chatIsOn = PlayerPrefs.GetInt("ChatOn", 1) == 1;
		invCur = PlayerPrefs.GetInt(Defs.InvertCamSN, 0) == 1;
		if (!Storager.hasKey(Defs.CoinsAfterTrainingSN))
		{
			Storager.setInt(Defs.CoinsAfterTrainingSN, 0, false);
		}
		if (Storager.getInt(Defs.CoinsAfterTrainingSN, false) == 1)
		{
			Storager.setInt(Defs.CoinsAfterTrainingSN, 0, false);
			int @int = Storager.getInt(Defs.Coins, false);
			Storager.setInt(Defs.Coins, @int + 15, false);
			AudioClip clip = Resources.Load("coin_get") as AudioClip;
			if (PlayerPrefsX.GetBool(PlayerPrefsX.SoundFXSetting, true))
			{
				NGUITools.PlaySound(clip);
			}
			FindObjectOfType<ExperienceController>().addExperience(15);
		}
		expController = GameObject.FindObjectOfType<ExperienceController>();
		expController.isShowRanks = true;
		expController.posRanks = new Vector2((21f + (float)profileBut.normal.background.width) * Defs.Coef + 16f * Defs.Coef, (float)Screen.height - 107f * Defs.Coef);
		PerformSkinsMakerQueryIfLiteEdition();
		string @string = PlayerPrefs.GetString(Defs.ShouldReoeatActionSett, string.Empty);
		if (@string.Equals(Defs.GoToProfileAction))
		{
			PlayerPrefs.SetString(Defs.ShouldReoeatActionSett, string.Empty);
			PlayerPrefs.Save();
		}
		else if (!@string.Equals(Defs.GoToSkinsMakerAction))
		{
		}
		InitFacebookEvents();
		Storager.setInt(Defs.EarnedCoins, 0, false);
		if (!Application.isEditor)
		{
			ServiceLocator.FacebookFacade.Init();
			FacebookSessionLoginBehavior sessionLoginBehavior = FacebookSessionLoginBehavior.SSO_WITH_FALLBACK;
			ServiceLocator.FacebookFacade.SetSessionLoginBehavior(sessionLoginBehavior);
		}
		Invoke("setEnabledGUI", 0.1f);
		if (GlobalGameController.isFullVersion)
		{
			PlayerPrefs.SetInt("FullVersion", 1);
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
		PlayerPrefs.SetInt("typeConnect__", 0);
		productIdentifiers = StoreKitEventListener.idsForFull;
		if (!GameObject.FindGameObjectWithTag("SkinsManager") && (bool)skinsManagerPrefab)
		{
			UnityEngine.Object.Instantiate(skinsManagerPrefab, Vector3.zero, Quaternion.identity);
		}
		if (!GameObject.FindGameObjectWithTag("WeaponManager") && (bool)weaponManagerPrefab)
		{
			UnityEngine.Object.Instantiate(weaponManagerPrefab, Vector3.zero, Quaternion.identity);
		}
		GlobalGameController.ResetParameters();
		GlobalGameController.Score = 0;
		_inAppGameObject = GameObject.FindGameObjectWithTag("InAppGameObject");
		_listener = _inAppGameObject.GetComponent<StoreKitEventListener>();
		if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
		{
			GoogleIABManager.purchaseSucceededEvent += purchaseSuccessful;
		}
		if (!Application.isEditor && iOSVersion > 5f)
		{
			FacebookManager.graphRequestCompletedEvent += facebookGraphReqCompl;
			FacebookManager.sessionOpenedEvent += facebookSessionOpened;
			FacebookManager.reauthorizationSucceededEvent += facebookreauthorizationSucceededEvent;
			_canUserUseFacebookComposer = ServiceLocator.FacebookFacade.CanUserUseFacebookComposer();
		}
		if (PlayerPrefs.GetInt(Defs.ShouldEnableShopSN, 0) == 1)
		{
			PlayerPrefs.SetInt(Defs.ShouldEnableShopSN, 0);
			PlayerPrefs.Save();
			HandleShopButton();
		}
	}

	private void SetInApp()
	{
		isInappWinOpen = !isInappWinOpen;
		expController.isShowRanks = !isInappWinOpen;
		if (isInappWinOpen)
		{
			Shop.sharedShop.loadShopCategories();
			if (StoreKitEventListener.restoreInProcess)
			{
				_purchaseActivityIndicator.SetActive(true);
			}
			if (PlayerPrefs.GetInt("MultyPlayer") != 1)
			{
				Time.timeScale = 0f;
			}
		}
		else
		{
			Shop.sharedShop.unloadShopCategories();
			if (_purchaseActivityIndicator == null)
			{
				Debug.LogWarning("SetInApp(): _purchaseActivityIndicator is null.");
			}
			else
			{
				_purchaseActivityIndicator.SetActive(false);
			}
		}
	}

	private void InitTwitter()
	{
		Debug.Log("InitTwitter(): init");
		string empty = string.Empty;
		string empty2 = string.Empty;
		if (GlobalGameController.isFullVersion)
		{
			empty = "cuMbTHM8izr9Mb3bIfcTxA";
			empty2 = "mpTLWIku4kIaQq7sTTi91wRLlvAxADhalhlEresnuI";
		}
		else
		{
			empty = "Jb7CwCaMgCQQiMViQRNHw";
			empty2 = "zGVrax4vqgs3CYf04O7glsoRbNT3vhIafte6lfm8w";
		}
		ServiceLocator.TwitterFacade.Init(empty, empty2);
		if (!ServiceLocator.TwitterFacade.IsLoggedIn())
		{
			TwitterLogin();
		}
		else
		{
			TwitterPost();
		}
	}

	private void TwitterLogin()
	{
		TwitterManager.loginSucceededEvent += OnTwitterLogin;
		TwitterManager.loginFailedEvent += OnTwitterLoginFailed;
		ServiceLocator.TwitterFacade.ShowLoginDialog();
	}

	private void OnTwitterLogin(string s)
	{
		TwitterManager.loginSucceededEvent -= OnTwitterLogin;
		TwitterManager.loginFailedEvent -= OnTwitterLoginFailed;
		TwitterPost();
	}

	private void OnTwitterLoginFailed(string error)
	{
		TwitterManager.loginSucceededEvent -= OnTwitterLogin;
		TwitterManager.loginFailedEvent -= OnTwitterLoginFailed;
	}

	private void TwitterPost()
	{
		TwitterManager.requestDidFinishEvent += OnTwitterPost;
		ServiceLocator.TwitterFacade.PostStatusUpdate(_SocialMessage());
	}

	private void OnTwitterPost(object result)
	{
		if (result != null)
		{
			TwitterManager.requestDidFinishEvent -= OnTwitterPost;
			showMessagTiwtter = true;
			Invoke("hideMessagTwitter", 3f);
		}
	}

	private void OnTwitterPostFailed(string _error)
	{
		TwitterManager.requestDidFinishEvent -= OnTwitterPost;
	}

	private void Update()
	{
		if (Application.platform == RuntimePlatform.Android && Input.GetKey(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

	private void setEnabledGUI()
	{
		isFirstFrame = false;
	}

	private bool FacebookSupported()
	{
		return (Application.platform != RuntimePlatform.IPhonePlayer) ? (Application.platform == RuntimePlatform.Android) : (iOSVersion > 5f);
	}

	private void InitFacebook()
	{
		if (!Application.isEditor)
		{
			clickButtonFacebook = true;
			if (!ServiceLocator.FacebookFacade.IsSessionValid())
			{
				Debug.Log("Facebook: !isSessionValid");
				string[] permissions = new string[1] { "email" };
				ServiceLocator.FacebookFacade.LoginWithReadPermissions(permissions);
			}
			else
			{
				Debug.Log("Facebook: isSessionValid");
				OnEventFacebookLogin();
			}
		}
	}

	private void InitFacebookEvents()
	{
		FacebookManager.reauthorizationSucceededEvent += OnEventFacebookLogin;
		FacebookManager.loginFailedEvent += OnEventFacebookLoginFailed;
		FacebookManager.sessionOpenedEvent += OnEventFacebookLogin;
	}

	private void CleanFacebookEvents()
	{
		FacebookManager.reauthorizationSucceededEvent -= OnEventFacebookLogin;
		FacebookManager.loginFailedEvent -= OnEventFacebookLoginFailed;
		FacebookManager.sessionOpenedEvent -= OnEventFacebookLogin;
	}

	private void OnEventFacebookLogin()
	{
		if (Application.isEditor || !clickButtonFacebook)
		{
			return;
		}
		Debug.Log("OnEventFacebookLogin");
		if (!ServiceLocator.FacebookFacade.IsSessionValid())
		{
			return;
		}
		if (_hasPublishPermission)
		{
			Debug.Log("sendMessag");
			clickButtonFacebook = false;
			showMessagFacebook = true;
			Invoke("hideMessag", 3f);
			Facebook.instance.postMessage(_SocialMessage(), completionHandler);
			return;
		}
		Debug.Log("poluchau permissions");
		string[] permissions = new string[2] { "publish_actions", "publish_stream" };
		ServiceLocator.FacebookFacade.ReauthorizeWithPublishPermissions(permissions, FacebookSessionDefaultAudience.Everyone);
		if (Application.platform == RuntimePlatform.Android)
		{
			_hasPublishPermission = true;
			_hasPublishActions = true;
		}
	}

	private void OnEventFacebookLoginFailed(P31Error error)
	{
		clickButtonFacebook = false;
		Debug.Log("OnEventFacebookLoginFailed=" + error);
	}

	private void purchaseSuccessful(GooglePurchase purchase)
	{
	}

	private void OnDestroy()
	{
		if (expController != null)
		{
			expController.isShowRanks = false;
		}
		FacebookManager.graphRequestCompletedEvent -= facebookGraphReqCompl;
		FacebookManager.sessionOpenedEvent -= facebookSessionOpened;
		FacebookManager.reauthorizationSucceededEvent -= facebookreauthorizationSucceededEvent;
		if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
		{
			GoogleIABManager.purchaseSucceededEvent -= purchaseSuccessful;
		}
		CleanFacebookEvents();
	}

	private void hideMessag()
	{
		showMessagFacebook = false;
	}

	private void hideMessagTwitter()
	{
		showMessagTiwtter = false;
	}
}
