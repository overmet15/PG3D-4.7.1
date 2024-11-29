using System.Collections.Generic;
using System.Security.Cryptography;
using Rilisoft.PixlGun3D;
using UnityEngine;

public sealed class Defs
{
	public enum RuntimeAndroidEdition
	{
		None = 0,
		Amazon = 1,
		GoogleLite = 2,
		GooglePro = 3
	}

	public static bool isSoundMusic;

	public static bool isSoundFX;

	public static float BottomOffs;

	public static Dictionary<string, string> mapNamesForUser;

	public static Dictionary<string, int> levelNumsForMusicInMult;

	public static List<int> levelsWithVarY;

	public static int NumberOfElixirs;

	public static float GoToProfileShopInterval;

	public static readonly string InvertCamSN;

	public static string PromSceneName;

	private static readonly byte[] _rsaParameters;

	public static float HalfLength
	{
		get
		{
			return 17f;
		}
	}

	public static string CapeEquppedSN
	{
		get
		{
			return "CapeEquppedSN";
		}
	}

	public static string HatEquppedSN
	{
		get
		{
			return "HatEquppedSN";
		}
	}

	public static string CapeNoneEqupped
	{
		get
		{
			return "cape_NoneEquipped";
		}
	}

	public static string HatNoneEqupped
	{
		get
		{
			return "hat_NoneEquipped";
		}
	}

	public static string SurvSkinsPath
	{
		get
		{
			return "EnemySkins/Survival";
		}
	}

	public static string TrainingCompleted_4_3Sett
	{
		get
		{
			return "TrainingCompleted_4_3Sett";
		}
	}

	public static string TrainingCompleted_4_4_Sett
	{
		get
		{
			return "TrainingCompleted_4_4_Sett";
		}
	}

	public static string ShouldEnableShopSN
	{
		get
		{
			return "ShouldEnableShopSN";
		}
	}

	public static string TrainingSceneName
	{
		get
		{
			return "Training";
		}
	}

	public static string CoinsAfterTrainingSN
	{
		get
		{
			return "CoinsAfterTrainingSN";
		}
	}

	public static bool IsTraining
	{
		get
		{
			return PlayerPrefs.GetInt(TrainingCompleted_4_4_Sett, 0) == 0 && Application.loadedLevelName.Equals(TrainingSceneName);
		}
	}

	public static string CapesDir
	{
		get
		{
			return "Capes";
		}
	}

	public static string HatsDir
	{
		get
		{
			return "Hats";
		}
	}

	public static string ArtLevsS
	{
		get
		{
			return "ArtLevsS";
		}
	}

	public static string ArtBoxS
	{
		get
		{
			return "ArtBoxS";
		}
	}

	public static string BestScoreSett
	{
		get
		{
			return "BestScoreSett";
		}
	}

	public static string SurvivalScoreSett
	{
		get
		{
			return "SurvivalScoreSett";
		}
	}

	public static string CurrSurvivalScoreSett
	{
		get
		{
			return "CurrSurvivalScoreSett";
		}
	}

	public static string InAppBoughtSett
	{
		get
		{
			return "BigAmmoPackBought";
		}
	}

	public static string CurrentWeaponSett
	{
		get
		{
			return "CurrentWeapon";
		}
	}

	public static string MinerWeaponSett
	{
		get
		{
			return "MinerWeaponSett";
		}
	}

	public static string SwordSett
	{
		get
		{
			return "SwordSett";
		}
	}

	public static int ScoreForSurplusAmmo
	{
		get
		{
			return 50;
		}
	}

	public static string CurrentHealthSett
	{
		get
		{
			return "CurrentHealthSett";
		}
	}

	public static string CurrentArmorSett
	{
		get
		{
			return "CurrentArmorSett";
		}
	}

	public static string LevelsWhereGetCoinS
	{
		get
		{
			return "LevelsWhereGetCoinS";
		}
	}

	public static string NumberOfElixirsSett
	{
		get
		{
			return "NumberOfElixirsSett";
		}
	}

	public static string ArmorType
	{
		get
		{
			return "ArmorType";
		}
	}

	public static string WeaponsGotInCampaign
	{
		get
		{
			return "WeaponsGotInCampaign";
		}
	}

	public static float Coef
	{
		get
		{
			return (float)Screen.height / 768f;
		}
	}

	public static string SkinEditorMode
	{
		get
		{
			return "SkinEditorMode";
		}
	}

	public static string SkinNameMultiplayer
	{
		get
		{
			return "SkinNameMultiplayer";
		}
	}

	public static string SkinIndexMultiplayer
	{
		get
		{
			return "SkinIndexMultiplayer";
		}
	}

	public static string SkinBaseName
	{
		get
		{
			return "Mult_Skin_";
		}
	}

	public static string MultSkinsDirectoryName
	{
		get
		{
			return "Multiplayer Skins";
		}
	}

	public static string CombatRifleSett
	{
		get
		{
			return "CombatRifleSett";
		}
	}

	public static string GoldenEagleSett
	{
		get
		{
			return "GoldenEagleSett";
		}
	}

	public static string MagicBowSett
	{
		get
		{
			return "MagicBowSett";
		}
	}

	public static string SPASSett
	{
		get
		{
			return "SPASSett";
		}
	}

	public static string GoldenAxeSett
	{
		get
		{
			return "GoldenAxeSett";
		}
	}

	public static string ChainsawS
	{
		get
		{
			return "ChainsawS";
		}
	}

	public static string FAMASS
	{
		get
		{
			return "FAMASS";
		}
	}

	public static string GlockSett
	{
		get
		{
			return "GlockSett";
		}
	}

	public static string ScytheSN
	{
		get
		{
			return "ScytheSN";
		}
	}

	public static string ShovelSN
	{
		get
		{
			return "ShovelSN";
		}
	}

	public static string Sword_2_SN
	{
		get
		{
			return "Sword_2_SN";
		}
	}

	public static string HammerSN
	{
		get
		{
			return "HammerSN";
		}
	}

	public static string StaffSN
	{
		get
		{
			return "StaffSN";
		}
	}

	public static string CrystalSPASSN
	{
		get
		{
			return "CrystalSPASSN";
		}
	}

	public static string CrystalGlockSN
	{
		get
		{
			return "CrystalGlockSN";
		}
	}

	public static string LaserRifleSN
	{
		get
		{
			return "LaserRifleSN";
		}
	}

	public static string LightSwordSN
	{
		get
		{
			return "LightSwordSN";
		}
	}

	public static string BerettaSN
	{
		get
		{
			return "BerettaSN";
		}
	}

	public static string MaceSN
	{
		get
		{
			return "MaceSN";
		}
	}

	public static string MinigunSN
	{
		get
		{
			return "MinigunSN";
		}
	}

	public static string CrossbowSN
	{
		get
		{
			return "CrossbowSN";
		}
	}

	public static string GoldenPickSN
	{
		get
		{
			return "GoldenPickSN";
		}
	}

	public static string TreeSN
	{
		get
		{
			return "TreeSN";
		}
	}

	public static string FireAxeSN
	{
		get
		{
			return "FireAxeSN";
		}
	}

	public static string _3PLShotgunSN
	{
		get
		{
			return "_3PLShotgunSN";
		}
	}

	public static string Revolver2SN
	{
		get
		{
			return "Revolver2SN";
		}
	}

	public static string CrystakPickSN
	{
		get
		{
			return "CrystakPickSN";
		}
	}

	public static string IronSwordSN
	{
		get
		{
			return "IronSwordSN";
		}
	}

	public static string GoldenSwordSN
	{
		get
		{
			return "GoldenSwordSN";
		}
	}

	public static string GoldenRed_StoneSN
	{
		get
		{
			return "GoldenRed_StoneSN";
		}
	}

	public static string GoldenSPASSN
	{
		get
		{
			return "GoldenSPASSN";
		}
	}

	public static string GoldenGlockSN
	{
		get
		{
			return "GoldenGlockSN";
		}
	}

	public static string RedMinigunSN
	{
		get
		{
			return "RedMinigunSN";
		}
	}

	public static string CrystalCrossbowSN
	{
		get
		{
			return "CrystalCrossbowSN";
		}
	}

	public static string RedLightSaberSN
	{
		get
		{
			return "RedLightSaberSN";
		}
	}

	public static string SandFamasSN
	{
		get
		{
			return "SandFamasSN";
		}
	}

	public static string WhiteBerettaSN
	{
		get
		{
			return "WhiteBerettaSN";
		}
	}

	public static string BlackEagleSN
	{
		get
		{
			return "BlackEagleSN";
		}
	}

	public static string CrystalAxeSN
	{
		get
		{
			return "CrystalAxeSN";
		}
	}

	public static string SteelAxeSN
	{
		get
		{
			return "SteelAxeSN";
		}
	}

	public static string WoodenBowSN
	{
		get
		{
			return "WoodenBowSN";
		}
	}

	public static string Chainsaw2SN
	{
		get
		{
			return "Chainsaw2SN";
		}
	}

	public static string SteelCrossbowSN
	{
		get
		{
			return "SteelCrossbowSN";
		}
	}

	public static string Hammer2SN
	{
		get
		{
			return "Hammer2SN";
		}
	}

	public static string Mace2SN
	{
		get
		{
			return "Mace2SN";
		}
	}

	public static string Sword_22SN
	{
		get
		{
			return "Sword_22SN";
		}
	}

	public static string Staff2SN
	{
		get
		{
			return "Staff2SN";
		}
	}

	public static string BarrettSN
	{
		get
		{
			return "BarrettSN";
		}
	}

	public static string SVDSN
	{
		get
		{
			return "SVDSN";
		}
	}

	public static string endmanskinBoughtSett
	{
		get
		{
			return "endmanskinBoughtSett";
		}
	}

	public static string chiefBoughtSett
	{
		get
		{
			return "chiefBoughtSett";
		}
	}

	public static string spaceengineerBoughtSett
	{
		get
		{
			return "spaceengineerBoughtSett";
		}
	}

	public static string nanosoldierBoughtSett
	{
		get
		{
			return "nanosoldierBoughtSett";
		}
	}

	public static string steelmanBoughtSett
	{
		get
		{
			return "steelmanBoughtSett";
		}
	}

	public static string captainSett
	{
		get
		{
			return "captainSett";
		}
	}

	public static string hawkSett
	{
		get
		{
			return "hawkSett";
		}
	}

	public static string greenGuySett
	{
		get
		{
			return "greenGuySett";
		}
	}

	public static string TunderGodSett
	{
		get
		{
			return "TunderGodSett";
		}
	}

	public static string gordonSett
	{
		get
		{
			return "gordonSett";
		}
	}

	public static string animeGirlSett
	{
		get
		{
			return "animeGirlSett";
		}
	}

	public static string emoGirlSett
	{
		get
		{
			return "emoGirlSett";
		}
	}

	public static string nurseSett
	{
		get
		{
			return "nurseSett";
		}
	}

	public static string magicGirlSett
	{
		get
		{
			return "magicGirlSett";
		}
	}

	public static string braveGirlSett
	{
		get
		{
			return "braveGirlSett";
		}
	}

	public static string glamGirlSett
	{
		get
		{
			return "glamGirlSett";
		}
	}

	public static string kityyGirlSett
	{
		get
		{
			return "kityyGirlSett";
		}
	}

	public static string famosBoySett
	{
		get
		{
			return "famosBoySett";
		}
	}

	public static string defaultPlayerName
	{
		get
		{
			return "Player";
		}
	}

	public static string Coins
	{
		get
		{
			return "Coins";
		}
	}

	public static string FirstLaunch
	{
		get
		{
			return "FirstLaunch";
		}
	}

	public static string inappsRestored_3_1
	{
		get
		{
			return "inappsRestored_3_1";
		}
	}

	public static string restoreWindowShownProfile
	{
		get
		{
			return "restoreWindowShownProfile";
		}
	}

	public static string restoreWindowShownSingle
	{
		get
		{
			return "restoreWindowShownSingle";
		}
	}

	public static string restoreWindowShownMult
	{
		get
		{
			return "restoreWindowShownMult";
		}
	}

	public static string initValsInKeychain
	{
		get
		{
			return "initValsInKeychain";
		}
	}

	public static string initValsInKeychain2
	{
		get
		{
			return "initValsInKeychain2";
		}
	}

	public static string initValsInKeychain3
	{
		get
		{
			return "initValsInKeychain3";
		}
	}

	public static string initValsInKeychain4
	{
		get
		{
			return "initValsInKeychain4";
		}
	}

	public static string initValsInKeychain5
	{
		get
		{
			return "initValsInKeychain5";
		}
	}

	public static string initValsInKeychain6
	{
		get
		{
			return "initValsInKeychain6";
		}
	}

	public static string initValsInKeychain7
	{
		get
		{
			return "initValsInKeychain7";
		}
	}

	public static string initValsInKeychain8
	{
		get
		{
			return "initValsInKeychain8";
		}
	}

	public static string initValsInKeychain9
	{
		get
		{
			return "initValsInKeychain9";
		}
	}

	public static string initValsInKeychain10
	{
		get
		{
			return "initValsInKeychain10";
		}
	}

	public static string initValsInKeychain11
	{
		get
		{
			return "initValsInKeychain11";
		}
	}

	public static string initValsInKeychain12
	{
		get
		{
			return "initValsInKeychain12";
		}
	}

	public static string initValsInKeychain13
	{
		get
		{
			return "initValsInKeychain13";
		}
	}

	public static string initValsInKeychain14
	{
		get
		{
			return "initValsInKeychain14";
		}
	}

	public static string initValsInKeychain15
	{
		get
		{
			return "_initValsInKeychain15_";
		}
	}

	public static string initValsInKeychain16
	{
		get
		{
			return "_initValsInKeychain16_";
		}
	}

	internal static int SaltSeed
	{
		get
		{
			return 2083243184;
		}
	}

	public static string SkinsMakerInProfileBought
	{
		get
		{
			return "SkinsMakerInProfileBought";
		}
	}

	public static string MostExpensiveWeapon
	{
		get
		{
			return "MostExpensiveWeapon";
		}
	}

	public static string MenuPersWeaponTag
	{
		get
		{
			return "MenuPersWeaponTag";
		}
	}

	public static string TrainingComplSett
	{
		get
		{
			return "TrainingComplSett";
		}
	}

	public static string EarnedCoins
	{
		get
		{
			return "EarnedCoins";
		}
	}

	public static string COOPScore
	{
		get
		{
			return "COOPScore";
		}
	}

	public static string SkinsWrittenToGallery
	{
		get
		{
			return "SkinsWrittenToGallery";
		}
	}

	public static float screenRation
	{
		get
		{
			return (float)Screen.width / (float)Screen.height;
		}
	}

	public static string NumOfMultSkinsSett
	{
		get
		{
			return "NumOfMultSkinsSett";
		}
	}

	public static string KilledZombiesSett
	{
		get
		{
			return "KilledZombiesSett";
		}
	}

	public static string WavesSurvivedS
	{
		get
		{
			return "WavesSurvivedS";
		}
	}

	public static string ProfileEnteredFromMenu
	{
		get
		{
			return "ProfileEnteredFromMenu";
		}
	}

	public static float DiffModif
	{
		get
		{
			float result = 0.6f;
			switch (PlayerPrefs.GetInt(DiffSett, 1))
			{
			case 1:
				result = 0.8f;
				break;
			case 2:
				result = 1f;
				break;
			}
			return result;
		}
	}

	public static string CancelButtonTitle
	{
		get
		{
			return "Cancel";
		}
	}

	public static int skinsMakerPrice
	{
		get
		{
			return (!IsProEdition) ? 50 : 45;
		}
	}

	public static string MainMenuScene
	{
		get
		{
			return "Menu_Winter_Utopia";
		}
	}

	public static string ShouldReoeatActionSett
	{
		get
		{
			return "ShouldReoeatActionSett";
		}
	}

	public static string DiffSett
	{
		get
		{
			return "DifficultySett";
		}
	}

	public static string GoToProfileAction
	{
		get
		{
			return "GoToProfileAction";
		}
	}

	public static string GoToSkinsMakerAction
	{
		get
		{
			return "GoToSkinsMakerAction";
		}
	}

	public static string GoToPresetsAction
	{
		get
		{
			return "GoToPresetsAction";
		}
	}

	public static string SurvivalSett
	{
		get
		{
			return "GoToPresetsAction";
		}
	}

	public static bool IsSurvival
	{
		get
		{
			return PlayerPrefs.GetInt(SurvivalSett, 0) == 1;
		}
	}

	public static string SkinsMakerInMainMenuPurchased
	{
		get
		{
			return "SkinsMakerInMainMenuPurchased";
		}
	}

	public static RuntimeAndroidEdition AndroidEdition
	{
		get
		{
			return RuntimeAndroidEdition.GooglePro;
		}
	}

	public static bool IsProEdition
	{
		get
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXEditor)
			{
				return true;
			}
			if (Application.platform == RuntimePlatform.Android)
			{
				return AndroidEdition != RuntimeAndroidEdition.GoogleLite;
			}
			return false;
		}
	}

	public static string ApplicationUrl
	{
		get
		{
			return (AndroidEdition != RuntimeAndroidEdition.Amazon) ? "http://krasnovapps.com/get.html" : "http://www.amazon.com/Pixel-Gun-Cooperative-Multiplayer-minecraft/dp/B00GEDHISE/ref=sr_1_1?s=mobile-apps&ie=UTF8&qid=1385554808&sr=1-1&keywords=pixel+gun+3d";
		}
	}

	static Defs()
	{
		isSoundMusic = false;
		isSoundFX = false;
		BottomOffs = 21f;
		mapNamesForUser = new Dictionary<string, string>();
		levelNumsForMusicInMult = new Dictionary<string, int>();
		levelsWithVarY = new List<int>();
		NumberOfElixirs = 1;
		GoToProfileShopInterval = 1f;
		InvertCamSN = "InvertCamSN";
		PromSceneName = "PromScene";
		_rsaParameters = new byte[308]
		{
			7, 2, 0, 0, 0, 164, 0, 0, 82, 83,
			65, 50, 0, 2, 0, 0, 17, 0, 0, 0,
			1, 24, 67, 211, 214, 189, 210, 144, 254, 145,
			230, 212, 19, 254, 185, 112, 117, 120, 142, 89,
			80, 227, 74, 157, 136, 99, 204, 254, 117, 105,
			106, 52, 143, 219, 180, 55, 4, 174, 130, 222,
			59, 143, 80, 32, 56, 220, 204, 215, 254, 202,
			38, 42, 34, 141, 116, 38, 68, 147, 247, 71,
			65, 49, 18, 153, 205, 10, 30, 210, 118, 97,
			196, 36, 168, 88, 201, 246, 230, 160, 110, 13,
			124, 85, 105, 5, 43, 72, 1, 158, 28, 194,
			234, 109, 169, 124, 57, 167, 5, 106, 4, 145,
			166, 174, 181, 8, 222, 238, 193, 247, 67, 4,
			63, 158, 68, 238, 149, 46, 126, 245, 244, 34,
			194, 82, 16, 202, 202, 47, 85, 234, 177, 145,
			103, 107, 6, 167, 139, 19, 113, 83, 144, 51,
			172, 211, 28, 133, 56, 20, 84, 65, 236, 67,
			16, 239, 26, 32, 10, 254, 38, 72, 99, 157,
			197, 181, 106, 238, 33, 247, 188, 47, 35, 40,
			87, 193, 215, 151, 33, 197, 170, 220, 239, 73,
			82, 102, 162, 100, 132, 69, 125, 74, 225, 224,
			235, 68, 230, 233, 9, 162, 182, 97, 205, 7,
			35, 71, 107, 239, 213, 14, 6, 135, 7, 137,
			140, 150, 80, 39, 253, 197, 12, 101, 164, 157,
			109, 89, 10, 134, 225, 17, 130, 168, 84, 111,
			116, 89, 20, 67, 132, 7, 204, 191, 33, 103,
			113, 0, 12, 11, 19, 139, 190, 49, 110, 98,
			16, 209, 75, 236, 139, 213, 86, 4, 8, 182,
			121, 126, 53, 5, 123, 132, 234, 114, 1, 125,
			120, 63, 150, 29, 192, 102, 100, 11, 230, 161,
			170, 133, 253, 231, 199, 89, 5, 45
		};
		mapNamesForUser.Add("Maze", "Temple");
		mapNamesForUser.Add("Cementery", "Graveyard");
		mapNamesForUser.Add("City", "Dead City");
		mapNamesForUser.Add("Gluk", "END World");
		mapNamesForUser.Add("Jail", "Prison");
		mapNamesForUser.Add("Hospital", "Hospital");
		mapNamesForUser.Add("Pool", "Pool");
		mapNamesForUser.Add("Slender", "Forest");
		mapNamesForUser.Add("Castle", "Hell");
		mapNamesForUser.Add("Ranch", "Shooting Range");
		mapNamesForUser.Add("Arena_MP", "Deadly Arena");
		mapNamesForUser.Add("Sky_islands", "Sky Islands");
		mapNamesForUser.Add("Dust", "Arabian City");
		mapNamesForUser.Add("Bridge", "Bridge");
		mapNamesForUser.Add("Farm", "Farm");
		mapNamesForUser.Add("Utopia", "Utopia");
		mapNamesForUser.Add("Aztec", "Aztec");
		mapNamesForUser.Add("Arena", "Arena");
		mapNamesForUser.Add("Assault", "Warehouse");
		mapNamesForUser.Add("Winter", "Winter");
		mapNamesForUser.Add("School", "School");
		mapNamesForUser.Add("Parkour", "Parkour City");
		mapNamesForUser.Add("Coliseum_MP", "Coliseum");
		levelNumsForMusicInMult.Add("Maze", 2);
		levelNumsForMusicInMult.Add("Cementery", 1);
		levelNumsForMusicInMult.Add("City", 3);
		levelNumsForMusicInMult.Add("Gluk", 6);
		levelNumsForMusicInMult.Add("Jail", 5);
		levelNumsForMusicInMult.Add("Hospital", 4);
		levelNumsForMusicInMult.Add("Pool", 1001);
		levelNumsForMusicInMult.Add("Slender", 9);
		levelNumsForMusicInMult.Add("Castle", 1002);
		levelNumsForMusicInMult.Add("Ranch", 1003);
		levelNumsForMusicInMult.Add("Arena_MP", 1004);
		levelNumsForMusicInMult.Add("Sky_islands", 1005);
		levelNumsForMusicInMult.Add("Dust", 1006);
		levelNumsForMusicInMult.Add("Bridge", 1007);
		levelNumsForMusicInMult.Add("Assault", 1008);
		levelNumsForMusicInMult.Add("Farm", 4001);
		levelNumsForMusicInMult.Add("Utopia", 4002);
		levelNumsForMusicInMult.Add("Arena", 7);
		levelNumsForMusicInMult.Add("Winter", 4003);
		levelNumsForMusicInMult.Add("Aztec", 4005);
		levelNumsForMusicInMult.Add("School", 1009);
		levelNumsForMusicInMult.Add("Parkour", 1010);
		levelNumsForMusicInMult.Add("Coliseum_MP", 1011);
		levelsWithVarY.Add(8);
		levelsWithVarY.Add(10);
		levelsWithVarY.Add(11);
		levelsWithVarY.Add(12);
		levelsWithVarY.Add(13);
		levelsWithVarY.Add(14);
		levelsWithVarY.Add(15);
		levelsWithVarY.Add(16);
		levelsWithVarY.Add(1005);
		levelsWithVarY.Add(1006);
		levelsWithVarY.Add(1007);
		levelsWithVarY.Add(1008);
		levelsWithVarY.Add(1009);
		levelsWithVarY.Add(1010);
		levelsWithVarY.Add(1011);
		levelsWithVarY.Add(4001);
		levelsWithVarY.Add(4002);
		levelsWithVarY.Add(4003);
		levelsWithVarY.Add(4005);
	}
}
