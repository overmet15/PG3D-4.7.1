using System.Collections.Generic;
using Prime31;
using UnityEngine;

public class FlurryPluginWrapper : MonoBehaviour
{
	public const string BackToMainMenu = "Back to Main Menu";

	public static string ModeEnteredEvent = "ModeEnteredEvent";

	public static string MapEnteredEvent = "MapEnteredEvent";

	public static string MapNameParameter = "MapName";

	public static string ModeParameter = "Mode";

	public static string ModePressedEvent = "ModePressed";

	public static string SocialEventName = "Post to Social";

	public static string SocialParName = "Service";

	public static string AppVersionParameter = "App_version";

	public static string MultiplayeLocalEvent = "Local Button Pressed";

	public static string MultiplayerWayDeaathmatchEvent = "Way_to_start_multiplayer_DEATHMATCH";

	public static string MultiplayerWayCOOPEvent = "Way_to_start_multiplayer_COOP";

	public static string MultiplayerWayCompanyEvent = "Way_to_start_multiplayer_Company";

	public static string WayName = "Button";

	public static readonly string HatsCapesShopPressedEvent = "Hats_Capes_Shop";

	public static string FreeCoinsEv = "FreeCoins";

	public static string FreeCoinsParName = "type";

	public static string RateUsEv = "Rate_Us";

	private readonly IDictionary<Defs.RuntimeAndroidEdition, string> _flurryApiKeys = new Dictionary<Defs.RuntimeAndroidEdition, string>
	{
		{
			Defs.RuntimeAndroidEdition.Amazon,
			"BQ44Q6FXJNJD376D3JJ2"
		},
		{
			Defs.RuntimeAndroidEdition.GoogleLite,
			"8XTSMP4SBR49B4FXHKKP"
		},
		{
			Defs.RuntimeAndroidEdition.GooglePro,
			"4BBDK6R8NYS889PSG46J"
		}
	};

	public static string MultiplayerWayEvent
	{
		get
		{
			return (PlayerPrefs.GetInt("COOP", 0) == 1) ? MultiplayerWayCOOPEvent : ((PlayerPrefs.GetInt("company", 0) != 1) ? MultiplayerWayDeaathmatchEvent : MultiplayerWayCompanyEvent);
		}
	}

	public static void LogLevelPressed(string n)
	{
		FlurryAndroid.logEvent(n + "Pressed", false);
	}

	public static void LogBoxOpened(string nm)
	{
		FlurryAndroid.logEvent(nm + "_Box_Opened", false);
	}

	public static void LogEventWithParameterAndValue(string ev, string pat, string val)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add(pat, val);
		Dictionary<string, string> parameters = dictionary;
		FlurryAndroid.logEvent(ev, parameters, false);
	}

	public static void LogEvent(string eventName)
	{
		FlurryAndroid.logEvent(eventName, false);
	}

	public static void LogModeEventWithValue(string val)
	{
		if (!PlayerPrefs.HasKey("Mode Pressed First Time"))
		{
			PlayerPrefs.SetInt("Mode Pressed First Time", 0);
			LogEventWithParameterAndValue("Mode Pressed First Time", ModeParameter, val);
		}
		else
		{
			LogEventWithParameterAndValue(ModePressedEvent, ModeParameter, val);
		}
	}

	public static void LogMultiplayerWayStart()
	{
		LogEventWithParameterAndValue(MultiplayerWayEvent, WayName, "Start");
		LogEvent("Start");
	}

	public static void LogMultiplayerWayQuckRandGame()
	{
		LogEventWithParameterAndValue(MultiplayerWayEvent, WayName, "Quick_rand_game");
		LogEvent("Random");
	}

	public static void LogMultiplayerWayCustom()
	{
		LogEventWithParameterAndValue(MultiplayerWayEvent, WayName, "Custom");
		LogEvent("Custom");
	}

	public static void LogDeathmatchModePress()
	{
		LogModeEventWithValue("Deathmatch");
		LogEvent("Deathmatch");
	}

	public static void LogCampaignModePress()
	{
		LogModeEventWithValue("Survival");
		LogEvent("Campaign");
	}

	public static void LogTrueSurvivalModePress()
	{
		LogModeEventWithValue("Arena_Survival");
		LogEvent("Survival");
	}

	public static void LogCooperativeModePress()
	{
		LogModeEventWithValue("COOP");
		LogEvent("Cooperative");
	}

	public static void LogSkinsMakerModePress()
	{
		LogEvent("Skins Maker");
	}

	public static void LogTwitter()
	{
		LogEventWithParameterAndValue(SocialEventName, SocialParName, "Twitter");
	}

	public static void LogFacebook()
	{
		LogEventWithParameterAndValue(SocialEventName, SocialParName, "Facebook");
	}

	public static void LogGamecenter()
	{
		LogEvent("Game Center");
	}

	public static void LogFreeCoinsFacebook()
	{
		LogEventWithParameterAndValue(FreeCoinsEv, FreeCoinsParName, "Facebook");
		LogEvent("Facebook");
	}

	public static void LogFreeCoinsTwitter()
	{
		LogEventWithParameterAndValue(FreeCoinsEv, FreeCoinsParName, "Twitter");
		LogEvent("Twitter");
	}

	public static void LogFreeCoinsYoutube()
	{
		LogEventWithParameterAndValue(FreeCoinsEv, FreeCoinsParName, "Youtube");
		LogEvent("YouTube");
	}

	public static void LogWaveReached(int waveNum)
	{
		FlurryAndroid.logEvent("Wave_" + waveNum, false);
	}

	public static void LogCoinEarned()
	{
		FlurryAndroid.logEvent("Earned Coin Survival", false);
	}

	public static void LogCoinEarned_COOP()
	{
		FlurryAndroid.logEvent("Earned Coin COOP", false);
	}

	public static void LogCoinEarned_Deathmatch()
	{
		FlurryAndroid.logEvent("Earned Coin Deathmatch", false);
	}

	public static void LogFreeCoinsRateUs()
	{
		FlurryAndroid.logEvent(RateUsEv, false);
	}

	public static void LogSkinsMakerEnteredEvent()
	{
		FlurryAndroid.logEvent("SkinsMaker", false);
	}

	public static void LogAddYourSkinBoughtEvent()
	{
		FlurryAndroid.logEvent("AddYourSkin_Bought", false);
	}

	public static void LogAddYourSkinTriedToBoughtEvent()
	{
		FlurryAndroid.logEvent("AddYourSkin_TriedToBought", false);
	}

	public static void LogAddYourSkinUsedEvent()
	{
		FlurryAndroid.logEvent("AddYourSkin_Used", false);
	}

	public static void LogMultiplayeLocalEvent()
	{
		LogEvent(MultiplayeLocalEvent);
	}

	public static void LogMultiplayeWorldwideEvent()
	{
		LogEvent("Worldwide");
	}

	public static void LogCategoryEnteredEvent(string catName)
	{
		LogEventWithParameterAndValue("Dhop_Category", "Category_name", catName);
	}

	public static void LogEnteringMap(int typeConnect, string mapName)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add(MapNameParameter, mapName);
		Dictionary<string, string> parameters = dictionary;
		string eventName = ((PlayerPrefs.GetInt("COOP", 0) != 0) ? "COOP" : "Deathmatch_WorldWide");
		FlurryAndroid.logEvent(eventName, parameters, false);
	}

	private void Start()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		string value;
		if (!_flurryApiKeys.TryGetValue(Defs.AndroidEdition, out value))
		{
			Debug.LogWarning("Flurry API key is not found!");
			value = string.Empty;
		}
		FlurryAndroid.onStartSession(value, false, false);
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void spaceDidDismissEvent(string space)
	{
		Debug.Log("spaceDidDismissEvent: " + space);
	}

	private void spaceWillLeaveApplicationEvent(string space)
	{
		Debug.Log("spaceWillLeaveApplicationEvent: " + space);
	}

	private void spaceDidFailToRenderEvent(string space)
	{
		Debug.Log("spaceDidFailToRenderEvent: " + space);
	}

	private void spaceDidReceiveAdEvent(string space)
	{
		Debug.Log("spaceDidReceiveAdEvent: " + space);
	}

	private void spaceDidFailToReceiveAdEvent(string space)
	{
		Debug.Log("spaceDidFailToReceiveAdEvent: " + space);
	}

	private void onCurrencyValueFailedToUpdateEvent(P31Error error)
	{
		Debug.LogError("onCurrencyValueFailedToUpdateEvent: " + error);
	}

	private void onCurrencyValueUpdatedEvent(string currency, float amount)
	{
		Debug.LogError("onCurrencyValueUpdatedEvent. currency: " + currency + ", amount: " + amount);
	}

	private void videoDidFinishEvent(string space)
	{
		Debug.Log("videoDidFinishEvent: " + space);
	}
}
