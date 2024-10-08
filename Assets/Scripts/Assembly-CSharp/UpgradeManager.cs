using System.Collections.Generic;

public class UpgradeManager
{
	public static List<List<string>> upgrades;

	static UpgradeManager()
	{
		upgrades = new List<List<string>>();
		List<string> item = new List<string>
		{
			WeaponManager.MinersWeaponTag,
			WeaponManager.GoldenPickTag,
			WeaponManager.CrystalPickTag
		};
		upgrades.Add(item);
		List<string> item2 = new List<string>
		{
			WeaponManager.StaffTag,
			WeaponManager.Staff2Tag
		};
		upgrades.Add(item2);
		List<string> item3 = new List<string>
		{
			WeaponManager.ChainsawTag,
			WeaponManager.Chainsaw2Tag
		};
		upgrades.Add(item3);
		List<string> item4 = new List<string>
		{
			WeaponManager.Sword_2Tag,
			WeaponManager.Sword_22Tag
		};
		upgrades.Add(item4);
		List<string> item5 = new List<string>
		{
			WeaponManager.WoodenBowTag,
			WeaponManager.MagicBowTag
		};
		upgrades.Add(item5);
		List<string> item6 = new List<string>
		{
			WeaponManager.SteelAxeTag,
			WeaponManager.GoldenAxeTag,
			WeaponManager.CrystalAxeTag
		};
		upgrades.Add(item6);
		List<string> item7 = new List<string>
		{
			WeaponManager.IronSwordTag,
			WeaponManager.GoldenSwordTag,
			WeaponManager.CrystalSwordTag
		};
		upgrades.Add(item7);
		List<string> item8 = new List<string>
		{
			WeaponManager.Red_StoneTag,
			WeaponManager.GoldenRed_StoneTag
		};
		upgrades.Add(item8);
		List<string> item9 = new List<string>
		{
			WeaponManager.SPASTag,
			WeaponManager.GoldenSPASTag,
			WeaponManager.CrystalSPASTag
		};
		upgrades.Add(item9);
		List<string> item10 = new List<string>
		{
			WeaponManager.HammerTag,
			WeaponManager.Hammer2Tag
		};
		upgrades.Add(item10);
		List<string> item11 = new List<string>
		{
			WeaponManager.SteelCrossbowTag,
			WeaponManager.CrossbowTag,
			WeaponManager.CrystalCrossbowTag
		};
		upgrades.Add(item11);
		List<string> item12 = new List<string>
		{
			WeaponManager.MaceTag,
			WeaponManager.Mace2Tag
		};
		upgrades.Add(item12);
		List<string> item13 = new List<string>
		{
			WeaponManager.MinigunTag,
			WeaponManager.RedMinigunTag
		};
		upgrades.Add(item13);
		List<string> item14 = new List<string>
		{
			WeaponManager.LightSwordTag,
			WeaponManager.RedLightSaberTag
		};
		upgrades.Add(item14);
		List<string> item15 = new List<string>
		{
			WeaponManager.FAMASTag,
			WeaponManager.SandFamasTag
		};
		upgrades.Add(item15);
		List<string> item16 = new List<string>
		{
			WeaponManager.BerettaTag,
			WeaponManager.WhiteBerettaTag
		};
		upgrades.Add(item16);
		List<string> item17 = new List<string>
		{
			WeaponManager.EagleTag,
			WeaponManager.BlackEagleTag
		};
		upgrades.Add(item17);
		List<string> item18 = new List<string>
		{
			WeaponManager.GlockTag,
			WeaponManager.GoldenGlockTag,
			WeaponManager.CrystalGlockTag
		};
		upgrades.Add(item18);
	}
}
