using System;
using UnityEngine;

public class GotToNextLevel : MonoBehaviour
{
	private Action OnPlayerAddedAct;

	private GameObject _player;

	private Player_move_c _playerMoveC;

	private bool runLoading;

	public Texture levelResult;

	private void Awake()
	{
		OnPlayerAddedAct = delegate
		{
			_player = GameObject.FindGameObjectWithTag("Player");
			_playerMoveC = GameObject.FindGameObjectWithTag("PlayerGun").GetComponent<Player_move_c>();
		};
		Initializer.PlayerAddedEvent += OnPlayerAddedAct;
	}

	private void OnDestroy()
	{
		Initializer.PlayerAddedEvent -= OnPlayerAddedAct;
	}

	private void Update()
	{
		if (_player == null || _playerMoveC == null || runLoading || !(Vector3.Distance(base.transform.position, _player.transform.position) < 1.5f))
		{
			return;
		}
		PlayerPrefs.SetFloat(Defs.CurrentHealthSett, _playerMoveC.CurHealth);
		PlayerPrefs.SetFloat(Defs.CurrentArmorSett, _playerMoveC.curArmor);
		PlayerPrefs.SetInt(Defs.ArmorType, _playerMoveC._armorType);
		runLoading = true;
		Debug.Log("end GlobalGameController.currentLevel " + GlobalGameController.currentLevel);
		if (PlayerPrefs.GetInt("FullVersion", 0) == 0 && GlobalGameController.currentLevel == 5)
		{
			GameObject.FindGameObjectWithTag("PlayerGun").GetComponent<Player_move_c>().showGUIUnlockFullVersion = true;
			return;
		}
		if (PlayerPrefs.GetInt(Defs.TrainingCompleted_4_4_Sett, 0) == 0)
		{
			if (!Storager.hasKey(Defs.CoinsAfterTrainingSN))
			{
				Storager.setInt(Defs.CoinsAfterTrainingSN, 0, false);
			}
			Storager.setInt(Defs.CoinsAfterTrainingSN, 1, false);
		}
		GoToNextLevel();
		levelResult = Resources.Load(ResPath.Combine("CoinsIndicationSystem", "level_complete")) as Texture;
	}

	public static void GoToNextLevel()
	{
		if (PlayerPrefs.GetInt(Defs.TrainingCompleted_4_4_Sett, 0) == 0)
		{
			PlayerPrefs.SetInt(Defs.TrainingCompleted_4_4_Sett, 1);
			PlayerPrefs.Save();
			LevelCompleteLoader.sceneName = Defs.MainMenuScene;
		}
		else
		{
			LevelCompleteLoader.action = null;
			LevelCompleteLoader.sceneName = "LevelComplete";
		}
		AutoFade.LoadLevel("LevelToCompleteProm", 2f, 0f, Color.white);
	}

	private void OnGUI()
	{
		if (levelResult != null)
		{
			float num = 520f * Defs.Coef;
			float num2 = 129f * Defs.Coef;
			GUI.DrawTexture(new Rect(((float)Screen.width - num) / 2f, (float)Screen.height / 2f - num2 * 3f / 3f, num, num2), levelResult);
		}
	}
}
