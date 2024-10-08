using UnityEngine;

public class CampaignLoading : MonoBehaviour
{
	private Texture fonToDraw;

	private Texture plashkaCoins;

	private Rect plashkaCoinsRect;

	private void Start()
	{
		string b;
		if (!Defs.IsSurvival)
		{
			if (PlayerPrefs.GetInt(Defs.TrainingCompleted_4_4_Sett, 0) != 0)
			{
				int num = 0;
				LevelBox levelBox = null;
				foreach (LevelBox campaignBox in LevelBox.campaignBoxes)
				{
					if (!campaignBox.name.Equals(CurrentCampaignGame.boXName))
					{
						continue;
					}
					levelBox = campaignBox;
					foreach (CampaignLevel level in campaignBox.levels)
					{
						if (level.sceneName.Equals(CurrentCampaignGame.levelSceneName))
						{
							num = campaignBox.levels.IndexOf(level);
							break;
						}
					}
				}
				bool flag = false;
				flag = num >= levelBox.levels.Count - 1;
				bool flag2 = false;
				if (!CampaignProgress.boxesLevelsAndStars[CurrentCampaignGame.boXName].ContainsKey(CurrentCampaignGame.levelSceneName))
				{
					flag2 = true;
				}
				b = (Defs.IsSurvival ? "gey_surv" : ((!flag2 || !flag) ? "gey_1" : "gey_15"));
			}
			else
			{
				b = string.Empty;
			}
		}
		else
		{
			b = "gey_surv";
		}
		plashkaCoins = ((PlayerPrefs.GetInt(Defs.TrainingCompleted_4_4_Sett, 0) != 0) ? (Resources.Load(ResPath.Combine("CoinsIndicationSystem", b)) as Texture) : null);
		float num2 = 500f * Defs.Coef;
		float height = 244f * Defs.Coef;
		plashkaCoinsRect = new Rect(((float)Screen.width - num2) / 2f, (float)Screen.height * 0.4f, num2, height);
		string b2 = ((PlayerPrefs.GetInt(Defs.TrainingCompleted_4_4_Sett, 0) != 0) ? Switcher.loadingNames[CurrentCampaignGame.currentLevel] : "Loading_Training");
		fonToDraw = Resources.Load(ResPath.Combine(Switcher.LoadingInResourcesPath, b2)) as Texture;
		if (Defs.IsSurvival)
		{
		}
		Invoke("Load", 2f);
	}

	private void OnGUI()
	{
		Rect position = new Rect(((float)Screen.width - 2048f * (float)Screen.height / 1154f) / 2f, 0f, 2048f * (float)Screen.height / 1154f, Screen.height);
		GUI.DrawTexture(position, fonToDraw, ScaleMode.StretchToFill);
		if (plashkaCoins != null)
		{
			GUI.DrawTexture(plashkaCoinsRect, plashkaCoins, ScaleMode.StretchToFill);
		}
	}

	private void Load()
	{
		if (Defs.IsSurvival)
		{
			Application.LoadLevel("Coliseum");
		}
		else
		{
			Application.LoadLevel((PlayerPrefs.GetInt(Defs.TrainingCompleted_4_4_Sett, 0) != 0) ? CurrentCampaignGame.levelSceneName : "Training");
		}
	}
}
