using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class Switcher : MonoBehaviour
{
	public static Dictionary<string, int> sceneNameToGameNum;

	public static string LoadingInResourcesPath;

	public static string[] loadingNames;

	public bool NoWait;

	public bool isGameOver;

	public GameObject coinsShopPrefab;

	private Texture fonToDraw;

	public GameObject skinsManagerPrefab;

	public GameObject weaponManagerPrefab;

	public GameObject ExperienceControllerPrefab;

	public GameObject flurryPrefab;

	public GameObject backgroundMusicPrefab;

	public GameObject guiHelperPrefab;

	public Texture plashkaCoins;

	private Rect plashkaCoinsRect;

	static Switcher()
	{
		sceneNameToGameNum = new Dictionary<string, int>();
		LoadingInResourcesPath = "LevelLoadings";
		loadingNames = new string[17]
		{
			"Loading_coliseum", "loading_Cementery", "Loading_Maze", "Loading_City", "Loading_Hospital", "Loading_Jail", "Loading_end_world_2", "Loading_Arena", "Loading_Area52", "Loading_Slender",
			"Loading_Hell", "Loading_bloody_farm", "Loading_most", "Loading_school", "Loading_utopia", "Loading_sky", "Loading_winter"
		};
		sceneNameToGameNum.Add("Training", 0);
		sceneNameToGameNum.Add("Cementery", 1);
		sceneNameToGameNum.Add("Maze", 2);
		sceneNameToGameNum.Add("City", 3);
		sceneNameToGameNum.Add("Hospital", 4);
		sceneNameToGameNum.Add("Jail", 5);
		sceneNameToGameNum.Add("Gluk_2", 6);
		sceneNameToGameNum.Add("Arena", 7);
		sceneNameToGameNum.Add("Area52", 8);
		sceneNameToGameNum.Add("Slender", 9);
		sceneNameToGameNum.Add("Castle", 10);
		sceneNameToGameNum.Add("Farm", 11);
		sceneNameToGameNum.Add("Bridge", 12);
		sceneNameToGameNum.Add("School", 13);
		sceneNameToGameNum.Add("Utopia", 14);
		sceneNameToGameNum.Add("Sky_islands", 15);
		sceneNameToGameNum.Add("Winter", 16);
	}

	private void Start()
	{
		PlayerPrefs.SetInt("MultyPlayer", 0);
		if (isGameOver)
		{
			fonToDraw = Resources.Load("dead") as Texture;
			return;
		}
		if (GlobalGameController.currentLevel == -1)
		{
			fonToDraw = Resources.Load("main_loading") as Texture;
		}
		if (NoWait)
		{
			LoadMenu();
		}
		else
		{
			Invoke("LoadMenu", 3.5f);
		}
		if (!GameObject.FindGameObjectWithTag("ExperienceController") && (bool)ExperienceControllerPrefab)
		{
			UnityEngine.Object.Instantiate(ExperienceControllerPrefab, Vector3.zero, Quaternion.identity);
		}
		if (!GameObject.FindGameObjectWithTag("SkinsManager") && (bool)skinsManagerPrefab)
		{
			UnityEngine.Object.Instantiate(skinsManagerPrefab, Vector3.zero, Quaternion.identity);
		}
		if (!GameObject.FindGameObjectWithTag("WeaponManager") && (bool)weaponManagerPrefab)
		{
			UnityEngine.Object.Instantiate(weaponManagerPrefab, Vector3.zero, Quaternion.identity);
		}
		if (!GameObject.FindGameObjectWithTag("Flurry") && (bool)flurryPrefab)
		{
			UnityEngine.Object.Instantiate(flurryPrefab, Vector3.zero, Quaternion.identity);
		}
		if (!GameObject.FindGameObjectWithTag("MenuBackgroundMusic") && (bool)backgroundMusicPrefab)
		{
			UnityEngine.Object.Instantiate(backgroundMusicPrefab, Vector3.zero, Quaternion.identity);
		}
		if (!GameObject.FindGameObjectWithTag("GUIHelper") && (bool)guiHelperPrefab)
		{
			UnityEngine.Object.Instantiate(guiHelperPrefab, Vector3.zero, Quaternion.identity);
		}
		if (!GameObject.FindGameObjectWithTag("ShopObj"))
		{
			GameObject original = Resources.Load("ShopObj") as GameObject;
			UnityEngine.Object.Instantiate(original, Vector3.zero, Quaternion.identity);
		}
		if (!GameObject.FindGameObjectWithTag("CoinsShop") && (bool)coinsShopPrefab)
		{
			object obj = new object();
			Storager.Initialize(obj != null);
			if (!Storager.hasKey(Defs.initValsInKeychain))
			{
				Storager.setInt(Defs.initValsInKeychain, 0, false);
				Storager.setInt(Defs.SwordSett, 0, false);
				Storager.setInt(Defs.MinerWeaponSett, 0, false);
				Storager.setInt(Defs.InAppBoughtSett, 0, false);
				Storager.setInt(Defs.CombatRifleSett, 0, false);
				Storager.setInt(Defs.GoldenEagleSett, 0, false);
				Storager.setInt(Defs.MagicBowSett, 0, false);
				Storager.setInt(Defs.SPASSett, 0, false);
				Storager.setInt(Defs.GoldenAxeSett, 0, false);
				foreach (KeyValuePair<string, string> value in InAppData.inAppData.Values)
				{
					if (!Storager.hasKey(value.Value))
					{
						Storager.setInt(value.Value, 0, false);
					}
				}
				if (PlayerPrefs.GetInt(Defs.SwordSett, 0) > 0)
				{
					Storager.setInt(Defs.SwordSett, PlayerPrefs.GetInt(Defs.SwordSett, 0), true);
				}
				if (PlayerPrefs.GetInt(Defs.MinerWeaponSett, 0) > 0)
				{
					Storager.setInt(Defs.MinerWeaponSett, PlayerPrefs.GetInt(Defs.MinerWeaponSett, 0), true);
				}
				if (PlayerPrefs.GetInt(Defs.CombatRifleSett, 0) > 0)
				{
					Storager.setInt(Defs.CombatRifleSett, PlayerPrefs.GetInt(Defs.CombatRifleSett, 0), true);
				}
				if (PlayerPrefs.GetInt(Defs.GoldenEagleSett, 0) > 0)
				{
					Storager.setInt(Defs.GoldenEagleSett, PlayerPrefs.GetInt(Defs.GoldenEagleSett, 0), true);
				}
				if (PlayerPrefs.GetInt(Defs.MagicBowSett, 0) > 0)
				{
					Storager.setInt(Defs.MagicBowSett, PlayerPrefs.GetInt(Defs.MagicBowSett, 0), true);
				}
				if (PlayerPrefs.GetInt(Defs.SPASSett, 0) > 0)
				{
					Storager.setInt(Defs.SPASSett, PlayerPrefs.GetInt(Defs.SPASSett, 0), true);
				}
				if (PlayerPrefs.GetInt(Defs.GoldenAxeSett, 0) > 0)
				{
					Storager.setInt(Defs.GoldenAxeSett, PlayerPrefs.GetInt(Defs.GoldenAxeSett, 0), true);
				}
				foreach (KeyValuePair<string, string> value2 in InAppData.inAppData.Values)
				{
					if (PlayerPrefs.GetInt(value2.Value, 0) > 0)
					{
						Storager.setInt(value2.Value, PlayerPrefs.GetInt(value2.Value, 0), true);
					}
				}
			}
			if (!Storager.hasKey(Defs.initValsInKeychain2))
			{
				Storager.setInt(Defs.initValsInKeychain2, 0, false);
				Storager.setInt(Defs.ChainsawS, 0, false);
				Storager.setInt(Defs.FAMASS, 0, false);
				Storager.setInt(Defs.GlockSett, 0, false);
			}
			if (!Storager.hasKey(Defs.initValsInKeychain3))
			{
				Storager.setInt(Defs.initValsInKeychain3, 0, false);
				Storager.setInt(Defs.ScytheSN, 0, false);
				Storager.setInt(Defs.ShovelSN, 0, false);
			}
			if (!Storager.hasKey(Defs.initValsInKeychain4))
			{
				Storager.setInt(Defs.initValsInKeychain4, 0, false);
				Storager.setInt(Defs.Sword_2_SN, 0, false);
				Storager.setInt(Defs.HammerSN, 0, false);
			}
			if (!Storager.hasKey(Defs.initValsInKeychain5))
			{
				Storager.setInt(Defs.initValsInKeychain5, 0, false);
				Storager.setInt(Defs.StaffSN, 0, false);
			}
			if (!Storager.hasKey(Defs.initValsInKeychain6))
			{
				Storager.setInt(Defs.initValsInKeychain6, 0, false);
				Storager.setInt(Defs.LaserRifleSN, 0, false);
			}
			if (!Storager.hasKey(Defs.initValsInKeychain7))
			{
				Storager.setInt(Defs.initValsInKeychain7, 0, false);
				Storager.setInt(Defs.SkinsMakerInProfileBought, 0, false);
			}
			if (!Storager.hasKey(Defs.initValsInKeychain8))
			{
				Storager.setInt(Defs.initValsInKeychain8, 0, false);
				Storager.setInt(Defs.LightSwordSN, 0, false);
				Storager.setInt(Defs.BerettaSN, 0, false);
			}
			if (!Storager.hasKey(Defs.initValsInKeychain9))
			{
				PlayerPrefs.Save();
				Storager.setInt(Defs.initValsInKeychain9, 0, false);
				Storager.setInt(Defs.MaceSN, 0, false);
				Storager.setInt(Defs.CrossbowSN, 0, false);
				Storager.setInt(Defs.MinigunSN, 0, false);
			}
			if (!Storager.hasKey(Defs.initValsInKeychain10))
			{
				Storager.setInt(Defs.initValsInKeychain10, 0, false);
				Storager.setInt(Defs.GoldenPickSN, 0, false);
				Storager.setInt(Defs.CrystakPickSN, 0, false);
				Storager.setInt(Defs.IronSwordSN, 0, false);
				Storager.setInt(Defs.GoldenSwordSN, 0, false);
				Storager.setInt(Defs.GoldenRed_StoneSN, 0, false);
				Storager.setInt(Defs.GoldenSPASSN, 0, false);
				Storager.setInt(Defs.GoldenGlockSN, 0, false);
				if (Storager.getInt(Defs.SwordSett, true) <= 0)
				{
				}
			}
			if (!Storager.hasKey(Defs.initValsInKeychain11))
			{
				Storager.setInt(Defs.initValsInKeychain11, 0, false);
				Storager.setInt(Defs.RedMinigunSN, 0, false);
				Storager.setInt(Defs.CrystalCrossbowSN, 0, false);
				Storager.setInt(Defs.RedLightSaberSN, 0, false);
				Storager.setInt(Defs.SandFamasSN, 0, false);
				Storager.setInt(Defs.WhiteBerettaSN, 0, false);
				Storager.setInt(Defs.BlackEagleSN, 0, false);
			}
			if (!Storager.hasKey(Defs.initValsInKeychain12))
			{
				Storager.setInt(Defs.initValsInKeychain12, 0, false);
				Storager.setInt(Defs.CrystalAxeSN, 0, false);
				Storager.setInt(Defs.SteelAxeSN, 0, false);
				Storager.setInt(Defs.WoodenBowSN, 0, false);
				Storager.setInt(Defs.Chainsaw2SN, 0, false);
				Storager.setInt(Defs.SteelCrossbowSN, 0, false);
				Storager.setInt(Defs.Hammer2SN, 0, false);
				Storager.setInt(Defs.Mace2SN, 0, false);
				Storager.setInt(Defs.Sword_22SN, 0, false);
				Storager.setInt(Defs.Staff2SN, 0, false);
			}
			if (!Storager.hasKey(Defs.initValsInKeychain13))
			{
				Storager.setInt(Defs.initValsInKeychain13, 0, false);
				Storager.setInt(Defs.CrystalGlockSN, 0, false);
				Storager.setInt(Defs.CrystalSPASSN, 0, false);
			}
			if (!Storager.hasKey(Defs.initValsInKeychain14))
			{
				Storager.setInt(Defs.initValsInKeychain14, 0, false);
				Storager.setInt(Defs.TreeSN, 0, false);
				Storager.setInt(Defs.FireAxeSN, 0, false);
				Storager.setInt(Defs._3PLShotgunSN, 0, false);
				Storager.setInt(Defs.Revolver2SN, 0, false);
			}
			if (!Storager.hasKey(Defs.initValsInKeychain15))
			{
				Storager.setInt(Defs.initValsInKeychain15, 0, false);
				string[][] categories = Wear.categories;
				foreach (string[] array in categories)
				{
					string[] array2 = array;
					foreach (string key in array2)
					{
						Storager.setInt(key, 0, false);
					}
				}
				Storager.setString(Defs.CapeEquppedSN, Defs.CapeNoneEqupped, false);
				Storager.setString(Defs.HatEquppedSN, Defs.HatNoneEqupped, false);
			}
			if (!Storager.hasKey(Defs.initValsInKeychain16))
			{
				Storager.setInt(Defs.initValsInKeychain16, 0, false);
				Storager.setInt(Defs.BarrettSN, 0, false);
				Storager.setInt(Defs.SVDSN, 0, false);
			}
			if (!Storager.hasKey(Defs.WeaponsGotInCampaign))
			{
				Storager.setString(Defs.WeaponsGotInCampaign, string.Empty, false);
			}
			if (!Storager.hasKey(Defs.LevelsWhereGetCoinS))
			{
				Storager.setString(Defs.LevelsWhereGetCoinS, string.Empty, false);
			}
			float num = 2f;
			int num2 = 5;
			string text = "640111933";
			if (!Storager.hasKey(Defs.Coins))
			{
				int val = ((Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon) ? 30 : ((!Defs.IsProEdition) ? 15 : 150));
				Storager.setInt(Defs.Coins, val, false);
			}
			if (Application.platform == RuntimePlatform.Android && Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
			{
				HashSet<string> hashSet = new HashSet<string>();
				hashSet.Add("75e0aef46c60e9dd3d426fd018ed7452");
				hashSet.Add("14e77ebb818b8ca919cf18e36cfd13f0");
				HashSet<string> hashSet2 = hashSet;
				if (hashSet2.Contains(SystemInfo.deviceUniqueIdentifier))
				{
					int @int = Storager.getInt(Defs.Coins, false);
					Storager.setInt(Defs.Coins, Math.Max(@int, 900), false);
				}
			}
			if (Application.platform == RuntimePlatform.WindowsEditor)
			{
				foreach (string key3 in WeaponManager.tagToStoreIDMapping.Keys)
				{
					if (WeaponManager.storeIDtoDefsSNMapping.ContainsKey(WeaponManager.tagToStoreIDMapping[key3]))
					{
						Storager.setInt(WeaponManager.storeIDtoDefsSNMapping[WeaponManager.tagToStoreIDMapping[key3]], 1, false);
					}
				}
				foreach (string value3 in LevelBox.weaponsFromBosses.Values)
				{
					string[] array3 = Storager.getString(Defs.WeaponsGotInCampaign, false).Split("#"[0]);
					List<string> list = new List<string>();
					string[] array4 = array3;
					foreach (string item in array4)
					{
						list.Add(item);
					}
					if (!list.Contains(value3))
					{
						list.Add(value3);
						Storager.setString(Defs.WeaponsGotInCampaign, string.Join("#"[0].ToString(), list.ToArray()), false);
					}
				}
			}
			if (Storager.getInt(Defs.SwordSett, true) > 0)
			{
				Storager.setInt(Defs.SwordSett, Storager.getInt(Defs.SwordSett, true), true);
			}
			if (Storager.getInt(Defs.MinerWeaponSett, true) > 0)
			{
				Storager.setInt(Defs.MinerWeaponSett, Storager.getInt(Defs.MinerWeaponSett, true), true);
			}
			if (Storager.getInt(Defs.CombatRifleSett, true) > 0)
			{
				Storager.setInt(Defs.CombatRifleSett, Storager.getInt(Defs.CombatRifleSett, true), true);
			}
			if (Storager.getInt(Defs.GoldenEagleSett, true) > 0)
			{
				Storager.setInt(Defs.GoldenEagleSett, Storager.getInt(Defs.GoldenEagleSett, true), true);
			}
			if (Storager.getInt(Defs.MagicBowSett, true) > 0)
			{
				Storager.setInt(Defs.MagicBowSett, Storager.getInt(Defs.MagicBowSett, true), true);
			}
			if (Storager.getInt(Defs.SPASSett, true) > 0)
			{
				Storager.setInt(Defs.SPASSett, Storager.getInt(Defs.SPASSett, true), true);
			}
			if (Storager.getInt(Defs.GoldenAxeSett, true) > 0)
			{
				Storager.setInt(Defs.GoldenAxeSett, Storager.getInt(Defs.GoldenAxeSett, true), true);
			}
			if (Storager.getInt(Defs.ChainsawS, true) > 0)
			{
				Storager.setInt(Defs.ChainsawS, Storager.getInt(Defs.ChainsawS, true), true);
			}
			if (Storager.getInt(Defs.GlockSett, true) > 0)
			{
				Storager.setInt(Defs.GlockSett, Storager.getInt(Defs.GlockSett, true), true);
			}
			if (Storager.getInt(Defs.FAMASS, true) > 0)
			{
				Storager.setInt(Defs.FAMASS, Storager.getInt(Defs.FAMASS, true), true);
			}
			if (Storager.getInt(Defs.ScytheSN, true) > 0)
			{
				Storager.setInt(Defs.ScytheSN, Storager.getInt(Defs.ScytheSN, true), true);
			}
			if (Storager.getInt(Defs.ShovelSN, true) > 0)
			{
				Storager.setInt(Defs.ShovelSN, Storager.getInt(Defs.ShovelSN, true), true);
			}
			if (Storager.getInt(Defs.HammerSN, true) > 0)
			{
				Storager.setInt(Defs.HammerSN, Storager.getInt(Defs.HammerSN, true), true);
			}
			if (Storager.getInt(Defs.Sword_2_SN, true) > 0)
			{
				Storager.setInt(Defs.Sword_2_SN, Storager.getInt(Defs.Sword_2_SN, true), true);
			}
			if (Storager.getInt(Defs.StaffSN, true) > 0)
			{
				Storager.setInt(Defs.StaffSN, Storager.getInt(Defs.StaffSN, true), true);
			}
			if (Storager.getInt(Defs.LaserRifleSN, true) > 0)
			{
				Storager.setInt(Defs.LaserRifleSN, Storager.getInt(Defs.LaserRifleSN, true), true);
			}
			if (Storager.getInt(Defs.SkinsMakerInProfileBought, true) > 0)
			{
				Storager.setInt(Defs.SkinsMakerInProfileBought, Storager.getInt(Defs.SkinsMakerInProfileBought, true), true);
			}
			if (Storager.getInt(Defs.LightSwordSN, true) > 0)
			{
				Storager.setInt(Defs.LightSwordSN, Storager.getInt(Defs.LightSwordSN, true), true);
			}
			if (Storager.getInt(Defs.BerettaSN, true) > 0)
			{
				Storager.setInt(Defs.BerettaSN, Storager.getInt(Defs.BerettaSN, true), true);
			}
			if (Storager.getInt(Defs.MaceSN, true) > 0)
			{
				Storager.setInt(Defs.MaceSN, Storager.getInt(Defs.MaceSN, true), true);
			}
			if (Storager.getInt(Defs.CrossbowSN, true) > 0)
			{
				Storager.setInt(Defs.CrossbowSN, Storager.getInt(Defs.CrossbowSN, true), true);
			}
			if (Storager.getInt(Defs.MinigunSN, true) > 0)
			{
				Storager.setInt(Defs.MinigunSN, Storager.getInt(Defs.MinigunSN, true), true);
			}
			if (Storager.getInt(Defs.GoldenPickSN, true) > 0)
			{
				Storager.setInt(Defs.GoldenPickSN, Storager.getInt(Defs.GoldenPickSN, true), true);
			}
			if (Storager.getInt(Defs.CrystakPickSN, true) > 0)
			{
				Storager.setInt(Defs.CrystakPickSN, Storager.getInt(Defs.CrystakPickSN, true), true);
			}
			if (Storager.getInt(Defs.IronSwordSN, true) > 0)
			{
				Storager.setInt(Defs.IronSwordSN, Storager.getInt(Defs.IronSwordSN, true), true);
			}
			if (Storager.getInt(Defs.GoldenSwordSN, true) > 0)
			{
				Storager.setInt(Defs.GoldenSwordSN, Storager.getInt(Defs.GoldenSwordSN, true), true);
			}
			if (Storager.getInt(Defs.GoldenRed_StoneSN, true) > 0)
			{
				Storager.setInt(Defs.GoldenRed_StoneSN, Storager.getInt(Defs.GoldenRed_StoneSN, true), true);
			}
			if (Storager.getInt(Defs.GoldenSPASSN, true) > 0)
			{
				Storager.setInt(Defs.GoldenSPASSN, Storager.getInt(Defs.GoldenSPASSN, true), true);
			}
			if (Storager.getInt(Defs.GoldenGlockSN, true) > 0)
			{
				Storager.setInt(Defs.GoldenGlockSN, Storager.getInt(Defs.GoldenGlockSN, true), true);
			}
			if (Storager.getInt(Defs.RedMinigunSN, true) > 0)
			{
				Storager.setInt(Defs.RedMinigunSN, Storager.getInt(Defs.RedMinigunSN, true), true);
			}
			if (Storager.getInt(Defs.CrystalCrossbowSN, true) > 0)
			{
				Storager.setInt(Defs.CrystalCrossbowSN, Storager.getInt(Defs.CrystalCrossbowSN, true), true);
			}
			if (Storager.getInt(Defs.RedLightSaberSN, true) > 0)
			{
				Storager.setInt(Defs.RedLightSaberSN, Storager.getInt(Defs.RedLightSaberSN, true), true);
			}
			if (Storager.getInt(Defs.SandFamasSN, true) > 0)
			{
				Storager.setInt(Defs.SandFamasSN, Storager.getInt(Defs.SandFamasSN, true), true);
			}
			if (Storager.getInt(Defs.WhiteBerettaSN, true) > 0)
			{
				Storager.setInt(Defs.WhiteBerettaSN, Storager.getInt(Defs.WhiteBerettaSN, true), true);
			}
			if (Storager.getInt(Defs.BlackEagleSN, true) > 0)
			{
				Storager.setInt(Defs.BlackEagleSN, Storager.getInt(Defs.BlackEagleSN, true), true);
			}
			if (Storager.getInt(Defs.CrystalAxeSN, true) > 0)
			{
				Storager.setInt(Defs.CrystalAxeSN, Storager.getInt(Defs.CrystalAxeSN, true), true);
			}
			if (Storager.getInt(Defs.SteelAxeSN, true) > 0)
			{
				Storager.setInt(Defs.SteelAxeSN, Storager.getInt(Defs.SteelAxeSN, true), true);
			}
			if (Storager.getInt(Defs.WoodenBowSN, true) > 0)
			{
				Storager.setInt(Defs.WoodenBowSN, Storager.getInt(Defs.WoodenBowSN, true), true);
			}
			if (Storager.getInt(Defs.Chainsaw2SN, true) > 0)
			{
				Storager.setInt(Defs.Chainsaw2SN, Storager.getInt(Defs.Chainsaw2SN, true), true);
			}
			if (Storager.getInt(Defs.SteelCrossbowSN, true) > 0)
			{
				Storager.setInt(Defs.SteelCrossbowSN, Storager.getInt(Defs.SteelCrossbowSN, true), true);
			}
			if (Storager.getInt(Defs.Hammer2SN, true) > 0)
			{
				Storager.setInt(Defs.Hammer2SN, Storager.getInt(Defs.Hammer2SN, true), true);
			}
			if (Storager.getInt(Defs.Mace2SN, true) > 0)
			{
				Storager.setInt(Defs.Mace2SN, Storager.getInt(Defs.Mace2SN, true), true);
			}
			if (Storager.getInt(Defs.Sword_22SN, true) > 0)
			{
				Storager.setInt(Defs.Sword_22SN, Storager.getInt(Defs.Sword_22SN, true), true);
			}
			if (Storager.getInt(Defs.Staff2SN, true) > 0)
			{
				Storager.setInt(Defs.Staff2SN, Storager.getInt(Defs.Staff2SN, true), true);
			}
			if (Storager.getInt(Defs.CrystalGlockSN, true) > 0)
			{
				Storager.setInt(Defs.CrystalGlockSN, Storager.getInt(Defs.CrystalGlockSN, true), true);
			}
			if (Storager.getInt(Defs.CrystalSPASSN, true) > 0)
			{
				Storager.setInt(Defs.CrystalSPASSN, Storager.getInt(Defs.CrystalSPASSN, true), true);
			}
			if (Storager.getInt(Defs.TreeSN, true) > 0)
			{
				Storager.setInt(Defs.TreeSN, Storager.getInt(Defs.TreeSN, true), true);
			}
			if (Storager.getInt(Defs.FireAxeSN, true) > 0)
			{
				Storager.setInt(Defs.FireAxeSN, Storager.getInt(Defs.FireAxeSN, true), true);
			}
			if (Storager.getInt(Defs._3PLShotgunSN, true) > 0)
			{
				Storager.setInt(Defs._3PLShotgunSN, Storager.getInt(Defs._3PLShotgunSN, true), true);
			}
			if (Storager.getInt(Defs.Revolver2SN, true) > 0)
			{
				Storager.setInt(Defs.Revolver2SN, Storager.getInt(Defs.Revolver2SN, true), true);
			}
			if (Storager.getInt(Defs.BarrettSN, true) > 0)
			{
				Storager.setInt(Defs.BarrettSN, Storager.getInt(Defs.BarrettSN, true), true);
			}
			if (Storager.getInt(Defs.SVDSN, true) > 0)
			{
				Storager.setInt(Defs.SVDSN, Storager.getInt(Defs.SVDSN, true), true);
			}
			string[][] categories2 = Wear.categories;
			foreach (string[] array5 in categories2)
			{
				string[] array6 = array5;
				foreach (string key2 in array6)
				{
					if (Storager.getInt(key2, true) > 0)
					{
						Storager.setInt(key2, Storager.getInt(key2, true), true);
					}
				}
			}
			foreach (KeyValuePair<string, string> value4 in InAppData.inAppData.Values)
			{
				if (Storager.getInt(value4.Value, true) > 0)
				{
					Storager.setInt(value4.Value, Storager.getInt(value4.Value, true), true);
				}
			}
			string[] array7 = Storager.getString(Defs.WeaponsGotInCampaign, false).Split("#"[0]);
			List<string> list2 = new List<string>();
			string[] array8 = array7;
			foreach (string item2 in array8)
			{
				list2.Add(item2);
			}
			foreach (string key4 in CampaignProgress.boxesLevelsAndStars.Keys)
			{
				foreach (string key5 in CampaignProgress.boxesLevelsAndStars[key4].Keys)
				{
					if (LevelBox.weaponsFromBosses.ContainsKey(key5) && !list2.Contains(LevelBox.weaponsFromBosses[key5]))
					{
						list2.Add(LevelBox.weaponsFromBosses[key5]);
					}
				}
			}
			Storager.setString(Defs.WeaponsGotInCampaign, string.Join("#"[0].ToString(), list2.ToArray()), false);
			UnityEngine.Object.Instantiate(coinsShopPrefab);
		}
		CampaignProgress.OpenNewBoxIfPossible();
		CampaignProgress.SaveCampaignProgress();
	}

	private void Method()
	{
	}

	private void OnGUI()
	{
		int depth = GUI.depth;
		if (isGameOver)
		{
			GUI.depth = 4;
		}
		Rect position = new Rect(((float)Screen.width - 2048f * (float)Screen.height / 1154f) / 2f, 0f, 2048f * (float)Screen.height / 1154f, Screen.height);
		GUI.DrawTexture(position, fonToDraw, ScaleMode.StretchToFill);
		if (plashkaCoins != null)
		{
			GUI.DrawTexture(plashkaCoinsRect, plashkaCoins, ScaleMode.StretchToFill);
		}
	}

	private void LoadMenu()
	{
		string text;
		switch (GlobalGameController.currentLevel)
		{
		case -1:
			text = Defs.MainMenuScene;
			break;
		case 0:
			text = "Cementery";
			break;
		case 1:
			text = "Maze";
			break;
		case 2:
			text = "City";
			break;
		case 3:
			text = "Hospital";
			break;
		case 4:
			text = "Jail";
			break;
		case 5:
			text = "Gluk_2";
			break;
		case 6:
			text = "Arena";
			break;
		case 7:
			text = "Area52";
			break;
		case 101:
			text = "Training";
			break;
		case 8:
			text = "Slender";
			break;
		case 9:
			text = "Castle";
			break;
		default:
			text = Defs.MainMenuScene;
			break;
		}
		Application.LoadLevel(text);
	}
}
