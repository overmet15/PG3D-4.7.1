using System.Collections.Generic;
using UnityEngine;

internal sealed class LevelBox
{
	public static List<LevelBox> campaignBoxes;

	public List<CampaignLevel> levels = new List<CampaignLevel>();

	public int starsToOpen;

	public string name;

	public string mapName;

	public string PreviewNAme = string.Empty;

	public static Dictionary<string, string> weaponsFromBosses;

	public int CompletionExperienceAward { get; set; }

	static LevelBox()
	{
		campaignBoxes = new List<LevelBox>();
		weaponsFromBosses = new Dictionary<string, string>();
		LevelBox item = new LevelBox
		{
			starsToOpen = int.MaxValue,
			PreviewNAme = "Box_coming_soon",
			name = "coming soon",
			CompletionExperienceAward = 0
		};
		weaponsFromBosses.Add("Farm", "Weapon2");
		weaponsFromBosses.Add("Cementery", "Weapon3");
		weaponsFromBosses.Add("City", "Weapon4");
		weaponsFromBosses.Add("Hospital", "Weapon8");
		weaponsFromBosses.Add("Jail", "Weapon5");
		weaponsFromBosses.Add("Slender", "Weapon51");
		weaponsFromBosses.Add("Area52", "Weapon52");
		weaponsFromBosses.Add("Bridge", WeaponManager.M16_2WN);
		LevelBox levelBox = new LevelBox
		{
			starsToOpen = (Debug.isDebugBuild ? 1 : 25),
			name = "minecraft",
			mapName = string.Empty,
			PreviewNAme = "Box_2",
			CompletionExperienceAward = 50
		};
		CampaignLevel item2 = new CampaignLevel
		{
			sceneName = "Utopia"
		};
		CampaignLevel item3 = new CampaignLevel
		{
			sceneName = "Maze"
		};
		CampaignLevel item4 = new CampaignLevel
		{
			sceneName = "Sky_islands"
		};
		CampaignLevel item5 = new CampaignLevel
		{
			sceneName = "Winter"
		};
		CampaignLevel item6 = new CampaignLevel
		{
			sceneName = "Castle"
		};
		CampaignLevel item7 = new CampaignLevel
		{
			sceneName = "Gluk_2"
		};
		levelBox.levels.Add(item2);
		levelBox.levels.Add(item3);
		levelBox.levels.Add(item4);
		levelBox.levels.Add(item5);
		levelBox.levels.Add(item6);
		levelBox.levels.Add(item7);
		LevelBox levelBox2 = new LevelBox
		{
			name = "Real",
			mapName = string.Empty,
			PreviewNAme = "Box_1",
			CompletionExperienceAward = 70
		};
		CampaignLevel item8 = new CampaignLevel
		{
			sceneName = "Farm"
		};
		CampaignLevel item9 = new CampaignLevel
		{
			sceneName = "Cementery"
		};
		CampaignLevel item10 = new CampaignLevel
		{
			sceneName = "City"
		};
		CampaignLevel item11 = new CampaignLevel
		{
			sceneName = "Hospital"
		};
		CampaignLevel item12 = new CampaignLevel
		{
			sceneName = "Bridge"
		};
		CampaignLevel item13 = new CampaignLevel
		{
			sceneName = "Jail"
		};
		CampaignLevel item14 = new CampaignLevel
		{
			sceneName = "Slender"
		};
		CampaignLevel item15 = new CampaignLevel
		{
			sceneName = "Area52"
		};
		CampaignLevel item16 = new CampaignLevel
		{
			sceneName = "School"
		};
		levelBox2.levels.Add(item8);
		levelBox2.levels.Add(item9);
		levelBox2.levels.Add(item10);
		levelBox2.levels.Add(item11);
		levelBox2.levels.Add(item12);
		levelBox2.levels.Add(item13);
		levelBox2.levels.Add(item14);
		levelBox2.levels.Add(item15);
		levelBox2.levels.Add(item16);
		campaignBoxes.Add(levelBox2);
		campaignBoxes.Add(levelBox);
		campaignBoxes.Add(item);
	}
}
