using System.Collections;
using UnityEngine;

public class BonusCreator : MonoBehaviour
{
	public GameObject[] bonusPrefabs;

	public float creationInterval = 15f;

	public float weaponCreationInterval = 30f;

	private Object[] weaponPrefabs;

	private int _lastWeapon = -1;

	private bool _isMultiplayer;

	private ArrayList bonuses = new ArrayList();

	private ArrayList _weapons = new ArrayList();

	public WeaponManager _weaponManager;

	private GameObject[] _bonusCreationZones;

	private ZombieCreator _zombieCreator;

	private ArrayList _weaponsProbDistr = new ArrayList();

	private float _DistrSum()
	{
		float num = 0f;
		foreach (int item in _weaponsProbDistr)
		{
			num += (float)item;
		}
		return num;
	}

	private void Awake()
	{
		if (Defs.IsSurvival)
		{
			creationInterval = 9f;
			weaponCreationInterval = 15f;
		}
		if (PlayerPrefs.GetInt("MultyPlayer") == 1)
		{
			_isMultiplayer = true;
		}
		else
		{
			_isMultiplayer = false;
		}
		if (!_isMultiplayer)
		{
			weaponPrefabs = GameObject.FindGameObjectWithTag("WeaponManager").GetComponent<WeaponManager>().weaponsInGame;
			Object[] array = weaponPrefabs;
			for (int i = 0; i < array.Length; i++)
			{
				GameObject gameObject = (GameObject)array[i];
				WeaponSounds component = gameObject.GetComponent<WeaponSounds>();
				_weaponsProbDistr.Add(component.Probability);
			}
		}
	}

	private void Start()
	{
		_bonusCreationZones = GameObject.FindGameObjectsWithTag("BonusCreationZone");
		_zombieCreator = base.gameObject.GetComponent<ZombieCreator>();
		_weaponManager = GameObject.FindGameObjectWithTag("WeaponManager").GetComponent<WeaponManager>();
	}

	private void Update()
	{
	}

	public void BeginCreateBonuses()
	{
		StartCoroutine(AddBonus());
		if (Defs.IsSurvival)
		{
			StartCoroutine(AddWeapon());
		}
	}

	public GameObject GetPrefabWithTag(string tagName)
	{
		Object[] array = weaponPrefabs;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.CompareTag(tagName))
			{
				return gameObject;
			}
		}
		return null;
	}

	private IEnumerator AddBonus()
	{
		while (true)
		{
			yield return new WaitForSeconds(creationInterval);
			int enemiesLeft = GlobalGameController.EnemiesToKill - _zombieCreator.NumOfDeadZombies;
			if (!Defs.IsSurvival && enemiesLeft <= 0 && !_zombieCreator.bossShowm)
			{
				break;
			}
			GameObject[] _bonuses = GameObject.FindGameObjectsWithTag("Bonus");
			if (_bonuses.Length > ((!Defs.IsSurvival) ? 5 : 3))
			{
				continue;
			}
			if (PlayerPrefs.GetInt("MultyPlayer") == 1 && PlayerPrefs.GetString("TypeConnect").Equals("inet"))
			{
				GameObject[] _players = GameObject.FindGameObjectsWithTag("Player");
				int minID = 0;
				GameObject[] array = _players;
				foreach (GameObject _playerTemp in array)
				{
					if ((bool)_playerTemp.transform.GetComponent<PhotonView>() && _playerTemp.transform.GetComponent<PhotonView>().viewID > minID)
					{
						minID = _playerTemp.transform.GetComponent<PhotonView>().viewID;
					}
				}
				WeaponManager _weapon = GameObject.FindGameObjectWithTag("WeaponManager").GetComponent<WeaponManager>();
				if (!(_weapon.myPlayer != null) || minID != _weapon.myPlayer.GetComponent<PhotonView>().viewID)
				{
					continue;
				}
			}
			GameObject spawnZone = _bonusCreationZones[Random.Range(0, _bonusCreationZones.Length)];
			BoxCollider spawnZoneCollider = spawnZone.GetComponent<BoxCollider>();
			Vector2 sz = new Vector2(spawnZoneCollider.size.x * spawnZone.transform.localScale.x, spawnZoneCollider.size.z * spawnZone.transform.localScale.z);
			Rect zoneRect = new Rect(spawnZone.transform.position.x - sz.x / 2f, spawnZone.transform.position.z - sz.y / 2f, sz.x, sz.y);
			Vector3 pos = new Vector3(zoneRect.x + Random.Range(0f, zoneRect.width), (!Defs.levelsWithVarY.Contains(_curLevel())) ? 0.24f : spawnZone.transform.position.y, zoneRect.y + Random.Range(0f, zoneRect.height));
			int type = Random.Range(0, 11);
			GameObject newBonus;
			if (!_isMultiplayer)
			{
				newBonus = (GameObject)Object.Instantiate(bonusPrefabs[_indexForType(type)], pos, Quaternion.identity);
				continue;
			}
			if (PlayerPrefs.GetString("TypeConnect").Equals("local"))
			{
				newBonus = (GameObject)Network.Instantiate(bonusPrefabs[_indexForType(type)], pos, Quaternion.identity, 0);
				continue;
			}
			newBonus = (GameObject)Object.Instantiate(bonusPrefabs[_indexForType(type)], pos, Quaternion.identity);
			int _id = PhotonNetwork.AllocateViewID();
			newBonus.GetComponent<PhotonView>().viewID = _id;
			_weaponManager.myTable.GetComponent<NetworkStartTable>().addBonus(_id, type, pos, Quaternion.identity);
		}
	}

	public void addBonusFromPhotonRPC(int _id, int _type, Vector3 _pos, Quaternion rot)
	{
		GameObject gameObject = (GameObject)Object.Instantiate(bonusPrefabs[_indexForType(_type)], _pos, rot);
		gameObject.GetComponent<PhotonView>().viewID = _id;
		gameObject.GetComponent<SettingBonus>().typeOfMass = _type;
	}

	private int _indexForType(int type)
	{
		int result = 0;
		switch (type)
		{
		case 9:
		case 10:
			result = 1;
			break;
		case 8:
			result = 2;
			break;
		}
		return result;
	}

	[RPC]
	private void delBonus(NetworkViewID id)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Bonus");
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			if (id.Equals(gameObject.GetComponent<NetworkView>().viewID))
			{
				Object.Destroy(gameObject);
				break;
			}
		}
	}

	private IEnumerator AddWeapon()
	{
		while (true)
		{
			yield return new WaitForSeconds(weaponCreationInterval);
			int enemiesLeft = GlobalGameController.EnemiesToKill - _zombieCreator.NumOfDeadZombies;
			if (!Defs.IsSurvival && enemiesLeft <= 0 && !_zombieCreator.bossShowm)
			{
				break;
			}
			GameObject spawnZone = _bonusCreationZones[Random.Range(0, _bonusCreationZones.Length)];
			BoxCollider spawnZoneCollider = spawnZone.GetComponent<BoxCollider>();
			Vector2 sz = new Vector2(spawnZoneCollider.size.x * spawnZone.transform.localScale.x, spawnZoneCollider.size.z * spawnZone.transform.localScale.z);
			Rect zoneRect = new Rect(spawnZone.transform.position.x - sz.x / 2f, spawnZone.transform.position.z - sz.y / 2f, sz.x, sz.y);
			Vector3 pos = new Vector3(zoneRect.x + Random.Range(0f, zoneRect.width), (!Defs.levelsWithVarY.Contains(_curLevel())) ? 0.24f : spawnZone.transform.position.y, zoneRect.y + Random.Range(0f, zoneRect.height));
			float sum = _DistrSum();
			int weaponNumber;
			do
			{
				weaponNumber = 0;
				float val = Random.Range(0f, sum);
				float curSum = 0f;
				for (int i = 0; i < _weaponsProbDistr.Count; i++)
				{
					if (val < curSum + (float)(int)_weaponsProbDistr[i])
					{
						weaponNumber = i;
						break;
					}
					curSum += (float)(int)_weaponsProbDistr[i];
				}
			}
			while (weaponNumber == _lastWeapon || (Defs.IsSurvival && !ZombieCreator.survivalAvailableWeapons.Contains(weaponPrefabs[weaponNumber].name)) || weaponPrefabs[weaponNumber].name.Equals(WeaponManager.PickWeaponName) || weaponPrefabs[weaponNumber].name.Equals(WeaponManager.SwordWeaponName) || weaponPrefabs[weaponNumber].name.Equals("Weapon9") || weaponPrefabs[weaponNumber].name.Equals(WeaponManager.CombatRifleWeaponName) || weaponPrefabs[weaponNumber].name.Equals(WeaponManager.GoldenEagleWeaponName) || weaponPrefabs[weaponNumber].name.Equals(WeaponManager.MagicBowWeaponName) || weaponPrefabs[weaponNumber].name.Equals(WeaponManager.SpasWeaponName) || weaponPrefabs[weaponNumber].name.Equals(WeaponManager.GoldenAxeWeaponnName) || weaponPrefabs[weaponNumber].name.Equals(WeaponManager.ChainsawWN) || weaponPrefabs[weaponNumber].name.Equals(WeaponManager.FAMASWN) || weaponPrefabs[weaponNumber].name.Equals(WeaponManager.GlockWN) || weaponPrefabs[weaponNumber].name.Equals(WeaponManager.ScytheWN) || weaponPrefabs[weaponNumber].name.Equals(WeaponManager.ShovelWN) || weaponPrefabs[weaponNumber].name.Equals(WeaponManager.Sword_2_WN) || weaponPrefabs[weaponNumber].name.Equals(WeaponManager.HammerWN) || weaponPrefabs[weaponNumber].name.Equals(WeaponManager.StaffWN) || weaponPrefabs[weaponNumber].name.Equals(WeaponManager.LaserRifleWN) || weaponPrefabs[weaponNumber].name.Equals(WeaponManager.LightSwordWN) || weaponPrefabs[weaponNumber].name.Equals(WeaponManager.BerettaWN) || weaponPrefabs[weaponNumber].name.Equals(WeaponManager.MaceWN) || weaponPrefabs[weaponNumber].name.Equals(WeaponManager.CrossbowWN) || weaponPrefabs[weaponNumber].name.Equals(WeaponManager.MinigunWN) || weaponPrefabs[weaponNumber].name.Equals(WeaponManager.GoldenPickWN) || weaponPrefabs[weaponNumber].name.Equals(WeaponManager.CrystalPickWN) || weaponPrefabs[weaponNumber].name.Equals(WeaponManager.IronSwordWN) || weaponPrefabs[weaponNumber].name.Equals(WeaponManager.GoldenSwordWN) || weaponPrefabs[weaponNumber].name.Equals(WeaponManager.GoldenRed_StoneWN) || weaponPrefabs[weaponNumber].name.Equals(WeaponManager.GoldenSPASWN) || weaponPrefabs[weaponNumber].name.Equals(WeaponManager.GoldenGlockWN) || weaponPrefabs[weaponNumber].name.Equals(WeaponManager.RedMinigunWN) || weaponPrefabs[weaponNumber].name.Equals(WeaponManager.CrystalCrossbowWN) || weaponPrefabs[weaponNumber].name.Equals(WeaponManager.RedLightSaberWN) || weaponPrefabs[weaponNumber].name.Equals(WeaponManager.SandFamasWN) || weaponPrefabs[weaponNumber].name.Equals(WeaponManager.WhiteBerettaWN) || weaponPrefabs[weaponNumber].name.Equals(WeaponManager.BlackEagleWN) || weaponPrefabs[weaponNumber].name.Equals(WeaponManager.CrystalAxeWN) || weaponPrefabs[weaponNumber].name.Equals(WeaponManager.SteelAxeWN) || weaponPrefabs[weaponNumber].name.Equals(WeaponManager.WoodenBowTag) || weaponPrefabs[weaponNumber].name.Equals(WeaponManager.Chainsaw2WN) || weaponPrefabs[weaponNumber].name.Equals(WeaponManager.SteelCrossbowWN) || weaponPrefabs[weaponNumber].name.Equals(WeaponManager.Hammer2WN) || weaponPrefabs[weaponNumber].name.Equals(WeaponManager.Mace2WN) || weaponPrefabs[weaponNumber].name.Equals(WeaponManager.Sword_22WN) || weaponPrefabs[weaponNumber].name.Equals(WeaponManager.Staff2WN) || weaponPrefabs[weaponNumber].name.Equals(WeaponManager.CrystalGlockWN) || weaponPrefabs[weaponNumber].name.Equals(WeaponManager.CrystalSPASWN) || weaponPrefabs[weaponNumber].name.Equals(WeaponManager.TreeWN) || weaponPrefabs[weaponNumber].name.Equals(WeaponManager.FireAxeWN) || weaponPrefabs[weaponNumber].name.Equals(WeaponManager._3pl_shotgunWN) || weaponPrefabs[weaponNumber].name.Equals(WeaponManager.Revolver2WN) || weaponPrefabs[weaponNumber].name.Equals(WeaponManager.BarrettWN) || weaponPrefabs[weaponNumber].name.Equals(WeaponManager.svdWN));
			GameObject wp = (GameObject)weaponPrefabs[weaponNumber];
			GameObject newBonus = _CreateBonus(wp, pos);
			_weapons.Add(newBonus);
			if (_weapons.Count > ((!Defs.IsSurvival) ? 5 : 3))
			{
				Object.Destroy((GameObject)_weapons[0]);
				_weapons.RemoveAt(0);
			}
		}
	}

	public static GameObject _CreateBonus(GameObject wp, Vector3 pos)
	{
		wp.transform.rotation = Quaternion.identity;
		WeaponSounds component = wp.GetComponent<WeaponSounds>();
		GameObject bonusBonus = component.bonusBonus;
		GameObject gameObject = (GameObject)Object.Instantiate(bonusBonus, pos, Quaternion.identity);
		gameObject.AddComponent<WeaponBonus>();
		WeaponBonus component2 = gameObject.GetComponent<WeaponBonus>();
		component2.weaponPrefab = wp;
		float num = 1f;
		gameObject.transform.localScale = ((wp.CompareTag("M249MachinegunWeapon") || wp.CompareTag("AK47")) ? new Vector3(1f, 1f, 1f) : ((!wp.CompareTag("Colt45Weapon")) ? new Vector3(num, num, num) : new Vector3(1f, 1f, 1f)));
		return gameObject;
	}

	private int _curLevel()
	{
		return (PlayerPrefs.GetInt("MultyPlayer") == 1) ? GlobalGameController.currentLevel : CurrentCampaignGame.currentLevel;
	}
}
