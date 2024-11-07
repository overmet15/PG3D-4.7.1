using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rilisoft.PixlGun3D
{
	internal sealed class LevelCompleteScript : MonoBehaviour
	{
		public GameObject quitButton;

		public GameObject menuButton;

		public GameObject retryButton;

		public GameObject nextButton;

		public GameObject shopButton;

		public GameObject brightStarPrototypeSprite;

		public GameObject darkStarPrototypeSprite;

		public GameObject award1coinSprite;

		public GameObject award15coinsSprite;

		public GameObject checkboxSpritePrototype;

		public AudioClip[] coinClips;

		public AudioClip[] starClips;

		public AudioClip shopButtonSound;

		public AudioClip awardClip;

		public GameObject finishBlockArt;

		public GameObject survivalResults;

		public GameObject[] statisticLabels;

		public GameObject gameOverSprite;

		private bool _awardConferred;

		private AudioSource _awardAudioSource;

		private ExperienceController _experienceController;

		private int _starCount;

		private Shop _shopInstance;

		private string _nextSceneName = string.Empty;

		private bool _isLastLevel;

		private int? _boxCompletionExperienceAward;

		private bool completedFirstTime;

		private bool _gameOver;

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
			if (PlayerPrefs.GetInt("IsGameOver", 0) == 1)
			{
				_gameOver = true;
				PlayerPrefs.SetInt("IsGameOver", 0);
			}
			_experienceController = InitializeExperienceController();
			BindButtonHandler(menuButton, HandleMenuButton);
			BindButtonHandler(retryButton, HandleRetryButton);
			BindButtonHandler(nextButton, HandleNextButton);
			BindButtonHandler(shopButton, HandleShopButton);
			BindButtonHandler(quitButton, HandleQuitButton);
			if (!Defs.IsSurvival)
			{
				int num = -1;
				LevelBox levelBox = null;
				foreach (LevelBox campaignBox in LevelBox.campaignBoxes)
				{
					if (!campaignBox.name.Equals(CurrentCampaignGame.boXName))
					{
						continue;
					}
					levelBox = campaignBox;
					for (int i = 0; i != campaignBox.levels.Count; i++)
					{
						CampaignLevel campaignLevel = campaignBox.levels[i];
						if (campaignLevel.sceneName.Equals(CurrentCampaignGame.levelSceneName))
						{
							num = i;
							break;
						}
					}
					break;
				}
				if (levelBox != null)
				{
					_isLastLevel = num >= levelBox.levels.Count - 1;
					_nextSceneName = levelBox.levels[(!_isLastLevel) ? (num + 1) : num].sceneName;
				}
				else
				{
					Debug.LogError("Current box not found in the list of boxes!");
					_isLastLevel = true;
					_nextSceneName = Application.loadedLevelName;
				}
				_starCount = InitializeStarCount();
				if (!_gameOver)
				{
					Dictionary<string, int> dictionary = CampaignProgress.boxesLevelsAndStars[CurrentCampaignGame.boXName];
					if (!dictionary.ContainsKey(CurrentCampaignGame.levelSceneName))
					{
						completedFirstTime = true;
						if (_isLastLevel)
						{
							_boxCompletionExperienceAward = levelBox.CompletionExperienceAward;
						}
						dictionary.Add(CurrentCampaignGame.levelSceneName, _starCount);
						FlurryPluginWrapper.LogEventWithParameterAndValue("LevelReached", "Level_Name", CurrentCampaignGame.levelSceneName);
					}
					else
					{
						dictionary[CurrentCampaignGame.levelSceneName] = Math.Max(dictionary[CurrentCampaignGame.levelSceneName], _starCount);
					}
					CampaignProgress.OpenNewBoxIfPossible();
					CampaignProgress.SaveCampaignProgress();
				}
				_awardConferred = InitializeAwardConferred();
			}
			survivalResults.SetActive(false);
			quitButton.SetActive(false);
			if (!_gameOver)
			{
				if (award1coinSprite != null && award15coinsSprite != null)
				{
					award1coinSprite.SetActive(!_awardConferred);
					award15coinsSprite.SetActive(_awardConferred);
				}
				GameObject[] array = statisticLabels;
				foreach (GameObject gameObject in array)
				{
					gameObject.SetActive(Defs.IsSurvival);
				}
				if (brightStarPrototypeSprite != null && darkStarPrototypeSprite != null)
				{
					StartCoroutine(DisplayLevelResult());
				}
				CoinsMessage.FireCoinsAddedEvent();
				return;
			}
			award1coinSprite.SetActive(false);
			award15coinsSprite.SetActive(false);
			nextButton.SetActive(false);
			checkboxSpritePrototype.SetActive(false);
			if (!Defs.IsSurvival && gameOverSprite != null)
			{
				gameOverSprite.SetActive(true);
			}
			if (Defs.IsSurvival)
			{
				StartCoroutine(DisplaySurvivalResult());
			}
			GameObject[] array2 = statisticLabels;
			foreach (GameObject gameObject2 in array2)
			{
				gameObject2.SetActive(Defs.IsSurvival);
			}
			if (!Defs.IsSurvival)
			{
				float x = (retryButton.transform.position.x - menuButton.transform.position.x) / 2f;
				Vector3 vector = new Vector3(x, 0f, 0f);
				menuButton.transform.position = retryButton.transform.position - vector;
				retryButton.transform.position += vector;
			}
			menuButton.SetActive(!Defs.IsSurvival);
		}

		private void OnDestroy()
		{
			if (_experienceController != null)
			{
				_experienceController.isShowRanks = false;
			}
		}

		private static void BindButtonHandler(GameObject button, EventHandler handler)
		{
			if (button != null)
			{
				ButtonHandler component = button.GetComponent<ButtonHandler>();
				if (component != null)
				{
					component.Clicked += handler;
				}
			}
		}

		private static int CalculateExperienceAward(int score)
		{
			int num = ((!Application.isEditor) ? 1 : 100);
			if (score < 15000 / num)
			{
				return 0;
			}
			if (score < 50000 / num)
			{
				return 5;
			}
			if (score < 100000 / num)
			{
				return 25;
			}
			if (score < 150000 / num)
			{
				return 50;
			}
			return 75;
		}

		private IEnumerator DisplaySurvivalResult()
        {
            if (_experienceController == null) _experienceController = FindObjectOfType<ExperienceController>();
            menuButton.SetActive(false);
			retryButton.SetActive(false);
			nextButton.SetActive(false);
			shopButton.SetActive(false);
			quitButton.SetActive(false);
			survivalResults.SetActive(true);
			int experienceAward = CalculateExperienceAward(GlobalGameController.Score);
			if (experienceAward > 0)
			{
				_experienceController.addExperience(experienceAward);
				yield return null;
			}
			while (_experienceController.isShowAdd)
			{
				yield return null;
			}
			retryButton.SetActive(true);
			shopButton.SetActive(true);
			quitButton.SetActive(true);
		}

		private static int InitializeCoinIndexBound()
		{
			int @int = PlayerPrefs.GetInt(Defs.DiffSett, 1);
			return @int + 1;
		}

		private IEnumerator DisplayLevelResult()
		{
			menuButton.SetActive(false);
			retryButton.SetActive(false);
			nextButton.SetActive(false);
			shopButton.SetActive(false);
			int coinIndexBound = InitializeCoinIndexBound();
			List<GameObject> stars = new List<GameObject>(3);
			for (int j = 0; j != 3; j++)
			{
				float x = -140f + (float)j * 140f;
				GameObject star = UnityEngine.Object.Instantiate(darkStarPrototypeSprite) as GameObject;
				star.transform.parent = darkStarPrototypeSprite.transform.parent;
				star.GetComponent<UISprite>().MakePixelPerfect();
				star.transform.localPosition = new Vector3(x, darkStarPrototypeSprite.transform.localPosition.y, 0f);
				star.transform.localScale = darkStarPrototypeSprite.transform.localScale;
				star.SetActive(true);
				stars.Add(star);
			}
			int currentStarIndex = 0;
			for (int checkboxIndex = 0; checkboxIndex < 3; checkboxIndex++)
			{
				if ((checkboxIndex == 1 && !CurrentCampaignGame.completeInTime) || (checkboxIndex == 2 && !CurrentCampaignGame.withoutHits))
				{
					continue;
				}
				yield return new WaitForSeconds(0.4f);
				GameObject star2 = UnityEngine.Object.Instantiate(brightStarPrototypeSprite) as GameObject;
				star2.transform.parent = brightStarPrototypeSprite.transform.parent;
				star2.GetComponent<UISprite>().MakePixelPerfect();
				star2.transform.localPosition = stars[currentStarIndex].transform.localPosition;
				star2.transform.localScale = stars[currentStarIndex].transform.localScale;
				star2.SetActive(true);
				UnityEngine.Object.Destroy(stars[currentStarIndex]);
				GameObject checkbox = UnityEngine.Object.Instantiate(checkboxSpritePrototype) as GameObject;
				checkbox.transform.parent = checkboxSpritePrototype.transform.parent;
				checkbox.GetComponent<UISprite>().MakePixelPerfect();
				checkbox.transform.localPosition = new Vector3(checkboxSpritePrototype.transform.localPosition.x, checkboxSpritePrototype.transform.localPosition.y - 50f * (float)checkboxIndex, checkboxSpritePrototype.transform.localPosition.z);
				checkbox.transform.localScale = checkboxSpritePrototype.transform.localScale;
				checkbox.SetActive(true);
				if (starClips != null && currentStarIndex < starClips.Length && starClips[currentStarIndex] != null && PlayerPrefsX.GetBool(PlayerPrefsX.SoundFXSetting, true))
				{
					NGUITools.PlaySound(starClips[currentStarIndex]);
				}
				yield return new WaitForSeconds(0.3f);
				if (currentStarIndex < coinIndexBound)
				{
					Storager.setInt(val: Storager.getInt(Defs.Coins, false) + 1, key: Defs.Coins, useICloud: false);
					if (coinClips != null && currentStarIndex < coinClips.Length && coinClips[currentStarIndex] != null && PlayerPrefsX.GetBool(PlayerPrefsX.SoundFXSetting, true))
					{
						NGUITools.PlaySound(coinClips[currentStarIndex]);
					}
					FlurryPluginWrapper.LogCoinEarned();
				}
				currentStarIndex++;
			}
			int gainedStarCount = currentStarIndex;
			if (_awardConferred)
			{
				yield return new WaitForSeconds(0.4f);
				Storager.setInt(val: Storager.getInt(Defs.Coins, false) + 15, key: Defs.Coins, useICloud: false);
				if (awardClip != null && PlayerPrefsX.GetBool(PlayerPrefsX.SoundFXSetting, true))
				{
					_awardAudioSource = NGUITools.PlaySound(awardClip);
				}
				for (int i = 0; i != 15; i++)
				{
					FlurryPluginWrapper.LogCoinEarned();
				}
			}
			UnityEngine.Object.Destroy(brightStarPrototypeSprite);
			UnityEngine.Object.Destroy(darkStarPrototypeSprite);
			if (_experienceController != null)
			{
				if (_awardConferred && awardClip != null)
				{
					yield return new WaitForSeconds(awardClip.length);
				}
				yield return new WaitForSeconds(1f);
				int experience = 0;
				if (gainedStarCount == 3)
				{
					experience += 5;
				}
				if (_boxCompletionExperienceAward.HasValue)
				{
					experience += _boxCompletionExperienceAward.Value;
				}
				if (experience != 0)
				{
					_experienceController.addExperience(experience);
				}
				while (_experienceController.isShowAdd)
				{
					yield return null;
				}
			}
			menuButton.SetActive(true);
			retryButton.SetActive(true);
			nextButton.SetActive(true);
			shopButton.SetActive(true);
		}

		private void HandleMenuButton(object sender, EventArgs args)
		{
			if (!(_shopInstance != null))
			{
				if (Defs.IsSurvival)
				{
					FlurryPluginWrapper.LogEvent("Back to Main Menu");
				}
				Application.LoadLevel((!Defs.IsSurvival) ? "ChooseLevel" : Defs.MainMenuScene);
			}
		}

		private void HandleQuitButton(object sender, EventArgs args)
		{
			FlurryPluginWrapper.LogEvent("Back to Main Menu");
			Application.LoadLevel(Defs.MainMenuScene);
		}

		private void HandleRetryButton(object sender, EventArgs args)
		{
			if (_shopInstance != null)
			{
				return;
			}
			WeaponManager component = GameObject.FindGameObjectWithTag("WeaponManager").GetComponent<WeaponManager>();
			if (!Defs.IsSurvival)
			{
				foreach (Weapon playerWeapon in component.playerWeapons)
				{
					WeaponSounds component2 = playerWeapon.weaponPrefab.GetComponent<WeaponSounds>();
					if (playerWeapon.currentAmmoInClip + playerWeapon.currentAmmoInBackpack < component2.InitialAmmo + component2.ammoInClip)
					{
						playerWeapon.currentAmmoInClip = component2.ammoInClip;
						playerWeapon.currentAmmoInBackpack = component2.InitialAmmo;
					}
				}
			}
			else
			{
				component.Reset();
			}
			PlayerPrefs.SetFloat(Defs.CurrentHealthSett, Player_move_c.MaxPlayerHealth);
			PlayerPrefs.SetFloat(Defs.CurrentArmorSett, 0f);
			PlayerPrefs.SetFloat(Defs.CurrentHealthSett, Player_move_c.MaxPlayerHealth);
			PlayerPrefs.SetFloat(Defs.CurrentArmorSett, 0f);
			GlobalGameController.Score = 0;
			Application.LoadLevel("CampaignLoading");
		}

		private void HandleNextButton(object sender, EventArgs args)
		{
			if (!(_shopInstance != null))
			{
				if (!_isLastLevel)
				{
					CurrentCampaignGame.levelSceneName = _nextSceneName;
					LevelArt.endOfBox = false;
					Application.LoadLevel("LevelArt");
				}
				else
				{
					LevelArt.endOfBox = true;
					Application.LoadLevel("LevelArt");
				}
			}
		}

		private void HandleShopButton(object sender, EventArgs args)
		{
			if (_shopInstance != null)
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
			if (!(_shopInstance == null))
			{
				_shopInstance.resumeAction = delegate
				{
				};
				_shopInstance.unloadShopCategories();
				_shopInstance = null;
			}
		}

		private static ExperienceController InitializeExperienceController()
		{
			ExperienceController experienceController = FindObjectOfType<ExperienceController>();
			experienceController.posRanks = new Vector2(21f * Defs.Coef, 21f * Defs.Coef);
			experienceController.isShowRanks = true;
			return experienceController;
		}

		private static int InitializeStarCount()
		{
			if (Application.isEditor)
			{
				return 3;
			}
			int num = 1;
			if (CurrentCampaignGame.completeInTime)
			{
				num++;
			}
			if (CurrentCampaignGame.withoutHits)
			{
				num++;
			}
			return num;
		}

		private bool InitializeAwardConferred()
		{
			return _isLastLevel && completedFirstTime;
		}
	}
}
