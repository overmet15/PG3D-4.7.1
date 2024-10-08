using System;
using System.Collections.Generic;
using System.Linq;
using Fuckhead.PixlGun3D;
using UnityEngine;

public sealed class StoreKitEventListener : MonoBehaviour
{
	public const string coin1 = "coin1";

	public const string coin2 = "coin2";

	public const string coin3 = "coin3.";

	public const string coin4 = "coin4";

	public const string coin5 = "coin5";

	public const string coin7 = "coin7";

	public const string coin8 = "coin8";

	private const int ConsuneFailureCountMax = 8;

	public const string bigAmmoPackID = "bigammopack";

	public const string crystalswordID = "crystalsword";

	public const string fullHealthID = "Fullhealth";

	public const string minerWeaponID = "MinerWeapon";

	internal readonly ICollection<IMarketProduct> _products = new List<IMarketProduct>();

	public readonly ICollection<GoogleSkuInfo> _skinProducts = new GoogleSkuInfo[0];

	public static bool billingSupported;

	public static readonly string[] coinIds;

	private static string[] _productIds;

	private HashSet<string> _productsToConsume = new HashSet<string>();

	private int _consumeFailureCount;

	private bool _hasCoins;

	public static string elixirSettName;

	public static string fullSettName;

	public static bool purchaseInProcess;

	public static bool restoreInProcess;

	public static GameObject purchaseActivityInd;

	public static string elixirID;

	public static string endmanskin;

	public static string chief;

	public static string spaceengineer;

	public static string nanosoldier;

	public static string steelman;

	public static string CaptainSkin;

	public static string HawkSkin;

	public static string GreenGuySkin;

	public static string TunderGodSkin;

	public static string GordonSkin;

	public static string animeGirl;

	public static string EMOGirl;

	public static string Nurse;

	public static string magicGirl;

	public static string braveGirl;

	public static string glamDoll;

	public static string kittyGirl;

	public static string famosBoy;

	public static string combatrifle;

	public static string goldeneagle;

	public static string magicbow;

	public static string fullVersion;

	public static string axe;

	public static string spas;

	public static string chainsaw;

	public static string famas;

	public static string glock;

	public static string scythe;

	public static string shovel;

	public static string crystalAxe;

	public static string steelAxe;

	public static string woodenBow;

	public static string chainsaw2;

	public static string steelCrossbow;

	public static string hammer2;

	public static string mace2;

	public static string sword_22;

	public static string staff2;

	public static string crystalGlock;

	public static string crystalSPAS;

	public static string hammer;

	public static string sword_2;

	public static string staff;

	public static string laser;

	public static string goldenPick;

	public static string crystalPick;

	public static string armor;

	public static string armor2;

	public static string armor3;

	public static string lightSword;

	public static string beretta;

	public static string mace;

	public static string minigun;

	public static string crossbow;

	public static string redMinigun;

	public static string crystalCrossbow;

	public static string redLightSaber;

	public static string sandFamas;

	public static string whiteBeretta;

	public static string blackEagle;

	public static string ironSword;

	public static string goldenSword;

	public static string goldenRedStone;

	public static string goldenSPAS;

	public static string goldenGlock;

	public static string tree;

	public static string fireAxe;

	public static string _3plShotgun;

	public static string revolver2;

	public static readonly string barrett;

	public static readonly string svd;

	public static readonly string[] skinIDs;

	public static readonly string[] idsForSingle;

	public static readonly string[] idsForMulti;

	public static readonly string[] idsForFull;

	public static readonly string[][] categoriesSingle;

	public static readonly string[][] categoriesMulti;

	public GameObject messagePrefab;

	public static string[] categoryNames;

	public AudioClip onEarnCoinsSound;

	static StoreKitEventListener()
	{
		elixirSettName = Defs.NumberOfElixirsSett;
		fullSettName = "FullVersion";
		purchaseInProcess = false;
		restoreInProcess = false;
		elixirID = ((!GlobalGameController.isFullVersion) ? "elixirlite" : "elixir");
		endmanskin = ((!GlobalGameController.isFullVersion) ? "endmanskinlite" : "endmanskin");
		chief = ((!GlobalGameController.isFullVersion) ? "chiefskinlite" : "chief");
		spaceengineer = ((!GlobalGameController.isFullVersion) ? "spaceengineerskinlite" : "spaceengineer");
		nanosoldier = ((!GlobalGameController.isFullVersion) ? "nanosoldierlite" : "nanosoldier");
		steelman = ((!GlobalGameController.isFullVersion) ? "steelmanlite" : "steelman");
		CaptainSkin = "captainskin";
		HawkSkin = "hawkskin";
		GreenGuySkin = "greenguyskin";
		TunderGodSkin = "thundergodskin";
		GordonSkin = "gordonskin";
		animeGirl = "animeGirl";
		EMOGirl = "EMOGirl";
		Nurse = "Nurse";
		magicGirl = "magicGirl";
		braveGirl = "braveGirl";
		glamDoll = "glamDoll";
		kittyGirl = "kittyGirl";
		famosBoy = "famosBoy";
		combatrifle = ((!GlobalGameController.isFullVersion) ? "combatriflelite" : "combatrifle");
		goldeneagle = ((!GlobalGameController.isFullVersion) ? "goldeneaglelite" : "goldeneagle");
		magicbow = ((!GlobalGameController.isFullVersion) ? "magicbowlite" : "magicbow");
		fullVersion = "extendedversion";
		axe = "axe";
		spas = "spas";
		chainsaw = "chainsaw";
		famas = "famas";
		glock = "glock";
		scythe = "scythe";
		shovel = "shovel";
		crystalAxe = "crystalAxe";
		steelAxe = "steelAxe";
		woodenBow = "woodenBow";
		chainsaw2 = "chainsaw2";
		steelCrossbow = "steelCrossbow";
		hammer2 = "hammer2";
		mace2 = "mace2";
		sword_22 = "sword_22";
		staff2 = "staff2";
		crystalGlock = "crystalGlock";
		crystalSPAS = "crystalSPAS";
		hammer = "hammer";
		sword_2 = "sword_2";
		staff = "staff";
		laser = "laser";
		goldenPick = "goldenPick";
		crystalPick = "crystalPick";
		armor = "armor";
		armor2 = "armor2";
		armor3 = "armor3";
		lightSword = "lightsword";
		beretta = "beretta";
		mace = "mace";
		minigun = "minigun";
		crossbow = "crossbow";
		redMinigun = "redMinigun";
		crystalCrossbow = "crystalCrossbow";
		redLightSaber = "redLightSaber";
		sandFamas = "sandFamas";
		whiteBeretta = "whiteBeretta";
		blackEagle = "blackEagle";
		ironSword = "ironSword";
		goldenSword = "goldenSword";
		goldenRedStone = "goldenRedStone";
		goldenSPAS = "goldenSPAS";
		goldenGlock = "goldenGlock";
		tree = "tree";
		fireAxe = "fireAxe";
		_3plShotgun = "_3plShotgun";
		revolver2 = "revolver2";
		barrett = "barrett";
		svd = "svd";
		categoryNames = new string[5] { "Premium", "Guns", "Melee", "Special", "Gear" };
		billingSupported = false;
		coinIds = new string[7] { "coin1", "coin7", "coin2", "coin3.", "coin4", "coin5", "coin8" };
		_productIds = new string[5] { "bigammopack", "Fullhealth", "crystalsword", "MinerWeapon", elixirID };
		skinIDs = new string[18]
		{
			endmanskin, chief, spaceengineer, nanosoldier, steelman, CaptainSkin, HawkSkin, GreenGuySkin, TunderGodSkin, GordonSkin,
			animeGirl, EMOGirl, Nurse, magicGirl, braveGirl, glamDoll, kittyGirl, famosBoy
		};
		idsForSingle = new string[11]
		{
			"bigammopack", "Fullhealth", ironSword, "MinerWeapon", steelAxe, spas, elixirID, glock, chainsaw, scythe,
			shovel
		};
		idsForMulti = new string[10]
		{
			idsForSingle[2],
			idsForSingle[3],
			steelAxe,
			woodenBow,
			combatrifle,
			spas,
			goldeneagle,
			idsForSingle[7],
			idsForSingle[8],
			famas
		};
		idsForFull = new string[1] { fullVersion };
		categoriesMulti = new string[5][]
		{
			new string[6] { sword_2, staff, laser, lightSword, hammer, beretta },
			new string[6]
			{
				idsForMulti[6],
				minigun,
				idsForMulti[3],
				idsForSingle[5],
				idsForSingle[7],
				idsForMulti[9]
			},
			new string[6]
			{
				idsForSingle[3],
				idsForSingle[4],
				idsForSingle[2],
				idsForSingle[8],
				mace,
				steelCrossbow
			},
			new string[6] { _3plShotgun, revolver2, fireAxe, tree, barrett, svd },
			new string[5]
			{
				idsForSingle[0],
				idsForSingle[1],
				armor,
				armor2,
				armor3
			}
		};
		categoriesSingle = new string[5][]
		{
			new string[6] { sword_2, staff, laser, lightSword, hammer, beretta },
			new string[6]
			{
				idsForMulti[6],
				minigun,
				idsForMulti[3],
				idsForSingle[5],
				idsForSingle[7],
				idsForMulti[9]
			},
			new string[6]
			{
				idsForSingle[3],
				idsForSingle[4],
				idsForSingle[2],
				idsForSingle[8],
				mace,
				steelCrossbow
			},
			new string[6] { _3plShotgun, revolver2, fireAxe, tree, barrett, svd },
			new string[5]
			{
				idsForSingle[0],
				idsForSingle[1],
				armor,
				armor2,
				armor3
			}
		};
	}

	private void Start()
	{
		GameObject gameObject = Resources.Load("ActivityIndicator") as GameObject;
		if (gameObject == null)
		{
			Debug.LogWarning("activityIndicatorPrefab == null");
		}
		else
		{
			purchaseActivityInd = UnityEngine.Object.Instantiate(gameObject) as GameObject;
		}
		if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
		{
			AmazonIAP.initiateItemDataRequest(coinIds);
			return;
		}
		string publicKey = ((Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite) ? "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA2fddncCpVwPU3m4ZzG8MfQTrxf3LBdjWwOV4LBRy2q4Kp/gPYi5QQaNJjsiQAbpIR51qSEJv9EomOi8+JZ4rWO52zOaLeumnKzpv++QVOllbGxaSwwSPDEZ0++eKmdsl5r+xzVvd20ey4n5tYotrRdYQfypZKYuHiMGvpsiIXf0rwv3yMNhVU7MDtbDgAs8zriBvPqCtkrRLnZdG+2dQEZ+hDPns0gO+N8y1V7odOHg4bDUceaK8al9DHcVKNItCMnOFyLHx++vKzHSLiXw2ojSUR1cfSbTkyyOTw9r9emHxxuGmc2/qWp7n/Qin1ksuAhyYFGOC9RClxxu1ygXKTQIDAQAB" : ((Defs.AndroidEdition != Defs.RuntimeAndroidEdition.GooglePro) ? string.Empty : "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAi1OdsmAFxtISsT+CTl7yl5Jim+qDnp21yI0K71wfK4TSOgHVn1nii4PAn+kOvDe+oqW78NPAv1CpTF8ES+9UCgJQKSIo2VCd2z6SWsE/PVtGvIBOvpywZObB9i0wXQt5Y4KPXOUVH+AONZrETgKLjr0rmG86pT2GNWH6LbNt6PSPaEkW9lQgEgEX/2g5lE8XySWr005MpwdJt1SD79eaMRhwECT9xcNmpn0tNZdBe49jPEHhK9a2OtvIeqJS7/zdqOT0kvO+dQEOUTZZ8GnfPeAGxqdZp4TA34yA1UZm321aIMOTZGQfyaxx+B3twjjpwtNDK1KuzdHzsgQNnJ3VMwIDAQAB"));
		//GoogleIAB.init(publicKey);
		//GoogleIAB.setAutoVerifySignatures(false);
	}

	private void OnEnable()
	{
		if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
		{
			AmazonIAPManager.onSdkAvailableEvent += HandleAmazonSdkAvailableEvent;
			AmazonIAPManager.onGetUserIdResponseEvent += HandleGetUserIdResponseEvent;
			AmazonIAPManager.itemDataRequestFinishedEvent += HandleItemDataRequestFinishedEvent;
			AmazonIAPManager.itemDataRequestFailedEvent += HandleItemDataRequestFailedEvent;
			AmazonIAPManager.purchaseSuccessfulEvent += HandlePurchaseSuccessfulEvent;
			AmazonIAPManager.purchaseFailedEvent += purchaseFailedEvent;
			AmazonIAPManager.purchaseUpdatesRequestSuccessfulEvent += HandlePurchaseUpdatesRequestSuccessfulEvent;
			AmazonIAPManager.purchaseUpdatesRequestFailedEvent += HandlePurchaseUpdatesRequestFailedEvent;
		}
		else
		{
			GoogleIABManager.billingSupportedEvent += billingSupportedEvent;
			GoogleIABManager.billingNotSupportedEvent += billingNotSupportedEvent;
			GoogleIABManager.queryInventorySucceededEvent += queryInventorySucceededEvent;
			GoogleIABManager.queryInventoryFailedEvent += queryInventoryFailedEvent;
			GoogleIABManager.purchaseCompleteAwaitingVerificationEvent += purchaseCompleteAwaitingVerificationEvent;
			GoogleIABManager.purchaseSucceededEvent += purchaseSucceededEvent;
			GoogleIABManager.purchaseFailedEvent += purchaseFailedEvent;
			GoogleIABManager.consumePurchaseSucceededEvent += consumePurchaseSucceededEvent;
			GoogleIABManager.consumePurchaseFailedEvent += consumePurchaseFailedEvent;
		}
	}

	private void OnDisable()
	{
		if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
		{
			AmazonIAPManager.onSdkAvailableEvent -= HandleAmazonSdkAvailableEvent;
			AmazonIAPManager.onGetUserIdResponseEvent -= HandleGetUserIdResponseEvent;
			AmazonIAPManager.itemDataRequestFinishedEvent -= HandleItemDataRequestFinishedEvent;
			AmazonIAPManager.itemDataRequestFailedEvent -= HandleItemDataRequestFailedEvent;
			AmazonIAPManager.purchaseSuccessfulEvent -= HandlePurchaseSuccessfulEvent;
			AmazonIAPManager.purchaseFailedEvent -= purchaseFailedEvent;
			AmazonIAPManager.purchaseUpdatesRequestSuccessfulEvent -= HandlePurchaseUpdatesRequestSuccessfulEvent;
			AmazonIAPManager.purchaseUpdatesRequestFailedEvent -= HandlePurchaseUpdatesRequestFailedEvent;
		}
		else
		{
			GoogleIABManager.billingSupportedEvent -= billingSupportedEvent;
			GoogleIABManager.billingNotSupportedEvent -= billingNotSupportedEvent;
			GoogleIABManager.queryInventorySucceededEvent -= queryInventorySucceededEvent;
			GoogleIABManager.queryInventoryFailedEvent -= queryInventoryFailedEvent;
			GoogleIABManager.purchaseCompleteAwaitingVerificationEvent -= purchaseCompleteAwaitingVerificationEvent;
			GoogleIABManager.purchaseSucceededEvent -= purchaseSucceededEvent;
			GoogleIABManager.purchaseFailedEvent -= purchaseFailedEvent;
			GoogleIABManager.consumePurchaseSucceededEvent -= consumePurchaseSucceededEvent;
			GoogleIABManager.consumePurchaseFailedEvent -= consumePurchaseFailedEvent;
		}
	}

	private void billingSupportedEvent()
	{
		billingSupported = true;
		Debug.Log("billingSupportedEvent");
		string[] array = _productIds.Concat(coinIds).ToArray();
		if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
		{
			AmazonIAP.initiateItemDataRequest(array);
		}
		else
		{
			GoogleIAB.queryInventory(array);
		}
	}

	private void billingNotSupportedEvent(string error)
	{
		billingSupported = false;
		Debug.LogWarning("billingNotSupportedEvent: " + error);
	}

	private void HandleAmazonSdkAvailableEvent(bool isSandboxMode)
	{
		Debug.Log("Amazon SDK available in sandbox mode: " + isSandboxMode);
		AmazonIAPManager.onSdkAvailableEvent -= HandleAmazonSdkAvailableEvent;
		billingSupported = true;
		string[] items = _productIds.Concat(coinIds).ToArray();
		AmazonIAP.initiateItemDataRequest(items);
	}

	private void HandleGetUserIdResponseEvent(string id)
	{
	}

	private void queryInventorySucceededEvent(List<GooglePurchase> purchases, List<GoogleSkuInfo> skus)
	{
		if (skus.Any((GoogleSkuInfo s) => s.productId == "skinsmaker"))
		{
			return;
		}
		string[] productIds = skus.Select((GoogleSkuInfo sku) => sku.productId).ToArray();
		string arg = string.Join(", ", productIds);
		string[] value = purchases.Select((GooglePurchase p) => p.productId).ToArray();
		string arg2 = string.Join(", ", value);
		string message = string.Format("queryInventorySucceededEvent;    Purchases: [{0}], Skus: [{1}]", arg2, arg);
		Debug.Log(message);
		IEnumerable<GoogleSkuInfo> source = skus.Where((GoogleSkuInfo s) => coinIds.Contains(s.productId));
		_hasCoins = source.Any();
		IEnumerable<GoogleMarketProduct> enumerable = skus.Where((GoogleSkuInfo s) => productIds.Contains(s.productId)).Select(MarketProductFactory.CreateGoogleMarketProduct);
		foreach (GoogleMarketProduct item in enumerable)
		{
			if (!_products.Contains(item))
			{
				_products.Add(item);
			}
		}
		foreach (GooglePurchase purchase in purchases)
		{
			if (purchase.productId == "MinerWeapon" || purchase.productId == "MinerWeapon".ToLower())
			{
				GameObject gameObject = GameObject.FindGameObjectWithTag("WeaponManager");
				if ((bool)gameObject)
				{
					gameObject.SendMessage("AddMinerWeaponToInventoryAndSaveInApp");
				}
			}
			else if (purchase.productId == "crystalsword")
			{
				GameObject gameObject2 = GameObject.FindGameObjectWithTag("WeaponManager");
				if ((bool)gameObject2)
				{
					gameObject2.SendMessage("AddSwordToInventoryAndSaveInApp");
				}
			}
			else
			{
				_productsToConsume.Add(purchase.productId);
			}
		}
		if (_productsToConsume.Any())
		{
			GoogleIAB.consumeProduct(_productsToConsume.First());
		}
		purchaseInProcess = false;
		restoreInProcess = false;
	}

	private void queryInventoryFailedEvent(string error)
	{
		Debug.LogWarning("Google: queryInventoryFailedEvent: " + error);
	}

	private void HandleItemDataRequestFinishedEvent(List<string> unavailableSkus, List<AmazonItem> availableItems)
	{
		try
		{
			string[] value = availableItems.Select((AmazonItem item) => item.sku).ToArray();
			string arg = string.Join(", ", value);
			string arg2 = string.Join(", ", unavailableSkus.ToArray());
			string message = string.Format("Item data request finished;    Unavailable skus: [{0}], Available skus: [{1}]", arg2, arg);
			Debug.Log(message);
			IEnumerable<AmazonItem> source = availableItems.Where((AmazonItem item) => coinIds.Contains(item.sku));
			_hasCoins = source.Any();
			IEnumerable<AmazonMarketProduct> enumerable = availableItems.Select(MarketProductFactory.CreateAmazonMarketProduct);
			foreach (AmazonMarketProduct item in enumerable)
			{
				if (!_products.Contains(item))
				{
					_products.Add(item);
				}
			}
		}
		finally
		{
			purchaseInProcess = false;
			restoreInProcess = false;
		}
	}

	private void HandleItemDataRequestFailedEvent()
	{
		Debug.LogWarning("Amamzon: Item data request failed.");
	}

	private void purchaseCompleteAwaitingVerificationEvent(string purchaseData, string signature)
	{
		Debug.Log("purchaseCompleteAwaitingVerificationEvent. purchaseData: " + purchaseData + ", signature: " + signature);
	}

	private void purchaseSucceededEvent(GooglePurchase purchase)
	{
		Debug.Log("Google: purchaseSucceededEvent: " + purchase);
		if (purchase.productId.Equals(elixirID))
		{
			PlayerPrefs.SetInt(elixirSettName, PlayerPrefs.GetInt(elixirSettName, 1) + 1);
			PlayerPrefs.Save();
		}
		else
		{
			int num = Array.IndexOf(coinIds, purchase.productId);
			if (num >= coinIds.GetLowerBound(0))
			{
				string message = string.Format("Process purchase {0}, coinInappsQuantity[{1}]", purchase.productId, num);
				Debug.Log(message);
				int val = Storager.getInt(Defs.Coins, false) + VirtualCurrencyHelper.coinInappsQuantity[num];
				Storager.setInt(Defs.Coins, val, false);
			}
		}
		string[] source = new string[3]
		{
			"MinerWeapon",
			"MinerWeapon".ToLower(),
			"crystalsword"
		};
		if (!source.Contains(purchase.productId) && Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
		{
			GoogleIAB.consumeProduct(purchase.productId);
		}
		purchaseInProcess = false;
		restoreInProcess = false;
	}

	private void purchaseFailedEvent(string error)
	{
		purchaseInProcess = false;
		Debug.LogWarning("purchaseFailedEvent: " + error);
		if (_productsToConsume.Any() && _consumeFailureCount < 8)
		{
			GoogleIAB.consumeProduct(_productsToConsume.Last());
		}
	}

	private void HandlePurchaseSuccessfulEvent(AmazonReceipt receipt)
	{
		Debug.Log("Amazon: purchaseSuccessfulEvent: " + receipt);
		try
		{
			int num = Array.IndexOf(coinIds, receipt.sku);
			if (num >= coinIds.GetLowerBound(0))
			{
				string message = string.Format("Process purchase {0}, coinInappsQuantity[{1}]", receipt.sku, num);
				Debug.Log(message);
				int val = Storager.getInt(Defs.Coins, false) + VirtualCurrencyHelper.coinInappsQuantity[num];
				Storager.setInt(Defs.Coins, val, false);
			}
		}
		finally
		{
			purchaseInProcess = false;
			restoreInProcess = false;
		}
	}

	private void consumePurchaseSucceededEvent(GooglePurchase purchase)
	{
		Debug.Log("consumePurchaseSucceededEvent: " + purchase);
		_productsToConsume.Remove(purchase.productId);
		if (_productsToConsume.Any())
		{
			GoogleIAB.consumeProduct(_productsToConsume.First());
		}
		else
		{
			_consumeFailureCount = 0;
		}
	}

	private void consumePurchaseFailedEvent(string error)
	{
		Debug.LogWarning("consumePurchaseFailedEvent: " + error);
		_consumeFailureCount++;
		if (_productsToConsume.Any() && _consumeFailureCount < 8)
		{
			GoogleIAB.consumeProduct(_productsToConsume.Last());
		}
	}

	private void HandlePurchaseUpdatesRequestSuccessfulEvent(List<string> revokedSkus, List<AmazonReceipt> receipts)
	{
	}

	private void HandlePurchaseUpdatesRequestFailedEvent()
	{
		Debug.LogWarning("Amazon: Purchase updates request failed.");
	}

	public bool HasCoins()
	{
		return _hasCoins;
	}

	public void ProvideContent()
	{
	}
}
