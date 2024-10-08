using System;
using System.Collections;
using UnityEngine;

public class WeaponsComparer : IComparer
{
	private static int baseLngth = "Weapon".Length;

	private static string[] multiplayerWeaponsOrd = new string[59]
	{
		WeaponManager.PistolWN,
		WeaponManager.ShotgunWN,
		WeaponManager.MP5WN,
		WeaponManager.AK47WN,
		WeaponManager.M16_2WN,
		WeaponManager.KnifeWN,
		WeaponManager.GoldenEagleWeaponName,
		WeaponManager.BlackEagleWN,
		WeaponManager.ObrezWN,
		WeaponManager.BerettaWN,
		WeaponManager.WhiteBerettaWN,
		WeaponManager.Revolver2WN,
		WeaponManager.GlockWN,
		WeaponManager.GoldenGlockWN,
		WeaponManager.CrystalGlockWN,
		WeaponManager.AlienGunWN,
		WeaponManager.CombatRifleWeaponName,
		WeaponManager.MinigunWN,
		WeaponManager.RedMinigunWN,
		WeaponManager.SpasWeaponName,
		WeaponManager.GoldenSPASWN,
		WeaponManager.CrystalSPASWN,
		WeaponManager._3pl_shotgunWN,
		WeaponManager.FAMASWN,
		WeaponManager.SandFamasWN,
		WeaponManager.WoodenBowWN,
		WeaponManager.MagicBowWeaponName,
		WeaponManager.LaserRifleWN,
		WeaponManager.GoldenRed_StoneWN,
		WeaponManager.LightSwordWN,
		WeaponManager.RedLightSaberWN,
		WeaponManager.PickWeaponName,
		WeaponManager.GoldenPickWN,
		WeaponManager.CrystalPickWN,
		WeaponManager.IronSwordWN,
		WeaponManager.GoldenSwordWN,
		WeaponManager.SwordWeaponName,
		WeaponManager.SteelAxeWN,
		WeaponManager.GoldenAxeWeaponnName,
		WeaponManager.CrystalAxeWN,
		WeaponManager.ChainsawWN,
		WeaponManager.Chainsaw2WN,
		WeaponManager.ScytheWN,
		WeaponManager.MaceWN,
		WeaponManager.Mace2WN,
		WeaponManager.ShovelWN,
		WeaponManager.SteelCrossbowWN,
		WeaponManager.CrossbowWN,
		WeaponManager.CrystalCrossbowWN,
		WeaponManager.Sword_2_WN,
		WeaponManager.Sword_22WN,
		WeaponManager.HammerWN,
		WeaponManager.Hammer2WN,
		WeaponManager.StaffWN,
		WeaponManager.Staff2WN,
		WeaponManager.FireAxeWN,
		WeaponManager.TreeWN,
		WeaponManager.BarrettWN,
		WeaponManager.svdWN
	};

	public int Compare(object x, object y)
	{
		string name = ((Weapon)x).weaponPrefab.name;
		string name2 = ((Weapon)y).weaponPrefab.name;
		if (PlayerPrefs.GetInt("MultyPlayer", 0) == 1)
		{
			return Array.IndexOf(multiplayerWeaponsOrd, name2).CompareTo(Array.IndexOf(multiplayerWeaponsOrd, name));
		}
		name = name.Substring(baseLngth);
		name2 = name2.Substring(baseLngth);
		int num = int.Parse(name);
		int num2 = int.Parse(name2);
		return num - num2;
	}
}
