using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Shop : MonoBehaviour
{
	public int trainState = -1;

	public static Shop sharedShop = null;

	public static float backWidthMult = 0.05f;

	public static float backHeightMult = 0.81f;

	public Action resumeAction;

	public Action<string> buyAction;

	public Texture shopHead;

	public Texture shopFon;

	public GUIStyle[] catStyles;

	public GameObject message;

	private GUIStyle _style;

	private Texture2D _n;

	private Texture2D _a;

	private float _tm;

	private bool famasBought;

	private bool famasUpgradeBought;

	private Texture _infoTexture;

	public GUIStyle restoreStyle;

	public GUIStyle resumeStyle;

	public Texture[] categoryHeads;

	public GUIStyle resumeCategories;

	public GUIStyle restoreWindButStyle;

	private GameObject customDialogPrefab;

	private static string previewsDir = "ShopPreviews";

	private static string InAppCategoriesDir = "InAppCategories";

	private GUIStyle[] goods;

	private Texture preview;

	private string chosenId;

	private int currentCategory = -1;

	private GUIStyle buyCat = new GUIStyle();

	private GUIStyle upgradeCat = new GUIStyle();

	private GameObject _purchaseActivityIndicator;

	private bool _gearCategoryEnabled = true;

	public GUIStyle hatsAndCapesStyle;

	private bool _hatsCapesEnabled;

	private static readonly string[] restorableProducts = new string[3]
	{
		"crystalsword",
		"MinerWeapon",
		"MinerWeapon".ToLower()
	};

	private bool _stopBlinkGuns;

	private bool _stopBlinkFAMASBuy;

	private bool _stopBlinkBack;

	private bool _stopBlinkGear;

	private float _blinkPause = 0.5f;

	private Rect _RectBuy
	{
		get
		{
			return new Rect((float)Screen.width / 2f - (float)buyCat.normal.background.width / 2f * Defs.Coef, (float)Screen.height - Defs.BottomOffs * Defs.Coef - (float)buyCat.normal.background.height * Defs.Coef, (float)buyCat.normal.background.width * Defs.Coef, (float)buyCat.normal.background.height * Defs.Coef);
		}
	}

	private float _CatWidth
	{
		get
		{
			int num = ((!_gearCategoryEnabled) ? (catStyles.Length - 1) : catStyles.Length);
			return (float)Screen.width / (float)(num + 1);
		}
	}

	private float _CatHeight
	{
		get
		{
			return _CatWidth * 1f;
		}
	}

	private float _CatOffs
	{
		get
		{
			int num = ((!_gearCategoryEnabled) ? (catStyles.Length - 1) : catStyles.Length);
			float catWidth = _CatWidth;
			return ((float)Screen.width - catWidth * (float)num) / (float)(num + 1);
		}
	}

	public void SetHatsAndCapesEnabled(bool v)
	{
		_hatsCapesEnabled = v;
	}

	private void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		sharedShop = this;
		_LoadShopButtons();
		_purchaseActivityIndicator = StoreKitEventListener.purchaseActivityInd;
		if (_purchaseActivityIndicator == null)
		{
			Debug.LogWarning("Start(): _purchaseActivityIndicator is null.");
		}
		else
		{
			_purchaseActivityIndicator.SetActive(false);
		}
	}

	private void _LoadShopButtons()
	{
		buyCat.normal.background = Resources.Load(ResPath.Combine(InAppCategoriesDir, "weapons_buy")) as Texture2D;
		buyCat.active.background = Resources.Load(ResPath.Combine(InAppCategoriesDir, "weapons_buy_n")) as Texture2D;
		upgradeCat.normal.background = Resources.Load(ResPath.Combine(InAppCategoriesDir, "weapons_upgrade")) as Texture2D;
		upgradeCat.active.background = Resources.Load(ResPath.Combine(InAppCategoriesDir, "weapons_upgrade_n")) as Texture2D;
	}

	private int _CurrentNumberOfUpgrades(string id, out bool maxUpgrade)
	{
		List<string> list = new List<string>();
		int num = 0;
		if (WeaponManager.tagToStoreIDMapping.ContainsValue(id))
		{
			string key = WeaponManager.tagToStoreIDMapping.Where((KeyValuePair<string, string> pair) => pair.Value.Equals(id)).ElementAt(0).Key;
			list.Add(key);
			foreach (List<string> upgrade in UpgradeManager.upgrades)
			{
				if (upgrade.Contains(key))
				{
					list = upgrade;
					break;
				}
			}
			num = list.Count;
			int num2 = list.Count - 1;
			while (num2 >= 0)
			{
				string key2 = list[num2];
				string defName = WeaponManager.storeIDtoDefsSNMapping[WeaponManager.tagToStoreIDMapping[key2]];
				if (_HasntBoughtGood(defName))
				{
					num--;
					num2--;
					continue;
				}
				break;
			}
		}
		if (id.Equals(StoreKitEventListener.elixirID) && Defs.NumberOfElixirs > 0)
		{
			num++;
		}
		maxUpgrade = num == ((list.Count <= 0) ? 1 : list.Count);
		return num;
	}

	private bool _HasntBoughtGood(string defName)
	{
		if (Defs.IsTraining)
		{
			return (!defName.Equals(Defs.FAMASS) || !famasBought) && (!defName.Equals(Defs.SandFamasSN) || !famasUpgradeBought);
		}
		return Storager.getInt(defName, true) == 0;
	}

	private void _ReloadIcon(int i)
	{
		goods[i] = new GUIStyle();
		string[] array = ((PlayerPrefs.GetInt("MultyPlayer") == 1) ? StoreKitEventListener.categoriesMulti[currentCategory] : StoreKitEventListener.categoriesSingle[currentCategory]);
		string id = array[i];
		string text = _TagForId(id);
		bool maxUpgrade;
		string text2 = text + "_icon" + (1 + _CurrentNumberOfUpgrades(array[i], out maxUpgrade));
		string a = "Goods";
		goods[i].normal.background = Resources.Load(ResPath.Combine(a, text2)) as Texture2D;
		goods[i].onNormal.background = Resources.Load(ResPath.Combine(a, text2 + "_n")) as Texture2D;
	}

	private string _TagForId(string id)
	{
		string result = id;
		if (WeaponManager.tagToStoreIDMapping.ContainsValue(id))
		{
			result = WeaponManager.tagToStoreIDMapping.Where((KeyValuePair<string, string> pair) => pair.Value == id).ElementAt(0).Key;
		}
		return result;
	}

	private void _ReloadPreview()
	{
		string id = chosenId;
		string text = _TagForId(id);
		bool maxUpgrade;
		preview = Resources.Load(ResPath.Combine(previewsDir, text + "_preview" + (1 + _CurrentNumberOfUpgrades(chosenId, out maxUpgrade)))) as Texture;
	}

	private void _ReloadCategoryImages()
	{
		_UnloadCategory();
		string[] array = ((PlayerPrefs.GetInt("MultyPlayer") == 1) ? StoreKitEventListener.categoriesMulti[currentCategory] : StoreKitEventListener.categoriesSingle[currentCategory]);
		chosenId = array[0];
		goods = new GUIStyle[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			_ReloadIcon(i);
		}
		_ReloadPreview();
	}

	private void _UnloadCategory()
	{
		if (goods != null)
		{
			GUIStyle[] array = goods;
			foreach (GUIStyle gUIStyle in array)
			{
				gUIStyle.normal.background = null;
				gUIStyle.onNormal.background = null;
			}
		}
		goods = null;
		preview = null;
		Resources.UnloadUnusedAssets();
	}

	private void showCategory()
	{
		GUI.depth = 0;
		bool flag = false;
		GUI.enabled = !StoreKitEventListener.restoreInProcess && !flag;
		RestoreButton(flag);
		GUI.enabled = !StoreKitEventListener.restoreInProcess && !flag;
		_purchaseActivityIndicator.SetActive(StoreKitEventListener.restoreInProcess);
		string[] idsArr = ((PlayerPrefs.GetInt("MultyPlayer") == 1) ? StoreKitEventListener.categoriesMulti[currentCategory] : StoreKitEventListener.categoriesSingle[currentCategory]);
		float num = (float)goods[0].normal.background.width * Defs.Coef;
		float height = (float)goods[0].normal.background.height * Defs.Coef;
		float num2 = ((float)Screen.width - num * (float)goods.Length) / 2f;
		for (int i = 0; i < goods.Length; i++)
		{
			Rect position = new Rect(num2 + num * (float)i, (float)Screen.height * 0.16f - 10f * Defs.Coef, num, height);
			bool flag2 = GUI.enabled;
			GUI.enabled = !Defs.IsTraining || i == goods.Length - 1;
			bool flag3 = GUI.Toggle(position, idsArr[i].Equals(chosenId), string.Empty, goods[i]);
			GUI.enabled = flag2;
			if (flag3 && !idsArr[i].Equals(chosenId))
			{
				_ChooseItemAtIndex(i);
			}
		}
		GUI.enabled = !StoreKitEventListener.purchaseInProcess && !flag;
		float num3 = 958f * Defs.Coef;
		float num4 = 381f * Defs.Coef;
		Rect rectBuy = _RectBuy;
		Rect position2 = new Rect((float)Screen.width / 2f - num3 / 2f, rectBuy.y - num4 - 3f * Defs.Coef + 21f * Defs.Coef, num3, num4);
		GUI.DrawTexture(position2, preview);
		bool flag4 = false;
		bool maxUpgrade;
		int num5 = _CurrentNumberOfUpgrades(chosenId, out maxUpgrade);
		flag4 = maxUpgrade;
		bool flag5 = GUI.enabled;
		GUI.enabled = trainState != 6;
		if (!flag4 && GUI.Button(rectBuy, string.Empty, (num5 != 0) ? upgradeCat : buyCat))
		{
			if (Defs.IsTraining)
			{
				_IncreaseState();
				if (trainState == 3 || trainState == 6)
				{
					_ResetBlink();
					_StartBlinkStyle(resumeStyle);
				}
				if (trainState == 2)
				{
					_ResetBlink();
					_StartBlinkStyle(upgradeCat);
				}
			}
			string id = chosenId;
			string text = _TagForId(id);
			List<string> list = new List<string>();
			list.Add(text);
			foreach (List<string> upgrade in UpgradeManager.upgrades)
			{
				if (upgrade.Contains(text))
				{
					list = upgrade;
					break;
				}
			}
			if (num5 < list.Count)
			{
				text = list[num5];
			}
			if (WeaponManager.tagToStoreIDMapping.ContainsKey(text))
			{
				id = WeaponManager.tagToStoreIDMapping[text];
			}
			Action act = null;
			act = delegate
			{
				coinsShop.thisScript.notEnoughCoins = false;
				coinsShop.thisScript.onReturnAction = null;
				if (id != null)
				{
					int num6 = ((!VirtualCurrencyHelper.prices.ContainsKey(id)) ? 10 : VirtualCurrencyHelper.prices[id]);
					int @int = Storager.getInt(Defs.Coins, false);
					int newCoins = @int - num6;
					Action actualBuy = delegate
					{
						if (!Defs.IsTraining)
						{
							Storager.setInt(Defs.Coins, newCoins, false);
						}
						else if (id.Equals(StoreKitEventListener.famas))
						{
							famasBought = true;
						}
						else if (id.Equals(StoreKitEventListener.sandFamas))
						{
							famasUpgradeBought = true;
						}
						WeaponManager component2 = GameObject.FindGameObjectWithTag("WeaponManager").GetComponent<WeaponManager>();
						component2.AddMinerWeapon(id);
						if (buyAction != null)
						{
							buyAction(id);
							GameObject gameObject2 = UnityEngine.Object.Instantiate(message) as GameObject;
							gameObject2.GetComponent<Message>().message = "Purchase was successful";
						}
						if (!Defs.IsTraining)
						{
							if (WeaponManager.tagToStoreIDMapping.ContainsValue(id))
							{
								IEnumerable<string> source = from item in WeaponManager.tagToStoreIDMapping
									where item.Value == id
									select item into kv
									select kv.Key;
								if (VirtualCurrencyHelper.prices.ContainsKey(id) && VirtualCurrencyHelper.prices[id] >= PlayerPrefs.GetInt(Defs.MostExpensiveWeapon, 0))
								{
									PlayerPrefs.SetInt(Defs.MostExpensiveWeapon, VirtualCurrencyHelper.prices[id]);
									PlayerPrefs.SetString(Defs.MenuPersWeaponTag, (source.Count() <= 0) ? string.Empty : source.ElementAt(0));
								}
							}
							string eventName = ((!InAppData.inappReadableNames.ContainsKey(id)) ? id : InAppData.inappReadableNames[id]);
							FlurryAndroid.logEvent(eventName, false);
						}
						_ReloadIcon(Array.IndexOf(idsArr, chosenId));
						_ReloadPreview();
						Resources.UnloadUnusedAssets();
					};
					Action action = delegate
					{
						Action<string> buyItem = null;
						buyItem = delegate(string pressedButton)
						{
							EtceteraAndroidManager.alertButtonClickedEvent -= buyItem;
							if (!pressedButton.Equals("Cancel"))
							{
								actualBuy();
							}
						};
						string text2 = string.Format("Do you want to buy {0}?", InAppData.inappReadableNames[id]);
						EtceteraAndroidManager.alertButtonClickedEvent += buyItem;
						EtceteraAndroid.showAlert(string.Empty, text2, "Buy", "Cancel");
					};
					Action<string> showShop = null;
					showShop = delegate(string pressedbutton)
					{
						EtceteraAndroidManager.alertButtonClickedEvent -= showShop;
						if (!pressedbutton.Equals("Cancel"))
						{
							coinsShop.thisScript.notEnoughCoins = true;
							coinsShop.thisScript.onReturnAction = act;
							coinsShop.showCoinsShop();
						}
					};
					if (Defs.IsTraining || newCoins >= 0)
					{
						actualBuy();
					}
					else
					{
						customDialogPrefab = Resources.Load("CustomDialog") as GameObject;
						if (customDialogPrefab != null)
						{
							GameObject gameObject = UnityEngine.Object.Instantiate(customDialogPrefab) as GameObject;
							CustomDialog component = gameObject.GetComponent<CustomDialog>();
							component.yesPressed = delegate
							{
								showShop("Yes!");
								customDialogPrefab = null;
							};
							component.noPressed = delegate
							{
								customDialogPrefab = null;
							};
						}
					}
				}
			};
			act();
		}
		GUI.enabled = flag5;
		GUI.enabled = !flag;
		Rect position3 = new Rect(21f * Defs.Coef, (float)Screen.height - ((float)resumeStyle.normal.background.height + Defs.BottomOffs) * Defs.Coef, (float)resumeStyle.normal.background.width * Defs.Coef, (float)resumeStyle.normal.background.height * Defs.Coef);
		bool flag6 = GUI.enabled;
		GUI.enabled = !Defs.IsTraining || trainState == 3 || trainState == 6;
		if (GUI.Button(position3, string.Empty, resumeStyle))
		{
			if (Defs.IsTraining && (trainState == 3 || trainState == 6))
			{
				_IncreaseState();
				if (trainState == 4)
				{
					_ResetBlink();
					_StartBlinkStyle(catStyles[catStyles.Length - 1]);
				}
				if (trainState == 7)
				{
					_ResetBlink();
					_StartBlinkStyle(resumeCategories);
				}
			}
			_UnloadCategory();
			currentCategory = -1;
		}
		GUI.enabled = flag6;
		coinsPlashka.thisScript.enabled = true && !flag;
	}

	private void _ChooseItemAtIndex(int i)
	{
		string[] array = ((PlayerPrefs.GetInt("MultyPlayer") == 1) ? StoreKitEventListener.categoriesMulti[currentCategory] : StoreKitEventListener.categoriesSingle[currentCategory]);
		chosenId = array[i];
		preview = null;
		_ReloadPreview();
		Resources.UnloadUnusedAssets();
	}

	private void RestoreButton(bool disable)
	{
	}

	public void SetGearCatEnabled(bool en)
	{
		_gearCategoryEnabled = en;
	}

	private IEnumerator _BlinkGear()
	{
		Texture2D normal = catStyles[catStyles.Length - 1].normal.background;
		Texture2D active = catStyles[catStyles.Length - 1].active.background;
		Texture2D tmp2 = null;
		do
		{
			tmp2 = catStyles[catStyles.Length - 1].normal.background;
			catStyles[catStyles.Length - 1].normal.background = catStyles[catStyles.Length - 1].active.background;
			catStyles[catStyles.Length - 1].active.background = tmp2;
			yield return new WaitForSeconds(_blinkPause);
		}
		while (!_stopBlinkGear);
		catStyles[catStyles.Length - 1].normal.background = normal;
		catStyles[catStyles.Length - 1].active.background = active;
	}

	private IEnumerator _BlinkBack()
	{
		Texture2D normal = resumeStyle.normal.background;
		Texture2D active = resumeStyle.active.background;
		Texture2D tmp2 = null;
		do
		{
			tmp2 = resumeStyle.normal.background;
			resumeStyle.normal.background = resumeStyle.active.background;
			resumeStyle.active.background = tmp2;
			yield return new WaitForSeconds(_blinkPause);
		}
		while (!_stopBlinkBack);
		resumeStyle.normal.background = normal;
		resumeStyle.active.background = active;
	}

	private IEnumerator _BlinkGuns()
	{
		Texture2D normal = catStyles[1].normal.background;
		Texture2D active = catStyles[1].active.background;
		Texture2D tmp2 = null;
		do
		{
			tmp2 = catStyles[1].normal.background;
			catStyles[1].normal.background = catStyles[1].active.background;
			catStyles[1].active.background = tmp2;
			yield return new WaitForSeconds(_blinkPause);
		}
		while (!_stopBlinkGuns);
		catStyles[1].normal.background = normal;
		catStyles[1].active.background = active;
	}

	private IEnumerator _BlinkFAMASBuy()
	{
		Texture2D normal = buyCat.normal.background;
		Texture2D active = buyCat.active.background;
		Texture2D tmp2 = null;
		do
		{
			tmp2 = buyCat.normal.background;
			buyCat.normal.background = buyCat.active.background;
			buyCat.active.background = tmp2;
			yield return new WaitForSeconds(_blinkPause);
		}
		while (!_stopBlinkFAMASBuy);
		buyCat.normal.background = normal;
		buyCat.active.background = active;
	}

	private void _RunBlinks()
	{
		if (_style != null && Time.realtimeSinceStartup - _tm >= _blinkPause)
		{
			_tm = Time.realtimeSinceStartup;
			_SwapStyle(_style);
		}
	}

	private void _SwapStyle(GUIStyle s)
	{
		Texture2D background = s.normal.background;
		s.normal.background = s.active.background;
		s.active.background = background;
	}

	private void _StartBlinkStyle(GUIStyle s)
	{
		_style = s;
		_n = s.normal.background;
		_a = s.active.background;
		_SwapStyle(_style);
		_tm = Time.realtimeSinceStartup;
	}

	private void _ResetBlink()
	{
		if (_style != null && _n != null && _a != null)
		{
			_style.normal.background = _n;
			_style.active.background = _a;
		}
		_style = null;
		_n = null;
		_a = null;
	}

	private void _ShowText()
	{
		if (!(_infoTexture == null))
		{
			float num = (float)_infoTexture.width * Defs.Coef;
			float num2 = (float)_infoTexture.height * Defs.Coef;
			Rect position;
			switch (trainState)
			{
			case 0:
				position = new Rect(_RectForCat(1).x + _RectForCat(1).width / 2f - num / 2f, _RectForCat(1).y + _RectForCat(1).height, num, num2);
				break;
			default:
				position = new Rect((float)Screen.width / 2f + 512f * Defs.Coef - num, (float)Screen.height - num2, num, num2);
				break;
			case 3:
				position = new Rect((float)Screen.width / 2f - num / 2f, (float)Screen.height - num2, num, num2);
				break;
			case 4:
				position = new Rect(_RectForCat(catStyles.Length - 1).x + _RectForCat(catStyles.Length - 1).width / 2f - num / 2f, _RectForCat(catStyles.Length - 1).y + _RectForCat(catStyles.Length - 1).height, num, num2);
				break;
			case 6:
			case 7:
				position = new Rect((float)Screen.width / 2f + 512f * Defs.Coef - num, _RectBuy.y + _RectBuy.height / 2f - num2 / 2f, num, num2);
				break;
			}
			GUI.DrawTexture(position, _infoTexture);
		}
	}

	private void _IncreaseState()
	{
		trainState++;
		if (trainState == 8)
		{
			_infoTexture = null;
		}
		if (trainState != 7 && trainState != 8)
		{
			_infoTexture = Resources.Load(ResPath.Combine("TrainingShop", "trainshop_" + trainState)) as Texture;
		}
	}

	private Rect _RectForCat(int i)
	{
		return new Rect(_CatOffs * (float)(i + 1) + _CatWidth * (float)i, (float)Screen.height * 0.56f - _CatHeight / 2f, _CatWidth, _CatHeight);
	}

	public void ShowShop(bool show)
	{
		if (show)
		{
			_RunBlinks();
			if (Defs.IsTraining && trainState == -1)
			{
				_IncreaseState();
				_StartBlinkStyle(catStyles[1]);
			}
			Rect position = new Rect(((float)Screen.width - 2048f * (float)Screen.height / 1154f) / 2f, 0f, 2048f * (float)Screen.height / 1154f, Screen.height);
			Rect position2 = new Rect((float)Screen.width * 0.5f - (float)shopHead.width * Defs.Coef * 0.5f, (float)Screen.height * 0.04f, (float)shopHead.width * Defs.Coef, (float)shopHead.height * Defs.Coef);
			if (currentCategory == -1)
			{
				coinsPlashka.thisScript.enabled = false;
				position2.y = (float)Screen.height * 0.16f;
				bool flag = GUI.enabled;
				GUI.enabled = true;
				GUI.DrawTexture(position, shopFon, ScaleMode.StretchToFill);
				GUI.DrawTexture(position2, shopHead);
				int num = ((!_gearCategoryEnabled) ? (catStyles.Length - 1) : catStyles.Length);
				for (int i = 0; i != num; i++)
				{
					Rect position3 = _RectForCat(i);
					bool flag2 = GUI.enabled;
					GUI.enabled = !Defs.IsTraining || (trainState == 0 && i == 1) || (trainState == 4 && i == num - 1);
					if (GUI.Button(position3, string.Empty, catStyles[i]))
					{
						if (!Defs.IsTraining)
						{
							FlurryPluginWrapper.LogCategoryEnteredEvent(StoreKitEventListener.categoryNames[i]);
						}
						currentCategory = i;
						_ReloadCategoryImages();
						if (Defs.IsTraining)
						{
							if (trainState == 0)
							{
								_ResetBlink();
								_IncreaseState();
								_StartBlinkStyle(buyCat);
								_ChooseItemAtIndex(goods.Length - 1);
							}
							if (trainState == 4)
							{
								_ResetBlink();
								_StartBlinkStyle(buyCat);
								_ChooseItemAtIndex(goods.Length - 1);
								_IncreaseState();
							}
						}
						break;
					}
					GUI.enabled = flag2;
				}
				bool flag3 = GUI.enabled;
				GUI.enabled = !Defs.IsTraining || trainState == 7 || trainState == 8;
				_shopResume(false);
				GUI.enabled = flag3;
				if (_hatsCapesEnabled && hatsAndCapesStyle.normal.background != null)
				{
					Rect position4 = new Rect((float)Screen.width - Defs.BottomOffs * Defs.Coef - (float)hatsAndCapesStyle.normal.background.width * Defs.Coef, (float)Screen.height - Defs.BottomOffs * Defs.Coef - (float)hatsAndCapesStyle.normal.background.height * Defs.Coef, (float)hatsAndCapesStyle.normal.background.width * Defs.Coef, (float)hatsAndCapesStyle.normal.background.height * Defs.Coef);
					if (GUI.Button(position4, string.Empty, hatsAndCapesStyle))
					{
						MenuBackgroundMusic.keepPlaying = true;
						ProfileShop.SceneToLoad = Defs.MainMenuScene;
						LoadConnectScene.interval = Defs.GoToProfileShopInterval;
						LoadConnectScene.textureToShow = Resources.Load("coinsFon") as Texture;
						LoadConnectScene.sceneToLoad = "ProfileShop";
						Application.LoadLevel(Defs.PromSceneName);
						return;
					}
				}
				GUI.enabled = flag;
			}
			else
			{
				GUI.DrawTexture(position, shopFon, ScaleMode.StretchToFill);
				position2.width = position2.height * ((float)categoryHeads[currentCategory].width / (float)categoryHeads[currentCategory].height);
				position2.width *= 0.83f;
				position2.height *= 0.83f;
				position2.x = (float)Screen.width / 2f - position2.width / 2f;
				GUI.DrawTexture(position2, categoryHeads[currentCategory]);
				showCategory();
			}
			GUI.enabled = true;
			if (Defs.IsTraining)
			{
				_ShowText();
			}
		}
		else if (coinsPlashka.thisScript != null)
		{
			coinsPlashka.thisScript.enabled = false;
		}
	}

	private void _shopResume(bool disableGUI)
	{
		float num = 0f;
		float num2 = 0f;
		if (currentCategory == -1)
		{
			num = (float)resumeCategories.normal.background.width * Defs.Coef;
			num2 = num * ((float)resumeCategories.normal.background.height / (float)resumeCategories.normal.background.width);
		}
		else
		{
			num = (float)resumeStyle.normal.background.width * Defs.Coef;
			num2 = num * ((float)resumeStyle.normal.background.height / (float)resumeStyle.normal.background.width);
		}
		Rect position = new Rect(21f * Defs.Coef, (float)Screen.height - ((float)resumeStyle.normal.background.height + 21f) * Defs.Coef, num, num2);
		if (GUI.Button(position, string.Empty, (currentCategory != -1) ? resumeStyle : resumeCategories))
		{
			_ResumeHandler();
		}
	}

	private void _ResumeHandler()
	{
		if (currentCategory == -1)
		{
			if (trainState == 7)
			{
				_ResetBlink();
				_IncreaseState();
				TrainingController.isNextStep = TrainingController.stepTrainingList["Shop"];
			}
			if (resumeAction != null)
			{
				resumeAction();
			}
		}
		else
		{
			currentCategory = -1;
		}
	}

	public void loadShopCategories()
	{
		catStyles[0].normal.background = Resources.Load("InAppCategories/premium") as Texture2D;
		catStyles[0].active.background = Resources.Load("InAppCategories/premium_n") as Texture2D;
		catStyles[1].normal.background = Resources.Load("InAppCategories/guns") as Texture2D;
		catStyles[1].active.background = Resources.Load("InAppCategories/guns_n") as Texture2D;
		catStyles[2].normal.background = Resources.Load("InAppCategories/melee") as Texture2D;
		catStyles[2].active.background = Resources.Load("InAppCategories/melee_n") as Texture2D;
		catStyles[3].normal.background = Resources.Load("InAppCategories/special") as Texture2D;
		catStyles[3].active.background = Resources.Load("InAppCategories/special_n") as Texture2D;
		catStyles[4].normal.background = Resources.Load("InAppCategories/gear") as Texture2D;
		catStyles[4].active.background = Resources.Load("InAppCategories/gear_n") as Texture2D;
		hatsAndCapesStyle.normal.background = Resources.Load("InAppCategories/access") as Texture2D;
		hatsAndCapesStyle.active.background = Resources.Load("InAppCategories/access_n") as Texture2D;
		loadShopItems();
	}

	private void unloadShopItems()
	{
	}

	private void loadShopItems()
	{
		StartCoroutine(inAppItemLoader());
	}

	private void loadInAppItemTexture(out Texture tex, string path)
	{
		tex = Resources.Load(path) as Texture;
	}

	private void unloadInAppItemStyle(ref GUIStyle style)
	{
		style.normal.background = null;
		style.active.background = null;
	}

	private void loadInAppItemStyle(ref GUIStyle style, string normal, string active)
	{
		style.normal.background = Resources.Load(normal) as Texture2D;
		style.active.background = Resources.Load(active) as Texture2D;
	}

	private IEnumerator inAppItemLoader()
	{
		yield return null;
	}

	public void unloadShopCategories()
	{
		if (catStyles != null)
		{
			GUIStyle[] array = catStyles;
			foreach (GUIStyle gUIStyle in array)
			{
				gUIStyle.normal.background = null;
				gUIStyle.active.background = null;
				gUIStyle.hover.background = null;
			}
			unloadShopItems();
			hatsAndCapesStyle.normal.background = null;
			hatsAndCapesStyle.active.background = null;
			Resources.UnloadUnusedAssets();
		}
	}
}
