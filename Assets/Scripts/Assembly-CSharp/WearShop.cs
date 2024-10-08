using System;
using System.Collections;
using UnityEngine;

public class WearShop : MonoBehaviour
{
	public static WearShop sharedShop;

	public Action resumeAction;

	public Action<string> buyAction;

	public Texture shopHead;

	public Texture shopFon;

	public GUIStyle[] catStyles;

	public GameObject message;

	public GUIStyle restoreStyle;

	public GUIStyle resumeStyle;

	public Texture[] categoryHeads;

	public GUIStyle resumeCategories;

	public GUIStyle restoreWindButStyle;

	public GameObject customDialogPrefab;

	private static string previewsDir = "WearShopPreviews";

	private static string InAppCategoriesDir = "InAppCategories";

	private GUIStyle[] goods;

	private Texture preview;

	private string chosenId;

	private int currentCategory = -1;

	private GUIStyle buyCat = new GUIStyle();

	private GUIStyle equipStyle = new GUIStyle();

	private GUIStyle unequioStyle = new GUIStyle();

	private Texture equipped;

	private GameObject _purchaseActivityIndicator;

	private string _currentCape;

	private string _currentHat;

	private Rect _RectBuy
	{
		get
		{
			return new Rect((float)Screen.width / 2f - (float)buyCat.normal.background.width / 2f * Defs.Coef, (float)Screen.height - Defs.BottomOffs * Defs.Coef - (float)buyCat.normal.background.height * Defs.Coef, (float)buyCat.normal.background.width * Defs.Coef, (float)buyCat.normal.background.height * Defs.Coef);
		}
	}

	private string _CurrentEquippedSN
	{
		get
		{
			return (currentCategory != 0) ? Defs.HatEquppedSN : Defs.CapeEquppedSN;
		}
	}

	private string _CurrentNoneEquipped
	{
		get
		{
			return (currentCategory != 0) ? Defs.HatNoneEqupped : Defs.CapeNoneEqupped;
		}
	}

	private string _CurrentEquippedWear
	{
		get
		{
			return (currentCategory != 0) ? _currentHat : _currentCape;
		}
		set
		{
			switch (currentCategory)
			{
			case 0:
				_currentCape = value;
				break;
			case 1:
				_currentHat = value;
				break;
			}
		}
	}

	private float _CatWidth
	{
		get
		{
			int num = catStyles.Length;
			float a = (float)Screen.width / (float)(num + 1);
			return Mathf.Min(a, (float)Screen.width / 4f);
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
			int num = catStyles.Length;
			float catWidth = _CatWidth;
			return ((float)Screen.width - catWidth * (float)num) / (float)(num + 1);
		}
	}

	private void Start()
	{
		_currentCape = Storager.getString(Defs.CapeEquppedSN, false);
		_currentHat = Storager.getString(Defs.HatEquppedSN, false);
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
		equipStyle.normal.background = Resources.Load(ResPath.Combine(InAppCategoriesDir, "equip")) as Texture2D;
		equipStyle.active.background = Resources.Load(ResPath.Combine(InAppCategoriesDir, "equip_n")) as Texture2D;
		unequioStyle.normal.background = Resources.Load(ResPath.Combine(InAppCategoriesDir, "unequip")) as Texture2D;
		unequioStyle.active.background = Resources.Load(ResPath.Combine(InAppCategoriesDir, "unequip_n")) as Texture2D;
		equipped = Resources.Load(ResPath.Combine(InAppCategoriesDir, "shop_eq")) as Texture;
	}

	private int _CurrentNumberOfUpgrades(string id, out bool maxUpgrade)
	{
		maxUpgrade = !_HasntBoughtGood(id);
		return (!_HasntBoughtGood(id)) ? 1 : 0;
	}

	private bool _HasntBoughtGood(string defName)
	{
		return Storager.getInt(defName, true) == 0;
	}

	private void _ReloadIcon(int i)
	{
		goods[i] = new GUIStyle();
		string[] array = Wear.categories[currentCategory];
		string id = array[i];
		string text = _TagForId(id);
		bool maxUpgrade;
		string text2 = text + "_icon" + (1 + _CurrentNumberOfUpgrades(array[i], out maxUpgrade));
		string a = "WearGoods";
		goods[i].normal.background = Resources.Load(ResPath.Combine(a, text2)) as Texture2D;
		goods[i].onNormal.background = Resources.Load(ResPath.Combine(a, text2 + "_n")) as Texture2D;
	}

	private string _TagForId(string id)
	{
		return id;
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
		string[] array = Wear.categories[currentCategory];
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
		string[] idsArr = Wear.categories[currentCategory];
		float num = (float)goods[0].normal.background.width * Defs.Coef;
		float height = (float)goods[0].normal.background.height * Defs.Coef;
		float num2 = ((float)Screen.width - num * (float)goods.Length) / 2f;
		float top = (float)Screen.height * 0.16f - 10f * Defs.Coef;
		for (int i = 0; i < goods.Length; i++)
		{
			Rect position = new Rect(num2 + num * (float)i, top, num, height);
			bool flag2 = GUI.enabled;
			bool flag3 = GUI.Toggle(position, idsArr[i].Equals(chosenId), string.Empty, goods[i]);
			GUI.enabled = flag2;
			if (flag3 && !idsArr[i].Equals(chosenId))
			{
				_ChooseItemAtIndex(i);
			}
		}
		if (!_CurrentEquippedWear.Equals(_CurrentNoneEquipped))
		{
			Rect position2 = new Rect(num2 + num * (float)Array.IndexOf(idsArr, _CurrentEquippedWear), top, num, height);
			GUI.DrawTexture(position2, equipped);
		}
		GUI.enabled = !StoreKitEventListener.purchaseInProcess && !flag;
		float num3 = 958f * Defs.Coef;
		float num4 = 381f * Defs.Coef;
		Rect rectBuy = _RectBuy;
		Rect position3 = new Rect((float)Screen.width / 2f - num3 / 2f, rectBuy.y - num4 - 3f * Defs.Coef + 21f * Defs.Coef, num3, num4);
		GUI.DrawTexture(position3, preview);
		bool flag4 = false;
		bool maxUpgrade;
		int num5 = _CurrentNumberOfUpgrades(chosenId, out maxUpgrade);
		flag4 = maxUpgrade;
		bool flag5 = GUI.enabled;
		bool flag6 = num5 != 0;
		bool flag7 = flag6 && _CurrentEquippedWear.Equals(chosenId);
		if (GUI.Button(rectBuy, string.Empty, (!flag6) ? buyCat : ((!flag7) ? equipStyle : unequioStyle)))
		{
			string id = chosenId;
			string tg = _TagForId(id);
			if (!flag6)
			{
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
							Storager.setInt(Defs.Coins, newCoins, false);
							Storager.setInt(tg, 1, true);
							Storager.setString(_CurrentEquippedSN, tg, false);
							_CurrentEquippedWear = tg;
							if (buyAction != null)
							{
								buyAction(id);
								GameObject gameObject2 = UnityEngine.Object.Instantiate(message) as GameObject;
								gameObject2.GetComponent<Message>().message = "Purchase was successful";
							}
							string eventName = ((!InAppData.inappReadableNames.ContainsKey(id)) ? id : InAppData.inappReadableNames[id]);
							FlurryAndroid.logEvent(eventName, false);
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
							string text = string.Format("Do you want to buy {0}?", InAppData.inappReadableNames[id]);
							EtceteraAndroidManager.alertButtonClickedEvent += buyItem;
							EtceteraAndroid.showAlert(string.Empty, text, "Buy", "Cancel");
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
						if (newCoins >= 0)
						{
							actualBuy();
						}
						else if (customDialogPrefab != null)
						{
							GameObject gameObject = UnityEngine.Object.Instantiate(customDialogPrefab) as GameObject;
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
			else if (!flag7)
			{
				Storager.setString(_CurrentEquippedSN, tg, false);
				_CurrentEquippedWear = tg;
			}
			else
			{
				Storager.setString(_CurrentEquippedSN, _CurrentNoneEquipped, false);
				_CurrentEquippedWear = _CurrentNoneEquipped;
			}
		}
		GUI.enabled = flag5;
		GUI.enabled = !flag;
		Rect position4 = new Rect(21f * Defs.Coef, (float)Screen.height - ((float)resumeStyle.normal.background.height + Defs.BottomOffs) * Defs.Coef, (float)resumeStyle.normal.background.width * Defs.Coef, (float)resumeStyle.normal.background.height * Defs.Coef);
		bool flag8 = GUI.enabled;
		if (GUI.Button(position4, string.Empty, resumeStyle))
		{
			_UnloadCategory();
			currentCategory = -1;
		}
		GUI.enabled = flag8;
		coinsPlashka.thisScript.enabled = true && !flag;
	}

	private void _ChooseItemAtIndex(int i)
	{
		string[] array = Wear.categories[currentCategory];
		chosenId = array[i];
		preview = null;
		_ReloadPreview();
		Resources.UnloadUnusedAssets();
	}

	private void RestoreButton(bool disable)
	{
	}

	private Rect _RectForCat(int i)
	{
		return new Rect(_CatOffs * (float)(i + 1) + _CatWidth * (float)i, (float)Screen.height * 0.56f - _CatHeight / 2f, _CatWidth, _CatHeight);
	}

	public void ShowShop(bool show)
	{
		if (show)
		{
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
				int num = catStyles.Length;
				for (int i = 0; i != num; i++)
				{
					Rect position3 = _RectForCat(i);
					bool flag2 = GUI.enabled;
					if (GUI.Button(position3, string.Empty, catStyles[i]))
					{
						currentCategory = i;
						_ReloadCategoryImages();
						break;
					}
					GUI.enabled = flag2;
				}
				bool flag3 = GUI.enabled;
				_shopResume(false);
				GUI.enabled = flag3;
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
		if (!GUI.Button(position, string.Empty, (currentCategory != -1) ? resumeStyle : resumeCategories))
		{
			return;
		}
		if (currentCategory == -1)
		{
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
		catStyles[0].normal.background = Resources.Load("InAppCategories/capes") as Texture2D;
		catStyles[0].active.background = Resources.Load("InAppCategories/capes_n") as Texture2D;
		catStyles[1].normal.background = Resources.Load("InAppCategories/hats") as Texture2D;
		catStyles[1].active.background = Resources.Load("InAppCategories/hats_n") as Texture2D;
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
			Resources.UnloadUnusedAssets();
		}
	}
}
