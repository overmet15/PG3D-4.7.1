using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ZombieCreator : MonoBehaviour
{
	private static int _ZombiesInWave;

	private int currentWave;

	private static List<List<string>> _enemiesInWaves;

	private static List<List<string>> _WeaponsAddedInWaves;

	public static List<string> survivalAvailableWeapons;

	private bool _generatingZombiesIsStopped;

	private int totalNumOfKilledEnemies;

	public GUIStyle labelStyle;

	private int[] _intervalArr = new int[3] { 6, 4, 3 };

	private int _genWithThisTimeInterval;

	private int _indexInTimesArray;

	private bool _drawWaveMsg;

	private string _msg = string.Empty;

	private GameObject[] _teleports;

	public bool bossShowm;

	public List<GameObject> zombiePrefabs = new List<GameObject>();

	private bool _isMultiplayer;

	private int _numOfLiveZombies;

	private int _numOfDeadZombies;

	private int _numOfDeadZombsSinceLastFast;

	public float curInterval = 10f;

	private GameObject[] _enemyCreationZones;

	private List<string[]> _enemies = new List<string[]>();
	public int NumOfLiveZombies
	{
		get
		{
			return _numOfLiveZombies;
		}
		set
		{
			_numOfLiveZombies = value;
		}
	}

	public bool IsLasTMonsRemains
	{
		get
		{
			return NumOfDeadZombies + 1 == NumOfEnemisesToKill && !bossShowm;
		}
	}

	public int NumOfDeadZombies
	{
		get
		{
			return _numOfDeadZombies;
		}
		set
		{
			if (bossShowm)
			{
				bossShowm = false;
				if (ZombieCreator.BossKilled != null)
				{
					ZombieCreator.BossKilled();
				}
				if (!LevelBox.weaponsFromBosses.ContainsKey(Application.loadedLevelName))
				{
					GameObject[] teleports = _teleports;
					foreach (GameObject gameObject in teleports)
					{
						gameObject.SetActive(true);
					}
				}
				return;
			}
			int num = value - _numOfDeadZombies;
			_numOfDeadZombies = value;
			totalNumOfKilledEnemies += num;
			NumOfLiveZombies -= num;
			if (!Defs.IsSurvival)
			{
				_numOfDeadZombsSinceLastFast += num;
				if (_numOfDeadZombsSinceLastFast == 12)
				{
					if (curInterval > 5f)
					{
						curInterval -= 5f;
					}
					_numOfDeadZombsSinceLastFast = 0;
				}
				if (IsLasTMonsRemains && ZombieCreator.LastEnemy != null)
				{
					ZombieCreator.LastEnemy();
				}
			}
			if (_numOfDeadZombies < NumOfEnemisesToKill)
			{
				return;
			}
			if (Defs.IsSurvival)
			{
				currentWave++;
				_UpdateIntervalStructures();
				_numOfDeadZombies = 0;
				_numOfDeadZombsSinceLastFast = 0;
				_UpdateZombiePrefabs();
				_UpdateAvailableWeapons();
				_generatingZombiesIsStopped = false;
				StartCoroutine(_DrawWaveMessage());
				GameObject gameObject2 = Initializer.CreateCoinAtPos(new Vector3(0f, 1f, 0f));
				gameObject2.GetComponent<CoinBonus>().SetPlayer();
				FlurryPluginWrapper.LogWaveReached(currentWave + 1);
			}
			else if (CurrentCampaignGame.currentLevel == 0)
			{
				GameObject[] teleports2 = _teleports;
				foreach (GameObject gameObject3 in teleports2)
				{
					if (Defs.IsTraining)
					{
						TrainingController.isNextStep = TrainingController.stepTrainingList["KillZombi"];
					}
					gameObject3.SetActive(true);
				}
			}
			else
			{
				_createBoss();
			}
		}
	}

	public static int NumOfEnemisesToKill
	{
		get
		{
			return (!Defs.IsSurvival) ? GlobalGameController.EnemiesToKill : _ZombiesInWave;
		}
	}

	public static event Action LastEnemy;

	public static event Action BossKilled;
	static ZombieCreator()
	{
		_ZombiesInWave = 45;
		_enemiesInWaves = new List<List<string>>();
		_WeaponsAddedInWaves = new List<List<string>>();
		survivalAvailableWeapons = new List<string>();
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		List<string> list3 = new List<string>();
		List<string> list4 = new List<string>();
		List<string> list5 = new List<string>();
		list.Add("1");
		list.Add("2");
		list.Add("15");
		list2.Add("1");
		list2.Add("2");
		list2.Add("15");
		list2.Add("11");
		list2.Add("12");
		list3.Add("3");
		list3.Add("9");
		list3.Add("10");
		list3.Add("11");
		list3.Add("12");
		list3.Add("15");
		list4.Add("49");
		list4.Add("9");
		list4.Add("24");
		list4.Add("29");
		list4.Add("38");
		list4.Add("16");
		list4.Add("48");
		list4.Add("10");
		list5.Add("37");
		list5.Add("46");
		list5.Add("47");
		list5.Add("23");
		list5.Add("24");
		list5.Add("38");
		list5.Add("50");
		list5.Add("20");
		list5.Add("51");
		_enemiesInWaves.Add(list);
		_enemiesInWaves.Add(list2);
		_enemiesInWaves.Add(list3);
		_enemiesInWaves.Add(list4);
		_enemiesInWaves.Add(list5);
		List<string> item = new List<string>
		{
			WeaponManager.PistolWN,
			WeaponManager.ShotgunWN,
			WeaponManager.MP5WN
		};
		List<string> item2 = new List<string>
		{
			WeaponManager.AK47WN,
			WeaponManager.RevolverWN
		};
		List<string> item3 = new List<string>
		{
			WeaponManager.M16_2WN,
			WeaponManager.ObrezWN
		};
		List<string> item4 = new List<string> { WeaponManager.MachinegunWN };
		List<string> item5 = new List<string> { WeaponManager.AlienGunWN };
		_WeaponsAddedInWaves.Add(item);
		_WeaponsAddedInWaves.Add(item2);
		_WeaponsAddedInWaves.Add(item3);
		_WeaponsAddedInWaves.Add(item4);
		_WeaponsAddedInWaves.Add(item5);
	}

	private IEnumerator _DrawFirstMessage()
	{
		_drawWaveMsg = true;
		_msg = "Wave 1";
		yield return new WaitForSeconds(2f);
		_drawWaveMsg = false;
	}

	private IEnumerator _DrawWaveMessage()
	{
		_drawWaveMsg = true;
		_msg = "Survived wave " + (currentWave + 1 - 1) + "\nWell done!";
		yield return new WaitForSeconds(2f);
		_msg = "Wave " + (currentWave + 1);
		yield return new WaitForSeconds(2f);
		_drawWaveMsg = false;
	}

	private void OnDestroy()
	{
		if (Defs.IsSurvival)
		{
			PlayerPrefs.SetInt(Defs.KilledZombiesSett, totalNumOfKilledEnemies);
			PlayerPrefs.SetInt(Defs.WavesSurvivedS, currentWave);
		}
	}

	private void _UpdateIntervalStructures()
	{
		_genWithThisTimeInterval = 0;
		_indexInTimesArray = 0;
		curInterval = _intervalArr[_indexInTimesArray];
	}

	private void Awake()
    {
        if (PlayerPrefs.GetInt("MultyPlayer") != 1)
		{
			GlobalGameController.curThr = GlobalGameController.thrStep;
			_enemies.Add(new string[6] { "1", "2", "1", "11", "12", "13" });
			_enemies.Add(new string[5] { "30", "31", "32", "33", "34" });
			_enemies.Add(new string[8] { "1", "2", "3", "9", "10", "12", "14", "15" });
			_enemies.Add(new string[6] { "1", "2", "4", "11", "9", "16" });
			_enemies.Add(new string[7] { "1", "2", "4", "9", "11", "10", "12" });
			_enemies.Add(new string[5] { "43", "44", "45", "46", "47" });
			_enemies.Add(new string[3] { "6", "7", "7" });
			_enemies.Add(new string[6] { "1", "2", "8", "10", "11", "12" });
			_enemies.Add(new string[3] { "18", "19", "20" });
			_enemies.Add(new string[5] { "21", "22", "23", "24", "25" });
			_enemies.Add(new string[2] { "1", "15" });
			_enemies.Add(new string[7] { "1", "3", "9", "10", "14", "15", "16" });
			_enemies.Add(new string[3] { "8", "21", "22" });
			_enemies.Add(new string[4] { "26", "27", "28", "29" });
			_enemies.Add(new string[5] { "35", "36", "37", "38", "48" });
			_enemies.Add(new string[4] { "39", "40", "41", "42" });
			_UpdateZombiePrefabs();
			survivalAvailableWeapons.Clear();
			_UpdateAvailableWeapons();
			_UpdateIntervalStructures();
			StartCoroutine(_DrawFirstMessage());
		}
	}

	private void _UpdateAvailableWeapons()
	{
		if (currentWave >= _WeaponsAddedInWaves.Count)
		{
			return;
		}
		foreach (string item in _WeaponsAddedInWaves[currentWave])
		{
			survivalAvailableWeapons.Add(item);
		}
	}

	private void _UpdateZombiePrefabs()
	{
		zombiePrefabs.Clear();
		string[] array = null;
		if (Defs.IsSurvival)
		{
			int index = ((currentWave < _enemiesInWaves.Count) ? currentWave : (_enemiesInWaves.Count - 1));
			array = _enemiesInWaves[index].ToArray();
		}
		else
		{
			array = null;
			array = ((CurrentCampaignGame.currentLevel != 0) ? _enemies[CurrentCampaignGame.currentLevel - 1] : new string[1] { "1" });
		}
		string[] array2 = array;
		foreach (string text in array2)
		{
			GameObject item = Resources.Load("Enemies/Enemy" + text + "_go") as GameObject;
			zombiePrefabs.Add(item);
		}
	}

	private void Start()
	{
		labelStyle.fontSize = Mathf.RoundToInt(50f * Defs.Coef);
		if (PlayerPrefs.GetInt("MultyPlayer") == 1)
		{
			_isMultiplayer = true;
		}
		else
		{
			_isMultiplayer = false;
		}
		_teleports = GameObject.FindGameObjectsWithTag("Portal");
		GameObject[] teleports = _teleports;
		foreach (GameObject gameObject in teleports)
		{
			gameObject.SetActive(false);
		}
		if (!_isMultiplayer)
		{
			_enemyCreationZones = GameObject.FindGameObjectsWithTag("EnemyCreationZone");
			if (!Defs.IsSurvival)
			{
				_ResetInterval();
			}
		}
	}

	private void OnGUI()
	{
		if (Defs.IsSurvival && _drawWaveMsg)
		{
			GUI.depth = 25;
			float num = (float)Screen.width / 2f;
			float num2 = (float)Screen.height / 3f;
			Rect position = new Rect((float)Screen.width / 2f - num / 2f, (float)Screen.height / 2f - num2, num, num2);
			GUI.Label(position, _msg, labelStyle);
		}
	}

	private void _ResetInterval()
	{
		curInterval = Mathf.Max(1f, curInterval);
	}

	public void BeganCreateEnemies()
	{
		StartCoroutine(AddZombies());
	}

	private IEnumerator AddZombies()
	{
		float halfLLength = 17f;
		float radius = 2.5f;
		do
		{
			int numOfZombsToAdd = GlobalGameController.ZombiesInWave;
			if (!Defs.IsSurvival)
			{
			}
			numOfZombsToAdd = Mathf.Min(numOfZombsToAdd, GlobalGameController.SimultaneousEnemiesOnLevelConstraint - NumOfLiveZombies);
			numOfZombsToAdd = Mathf.Min(numOfZombsToAdd, NumOfEnemisesToKill - (NumOfDeadZombies + NumOfLiveZombies));
			string[] enemies_2 = null;
			if (!Defs.IsSurvival)
			{
				enemies_2 = ((CurrentCampaignGame.currentLevel != 0) ? _enemies[CurrentCampaignGame.currentLevel - 1] : new string[1] { "1" });
			}
			else
			{
				int ind = ((currentWave < _enemiesInWaves.Count) ? currentWave : (_enemiesInWaves.Count - 1));
				enemies_2 = _enemiesInWaves[ind].ToArray();
			}
			for (int i = 0; i < numOfZombsToAdd; i++)
			{
				int typeOfZomb = UnityEngine.Random.Range(0, enemies_2.Length);
				GameObject spawnZone = ((!Defs.IsSurvival) ? _enemyCreationZones[UnityEngine.Random.Range(0, _enemyCreationZones.Length)] : _enemyCreationZones[i % _enemyCreationZones.Length]);
				UnityEngine.Object.Instantiate(position: _createPos(spawnZone), original: zombiePrefabs[typeOfZomb], rotation: Quaternion.identity);
			}
			if (Defs.IsSurvival && NumOfDeadZombies + NumOfLiveZombies >= NumOfEnemisesToKill)
			{
				_generatingZombiesIsStopped = true;
				do
				{
					yield return new WaitForEndOfFrame();
				}
				while (_generatingZombiesIsStopped);
			}
			yield return new WaitForSeconds(curInterval);
			if (Defs.IsSurvival)
			{
				_genWithThisTimeInterval++;
				if (_genWithThisTimeInterval == 3 && _indexInTimesArray < _intervalArr.Length - 1)
				{
					_indexInTimesArray++;
				}
				curInterval = _intervalArr[_indexInTimesArray];
			}
		}
		while (NumOfDeadZombies + NumOfLiveZombies < NumOfEnemisesToKill || Defs.IsSurvival);
	}

	private Vector3 _createPos(GameObject spawnZone)
	{
		BoxCollider component = spawnZone.GetComponent<BoxCollider>();
		Vector2 vector = new Vector2(component.size.x * spawnZone.transform.localScale.x, component.size.z * spawnZone.transform.localScale.z);
		Rect rect = new Rect(spawnZone.transform.position.x - vector.x / 2f, spawnZone.transform.position.z - vector.y / 2f, vector.x, vector.y);
		Vector3 result = new Vector3(rect.x + UnityEngine.Random.Range(0f, rect.width), (!Defs.levelsWithVarY.Contains(CurrentCampaignGame.currentLevel) || Defs.IsSurvival) ? 0f : spawnZone.transform.position.y, rect.y + UnityEngine.Random.Range(0f, rect.height));
		return result;
	}

	private void _createBoss()
	{
		GameObject gameObject = null;
		float num = float.PositiveInfinity;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("Player");
		if (!gameObject2)
		{
			return;
		}
		GameObject[] enemyCreationZones = _enemyCreationZones;
		foreach (GameObject gameObject3 in enemyCreationZones)
		{
			float num2 = Vector3.Distance(gameObject2.transform.position, gameObject3.transform.position);
			float num3 = Mathf.Abs(gameObject2.transform.position.y - gameObject3.transform.position.y);
			if (num2 > 15f && num2 < num && num3 < 2.5f)
			{
				num = num2;
				gameObject = gameObject3;
			}
		}
		if (!gameObject)
		{
			gameObject = _enemyCreationZones[0];
		}
		Vector3 position = _createPos(gameObject);
		string b = "Boss" + CurrentCampaignGame.currentLevel;
		GameObject original = Resources.Load(ResPath.Combine("Bosses", b)) as GameObject;
		UnityEngine.Object.Instantiate(original, position, Quaternion.identity);
		bossShowm = true;
	}
}
