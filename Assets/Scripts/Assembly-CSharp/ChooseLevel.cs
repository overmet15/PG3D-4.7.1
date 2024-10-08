using System;
using System.Collections.Generic;
using Fuckhead.PixlGun3D;
using UnityEngine;

internal sealed class ChooseLevel : MonoBehaviour
{
	private sealed class LevelInfo
	{
		public bool Enabled { get; set; }

		public string Name { get; set; }

		public int StarGainedCount { get; set; }
	}

	public GameObject panel;

	public GameObject[] starEnabledPrototypes;

	public GameObject[] starDisabledPrototypes;

	public GameObject gainedStarCountLabel;

	public GameObject backButton;

	public GameObject shopButton;

	public GameObject[] boxOneLevelButtons;

	public GameObject[] boxTwoLevelButtons;

	public AudioClip shopButtonSound;

	public GameObject backgroundHolder;

	public GameObject[] boxContents;

	private float _timeStarted;

	private int _boxIndex;

	private GameObject[] _boxLevelButtons;

	private string _gainedStarCount = string.Empty;

	private IList<LevelInfo> _levelInfos = new List<LevelInfo>();

	private Shop _shopInstance;

	private float _timeWhenShopWasClosed;

	private void OnGUI()
	{
		if (_shopInstance != null)
		{
			_shopInstance.SetHatsAndCapesEnabled(false);
			_shopInstance.SetGearCatEnabled(false);
			_shopInstance.ShowShop(true);
		}
	}

	private void Start()
	{
		_timeStarted = Time.realtimeSinceStartup;
		bool draggableLayout = panel != null && panel.GetComponent<UIDraggablePanel>() != null && panel.GetComponent<UIDraggablePanel>().enabled;
		_boxIndex = LevelBox.campaignBoxes.FindIndex((LevelBox b) => b.name == CurrentCampaignGame.boXName);
		if (_boxIndex == -1)
		{
			Debug.LogWarning("Box not found in list!");
			throw new InvalidOperationException("Box not found in list!");
		}
		IList<LevelInfo> levelInfos;
		if (true)
		{
			IList<LevelInfo> list = InitializeLevelInfos(draggableLayout);
			levelInfos = list;
		}
		else
		{
			levelInfos = InitializeLevelInfosWithTestData(draggableLayout);
		}
		_levelInfos = levelInfos;
		_gainedStarCount = InitializeGainStarCount(_levelInfos);
		Texture texture = null;
		if (CurrentCampaignGame.boXName == "Real")
		{
			_boxLevelButtons = boxOneLevelButtons;
			texture = Resources.Load(ResPath.Combine("Boxes", "Box1_map")) as Texture;
		}
		else if (CurrentCampaignGame.boXName == "minecraft")
		{
			_boxLevelButtons = boxTwoLevelButtons;
			texture = Resources.Load(ResPath.Combine("Boxes", "Box2_bck")) as Texture;
		}
		else
		{
			Debug.LogError("Unknown box: " + CurrentCampaignGame.boXName);
		}
		if (texture == null)
		{
			Debug.LogError("Could not load texture for background in the box " + CurrentCampaignGame.boXName);
			throw new InvalidOperationException();
		}
		if (backgroundHolder == null)
		{
			Debug.LogError("Background holder is null.");
			throw new InvalidOperationException();
		}
		UITexture component = backgroundHolder.GetComponent<UITexture>();
		if (component == null)
		{
			Debug.LogError("Could not find UITexture component.");
			throw new InvalidOperationException();
		}
		component.mainTexture = texture;
		InitializeLevelButtons();
		InitializeFixedDisplay();
	}

	private void InitializeFixedDisplay()
	{
		if (backButton != null)
		{
			backButton.GetComponent<ButtonHandler>().Clicked += HandleBackButton;
		}
		if (shopButton != null)
		{
			shopButton.GetComponent<ButtonHandler>().Clicked += HandleShopButton;
		}
		if (gainedStarCountLabel != null)
		{
			gainedStarCountLabel.GetComponent<UILabel>().text = _gainedStarCount;
		}
	}

	private void HandleBackButton(object sender, EventArgs args)
	{
		if (!(_shopInstance != null) && !(Time.time - _timeWhenShopWasClosed < 1f))
		{
			Application.LoadLevel("CampaignChooseBox");
		}
	}

	private void HandleShopButton(object sender, EventArgs args)
	{
		if (!(_shopInstance == null))
		{
			return;
		}
		_shopInstance = Shop.sharedShop;
		if (_shopInstance != null)
		{
			if (shopButtonSound != null && PlayerPrefsX.GetBool(PlayerPrefsX.SoundFXSetting, true))
			{
				NGUITools.PlaySound(shopButtonSound);
			}
			_shopInstance.loadShopCategories();
			_shopInstance.resumeAction = HandleResumeFromShop;
		}
	}

	private void HandleResumeFromShop()
	{
		if (_shopInstance != null)
		{
			_shopInstance.resumeAction = delegate
			{
			};
			_shopInstance.unloadShopCategories();
			_shopInstance = null;
			_timeWhenShopWasClosed = Time.time;
		}
	}

	private void InitializeLevelButtons()
	{
		if (starEnabledPrototypes != null)
		{
			GameObject[] array = starEnabledPrototypes;
			foreach (GameObject gameObject in array)
			{
				if (gameObject != null)
				{
					gameObject.SetActive(false);
				}
			}
		}
		if (starDisabledPrototypes != null)
		{
			GameObject[] array2 = starDisabledPrototypes;
			foreach (GameObject gameObject2 in array2)
			{
				if (gameObject2 != null)
				{
					gameObject2.SetActive(false);
				}
			}
		}
		if (boxContents != null)
		{
			for (int k = 0; k != boxContents.Length; k++)
			{
				boxContents[k].SetActive(k == _boxIndex);
			}
			if (_boxLevelButtons == null)
			{
				throw new InvalidOperationException("Box level buttons are null.");
			}
			GameObject[] boxLevelButtons = _boxLevelButtons;
			foreach (GameObject gameObject3 in boxLevelButtons)
			{
				if (gameObject3 != null)
				{
					UIImageButton component = gameObject3.GetComponent<UIImageButton>();
					if (component != null)
					{
						component.isEnabled = false;
					}
				}
			}
			int num = Math.Min(_levelInfos.Count, _boxLevelButtons.Length);
			for (int m = 0; m != num; m++)
			{
				LevelInfo levelInfo = _levelInfos[m];
				GameObject gameObject4 = _boxLevelButtons[m];
				gameObject4.transform.parent = gameObject4.transform.parent;
				gameObject4.GetComponent<UIImageButton>().isEnabled = levelInfo.Enabled;
				gameObject4.AddComponent<ButtonHandler>();
				string levelName = levelInfo.Name;
				gameObject4.GetComponent<ButtonHandler>().Clicked += delegate
				{
					HandleLevelButton(levelName);
				};
				gameObject4.SetActive(true);
				for (int n = 0; n != starEnabledPrototypes.Length; n++)
				{
					if (levelInfo.Enabled)
					{
						GameObject gameObject5 = starEnabledPrototypes[n];
						if (!(gameObject5 == null))
						{
							GameObject gameObject6 = UnityEngine.Object.Instantiate(gameObject5) as GameObject;
							gameObject6.transform.parent = gameObject4.transform;
							gameObject6.GetComponent<UICheckbox>().startsChecked = n < levelInfo.StarGainedCount;
							gameObject6.transform.localPosition = gameObject5.transform.localPosition;
							gameObject6.transform.localScale = gameObject5.transform.localScale;
							gameObject6.SetActive(true);
						}
					}
				}
			}
			GameObject[] array3 = starEnabledPrototypes;
			foreach (GameObject gameObject7 in array3)
			{
				if (gameObject7 != null)
				{
					UnityEngine.Object.Destroy(gameObject7);
				}
			}
			GameObject[] array4 = starDisabledPrototypes;
			foreach (GameObject gameObject8 in array4)
			{
				if (gameObject8 != null)
				{
					UnityEngine.Object.Destroy(gameObject8);
				}
			}
			return;
		}
		throw new InvalidOperationException("boxContents == 0");
	}

	private void HandleLevelButton(string levelName)
	{
		if (!(_shopInstance != null) && !(Time.realtimeSinceStartup - _timeStarted < 0.15f))
		{
			CurrentCampaignGame.levelSceneName = levelName;
			GameObject.FindGameObjectWithTag("WeaponManager").GetComponent<WeaponManager>().Reset();
			PlayerPrefs.SetFloat(Defs.CurrentHealthSett, Player_move_c.MaxPlayerHealth);
			PlayerPrefs.SetFloat(Defs.CurrentArmorSett, 0f);
			PlayerPrefs.SetInt(Defs.ArmorType, 0);
			FlurryPluginWrapper.LogLevelPressed(CurrentCampaignGame.levelSceneName);
			LevelArt.endOfBox = false;
			Application.LoadLevel("LevelArt");
		}
	}

	private static IList<LevelInfo> InitializeLevelInfosWithTestData(bool draggableLayout = false)
	{
		List<LevelInfo> list = new List<LevelInfo>();
		list.Add(new LevelInfo
		{
			Enabled = true,
			Name = "Cementery",
			StarGainedCount = 1
		});
		list.Add(new LevelInfo
		{
			Enabled = true,
			Name = "City",
			StarGainedCount = 3
		});
		list.Add(new LevelInfo
		{
			Enabled = false,
			Name = "Hospital"
		});
		return list;
	}

	private static IList<LevelInfo> InitializeLevelInfos(bool draggableLayout = false)
	{
		List<LevelInfo> list = new List<LevelInfo>();
		string boxName = CurrentCampaignGame.boXName;
		int num = LevelBox.campaignBoxes.FindIndex((LevelBox b) => b.name == boxName);
		if (num == -1)
		{
			Debug.LogWarning("Box not found in list!");
			return list;
		}
		LevelBox levelBox = LevelBox.campaignBoxes[num];
		List<CampaignLevel> levels = levelBox.levels;
		Dictionary<string, int> value;
		if (!CampaignProgress.boxesLevelsAndStars.TryGetValue(boxName, out value))
		{
			Debug.LogWarning("Box not found in dictionary!");
			value = new Dictionary<string, int>();
		}
		for (int i = 0; i != levels.Count; i++)
		{
			string sceneName = levels[i].sceneName;
			int value2 = 0;
			value.TryGetValue(sceneName, out value2);
			LevelInfo levelInfo = new LevelInfo();
			levelInfo.Enabled = i <= value.Count;
			levelInfo.Name = sceneName;
			levelInfo.StarGainedCount = value2;
			LevelInfo item = levelInfo;
			list.Add(item);
		}
		return list;
	}

	private static string InitializeGainStarCount(IList<LevelInfo> levelInfos)
	{
		int num = 3 * levelInfos.Count;
		int num2 = 0;
		foreach (LevelInfo levelInfo in levelInfos)
		{
			num2 += levelInfo.StarGainedCount;
		}
		return string.Format("{0}/{1}", num2, num);
	}
}
