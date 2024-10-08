using System.Collections.Generic;

public class VirtualCurrencyHelper
{
	public static int[] coinInappsQuantity;

	public static Dictionary<string, int> prices;

	static VirtualCurrencyHelper()
	{
		coinInappsQuantity = new int[7] { 15, 45, 80, 165, 330, 800, 2000 };
		prices = new Dictionary<string, int>();
		prices.Add("crystalsword", 35);
		prices.Add("Fullhealth", 15);
		prices.Add("bigammopack", 15);
		prices.Add("MinerWeapon", 30);
		prices.Add(StoreKitEventListener.elixirID, 15);
		prices.Add(StoreKitEventListener.combatrifle, 75);
		prices.Add(StoreKitEventListener.magicbow, 40);
		prices.Add(StoreKitEventListener.goldeneagle, 45);
		prices.Add(StoreKitEventListener.chief, 25);
		prices.Add(StoreKitEventListener.nanosoldier, 25);
		prices.Add(StoreKitEventListener.endmanskin, 25);
		prices.Add(StoreKitEventListener.spaceengineer, 25);
		prices.Add(StoreKitEventListener.steelman, 25);
		prices.Add(StoreKitEventListener.CaptainSkin, 25);
		prices.Add(StoreKitEventListener.HawkSkin, 25);
		prices.Add(StoreKitEventListener.TunderGodSkin, 25);
		prices.Add(StoreKitEventListener.GreenGuySkin, 25);
		prices.Add(StoreKitEventListener.GordonSkin, 25);
		prices.Add(StoreKitEventListener.axe, 15);
		prices.Add(StoreKitEventListener.spas, 60);
		prices.Add(StoreKitEventListener.armor, 10);
		prices.Add(StoreKitEventListener.armor2, 15);
		prices.Add(StoreKitEventListener.armor3, 20);
		prices.Add(StoreKitEventListener.chainsaw, 75);
		prices.Add(StoreKitEventListener.famas, 75);
		prices.Add(StoreKitEventListener.glock, 45);
		prices.Add(StoreKitEventListener.scythe, 60);
		prices.Add(StoreKitEventListener.shovel, 30);
		prices.Add(StoreKitEventListener.hammer, 70);
		prices.Add(StoreKitEventListener.sword_2, 120);
		prices.Add(StoreKitEventListener.staff, 180);
		prices.Add(StoreKitEventListener.laser, 180);
		prices.Add(StoreKitEventListener.lightSword, 120);
		prices.Add(StoreKitEventListener.beretta, 90);
		prices.Add(StoreKitEventListener.magicGirl, 25);
		prices.Add(StoreKitEventListener.braveGirl, 25);
		prices.Add(StoreKitEventListener.glamDoll, 25);
		prices.Add(StoreKitEventListener.kittyGirl, 25);
		prices.Add(StoreKitEventListener.famosBoy, 25);
		prices.Add(StoreKitEventListener.mace, 85);
		prices.Add(StoreKitEventListener.crossbow, 30);
		prices.Add(StoreKitEventListener.minigun, 200);
		prices.Add(StoreKitEventListener.goldenPick, 15);
		prices.Add(StoreKitEventListener.crystalPick, 25);
		prices.Add(StoreKitEventListener.ironSword, 50);
		prices.Add(StoreKitEventListener.goldenSword, 25);
		prices.Add(StoreKitEventListener.goldenRedStone, 45);
		prices.Add(StoreKitEventListener.goldenSPAS, 30);
		prices.Add(StoreKitEventListener.crystalSPAS, 45);
		prices.Add(StoreKitEventListener.goldenGlock, 25);
		prices.Add(StoreKitEventListener.crystalGlock, 50);
		prices.Add(StoreKitEventListener.redMinigun, 50);
		prices.Add(StoreKitEventListener.crystalCrossbow, 45);
		prices.Add(StoreKitEventListener.redLightSaber, 45);
		prices.Add(StoreKitEventListener.sandFamas, 30);
		prices.Add(StoreKitEventListener.whiteBeretta, 35);
		prices.Add(StoreKitEventListener.blackEagle, 25);
		prices.Add(StoreKitEventListener.crystalAxe, 35);
		prices.Add(StoreKitEventListener.steelAxe, 30);
		prices.Add(StoreKitEventListener.woodenBow, 80);
		prices.Add(StoreKitEventListener.chainsaw2, 35);
		prices.Add(StoreKitEventListener.steelCrossbow, 105);
		prices.Add(StoreKitEventListener.hammer2, 25);
		prices.Add(StoreKitEventListener.mace2, 25);
		prices.Add(StoreKitEventListener.sword_22, 50);
		prices.Add(StoreKitEventListener.staff2, 60);
		prices.Add(StoreKitEventListener.tree, 75);
		prices.Add(StoreKitEventListener.fireAxe, 100);
		prices.Add(StoreKitEventListener._3plShotgun, 150);
		prices.Add(StoreKitEventListener.revolver2, 95);
		prices.Add(Wear.cape_Archimage, 65);
		prices.Add(Wear.cape_BloodyDemon, 50);
		prices.Add(Wear.cape_RoyalKnight, 65);
		prices.Add(Wear.cape_SkeletonLord, 75);
		prices.Add(Wear.cape_EliteCrafter, 50);
		prices.Add(Wear.hat_DiamondHelmet, 65);
		prices.Add(Wear.hat_Headphones, 50);
		prices.Add(Wear.hat_ManiacMask, 65);
		prices.Add(Wear.hat_KingsCrown, 150);
		prices.Add(Wear.hat_SeriousManHat, 50);
		prices.Add(StoreKitEventListener.barrett, 199);
		prices.Add(StoreKitEventListener.svd, 220);
	}
}
