using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class ZombiManager : MonoBehaviour
{
	public float timeGame;

	public float nextTimeSynch;

	public float nextAddZombi;

	public List<GameObject> zombiePrefabs = new List<GameObject>();

	private List<string[]> _enemies = new List<string[]>();

	private GameObject[] _enemyCreationZones;

	public bool startGame;

	public float maxTimeGame = 240f;

	public PhotonView photonView;

	private void Awake()
	{
		try
		{
			string[] array = null;
			array = new string[10] { "1", "15", "14", "2", "3", "9", "11", "12", "10", "16" };
			string[] array2 = array;
			foreach (string text in array2)
			{
				GameObject item = Resources.Load("Enemies/Enemy" + text + "_go") as GameObject;
				zombiePrefabs.Add(item);
			}
		}
		catch (Exception exception)
		{
			Debug.LogError("Cooperative mode failure.");
			Debug.LogException(exception);
			throw;
		}
	}

	private void Start()
	{
		try
		{
			nextAddZombi = 5f;
			_enemyCreationZones = GameObject.FindGameObjectsWithTag("EnemyCreationZone");
			photonView = PhotonView.Get(this);
		}
		catch (Exception exception)
		{
			Debug.LogError("Cooperative mode failure.");
			Debug.LogException(exception);
			throw;
		}
	}

	[RPC]
	private void synchTime(float _time)
	{
		timeGame = _time;
	}

	private void Update()
	{
		try
		{
			if (!startGame && GameObject.FindGameObjectsWithTag("Player").Length > 0)
			{
				startGame = true;
				timeGame = 0f;
				nextTimeSynch = 0f;
				nextAddZombi = 0f;
			}
			if (startGame && GameObject.FindGameObjectsWithTag("Player").Length == 0)
			{
				startGame = false;
				timeGame = 0f;
				nextTimeSynch = 0f;
				nextAddZombi = 0f;
			}
			if (!startGame)
			{
				return;
			}
			if (startGame)
			{
				timeGame += Time.deltaTime;
			}
			PhotonView photonView = PhotonView.Get(this);
			if (photonView.isMine && timeGame > nextTimeSynch)
			{
				photonView.RPC("synchTime", PhotonTargets.Others, timeGame);
				nextTimeSynch = timeGame + 3f;
			}
			if (photonView.isMine && timeGame > maxTimeGame)
			{
				startGame = false;
				timeGame = 0f;
				GameObject[] array = GameObject.FindGameObjectsWithTag("NetworkTable");
				float num = -100f;
				string text = string.Empty;
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].GetComponent<NetworkStartTable>().score > num)
					{
						num = array[i].GetComponent<NetworkStartTable>().score;
						text = array[i].GetComponent<NetworkStartTable>().NamePlayer;
					}
				}
				photonView.RPC("win", PhotonTargets.All, text);
			}
			if (timeGame > nextAddZombi && photonView.isMine && GameObject.FindGameObjectsWithTag("Enemy").Length < 15)
			{
				float num2 = 4f;
				if (timeGame > maxTimeGame * 0.4f)
				{
					num2 = 3f;
				}
				if (timeGame > maxTimeGame * 0.8f)
				{
					num2 = 2f;
				}
				nextAddZombi += num2;
				addZombi();
			}
		}
		catch (Exception exception)
		{
			Debug.LogError("Cooperative mode failure.");
			Debug.LogException(exception);
			throw;
		}
	}

	[RPC]
	private void win(string _winer)
	{
		GameObject.FindGameObjectWithTag("WeaponManager").GetComponent<WeaponManager>().myTable.GetComponent<NetworkStartTable>().win(_winer);
	}

	private void addZombi()
	{
		GameObject gameObject = _enemyCreationZones[UnityEngine.Random.Range(0, _enemyCreationZones.Length)];
		BoxCollider component = gameObject.GetComponent<BoxCollider>();
		Vector2 vector = new Vector2(component.size.x * gameObject.transform.localScale.x, component.size.z * gameObject.transform.localScale.z);
		Rect rect = new Rect(gameObject.transform.position.x - vector.x / 2f, gameObject.transform.position.z - vector.y / 2f, vector.x, vector.y);
		Vector3 vector2 = new Vector3(rect.x + UnityEngine.Random.Range(0f, rect.width), (!Defs.levelsWithVarY.Contains(GlobalGameController.currentLevel)) ? 0f : gameObject.transform.position.y, rect.y + UnityEngine.Random.Range(0f, rect.height));
		int num = 0;
		float num2 = timeGame / maxTimeGame * 100f;
		if (num2 < 15f)
		{
			num = UnityEngine.Random.Range(0, 3);
		}
		if (num2 >= 15f && num2 < 30f)
		{
			num = UnityEngine.Random.Range(0, 5);
		}
		if (num2 >= 30f && num2 < 45f)
		{
			num = UnityEngine.Random.Range(0, 6);
		}
		if (num2 >= 45f && num2 < 60f)
		{
			num = UnityEngine.Random.Range(3, 8);
		}
		if (num2 >= 60f && num2 < 75f)
		{
			num = UnityEngine.Random.Range(5, 9);
		}
		if (num2 >= 75f)
		{
			num = UnityEngine.Random.Range(5, 10);
		}
		photonView.RPC("addZombiRPC", PhotonTargets.All, num, vector2, PhotonNetwork.AllocateViewID());
	}

	[RPC]
	private void addZombiRPC(int typeOfZomb, Vector3 pos, int _id)
	{
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(zombiePrefabs[typeOfZomb], pos, Quaternion.identity);
		gameObject.GetComponent<ZombiUpravlenie>().typeZombInMas = typeOfZomb;
		PhotonView component = gameObject.GetComponent<PhotonView>();
		component.viewID = _id;
	}
}
