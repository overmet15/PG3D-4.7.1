using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class WeaponManager : MonoBehaviour
{
	public struct infoClient
	{
		public string ipAddress;

		public string name;

		public string coments;
	}

	public static string CrystalSwordTag;

	public static string MinersWeaponTag;

	public static string MaceTag;

	public static string CrossbowTag;

	public static string GoldenPickTag;

	public static string IronSwordTag;

	public static string GoldenSwordTag;

	public static string GoldenSPASTag;

	public static string RedMinigunTag;

	public static string RedLightSaberTag;

	public static string WhiteBerettaTag;

	public static string CrystalAxeTag;

	public static string WoodenBowTag;

	public static string SteelCrossbowTag;

	public static string Mace2Tag;

	public static Dictionary<string, string> campaignBonusWeapons;

	public static Dictionary<string, string> tagToStoreIDMapping;

	public static Dictionary<string, string> storeIDtoDefsSNMapping;

	public static string[] multiplayerWeaponTags;

	private static string[] _initialMultiplayerWeaponTags;

	public HostData hostDataServer;

	public string ServerIp;

	public GameObject myPlayer;

	public GameObject myGun;

	public GameObject myTable;

	private GameObject[] _weaponsInGame;

	private ArrayList _playerWeapons = new ArrayList();

	public int CurrentWeaponIndex;

	public Camera useCam;

	private WeaponSounds _currentWeaponSounds = new WeaponSounds();

	private Dictionary<string, Action> _purchaseActinos = new Dictionary<string, Action>();

	public List<infoClient> players = new List<infoClient>();

	public static string AlienGunTag
	{
		get
		{
			return "AlienGun";
		}
	}

	public static string m16Tag
	{
		get
		{
			return "m16";
		}
	}

	public static string EagleTag
	{
		get
		{
			return "Eagle 1";
		}
	}

	public static string GoldenAxeTag
	{
		get
		{
			return "GoldenAxe";
		}
	}

	public static string SPASTag
	{
		get
		{
			return "SPAS";
		}
	}

	public static string GlockTag
	{
		get
		{
			return "Glock";
		}
	}

	public static string FAMASTag
	{
		get
		{
			return "FAMAS";
		}
	}

	public static string ChainsawTag
	{
		get
		{
			return "Chainsaw";
		}
	}

	public static string ScytheTag
	{
		get
		{
			return "Scythe";
		}
	}

	public static string ShovelTag
	{
		get
		{
			return "Shovel";
		}
	}

	public static string HammerTag
	{
		get
		{
			return "Hammer";
		}
	}

	public static string Sword_2Tag
	{
		get
		{
			return "Sword_2";
		}
	}

	public static string StaffTag
	{
		get
		{
			return "Staff";
		}
	}

	public static string Red_StoneTag
	{
		get
		{
			return "Red_Stone";
		}
	}

	public static string LightSwordTag
	{
		get
		{
			return "LightSword";
		}
	}

	public static string BerettaTag
	{
		get
		{
			return "Beretta";
		}
	}

	public static string MagicBowTag
	{
		get
		{
			return "Bow";
		}
	}

	public static string MinigunTag
	{
		get
		{
			return "Minigun";
		}
	}

	public static string CrystalPickTag
	{
		get
		{
			return "CrystalPick";
		}
	}

	public static string GoldenRed_StoneTag
	{
		get
		{
			return "GoldenRed_Stone";
		}
	}

	public static string GoldenGlockTag
	{
		get
		{
			return "GoldenGlock";
		}
	}

	public static string CrystalCrossbowTag
	{
		get
		{
			return "CrystalCrossbow";
		}
	}

	public static string SandFamasTag
	{
		get
		{
			return "SandFamas";
		}
	}

	public static string BlackEagleTag
	{
		get
		{
			return "BlackEagle";
		}
	}

	public static string SteelAxeTag
	{
		get
		{
			return "SteelAxe";
		}
	}

	public static string Chainsaw2Tag
	{
		get
		{
			return "Chainsaw 2";
		}
	}

	public static string Hammer2Tag
	{
		get
		{
			return "Hammer 2";
		}
	}

	public static string Sword_22Tag
	{
		get
		{
			return "Sword_2 2";
		}
	}

	public static string Staff2Tag
	{
		get
		{
			return "Staff 2";
		}
	}

	public static string CrystalGlockTag
	{
		get
		{
			return "CrystalGlock";
		}
	}

	public static string CrystalSPASTag
	{
		get
		{
			return "CrystalSPAS";
		}
	}

	public static string TreeTag
	{
		get
		{
			return "Tree";
		}
	}

	public static string FireAxeTag
	{
		get
		{
			return "Fire_Axe";
		}
	}

	public static string _3pl_ShotgunTag
	{
		get
		{
			return "3pl_Shotgun";
		}
	}

	public static string Revolver2Tag
	{
		get
		{
			return "Revolver2";
		}
	}

	public static string BarrettTag
	{
		get
		{
			return "Barrett50Cal";
		}
	}

	public static string svdTag
	{
		get
		{
			return "SVD";
		}
	}

	public static string PistolWN
	{
		get
		{
			return "Weapon1";
		}
	}

	public static string ShotgunWN
	{
		get
		{
			return "Weapon2";
		}
	}

	public static string MP5WN
	{
		get
		{
			return "Weapon3";
		}
	}

	public static string RevolverWN
	{
		get
		{
			return "Weapon4";
		}
	}

	public static string MachinegunWN
	{
		get
		{
			return "Weapon5";
		}
	}

	public static string AK47WN
	{
		get
		{
			return "Weapon8";
		}
	}

	public static string KnifeWN
	{
		get
		{
			return "Weapon9";
		}
	}

	public static string ObrezWN
	{
		get
		{
			return "Weapon51";
		}
	}

	public static string AlienGunWN
	{
		get
		{
			return "Weapon52";
		}
	}

	public static string _initialWeaponName
	{
		get
		{
			return "FirstPistol";
		}
	}

	public static string PickWeaponName
	{
		get
		{
			return "Weapon6";
		}
	}

	public static string MultiplayerMeleeTag
	{
		get
		{
			return "Knife";
		}
	}

	public static string SwordWeaponName
	{
		get
		{
			return "Weapon7";
		}
	}

	public static string CombatRifleWeaponName
	{
		get
		{
			return "Weapon10";
		}
	}

	public static string GoldenEagleWeaponName
	{
		get
		{
			return "Weapon11";
		}
	}

	public static string MagicBowWeaponName
	{
		get
		{
			return "Weapon12";
		}
	}

	public static string SpasWeaponName
	{
		get
		{
			return "Weapon13";
		}
	}

	public static string GoldenAxeWeaponnName
	{
		get
		{
			return "Weapon14";
		}
	}

	public static string ChainsawWN
	{
		get
		{
			return "Weapon15";
		}
	}

	public static string FAMASWN
	{
		get
		{
			return "Weapon16";
		}
	}

	public static string GlockWN
	{
		get
		{
			return "Weapon17";
		}
	}

	public static string ScytheWN
	{
		get
		{
			return "Weapon18";
		}
	}

	public static string ShovelWN
	{
		get
		{
			return "Weapon19";
		}
	}

	public static string HammerWN
	{
		get
		{
			return "Weapon20";
		}
	}

	public static string Sword_2_WN
	{
		get
		{
			return "Weapon21";
		}
	}

	public static string StaffWN
	{
		get
		{
			return "Weapon22";
		}
	}

	public static string LaserRifleWN
	{
		get
		{
			return "Weapon23";
		}
	}

	public static string LightSwordWN
	{
		get
		{
			return "Weapon24";
		}
	}

	public static string BerettaWN
	{
		get
		{
			return "Weapon25";
		}
	}

	public static string MaceWN
	{
		get
		{
			return "Weapon26";
		}
	}

	public static string CrossbowWN
	{
		get
		{
			return "Weapon27";
		}
	}

	public static string MinigunWN
	{
		get
		{
			return "Weapon28";
		}
	}

	public static string GoldenPickWN
	{
		get
		{
			return "Weapon29";
		}
	}

	public static string CrystalPickWN
	{
		get
		{
			return "Weapon30";
		}
	}

	public static string IronSwordWN
	{
		get
		{
			return "Weapon31";
		}
	}

	public static string GoldenSwordWN
	{
		get
		{
			return "Weapon32";
		}
	}

	public static string GoldenRed_StoneWN
	{
		get
		{
			return "Weapon33";
		}
	}

	public static string GoldenSPASWN
	{
		get
		{
			return "Weapon34";
		}
	}

	public static string GoldenGlockWN
	{
		get
		{
			return "Weapon35";
		}
	}

	public static string RedMinigunWN
	{
		get
		{
			return "Weapon36";
		}
	}

	public static string CrystalCrossbowWN
	{
		get
		{
			return "Weapon37";
		}
	}

	public static string RedLightSaberWN
	{
		get
		{
			return "Weapon38";
		}
	}

	public static string SandFamasWN
	{
		get
		{
			return "Weapon39";
		}
	}

	public static string WhiteBerettaWN
	{
		get
		{
			return "Weapon40";
		}
	}

	public static string BlackEagleWN
	{
		get
		{
			return "Weapon41";
		}
	}

	public static string CrystalAxeWN
	{
		get
		{
			return "Weapon42";
		}
	}

	public static string SteelAxeWN
	{
		get
		{
			return "Weapon43";
		}
	}

	public static string WoodenBowWN
	{
		get
		{
			return "Weapon44";
		}
	}

	public static string Chainsaw2WN
	{
		get
		{
			return "Weapon45";
		}
	}

	public static string SteelCrossbowWN
	{
		get
		{
			return "Weapon46";
		}
	}

	public static string Hammer2WN
	{
		get
		{
			return "Weapon47";
		}
	}

	public static string Mace2WN
	{
		get
		{
			return "Weapon48";
		}
	}

	public static string Sword_22WN
	{
		get
		{
			return "Weapon49";
		}
	}

	public static string Staff2WN
	{
		get
		{
			return "Weapon50";
		}
	}

	public static string M16_2WN
	{
		get
		{
			return "Weapon53";
		}
	}

	public static string CrystalGlockWN
	{
		get
		{
			return "Weapon54";
		}
	}

	public static string CrystalSPASWN
	{
		get
		{
			return "Weapon55";
		}
	}

	public static string TreeWN
	{
		get
		{
			return "Weapon56";
		}
	}

	public static string FireAxeWN
	{
		get
		{
			return "Weapon57";
		}
	}

	public static string _3pl_shotgunWN
	{
		get
		{
			return "Weapon58";
		}
	}

	public static string Revolver2WN
	{
		get
		{
			return "Weapon59";
		}
	}

	public static string BarrettWN
	{
		get
		{
			return "Weapon60";
		}
	}

	public static string svdWN
	{
		get
		{
			return "Weapon61";
		}
	}

	public UnityEngine.GameObject[] weaponsInGame
	{
		get
		{
			return _weaponsInGame;
		}
	}

	public ArrayList playerWeapons
	{
		get
		{
			return _playerWeapons;
		}
	}

	public WeaponSounds currentWeaponSounds
	{
		get
		{
			return _currentWeaponSounds;
		}
		set
		{
			_currentWeaponSounds = value;
		}
	}

	static WeaponManager()
	{
		CrystalSwordTag = "CrystalSword";
		MinersWeaponTag = "MinersWeapon";
		MaceTag = "Mace";
		CrossbowTag = "Crossbow";
		GoldenPickTag = "GoldenPick";
		IronSwordTag = "IronSword";
		GoldenSwordTag = "GoldenSword";
		GoldenSPASTag = "GoldenSPAS";
		RedMinigunTag = "RedMinigun";
		RedLightSaberTag = "RedLightSaber";
		WhiteBerettaTag = "WhiteBeretta";
		CrystalAxeTag = "CrystalAxe";
		WoodenBowTag = "WoodenBow";
		SteelCrossbowTag = "SteelCrossbow";
		Mace2Tag = "Mace 2";
		campaignBonusWeapons = new Dictionary<string, string>();
		tagToStoreIDMapping = new Dictionary<string, string>();
		storeIDtoDefsSNMapping = new Dictionary<string, string>();
		multiplayerWeaponTags = new string[56]
		{
			MultiplayerMeleeTag, _initialWeaponName, "FirstShotgun", "UziWeapon", CrystalSwordTag, MinersWeaponTag, m16Tag, EagleTag, MagicBowTag, GoldenAxeTag,
			SPASTag, GlockTag, FAMASTag, ChainsawTag, ScytheTag, ShovelTag, HammerTag, Sword_2Tag, StaffTag, Red_StoneTag,
			LightSwordTag, BerettaTag, MinigunTag, CrossbowTag, MaceTag, GoldenPickTag, CrystalPickTag, IronSwordTag, GoldenSwordTag, GoldenSPASTag,
			GoldenGlockTag, GoldenRed_StoneTag, RedMinigunTag, CrystalCrossbowTag, RedLightSaberTag, SandFamasTag, WhiteBerettaTag, BlackEagleTag, CrystalAxeTag, SteelAxeTag,
			WoodenBowTag, Chainsaw2Tag, SteelCrossbowTag, Hammer2Tag, Mace2Tag, Sword_22Tag, Staff2Tag, AlienGunTag, CrystalGlockTag, CrystalSPASTag,
			TreeTag, FireAxeTag, _3pl_ShotgunTag, Revolver2Tag, BarrettTag, svdTag
		};
		_initialMultiplayerWeaponTags = new string[3]
		{
			multiplayerWeaponTags[0],
			multiplayerWeaponTags[1],
			multiplayerWeaponTags[2]
		};
		tagToStoreIDMapping.Add(CrystalSwordTag, "crystalsword");
		tagToStoreIDMapping.Add(MinersWeaponTag, "MinerWeapon");
		tagToStoreIDMapping.Add(m16Tag, StoreKitEventListener.combatrifle);
		tagToStoreIDMapping.Add(EagleTag, StoreKitEventListener.goldeneagle);
		tagToStoreIDMapping.Add(MagicBowTag, StoreKitEventListener.magicbow);
		tagToStoreIDMapping.Add(GoldenAxeTag, StoreKitEventListener.axe);
		tagToStoreIDMapping.Add(SPASTag, StoreKitEventListener.spas);
		tagToStoreIDMapping.Add(GlockTag, StoreKitEventListener.glock);
		tagToStoreIDMapping.Add(FAMASTag, StoreKitEventListener.famas);
		tagToStoreIDMapping.Add(ChainsawTag, StoreKitEventListener.chainsaw);
		tagToStoreIDMapping.Add(ScytheTag, StoreKitEventListener.scythe);
		tagToStoreIDMapping.Add(ShovelTag, StoreKitEventListener.shovel);
		tagToStoreIDMapping.Add(HammerTag, StoreKitEventListener.hammer);
		tagToStoreIDMapping.Add(Sword_2Tag, StoreKitEventListener.sword_2);
		tagToStoreIDMapping.Add(StaffTag, StoreKitEventListener.staff);
		tagToStoreIDMapping.Add(Red_StoneTag, StoreKitEventListener.laser);
		tagToStoreIDMapping.Add(LightSwordTag, StoreKitEventListener.lightSword);
		tagToStoreIDMapping.Add(BerettaTag, StoreKitEventListener.beretta);
		tagToStoreIDMapping.Add(MaceTag, StoreKitEventListener.mace);
		tagToStoreIDMapping.Add(CrossbowTag, StoreKitEventListener.crossbow);
		tagToStoreIDMapping.Add(MinigunTag, StoreKitEventListener.minigun);
		tagToStoreIDMapping.Add(GoldenPickTag, StoreKitEventListener.goldenPick);
		tagToStoreIDMapping.Add(CrystalPickTag, StoreKitEventListener.crystalPick);
		tagToStoreIDMapping.Add(IronSwordTag, StoreKitEventListener.ironSword);
		tagToStoreIDMapping.Add(GoldenSwordTag, StoreKitEventListener.goldenSword);
		tagToStoreIDMapping.Add(GoldenRed_StoneTag, StoreKitEventListener.goldenRedStone);
		tagToStoreIDMapping.Add(GoldenSPASTag, StoreKitEventListener.goldenSPAS);
		tagToStoreIDMapping.Add(GoldenGlockTag, StoreKitEventListener.goldenGlock);
		tagToStoreIDMapping.Add(RedMinigunTag, StoreKitEventListener.redMinigun);
		tagToStoreIDMapping.Add(CrystalCrossbowTag, StoreKitEventListener.crystalCrossbow);
		tagToStoreIDMapping.Add(RedLightSaberTag, StoreKitEventListener.redLightSaber);
		tagToStoreIDMapping.Add(SandFamasTag, StoreKitEventListener.sandFamas);
		tagToStoreIDMapping.Add(WhiteBerettaTag, StoreKitEventListener.whiteBeretta);
		tagToStoreIDMapping.Add(BlackEagleTag, StoreKitEventListener.blackEagle);
		tagToStoreIDMapping.Add(CrystalAxeTag, StoreKitEventListener.crystalAxe);
		tagToStoreIDMapping.Add(SteelAxeTag, StoreKitEventListener.steelAxe);
		tagToStoreIDMapping.Add(WoodenBowTag, StoreKitEventListener.woodenBow);
		tagToStoreIDMapping.Add(Chainsaw2Tag, StoreKitEventListener.chainsaw2);
		tagToStoreIDMapping.Add(SteelCrossbowTag, StoreKitEventListener.steelCrossbow);
		tagToStoreIDMapping.Add(Hammer2Tag, StoreKitEventListener.hammer2);
		tagToStoreIDMapping.Add(Mace2Tag, StoreKitEventListener.mace2);
		tagToStoreIDMapping.Add(Sword_22Tag, StoreKitEventListener.sword_22);
		tagToStoreIDMapping.Add(Staff2Tag, StoreKitEventListener.staff2);
		tagToStoreIDMapping.Add(CrystalGlockTag, StoreKitEventListener.crystalGlock);
		tagToStoreIDMapping.Add(CrystalSPASTag, StoreKitEventListener.crystalSPAS);
		tagToStoreIDMapping.Add(TreeTag, StoreKitEventListener.tree);
		tagToStoreIDMapping.Add(FireAxeTag, StoreKitEventListener.fireAxe);
		tagToStoreIDMapping.Add(_3pl_ShotgunTag, StoreKitEventListener._3plShotgun);
		tagToStoreIDMapping.Add(Revolver2Tag, StoreKitEventListener.revolver2);
		tagToStoreIDMapping.Add(BarrettTag, StoreKitEventListener.barrett);
		tagToStoreIDMapping.Add(svdTag, StoreKitEventListener.svd);
		storeIDtoDefsSNMapping.Add("crystalsword", Defs.SwordSett);
		storeIDtoDefsSNMapping.Add("MinerWeapon", Defs.MinerWeaponSett);
		storeIDtoDefsSNMapping.Add(StoreKitEventListener.combatrifle, Defs.CombatRifleSett);
		storeIDtoDefsSNMapping.Add(StoreKitEventListener.goldeneagle, Defs.GoldenEagleSett);
		storeIDtoDefsSNMapping.Add(StoreKitEventListener.magicbow, Defs.MagicBowSett);
		storeIDtoDefsSNMapping.Add(StoreKitEventListener.axe, Defs.GoldenAxeSett);
		storeIDtoDefsSNMapping.Add(StoreKitEventListener.spas, Defs.SPASSett);
		storeIDtoDefsSNMapping.Add(StoreKitEventListener.glock, Defs.GlockSett);
		storeIDtoDefsSNMapping.Add(StoreKitEventListener.famas, Defs.FAMASS);
		storeIDtoDefsSNMapping.Add(StoreKitEventListener.chainsaw, Defs.ChainsawS);
		storeIDtoDefsSNMapping.Add(StoreKitEventListener.scythe, Defs.ScytheSN);
		storeIDtoDefsSNMapping.Add(StoreKitEventListener.shovel, Defs.ShovelSN);
		storeIDtoDefsSNMapping.Add(StoreKitEventListener.hammer, Defs.HammerSN);
		storeIDtoDefsSNMapping.Add(StoreKitEventListener.sword_2, Defs.Sword_2_SN);
		storeIDtoDefsSNMapping.Add(StoreKitEventListener.staff, Defs.StaffSN);
		storeIDtoDefsSNMapping.Add(StoreKitEventListener.laser, Defs.LaserRifleSN);
		storeIDtoDefsSNMapping.Add(StoreKitEventListener.lightSword, Defs.LightSwordSN);
		storeIDtoDefsSNMapping.Add(StoreKitEventListener.beretta, Defs.BerettaSN);
		storeIDtoDefsSNMapping.Add(StoreKitEventListener.mace, Defs.MaceSN);
		storeIDtoDefsSNMapping.Add(StoreKitEventListener.crossbow, Defs.CrossbowSN);
		storeIDtoDefsSNMapping.Add(StoreKitEventListener.minigun, Defs.MinigunSN);
		storeIDtoDefsSNMapping.Add(StoreKitEventListener.goldenPick, Defs.GoldenPickSN);
		storeIDtoDefsSNMapping.Add(StoreKitEventListener.crystalPick, Defs.CrystakPickSN);
		storeIDtoDefsSNMapping.Add(StoreKitEventListener.ironSword, Defs.IronSwordSN);
		storeIDtoDefsSNMapping.Add(StoreKitEventListener.goldenSword, Defs.GoldenSwordSN);
		storeIDtoDefsSNMapping.Add(StoreKitEventListener.goldenRedStone, Defs.GoldenRed_StoneSN);
		storeIDtoDefsSNMapping.Add(StoreKitEventListener.goldenSPAS, Defs.GoldenSPASSN);
		storeIDtoDefsSNMapping.Add(StoreKitEventListener.goldenGlock, Defs.GoldenGlockSN);
		storeIDtoDefsSNMapping.Add(StoreKitEventListener.redMinigun, Defs.RedMinigunSN);
		storeIDtoDefsSNMapping.Add(StoreKitEventListener.crystalCrossbow, Defs.CrystalCrossbowSN);
		storeIDtoDefsSNMapping.Add(StoreKitEventListener.redLightSaber, Defs.RedLightSaberSN);
		storeIDtoDefsSNMapping.Add(StoreKitEventListener.sandFamas, Defs.SandFamasSN);
		storeIDtoDefsSNMapping.Add(StoreKitEventListener.whiteBeretta, Defs.WhiteBerettaSN);
		storeIDtoDefsSNMapping.Add(StoreKitEventListener.blackEagle, Defs.BlackEagleSN);
		storeIDtoDefsSNMapping.Add(StoreKitEventListener.crystalAxe, Defs.CrystalAxeSN);
		storeIDtoDefsSNMapping.Add(StoreKitEventListener.steelAxe, Defs.SteelAxeSN);
		storeIDtoDefsSNMapping.Add(StoreKitEventListener.woodenBow, Defs.WoodenBowSN);
		storeIDtoDefsSNMapping.Add(StoreKitEventListener.chainsaw2, Defs.Chainsaw2SN);
		storeIDtoDefsSNMapping.Add(StoreKitEventListener.steelCrossbow, Defs.SteelCrossbowSN);
		storeIDtoDefsSNMapping.Add(StoreKitEventListener.hammer2, Defs.Hammer2SN);
		storeIDtoDefsSNMapping.Add(StoreKitEventListener.mace2, Defs.Mace2SN);
		storeIDtoDefsSNMapping.Add(StoreKitEventListener.sword_22, Defs.Sword_22SN);
		storeIDtoDefsSNMapping.Add(StoreKitEventListener.staff2, Defs.Staff2SN);
		storeIDtoDefsSNMapping.Add(StoreKitEventListener.crystalGlock, Defs.CrystalGlockSN);
		storeIDtoDefsSNMapping.Add(StoreKitEventListener.crystalSPAS, Defs.CrystalSPASSN);
		storeIDtoDefsSNMapping.Add(StoreKitEventListener.tree, Defs.TreeSN);
		storeIDtoDefsSNMapping.Add(StoreKitEventListener.fireAxe, Defs.FireAxeSN);
		storeIDtoDefsSNMapping.Add(StoreKitEventListener._3plShotgun, Defs._3PLShotgunSN);
		storeIDtoDefsSNMapping.Add(StoreKitEventListener.revolver2, Defs.Revolver2SN);
		storeIDtoDefsSNMapping.Add(StoreKitEventListener.barrett, Defs.BarrettSN);
		storeIDtoDefsSNMapping.Add(StoreKitEventListener.svd, Defs.SVDSN);
	}

    private UnityEngine.GameObject[] GetWeaponPrefabs()
    {
        string[] weaponsToLoad = new string[61]{
        "Weapon1", "Weapon2", "Weapon3", "Weapon4", "Weapon5", "Weapon6", "Weapon7", "Weapon8", "Weapon9", "Weapon10",
        "Weapon11", "Weapon12", "Weapon13", "Weapon14", "Weapon15", "Weapon16", "Weapon17", "Weapon18", "Weapon19", "Weapon20",
        "Weapon21", "Weapon22", "Weapon23", "Weapon24", "Weapon25", "Weapon26", "Weapon27", "Weapon28", "Weapon29", "Weapon30",
        "Weapon31", "Weapon32", "Weapon33", "Weapon34", "Weapon35", "Weapon36", "Weapon37", "Weapon38", "Weapon39", "Weapon40",
        "Weapon41", "Weapon42", "Weapon43", "Weapon44", "Weapon45", "Weapon46", "Weapon47", "Weapon48", "Weapon49", "Weapon50",
        "Weapon51", "Weapon52", "Weapon53", "Weapon54", "Weapon55", "Weapon56", "Weapon57", "Weapon58", "Weapon59", "Weapon60",
        "Weapon61"};
        GameObject[] toReturn = new GameObject[weaponsToLoad.Length];

        for (int i = 0; i < weaponsToLoad.Length; i++) // Start from 0, not 1
        {
            toReturn[i] = Resources.Load<GameObject>("weapons/" + weaponsToLoad[i]); // Use i instead of i+1
        }

        return toReturn;
    }


	public void Reset()
	{
		_playerWeapons.Clear();
		CurrentWeaponIndex = 0;
		string[] array = Storager.getString(Defs.WeaponsGotInCampaign, false).Split("#"[0]);
		List<string> list = new List<string>();
		string[] array2 = array;
		foreach (string item in array2)
		{
			list.Add(item);
		}
		for (int j = 0; j < weaponsInGame.Length; j++)
		{
		GameObject theGunThatInits = weaponsInGame[j];
		
		if (theGunThatInits.CompareTag(_initialWeaponName) || theGunThatInits.CompareTag("Knife") || 
					(!Defs.IsSurvival && PlayerPrefs.GetInt(Defs.TrainingCompleted_4_4_Sett, 0) != 0 && PlayerPrefs.GetInt("MultyPlayer") != 1 && LevelBox.weaponsFromBosses.ContainsValue(theGunThatInits.name) && list.Contains(theGunThatInits.name)) || 
					(!Defs.IsSurvival && PlayerPrefs.GetInt(Defs.TrainingCompleted_4_4_Sett, 0) != 0 && PlayerPrefs.GetInt("MultyPlayer") == 1 && theGunThatInits.name.Equals(AlienGunWN) && list.Contains(AlienGunWN)) || 
					//(!Defs.IsSurvival && PlayerPrefs.GetInt(Defs.TrainingCompleted_4_4_Sett, 0) != 0 && PlayerPrefs.GetInt("MultyPlayer") == 1 && Array.IndexOf(multiplayerWeaponTags, theGunThatInits.tag) >= 0))
					(!Defs.IsSurvival && PlayerPrefs.GetInt(Defs.TrainingCompleted_4_4_Sett, 0) != 0 && PlayerPrefs.GetInt("MultyPlayer") == 1))
		{
			Weapon weapon = new Weapon();
			weapon.weaponPrefab = theGunThatInits;
			weapon.currentAmmoInBackpack = weapon.weaponPrefab.GetComponent<WeaponSounds>().InitialAmmo;
			weapon.currentAmmoInClip = weapon.weaponPrefab.GetComponent<WeaponSounds>().ammoInClip;
			_playerWeapons.Add(weapon);
		}
	}
		if (PlayerPrefs.GetInt(Defs.TrainingCompleted_4_4_Sett, 0) == 0)
		{
			playerWeapons.Sort(new WeaponsComparer());
			return;
		}
		AddWeaponIfBought(Defs.MinerWeaponSett, PickWeaponName);
		AddWeaponIfBought(Defs.GoldenPickSN, GoldenPickWN);
		AddWeaponIfBought(Defs.CrystakPickSN, CrystalPickWN);
		AddWeaponIfBought(Defs.SteelAxeSN, SteelAxeWN);
		AddWeaponIfBought(Defs.GoldenAxeSett, GoldenAxeWeaponnName);
		AddWeaponIfBought(Defs.CrystalAxeSN, CrystalAxeWN);
		AddWeaponIfBought(Defs.SPASSett, SpasWeaponName);
		AddWeaponIfBought(Defs.ChainsawS, ChainsawWN);
		AddWeaponIfBought(Defs.Chainsaw2SN, Chainsaw2WN);
		AddWeaponIfBought(Defs.GlockSett, GlockWN);
		AddWeaponIfBought(Defs.ScytheSN, ScytheWN);
		AddWeaponIfBought(Defs.ShovelSN, ShovelWN);
		AddWeaponIfBought(Defs.Sword_2_SN, Sword_2_WN);
		AddWeaponIfBought(Defs.Sword_22SN, Sword_22WN);
		AddWeaponIfBought(Defs.HammerSN, HammerWN);
		AddWeaponIfBought(Defs.Hammer2SN, Hammer2WN);
		AddWeaponIfBought(Defs.LaserRifleSN, LaserRifleWN);
		AddWeaponIfBought(Defs.LightSwordSN, LightSwordWN);
		AddWeaponIfBought(Defs.BerettaSN, BerettaWN);
		AddWeaponIfBought(Defs.CombatRifleSett, CombatRifleWeaponName);
		AddWeaponIfBought(Defs.GoldenEagleSett, GoldenEagleWeaponName);
		AddWeaponIfBought(Defs.WoodenBowSN, WoodenBowWN);
		AddWeaponIfBought(Defs.MagicBowSett, MagicBowWeaponName);
		AddWeaponIfBought(Defs.FAMASS, FAMASWN);
		AddWeaponIfBought(Defs.StaffSN, StaffWN);
		AddWeaponIfBought(Defs.Staff2SN, Staff2WN);
		AddWeaponIfBought(Defs.MaceSN, MaceWN);
		AddWeaponIfBought(Defs.Mace2SN, Mace2WN);
		AddWeaponIfBought(Defs.MinigunSN, MinigunWN);
		AddWeaponIfBought(Defs.IronSwordSN, IronSwordWN);
		AddWeaponIfBought(Defs.GoldenSwordSN, GoldenSwordWN);
		string swordSett = Defs.SwordSett;
		string swordWeaponName = SwordWeaponName;
		AddWeaponIfBought(swordSett, swordWeaponName);
		AddWeaponIfBought(Defs.GoldenRed_StoneSN, GoldenRed_StoneWN);
		AddWeaponIfBought(Defs.GoldenSPASSN, GoldenSPASWN);
		AddWeaponIfBought(Defs.CrystalSPASSN, CrystalSPASWN);
		AddWeaponIfBought(Defs.GoldenGlockSN, GoldenGlockWN);
		AddWeaponIfBought(Defs.CrystalGlockSN, CrystalGlockWN);
		AddWeaponIfBought(Defs.RedMinigunSN, RedMinigunWN);
		AddWeaponIfBought(Defs.SteelCrossbowSN, SteelCrossbowWN);
		AddWeaponIfBought(Defs.CrossbowSN, CrossbowWN);
		AddWeaponIfBought(Defs.CrystalCrossbowSN, CrystalCrossbowWN);
		AddWeaponIfBought(Defs.RedLightSaberSN, RedLightSaberWN);
		AddWeaponIfBought(Defs.SandFamasSN, SandFamasWN);
		AddWeaponIfBought(Defs.WhiteBerettaSN, WhiteBerettaWN);
		AddWeaponIfBought(Defs.BlackEagleSN, BlackEagleWN);
		AddWeaponIfBought(Defs.TreeSN, TreeWN);
		AddWeaponIfBought(Defs.FireAxeSN, FireAxeWN);
		AddWeaponIfBought(Defs._3PLShotgunSN, _3pl_shotgunWN);
		AddWeaponIfBought(Defs.Revolver2SN, Revolver2WN);
		AddWeaponIfBought(Defs.BarrettSN, BarrettWN);
		AddWeaponIfBought(Defs.SVDSN, svdWN);
		playerWeapons.Sort(new WeaponsComparer());
	}

private void AddWeaponIfBought(string settName, string prefabName)
{
    if (Storager.getInt(settName, true) <= 0)
    {
        return;
    }
    
    UnityEngine.Object[] array = weaponsInGame;
    for (int i = 0; i < array.Length; i++)
    {
        GameObject gameObject = array[i] as GameObject; 
        if (gameObject != null && gameObject.name.Equals(prefabName))
        {
            Weapon weapon = new Weapon();
            weapon.weaponPrefab = gameObject;
            WeaponSounds weaponSounds = weapon.weaponPrefab.GetComponent<WeaponSounds>();
            
            if (weaponSounds != null)
            {
                weapon.currentAmmoInBackpack = weaponSounds.InitialAmmo;
                weapon.currentAmmoInClip = weaponSounds.ammoInClip;
            }

            _playerWeapons.Add(weapon);
            _RemovePrevVersionsOfUpgrade(gameObject.tag);
            break;
        }
    }
}

	public bool AddWeapon(GameObject weaponPrefab, out int score)
	{
		score = 0;
		foreach (Weapon playerWeapon in playerWeapons)
		{
			if (playerWeapon.weaponPrefab.CompareTag(weaponPrefab.tag))
			{
				int idx = playerWeapons.IndexOf(playerWeapon);
				if (!AddAmmo(idx))
				{
					score += Defs.ScoreForSurplusAmmo;
				}
				return false;
			}
		}
		Weapon weapon2 = new Weapon();
		weapon2.weaponPrefab = weaponPrefab;
		weapon2.currentAmmoInBackpack = weapon2.weaponPrefab.GetComponent<WeaponSounds>().InitialAmmo;
		weapon2.currentAmmoInClip = weapon2.weaponPrefab.GetComponent<WeaponSounds>().ammoInClip;
		playerWeapons.Add(weapon2);
		string tg = weaponPrefab.tag;
		_RemovePrevVersionsOfUpgrade(tg);
		playerWeapons.Sort(new WeaponsComparer());
		CurrentWeaponIndex = playerWeapons.IndexOf(weapon2);
		return true;
	}

	private void _RemovePrevVersionsOfUpgrade(string tg)
	{
		foreach (List<string> upgrade in UpgradeManager.upgrades)
		{
			int num = upgrade.IndexOf(tg);
			if (num == -1)
			{
				continue;
			}
			for (int i = 0; i < num; i++)
			{
				List<Weapon> list = new List<Weapon>();
				for (int j = 0; j < playerWeapons.Count; j++)
				{
					Weapon weapon = playerWeapons[j] as Weapon;
					if (weapon.weaponPrefab.tag.Equals(upgrade[i]))
					{
						list.Add(weapon);
					}
				}
				for (int k = 0; k < list.Count; k++)
				{
					playerWeapons.Remove(list[k]);
				}
			}
			break;
		}
	}

	public GameObject GetPickPrefab()
	{
		UnityEngine.GameObject[] array = weaponsInGame;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = array[i];
			if (gameObject.name.Equals(PickWeaponName))
			{
				return gameObject;
			}
		}
		return null;
	}

	public GameObject GetSwordPrefab()
	{
		UnityEngine.Object[] array = weaponsInGame;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.name.Equals(SwordWeaponName))
			{
				return gameObject;
			}
		}
		return null;
	}

	public GameObject GetBarrettPrefab()
	{
		UnityEngine.Object[] array = weaponsInGame;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.name.Equals(BarrettWN))
			{
				return gameObject;
			}
		}
		return null;
	}

	public GameObject GetSVDPrefab()
	{
		UnityEngine.Object[] array = weaponsInGame;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.name.Equals(svdWN))
			{
				return gameObject;
			}
		}
		return null;
	}

	public GameObject GetCombatRiflePrefab()
	{
		UnityEngine.Object[] array = weaponsInGame;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.name.Equals(CombatRifleWeaponName))
			{
				return gameObject;
			}
		}
		return null;
	}

	public GameObject GetGoldenPickPref()
	{
		UnityEngine.Object[] array = weaponsInGame;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.name.Equals(GoldenPickWN))
			{
				return gameObject;
			}
		}
		return null;
	}

	public GameObject GetCrystPickPref()
	{
		UnityEngine.Object[] array = weaponsInGame;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.name.Equals(CrystalPickWN))
			{
				return gameObject;
			}
		}
		return null;
	}

	public GameObject GetRedMinigunPref()
	{
		UnityEngine.Object[] array = weaponsInGame;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.name.Equals(RedMinigunWN))
			{
				return gameObject;
			}
		}
		return null;
	}

	public GameObject GetCrystCrossbowPref()
	{
		UnityEngine.Object[] array = weaponsInGame;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.name.Equals(CrystalCrossbowWN))
			{
				return gameObject;
			}
		}
		return null;
	}

	public GameObject GetCrystGlockPref()
	{
		UnityEngine.Object[] array = weaponsInGame;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.name.Equals(CrystalGlockWN))
			{
				return gameObject;
			}
		}
		return null;
	}

	public GameObject GetCrystalSPASPref()
	{
		UnityEngine.Object[] array = weaponsInGame;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.name.Equals(CrystalSPASWN))
			{
				return gameObject;
			}
		}
		return null;
	}

	public GameObject GetMacePrefab()
	{
		UnityEngine.Object[] array = weaponsInGame;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.name.Equals(MaceWN))
			{
				return gameObject;
			}
		}
		return null;
	}

	public GameObject GetCrossbowPrefab()
	{
		UnityEngine.Object[] array = weaponsInGame;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.name.Equals(CrossbowWN))
			{
				return gameObject;
			}
		}
		return null;
	}

	public GameObject GetMinigunPrefab()
	{
		UnityEngine.Object[] array = weaponsInGame;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.name.Equals(MinigunWN))
			{
				return gameObject;
			}
		}
		return null;
	}

	public GameObject GetIronSwordPrefab()
	{
		UnityEngine.Object[] array = weaponsInGame;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.name.Equals(IronSwordWN))
			{
				return gameObject;
			}
		}
		return null;
	}

	public GameObject GetGoldenSwordPrefab()
	{
		UnityEngine.Object[] array = weaponsInGame;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.name.Equals(GoldenSwordWN))
			{
				return gameObject;
			}
		}
		return null;
	}

	public GameObject GetGoldenSPASPrefab()
	{
		UnityEngine.Object[] array = weaponsInGame;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.name.Equals(GoldenSPASWN))
			{
				return gameObject;
			}
		}
		return null;
	}

	public GameObject GetGoldenGlockPrefab()
	{
		UnityEngine.Object[] array = weaponsInGame;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.name.Equals(GoldenGlockWN))
			{
				return gameObject;
			}
		}
		return null;
	}

	public GameObject GetGoldenRedStonePrefab()
	{
		UnityEngine.Object[] array = weaponsInGame;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.name.Equals(GoldenRed_StoneWN))
			{
				return gameObject;
			}
		}
		return null;
	}

	public GameObject GetCrystalAxePrefab()
	{
		UnityEngine.Object[] array = weaponsInGame;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.name.Equals(CrystalAxeWN))
			{
				return gameObject;
			}
		}
		return null;
	}

	public GameObject GetSteelAxePrefab()
	{
		UnityEngine.Object[] array = weaponsInGame;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.name.Equals(SteelAxeWN))
			{
				return gameObject;
			}
		}
		return null;
	}

	public GameObject GetWoodenBowPrefab()
	{
		UnityEngine.Object[] array = weaponsInGame;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.name.Equals(WoodenBowWN))
			{
				return gameObject;
			}
		}
		return null;
	}

	public GameObject GetChainsaw2Prefab()
	{
		UnityEngine.Object[] array = weaponsInGame;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.name.Equals(Chainsaw2WN))
			{
				return gameObject;
			}
		}
		return null;
	}

	public GameObject GetSteelCrossbowPrefab()
	{
		UnityEngine.Object[] array = weaponsInGame;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.name.Equals(SteelCrossbowWN))
			{
				return gameObject;
			}
		}
		return null;
	}

	public GameObject GetHammer2Prefab()
	{
		UnityEngine.Object[] array = weaponsInGame;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.name.Equals(Hammer2WN))
			{
				return gameObject;
			}
		}
		return null;
	}

	public GameObject GetMace2Prefab()
	{
		UnityEngine.Object[] array = weaponsInGame;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.name.Equals(Mace2WN))
			{
				return gameObject;
			}
		}
		return null;
	}

	public GameObject GetSword_22Prefab()
	{
		UnityEngine.Object[] array = weaponsInGame;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.name.Equals(Sword_22WN))
			{
				return gameObject;
			}
		}
		return null;
	}

	public GameObject GetStaff2Prefab()
	{
		UnityEngine.Object[] array = weaponsInGame;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.name.Equals(Staff2WN))
			{
				return gameObject;
			}
		}
		return null;
	}

	public GameObject GetGoldenEaglePrefab()
	{
		UnityEngine.Object[] array = weaponsInGame;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.name.Equals(GoldenEagleWeaponName))
			{
				return gameObject;
			}
		}
		return null;
	}

	public GameObject GetBlackEaglePrefab()
	{
		UnityEngine.Object[] array = weaponsInGame;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.name.Equals(BlackEagleWN))
			{
				return gameObject;
			}
		}
		return null;
	}

	public GameObject GetMagicBowPrefab()
	{
		UnityEngine.Object[] array = weaponsInGame;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.name.Equals(MagicBowWeaponName))
			{
				return gameObject;
			}
		}
		return null;
	}

	public GameObject GetSPASPrefab()
	{
		UnityEngine.Object[] array = weaponsInGame;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.name.Equals(SpasWeaponName))
			{
				return gameObject;
			}
		}
		return null;
	}

	public GameObject GetStaffPrefab()
	{
		UnityEngine.Object[] array = weaponsInGame;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.name.Equals(StaffWN))
			{
				return gameObject;
			}
		}
		return null;
	}

	public GameObject GetAxePrefab()
	{
		UnityEngine.Object[] array = weaponsInGame;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.name.Equals(GoldenAxeWeaponnName))
			{
				return gameObject;
			}
		}
		return null;
	}

	public GameObject GetChainsawPrefab()
	{
		UnityEngine.Object[] array = weaponsInGame;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.name.Equals(ChainsawWN))
			{
				return gameObject;
			}
		}
		return null;
	}

	public GameObject GetGlockPrefab()
	{
		UnityEngine.Object[] array = weaponsInGame;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.name.Equals(GlockWN))
			{
				return gameObject;
			}
		}
		return null;
	}

	public GameObject GetFAMASPrefab()
	{
		UnityEngine.Object[] array = weaponsInGame;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.name.Equals(FAMASWN))
			{
				return gameObject;
			}
		}
		return null;
	}

	public GameObject GetSandFamasPrefab()
	{
		UnityEngine.Object[] array = weaponsInGame;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.name.Equals(SandFamasWN))
			{
				return gameObject;
			}
		}
		return null;
	}

	public GameObject GetScythePrefab()
	{
		UnityEngine.Object[] array = weaponsInGame;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.name.Equals(ScytheWN))
			{
				return gameObject;
			}
		}
		return null;
	}

	public GameObject GetShovelPrefab()
	{
		UnityEngine.Object[] array = weaponsInGame;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.name.Equals(ShovelWN))
			{
				return gameObject;
			}
		}
		return null;
	}

	public GameObject GetTreePrefab()
	{
		UnityEngine.Object[] array = weaponsInGame;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.name.Equals(TreeWN))
			{
				return gameObject;
			}
		}
		return null;
	}

	public GameObject GetFireAxePrefab()
	{
		UnityEngine.Object[] array = weaponsInGame;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.name.Equals(FireAxeWN))
			{
				return gameObject;
			}
		}
		return null;
	}

	public GameObject Get3plShotgunPrefab()
	{
		UnityEngine.Object[] array = weaponsInGame;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.name.Equals(_3pl_shotgunWN))
			{
				return gameObject;
			}
		}
		return null;
	}

	public GameObject GetRevolver2Prefab()
	{
		UnityEngine.Object[] array = weaponsInGame;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.name.Equals(Revolver2WN))
			{
				return gameObject;
			}
		}
		return null;
	}

	public GameObject GetSword_2_Prefab()
	{
		UnityEngine.Object[] array = weaponsInGame;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.name.Equals(Sword_2_WN))
			{
				return gameObject;
			}
		}
		return null;
	}

	public GameObject GetHammerPrefab()
	{
		UnityEngine.Object[] array = weaponsInGame;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.name.Equals(HammerWN))
			{
				return gameObject;
			}
		}
		return null;
	}

	public GameObject GetLaserRiflePrefab()
	{
		UnityEngine.Object[] array = weaponsInGame;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.name.Equals(LaserRifleWN))
			{
				return gameObject;
			}
		}
		return null;
	}

	public GameObject GetLightSwordPrefab()
	{
		UnityEngine.Object[] array = weaponsInGame;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.name.Equals(LightSwordWN))
			{
				return gameObject;
			}
		}
		return null;
	}

	public GameObject GetRedLightSaberPrefab()
	{
		UnityEngine.Object[] array = weaponsInGame;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.name.Equals(RedLightSaberWN))
			{
				return gameObject;
			}
		}
		return null;
	}

	public GameObject GetBerettaPrefab()
	{
		UnityEngine.Object[] array = weaponsInGame;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.name.Equals(BerettaWN))
			{
				return gameObject;
			}
		}
		return null;
	}

	public GameObject GetWhiteBerettaPrefab()
	{
		UnityEngine.Object[] array = weaponsInGame;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.name.Equals(WhiteBerettaWN))
			{
				return gameObject;
			}
		}
		return null;
	}

	public bool AddAmmo(int idx = -1)
	{
		if (idx == -1)
		{
			idx = CurrentWeaponIndex;
		}
		if (idx == CurrentWeaponIndex && currentWeaponSounds.isMelee)
		{
			return false;
		}
		Weapon weapon = (Weapon)playerWeapons[idx];
		WeaponSounds component = weapon.weaponPrefab.GetComponent<WeaponSounds>();
		if (weapon.currentAmmoInBackpack < component.MaxAmmoWithRespectToInApp)
		{
			weapon.currentAmmoInBackpack += component.ammoInClip;
			if (weapon.currentAmmoInBackpack > component.MaxAmmoWithRespectToInApp)
			{
				weapon.currentAmmoInBackpack = component.MaxAmmoWithRespectToInApp;
			}
			return true;
		}
		return false;
	}

	public void SetMaxAmmoFrAllWeapons()
	{
		foreach (Weapon playerWeapon in playerWeapons)
		{
			playerWeapon.currentAmmoInClip = playerWeapon.weaponPrefab.GetComponent<WeaponSounds>().ammoInClip;
			playerWeapon.currentAmmoInBackpack = playerWeapon.weaponPrefab.GetComponent<WeaponSounds>().MaxAmmoWithRespectToInApp;

        }
	}

	private void Start()
	{
		_purchaseActinos.Add("MinerWeapon", AddMinerWeaponToInventoryAndSaveInApp);
		_purchaseActinos.Add("crystalsword", AddSwordToInventoryAndSaveInApp);
		_purchaseActinos.Add(StoreKitEventListener.combatrifle, AddCombatRifle);
		_purchaseActinos.Add(StoreKitEventListener.goldeneagle, AddGoldenEagle);
		_purchaseActinos.Add(StoreKitEventListener.magicbow, AddMagicBow);
		_purchaseActinos.Add(StoreKitEventListener.axe, AddGoldenAxe);
		_purchaseActinos.Add(StoreKitEventListener.spas, AddSPAS);
		_purchaseActinos.Add(StoreKitEventListener.chainsaw, AddChainsaw);
		_purchaseActinos.Add(StoreKitEventListener.glock, AddGlock);
		_purchaseActinos.Add(StoreKitEventListener.famas, AddFAMAS);
		_purchaseActinos.Add(StoreKitEventListener.scythe, AddScythe);
		_purchaseActinos.Add(StoreKitEventListener.shovel, AddShovel);
		_purchaseActinos.Add(StoreKitEventListener.sword_2, AddSword_2);
		_purchaseActinos.Add(StoreKitEventListener.hammer, AddHammer);
		_purchaseActinos.Add(StoreKitEventListener.staff, AddStaff);
		_purchaseActinos.Add(StoreKitEventListener.laser, AddLaser);
		_purchaseActinos.Add(StoreKitEventListener.lightSword, AddLightSword);
		_purchaseActinos.Add(StoreKitEventListener.beretta, AddBeretta);
		_purchaseActinos.Add(StoreKitEventListener.mace, AddMace);
		_purchaseActinos.Add(StoreKitEventListener.crossbow, AddCrossbow);
		_purchaseActinos.Add(StoreKitEventListener.minigun, AddMinigun);
		_purchaseActinos.Add(StoreKitEventListener.goldenPick, AddGoldenPick);
		_purchaseActinos.Add(StoreKitEventListener.crystalPick, AddCrystalPick);
		_purchaseActinos.Add(StoreKitEventListener.ironSword, AddIronSword);
		_purchaseActinos.Add(StoreKitEventListener.goldenSword, AddGoldenSword);
		_purchaseActinos.Add(StoreKitEventListener.goldenRedStone, AddGoldenRed_stone);
		_purchaseActinos.Add(StoreKitEventListener.goldenGlock, AddGoldenGlock);
		_purchaseActinos.Add(StoreKitEventListener.goldenSPAS, AddGoldenSPAS);
		_purchaseActinos.Add(StoreKitEventListener.redMinigun, AddRedMinigun);
		_purchaseActinos.Add(StoreKitEventListener.crystalCrossbow, AddCrystCrossbow);
		_purchaseActinos.Add(StoreKitEventListener.redLightSaber, AddRedLightSaber);
		_purchaseActinos.Add(StoreKitEventListener.sandFamas, AddSandFamas);
		_purchaseActinos.Add(StoreKitEventListener.whiteBeretta, AddWhiteBeretta);
		_purchaseActinos.Add(StoreKitEventListener.blackEagle, AddBlackEagle);
		_purchaseActinos.Add(StoreKitEventListener.crystalAxe, AddCrystalAxe);
		_purchaseActinos.Add(StoreKitEventListener.steelAxe, AddSteelAxe);
		_purchaseActinos.Add(StoreKitEventListener.woodenBow, AddWoodenBow);
		_purchaseActinos.Add(StoreKitEventListener.chainsaw2, AddChainsaw2);
		_purchaseActinos.Add(StoreKitEventListener.steelCrossbow, AddSteelCrossbow);
		_purchaseActinos.Add(StoreKitEventListener.hammer2, AddHammer2);
		_purchaseActinos.Add(StoreKitEventListener.mace2, AddMace2);
		_purchaseActinos.Add(StoreKitEventListener.sword_22, AddSword_22);
		_purchaseActinos.Add(StoreKitEventListener.staff2, AddStaff2);
		_purchaseActinos.Add(StoreKitEventListener.crystalGlock, AddCrystalGlock);
		_purchaseActinos.Add(StoreKitEventListener.crystalSPAS, AddCrystalSPAS);
		_purchaseActinos.Add(StoreKitEventListener.tree, AddTree);
		_purchaseActinos.Add(StoreKitEventListener.fireAxe, AddFireAxe);
		_purchaseActinos.Add(StoreKitEventListener._3plShotgun, Add3plShotgun);
		_purchaseActinos.Add(StoreKitEventListener.revolver2, AddRevolver2);
		_purchaseActinos.Add(StoreKitEventListener.barrett, AddBarrett);
		_purchaseActinos.Add(StoreKitEventListener.svd, AddSVD);
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		if (!Application.isEditor && Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
		{
			GoogleIABManager.purchaseSucceededEvent += AddWeapon;
		}
		_weaponsInGame = GetWeaponPrefabs();
		Reset();
	}

	public void AddStaff()
	{
		Player_move_c.SaveStaffPrefs();
		int score;
		AddWeapon(GetStaffPrefab(), out score);
	}

	public void AddLaser()
	{
		Player_move_c.SaveLaserRiflePrefs();
		int score;
		AddWeapon(GetLaserRiflePrefab(), out score);
	}

	public void AddGoldenAxe()
	{
		Player_move_c.SaveMGoldenAxeInPrefs();
		int score;
		AddWeapon(GetAxePrefab(), out score);
	}

	public void AddLightSword()
	{
		Player_move_c.SaveLightSwordInPrefs();
		int score;
		AddWeapon(GetLightSwordPrefab(), out score);
	}

	public void AddRedLightSaber()
	{
		Player_move_c.SaveRedLightSaberInPrefs();
		int score;
		AddWeapon(GetRedLightSaberPrefab(), out score);
	}

	public void AddTree()
	{
		Player_move_c.SaveTreeInPrefs();
		int score;
		AddWeapon(GetTreePrefab(), out score);
	}

	public void AddFireAxe()
	{
		Player_move_c.SaveFireAxeInPrefs();
		int score;
		AddWeapon(GetFireAxePrefab(), out score);
	}

	public void Add3plShotgun()
	{
		Player_move_c.Save3plShotInnPrefs();
		int score;
		AddWeapon(Get3plShotgunPrefab(), out score);
	}

	public void AddRevolver2()
	{
		Player_move_c.SaveRevolver2InPrefs();
		int score;
		AddWeapon(GetRevolver2Prefab(), out score);
	}

	public void AddMace()
	{
		Player_move_c.SaveMaceInPrefs();
		int score;
		AddWeapon(GetMacePrefab(), out score);
	}

	public void AddCrossbow()
	{
		Player_move_c.SaveCrossbowInPrefs();
		int score;
		AddWeapon(GetCrossbowPrefab(), out score);
	}

	public void AddCrystalGlock()
	{
		Player_move_c.SaveCrystalGlockPrefs();
		int score;
		AddWeapon(GetCrystGlockPref(), out score);
	}

	public void AddCrystalSPAS()
	{
		Player_move_c.SaveCrystalSPASInPrefs();
		int score;
		AddWeapon(GetCrystalSPASPref(), out score);
	}

	public void AddGoldenPick()
	{
		Player_move_c.SaveGoldenPickPrefs();
		int score;
		AddWeapon(GetGoldenPickPref(), out score);
	}

	public void AddCrystalPick()
	{
		Player_move_c.SaveCrystalPickPrefs();
		int score;
		AddWeapon(GetCrystPickPref(), out score);
	}

	public void AddMinigun()
	{
		Player_move_c.SaveMinigunInPrefs();
		int score;
		AddWeapon(GetMinigunPrefab(), out score);
	}

	public void AddBeretta()
	{
		Player_move_c.SaveBerettaInPrefs();
		int score;
		AddWeapon(GetBerettaPrefab(), out score);
	}

	public void AddWhiteBeretta()
	{
		Player_move_c.SaveWhiteBerettaInPrefs();
		int score;
		AddWeapon(GetWhiteBerettaPrefab(), out score);
	}

	public void AddSPAS()
	{
		Player_move_c.SaveSPASInPrefs();
		int score;
		AddWeapon(GetSPASPrefab(), out score);
	}

	public void AddChainsaw()
	{
		Player_move_c.SaveChainsawInPrefs();
		int score;
		AddWeapon(GetChainsawPrefab(), out score);
	}

	public void AddGlock()
	{
		Player_move_c.SaveGlockInPrefs();
		int score;
		AddWeapon(GetGlockPrefab(), out score);
	}

	public void AddFAMAS()
	{
		Player_move_c.SaveFAMASPrefs();
		int score;
		AddWeapon(GetFAMASPrefab(), out score);
	}

	public void AddSandFamas()
	{
		Player_move_c.SaveSandFamasInPrefs();
		int score;
		AddWeapon(GetSandFamasPrefab(), out score);
	}

	public void AddScythe()
	{
		Player_move_c.SaveScytheInPrefs();
		int score;
		AddWeapon(GetScythePrefab(), out score);
	}

	public void AddRedMinigun()
	{
		Player_move_c.SaveRedMinigunPrefs();
		int score;
		AddWeapon(GetRedMinigunPref(), out score);
	}

	public void AddCrystCrossbow()
	{
		Player_move_c.SaveCrystalCrossbowInPrefs();
		int score;
		AddWeapon(GetCrystCrossbowPref(), out score);
	}

	public void AddShovel()
	{
		Player_move_c.SaveShovelPrefs();
		int score;
		AddWeapon(GetShovelPrefab(), out score);
	}

	public void AddSword_2()
	{
		Player_move_c.SaveSword_2_InPrefs();
		int score;
		AddWeapon(GetSword_2_Prefab(), out score);
	}

	public void AddHammer()
	{
		Player_move_c.SaveHammerPrefs();
		int score;
		AddWeapon(GetHammerPrefab(), out score);
	}

	public void AddIronSword()
	{
		Player_move_c.SaveIronSwordInPrefs();
		int score;
		AddWeapon(GetIronSwordPrefab(), out score);
	}

	public void AddGoldenSword()
	{
		Player_move_c.SaveGoldenSwordInPrefs();
		int score;
		AddWeapon(GetGoldenSwordPrefab(), out score);
	}

	public void AddGoldenSPAS()
	{
		Player_move_c.SaveGoldenSPASSN();
		int score;
		AddWeapon(GetGoldenSPASPrefab(), out score);
	}

	public void AddGoldenRed_stone()
	{
		Player_move_c.SaveGoldenRed_Stone();
		int score;
		AddWeapon(GetGoldenRedStonePrefab(), out score);
	}

	public void AddGoldenGlock()
	{
		Player_move_c.SaveGoldenGlockInPrefs();
		int score;
		AddWeapon(GetGoldenGlockPrefab(), out score);
	}

	public void AddCrystalAxe()
	{
		Player_move_c.SaveCrystalAxeInPrefs();
		int score;
		AddWeapon(GetCrystalAxePrefab(), out score);
	}

	public void AddSteelAxe()
	{
		Player_move_c.SaveSteelAxeInPrefs();
		int score;
		AddWeapon(GetSteelAxePrefab(), out score);
	}

	public void AddWoodenBow()
	{
		Player_move_c.SaveWoodenBowInPrefs();
		int score;
		AddWeapon(GetWoodenBowPrefab(), out score);
	}

	public void AddChainsaw2()
	{
		Player_move_c.SaveChainsaw2InPrefs();
		int score;
		AddWeapon(GetChainsaw2Prefab(), out score);
	}

	public void AddSteelCrossbow()
	{
		Player_move_c.SaveSteelCrossbowInPrefs();
		int score;
		AddWeapon(GetSteelCrossbowPrefab(), out score);
	}

	public void AddHammer2()
	{
		Player_move_c.SaveHammer2InPrefs();
		int score;
		AddWeapon(GetHammer2Prefab(), out score);
	}

	public void AddMace2()
	{
		Player_move_c.SaveMace2InPrefs();
		int score;
		AddWeapon(GetMace2Prefab(), out score);
	}

	public void AddSword_22()
	{
		Player_move_c.SaveSword_22InPrefs();
		int score;
		AddWeapon(GetSword_22Prefab(), out score);
	}

	public void AddStaff2()
	{
		Player_move_c.SaveStaff2InPrefs();
		int score;
		AddWeapon(GetStaff2Prefab(), out score);
	}

	public void AddMinerWeaponToInventoryAndSaveInApp()
	{
		Player_move_c.SaveMinerWeaponInPrefabs();
		int score;
		AddWeapon(GetPickPrefab(), out score);
	}

	public void AddSwordToInventoryAndSaveInApp()
	{
		Player_move_c.SaveSwordInPrefs();
		int score;
		AddWeapon(GetSwordPrefab(), out score);
	}

	public void AddBarrett()
	{
		Player_move_c.SaveBarrettInPrefabs();
		int score;
		AddWeapon(GetBarrettPrefab(), out score);
	}

	public void AddSVD()
	{
		Player_move_c.SaveSvdInPrefs();
		int score;
		AddWeapon(GetSVDPrefab(), out score);
	}

	public void AddCombatRifle()
	{
		Player_move_c.SaveCombatRifleInPrefs();
		int score;
		AddWeapon(GetCombatRiflePrefab(), out score);
	}

	public void AddGoldenEagle()
	{
		Player_move_c.SaveGoldenEagleInPrefs();
		int score;
		AddWeapon(GetGoldenEaglePrefab(), out score);
	}

	public void AddBlackEagle()
	{
		Player_move_c.SaveBlackEagleInPrefs();
		int score;
		AddWeapon(GetBlackEaglePrefab(), out score);
	}

	public void AddMagicBow()
	{
		Player_move_c.SaveMagicBowInPrefs();
		int score;
		AddWeapon(GetMagicBowPrefab(), out score);
	}

	public void AddMinerWeapon(string id)
	{
		if (id == null)
		{
			throw new ArgumentNullException("id");
		}
		if (_purchaseActinos.ContainsKey(id))
		{
			_purchaseActinos[id]();
		}
	}

	private void AddWeapon(GooglePurchase p)
	{
		try
		{
			AddMinerWeapon(p.productId);
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
	}

	private void OnDestroy()
	{
		if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
		{
			GoogleIABManager.purchaseSucceededEvent -= AddWeapon;
		}
	}

	public void Reload()
	{
		currentWeaponSounds.animationObject.GetComponent<Animation>().Stop("Empty");
		currentWeaponSounds.animationObject.GetComponent<Animation>().CrossFade("Shoot");
		currentWeaponSounds.animationObject.GetComponent<Animation>().Play("Reload");
		int num = currentWeaponSounds.ammoInClip - ((Weapon)playerWeapons[CurrentWeaponIndex]).currentAmmoInClip;
		if (((Weapon)playerWeapons[CurrentWeaponIndex]).currentAmmoInBackpack >= num)
		{
			((Weapon)playerWeapons[CurrentWeaponIndex]).currentAmmoInClip += num;
			((Weapon)playerWeapons[CurrentWeaponIndex]).currentAmmoInBackpack -= num;
		}
		else
		{
			((Weapon)playerWeapons[CurrentWeaponIndex]).currentAmmoInClip += ((Weapon)playerWeapons[CurrentWeaponIndex]).currentAmmoInBackpack;
			((Weapon)playerWeapons[CurrentWeaponIndex]).currentAmmoInBackpack = 0;
		}
	}
}