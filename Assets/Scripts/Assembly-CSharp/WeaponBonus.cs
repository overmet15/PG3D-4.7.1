using System.Collections.Generic;
using UnityEngine;

public class WeaponBonus : MonoBehaviour
{
	public GameObject weaponPrefab;

	private GameObject _player;

	private Player_move_c _playerMoveC;

	private bool runLoading;

	public Texture levelResult;

	private void Start()
	{
		_player = GameObject.FindGameObjectWithTag("Player");
		_playerMoveC = GameObject.FindGameObjectWithTag("PlayerGun").GetComponent<Player_move_c>();
		if (!Defs.IsSurvival)
		{
			GameObject gameObject = Object.Instantiate(Resources.Load("BonusFX") as GameObject, Vector3.zero, Quaternion.identity) as GameObject;
			gameObject.transform.parent = base.transform;
			gameObject.transform.localPosition = Vector3.zero;
		}
	}

	private void Update()
	{
		float num = 120f;
		base.transform.Rotate(base.transform.InverseTransformDirection(Vector3.up), num * Time.deltaTime);
		if (runLoading || !(Vector3.Distance(base.transform.position, _player.transform.position) < 1.5f))
		{
			return;
		}
		_playerMoveC.AddWeapon(weaponPrefab);
		if (Defs.IsSurvival || Defs.IsTraining)
		{
			if (Defs.IsTraining)
			{
				TrainingController.isNextStep = TrainingController.stepTrainingList["GetTheGun"];
			}
			Object.Destroy(base.gameObject);
			return;
		}
		string[] array = Storager.getString(Defs.WeaponsGotInCampaign, false).Split("#"[0]);
		List<string> list = new List<string>();
		string[] array2 = array;
		foreach (string item in array2)
		{
			list.Add(item);
		}
		if (!list.Contains(LevelBox.weaponsFromBosses[Application.loadedLevelName]))
		{
			list.Add(LevelBox.weaponsFromBosses[Application.loadedLevelName]);
			Storager.setString(Defs.WeaponsGotInCampaign, string.Join("#"[0].ToString(), list.ToArray()), false);
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
		LevelCompleteLoader.action = null;
		LevelCompleteLoader.sceneName = "LevelComplete";
		AutoFade.LoadLevel("LevelToCompleteProm", 2f, 0f, Color.white);
		levelResult = Resources.Load(ResPath.Combine("CoinsIndicationSystem", "level_complete")) as Texture;
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
