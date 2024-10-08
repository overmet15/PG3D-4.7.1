using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class Initializer : MonoBehaviour
{
	public GameObject _purchaseActivityIndicator;

	public GameObject tc;

	public GameObject tempCam;

	public bool isDisconnect;

	public int countConnectToRoom;

	public float timerShowNotConnectToRoom;

	public Texture reconnectTexture;

	public Texture noConnectRoomTexture;

	public GUIStyle back;

	public bool isCancelReConnect;

	private GameObject _playerPrefab;

	public GameObject networkTablePref;

	private bool _isMultiplayer;

	public GUIStyle messagesStyle;

	public bool isNotConnectRoom;

	private Vector2 scrollPosition = Vector2.zero;

	private List<Vector3> _initPlayerPositions = new List<Vector3>();

	private List<float> _rots = new List<float>();

	private float koofScreen = (float)Screen.height / 768f;

	public WeaponManager _weaponManager;

	public bool showMaxPlayer;

	public bool showDisconnect;

	public float timerShow = -1f;

	public Transform playerPrefab;

	public Texture fonLoadingScene;

	private bool showLoading;

	public static event Action PlayerAddedEvent;

	private void Awake()
	{
		PlayerPrefs.SetInt("ExitGame", 0);
		GameObject gameObject = null;
		if (PlayerPrefs.GetInt("MultyPlayer") == 1)
		{
			if (Defs.levelNumsForMusicInMult.ContainsKey(Application.loadedLevelName))
			{
				GlobalGameController.currentLevel = Defs.levelNumsForMusicInMult[Application.loadedLevelName];
			}
			gameObject = Resources.Load("BackgroundMusic/BackgroundMusic_Level" + GlobalGameController.currentLevel) as GameObject;
		}
		else if (CurrentCampaignGame.currentLevel == 0)
		{
			string path = "BackgroundMusic/" + ((!Defs.IsSurvival) ? "Background_Training" : "BackgroundMusic_Level0");
			gameObject = Resources.Load(path) as GameObject;
		}
		else
		{
			gameObject = Resources.Load("BackgroundMusic/BackgroundMusic_Level" + CurrentCampaignGame.currentLevel) as GameObject;
		}
		UnityEngine.Object.Instantiate(gameObject);
		if (PlayerPrefs.GetInt("MultyPlayer") == 1 || Defs.IsSurvival)
		{
			return;
		}
		string[] array = Storager.getString(Defs.LevelsWhereGetCoinS, false).Split("#"[0]);
		List<string> list = new List<string>();
		string[] array2 = array;
		foreach (string item in array2)
		{
			list.Add(item);
		}
		if (!list.Contains(Application.loadedLevelName) || Defs.IsTraining)
		{
			GameObject gameObject2 = GameObject.FindGameObjectWithTag("Configurator");
			CoinConfigurator component = gameObject2.GetComponent<CoinConfigurator>();
			if (component.CoinIsPresent)
			{
				CreateCoinAtPos(component.pos);
			}
		}
	}

	public static GameObject CreateCoinAtPos(Vector3 pos)
	{
		GameObject original = Resources.Load("coin") as GameObject;
		return UnityEngine.Object.Instantiate(original, pos, Quaternion.Euler(270f, 0f, 0f)) as GameObject;
	}

	private void Start()
	{
		_purchaseActivityIndicator = StoreKitEventListener.purchaseActivityInd;
		PlayerPrefs.SetInt("StartAfterDisconnect", 0);
		PhotonNetwork.isMessageQueueRunning = true;
		if (PlayerPrefs.GetInt("MultyPlayer") == 1)
		{
			_isMultiplayer = true;
		}
		else
		{
			_isMultiplayer = false;
		}
		_weaponManager = GameObject.FindGameObjectWithTag("WeaponManager").GetComponent<WeaponManager>();
		_weaponManager.players.Clear();
		if (!_isMultiplayer)
		{
			_initPlayerPositions.Add(new Vector3(12f, 1f, 9f));
			_initPlayerPositions.Add(new Vector3(17f, 1f, -15f));
			_initPlayerPositions.Add(new Vector3(-30f, 1f, -35f));
			_initPlayerPositions.Add(new Vector3(0f, 1f, 0f));
			_initPlayerPositions.Add(new Vector3(-33f, 1.2f, -13f));
			_initPlayerPositions.Add(new Vector3(-2.67f, 1f, 2.67f));
			_initPlayerPositions.Add(new Vector3(0f, 1f, 0f));
			_initPlayerPositions.Add(new Vector3(19f, 1f, -0.8f));
			_initPlayerPositions.Add(new Vector3(-28.5f, 1.75f, -3.73f));
			_initPlayerPositions.Add(new Vector3(-2.5f, 1.75f, 0f));
			_initPlayerPositions.Add(new Vector3(-1.596549f, 2.5f, 2.684792f));
			_initPlayerPositions.Add(new Vector3(-6.611357f, 1.5f, -105.2573f));
			_initPlayerPositions.Add(new Vector3(-41.42841f, 5.1f, 15.08791f));
			_initPlayerPositions.Add(new Vector3(5f, 2.5f, 0f));
			_initPlayerPositions.Add(new Vector3(0f, 2.5f, 0f));
			_initPlayerPositions.Add(new Vector3(-7.3f, 3.6f, 6.46f));
			_rots.Add(0f);
			_rots.Add(0f);
			_rots.Add(270f);
			_rots.Add(0f);
			_rots.Add(180f);
			_rots.Add(0f);
			_rots.Add(0f);
			_rots.Add(270f);
			_rots.Add(270f);
			_rots.Add(270f);
			_rots.Add(0f);
			_rots.Add(0f);
			_rots.Add(90f);
			_rots.Add(0f);
			_rots.Add(0f);
			_rots.Add(90f);
			int @int = Storager.getInt(Defs.EarnedCoins, false);
			if (@int > 0)
			{
				GameObject original = Resources.Load("MessageCoinsObject") as GameObject;
				GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(original);
			}
			AddPlayer();
			return;
		}
		Vector3 position = new Vector3(17f, 11f, 17f);
		Quaternion rotation = Quaternion.Euler(new Vector3(39f, 226f, 0f));
		if (PlayerPrefs.GetString("MapName").Equals("Maze"))
		{
			position = new Vector3(23f, 5.25f, -20.5f);
			rotation = Quaternion.Euler(new Vector3(33f, -50f, 0f));
		}
		if (PlayerPrefs.GetString("MapName").Equals("Cementery"))
		{
			position = new Vector3(17f, 11f, 17f);
			rotation = Quaternion.Euler(new Vector3(39f, 226f, 0f));
		}
		if (PlayerPrefs.GetString("MapName").Equals("Hospital"))
		{
			position = new Vector3(9.5f, 3.2f, 9.5f);
			rotation = Quaternion.Euler(new Vector3(25f, -140f, 0f));
		}
		if (PlayerPrefs.GetString("MapName").Equals("City"))
		{
			position = new Vector3(17f, 11f, 17f);
			rotation = Quaternion.Euler(new Vector3(39f, 226f, 0f));
		}
		if (PlayerPrefs.GetString("MapName").Equals("Jail"))
		{
			position = new Vector3(13.5f, 2.9f, 3.1f);
			rotation = Quaternion.Euler(new Vector3(11f, -66f, 0f));
		}
		if (PlayerPrefs.GetString("MapName").Equals("Gluk"))
		{
			position = new Vector3(17f, 11f, 17f);
			rotation = Quaternion.Euler(new Vector3(39f, 226f, 0f));
		}
		if (PlayerPrefs.GetString("MapName").Equals("Pool"))
		{
			position = new Vector3(-17.36495f, 5.448204f, -5.605346f);
			rotation = Quaternion.Euler(new Vector3(31.34471f, 31.34471f, 0.2499542f));
		}
		if (PlayerPrefs.GetString("MapName").Equals("Slender"))
		{
			position = new Vector3(31.82355f, 5.959687f, 37.378f);
			rotation = Quaternion.Euler(new Vector3(36.08264f, -110.1159f, 2.307983f));
		}
		if (PlayerPrefs.GetString("MapName").Equals("Castle"))
		{
			position = new Vector3(-12.3107f, 4.9f, 0.2716838f);
			rotation = Quaternion.Euler(new Vector3(26.89935f, 89.99986f, 0f));
		}
		if (PlayerPrefs.GetString("MapName").Equals("Bridge"))
		{
			position = new Vector3(-14.22702f, 14.6011f, -74.93485f);
			rotation = Quaternion.Euler(new Vector3(24.68127f, -151.4293f, 0.2789154f));
		}
		if (PlayerPrefs.GetString("MapName").Equals("Farm"))
		{
			position = new Vector3(22.4933f, 16.03175f, -35.17904f);
			rotation = Quaternion.Euler(new Vector3(29.99995f, -28.62347f, 0f));
		}
		if (PlayerPrefs.GetString("MapName").Equals("School"))
		{
			position = new Vector3(-19.52079f, 2.868755f, -19.50274f);
			rotation = Quaternion.Euler(new Vector3(14.96701f, 40.79106f, 1.266037f));
		}
		if (PlayerPrefs.GetString("MapName").Equals("Sky_islands"))
		{
			position = new Vector3(-3.111776f, 21.94557f, 25.31594f);
			rotation = Quaternion.Euler(new Vector3(41.94537f, -143.1731f, 6.383652f));
		}
		if (PlayerPrefs.GetString("MapName").Equals("Dust"))
		{
			position = new Vector3(-12.67253f, 6.92115f, 28.89415f);
			rotation = Quaternion.Euler(new Vector3(28.46265f, 147.2818f, 0.2389221f));
		}
		if (PlayerPrefs.GetString("MapName").Equals("Utopia"))
		{
			position = new Vector3(-10.62854f, 10.01794f, -51.20456f);
			rotation = Quaternion.Euler(new Vector3(13.26845f, 16.31204f, 1.440735f));
		}
		if (PlayerPrefs.GetString("MapName").Equals("Assault"))
		{
			position = new Vector3(19.36158f, 19.61019f, -24.24763f);
			rotation = Quaternion.Euler(new Vector3(35.9299f, -11.80757f, -1.581451f));
		}
		if (PlayerPrefs.GetString("MapName").Equals("Aztec"))
		{
			position = new Vector3(-6.693532f, 11.69715f, 24.21659f);
			rotation = Quaternion.Euler(new Vector3(41.08192f, 134.5497f, -1.380188f));
		}
		if (PlayerPrefs.GetString("MapName").Equals("Parkour"))
		{
			position = new Vector3(-7.352654f, 113.1507f, -29.85653f);
			rotation = Quaternion.Euler(new Vector3(11.99559f, -16.57709f, 0f));
		}
		if (PlayerPrefs.GetString("MapName").Equals("Coliseum_MP"))
		{
			position = new Vector3(14.32691f, 9.814805f, -20.59482f);
			rotation = Quaternion.Euler(new Vector3(11.60112f, -34.35773f, 0f));
		}
		tc = UnityEngine.Object.Instantiate(tempCam, position, rotation) as GameObject;
		if (PlayerPrefs.GetString("TypeConnect").Equals("local"))
		{
			if (PlayerPrefs.GetString("TypeGame").Equals("client"))
			{
				bool useNat = !Network.HavePublicAddress();
				Network.useNat = useNat;
				Debug.Log(_weaponManager.ServerIp + " " + Network.Connect(_weaponManager.ServerIp, 25002));
			}
			else
			{
				_weaponManager.myTable = (GameObject)Network.Instantiate(networkTablePref, base.transform.position, base.transform.rotation, 0);
			}
		}
		else
		{
			_weaponManager.myTable = PhotonNetwork.Instantiate("NetworkTable", base.transform.position, base.transform.rotation, 0);
		}
	}

[RPC]
private void SpawnOnNetwork(Vector3 pos, Quaternion rot, int id1, PhotonPlayer np)
{
    GameObject instantiatedObject = UnityEngine.Object.Instantiate(networkTablePref, pos, rot) as GameObject;
    Transform transform = instantiatedObject.transform;
    PhotonView component = transform.GetComponent<PhotonView>();
    component.viewID = id1;
}

	private void AddPlayer()
	{
		_playerPrefab = Resources.Load("Player") as GameObject;
		Vector3 position;
		float y;
		if (Defs.IsSurvival)
		{
			position = new Vector3(0f, 2.5f, 0f);
			y = 0f;
		}
		else if (PlayerPrefs.GetInt(Defs.TrainingCompleted_4_4_Sett, 0) == 0)
		{
			position = new Vector3(-0.72f, 1.75f, -13.23f);
			y = 0f;
		}
		else
		{
			int index = Mathf.Max(0, CurrentCampaignGame.currentLevel - 1);
			position = ((CurrentCampaignGame.currentLevel != 0) ? _initPlayerPositions[index] : new Vector3(-0.72f, 1.75f, -13.23f));
			y = ((CurrentCampaignGame.currentLevel != 0) ? _rots[index] : 0f);
		}
		UnityEngine.Object.Instantiate(_playerPrefab, position, Quaternion.Euler(0f, y, 0f));
		Invoke("SetupObjectThatNeedsPlayer", 0.01f);
	}

	public void SetupObjectThatNeedsPlayer()
	{
		if (PlayerPrefs.GetInt("MultyPlayer") == 1)
		{
			Initializer.PlayerAddedEvent();
			return;
		}
		GameObject gameObject = GameObject.FindGameObjectWithTag("CoinBonus");
		if ((bool)gameObject)
		{
			CoinBonus component = gameObject.GetComponent<CoinBonus>();
			if ((bool)component)
			{
				component.SetPlayer();
			}
		}
		if (!Defs.IsTraining)
		{
			GetComponent<ZombieCreator>().BeganCreateEnemies();
		}
		GetComponent<BonusCreator>().BeginCreateBonuses();
		Initializer.PlayerAddedEvent();
	}

	private void OnGUI()
	{
		float num = (float)Screen.height / 768f;
		if (isDisconnect && timerShowNotConnectToRoom < 0f)
		{
			GUI.DrawTexture(new Rect((float)Screen.width * 0.5f - (float)reconnectTexture.width * num * 0.5f, (float)Screen.height * 0.7f - (float)reconnectTexture.height * 0.5f * num, (float)reconnectTexture.width * num, (float)reconnectTexture.height * num), reconnectTexture);
			if (GUI.Button(new Rect((float)Screen.width * 0.5f - (float)back.active.background.width / 2f * koofScreen, (float)Screen.height * 0.9f - (float)back.active.background.height / 2f * num, (float)back.active.background.width * num, (float)back.active.background.height * num), string.Empty, back))
			{
				isCancelReConnect = true;
				if (PhotonNetwork.connected)
				{
					ConnectGUI.Local();
				}
				else
				{
					LoadConnectScene.textureToShow = null;
					LoadConnectScene.sceneToLoad = "ConnectScene";
					Application.LoadLevel(Defs.PromSceneName);
				}
			}
		}
		if (isDisconnect && timerShowNotConnectToRoom > 0f)
		{
			timerShowNotConnectToRoom -= Time.deltaTime;
			if (timerShowNotConnectToRoom > 0f)
			{
				GUI.DrawTexture(new Rect((float)Screen.width * 0.5f - (float)noConnectRoomTexture.width * num * 0.5f, (float)Screen.height * 0.7f - (float)noConnectRoomTexture.height * 0.5f * num, (float)noConnectRoomTexture.width * num, (float)noConnectRoomTexture.height * num), noConnectRoomTexture);
			}
			else if (PhotonNetwork.connected)
			{
				ConnectGUI.Local();
			}
			else
			{
				LoadConnectScene.textureToShow = null;
				LoadConnectScene.sceneToLoad = "ConnectScene";
				Application.LoadLevel(Defs.PromSceneName);
			}
		}
		messagesStyle.alignment = TextAnchor.MiddleCenter;
		messagesStyle.fontSize = Mathf.RoundToInt(60f * (float)Screen.height / 768f);
		messagesStyle.normal.textColor = Color.white;
		Rect position = new Rect(0f, (float)Screen.height * 0.15f, Screen.width, (float)Screen.height * 0.2f);
		if (showLoading && PlayerPrefs.GetInt("MultyPlayer") == 1 && PlayerPrefs.GetString("TypeConnect").Equals("inet"))
		{
			if (_weaponManager.myTable != null)
			{
				_weaponManager.myTable.GetComponent<NetworkStartTable>().isShowNickTable = false;
				_weaponManager.myTable.GetComponent<NetworkStartTable>().showTable = false;
			}
			Rect position2 = new Rect(((float)Screen.width - 2048f * (float)Screen.height / 1154f) / 2f, 0f, 2048f * (float)Screen.height / 1154f, Screen.height);
			if (fonLoadingScene == null)
			{
				fonLoadingScene = Resources.Load("main_loading") as Texture;
			}
			GUI.DrawTexture(position2, fonLoadingScene, ScaleMode.StretchToFill);
		}
		if (showMaxPlayer)
		{
			GUI.Label(position, "Server is full...", messagesStyle);
		}
		position.y = (float)Screen.height / 2f;
		if (showDisconnect)
		{
			GUI.Label(position, "Ops, internet fails...\ncheck the connection.", messagesStyle);
		}
	}

	private void Update()
	{
		if (timerShow > 0f)
		{
			timerShow -= Time.deltaTime;
			Debug.Log("OnLeftRoom (local) init update");
			showLoading = true;
			Invoke("goToConnect", 0.1f);
		}
	}

	private void OnConnectedToServer()
	{
		_weaponManager.myTable = (GameObject)Network.Instantiate(networkTablePref, base.transform.position, base.transform.rotation, 0);
		Debug.Log("OnConnectedToServer");
	}

	private void OnFailedToConnect(NetworkConnectionError error)
	{
		Debug.Log("Could not connect to server: " + error);
		if (error == NetworkConnectionError.TooManyConnectedPlayers)
		{
			showMaxPlayer = true;
		}
		if (error == NetworkConnectionError.ConnectionFailed)
		{
			showDisconnect = true;
		}
		timerShow = 5f;
		Debug.Log("timerShow=5f;");
		if (!(_weaponManager == null) && !(_weaponManager.myTable == null))
		{
			_weaponManager.myTable.GetComponent<NetworkStartTable>().isShowNickTable = false;
			_weaponManager.myTable.GetComponent<NetworkStartTable>().showTable = false;
		}
	}

	private void goToConnect()
	{
		ConnectGUI.Local();
	}

	public void OnLeftRoom()
	{
		Debug.Log("OnLeftRoom (local) init");
		if (PlayerPrefs.GetInt("ExitGame") == 1)
		{
			showLoading = true;
			Invoke("goToConnect", 0.1f);
			if (!(_weaponManager == null) && !(_weaponManager.myTable == null))
			{
				_weaponManager.myTable.GetComponent<NetworkStartTable>().isShowNickTable = false;
				_weaponManager.myTable.GetComponent<NetworkStartTable>().showTable = false;
			}
		}
	}

	public void OnDisconnectedFromPhoton()
	{
		Debug.Log("OnDisconnectedFromPhotoninit");
	}

	private void OnConnectionFail(DisconnectCause cause)
	{
		timerShowNotConnectToRoom = -1f;
		isCancelReConnect = false;
		isNotConnectRoom = false;
		countConnectToRoom = 0;
		PlayerPrefs.SetString("TypeGame", "client");
		Debug.Log("OnConnectionFail " + GlobalGameController.Score);
		Debug.Log("OnConnectionFail " + cause);
		tc.SetActive(true);
		GameObject[] array = GameObject.FindGameObjectsWithTag("Bonus");
		for (int i = 0; i < array.Length; i++)
		{
			UnityEngine.Object.Destroy(array[i]);
		}
		GameObject[] array2 = GameObject.FindGameObjectsWithTag("Enemy");
		for (int j = 0; j < array2.Length; j++)
		{
			UnityEngine.Object.Destroy(array2[j]);
		}
		GameObject gameObject = GameObject.FindGameObjectWithTag("InGameGUI");
		if (gameObject != null)
		{
			UnityEngine.Object.Destroy(gameObject);
		}
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("ChatViewer");
		if (gameObject2 != null)
		{
			UnityEngine.Object.Destroy(gameObject2);
		}
		isDisconnect = true;
		Invoke("ConnectToPhoton", 3f);
		_purchaseActivityIndicator.SetActive(true);
	}

	private void ConnectToPhoton()
	{
		if (!isCancelReConnect)
		{
			Debug.Log("ConnectToPhoton ");
			if (PlayerPrefs.GetInt("COOP", 0) == 1)
			{
				PhotonNetwork.ConnectUsingSettings("v" + GlobalGameController.AppVersion + "COOP");
			}
			else if (PlayerPrefs.GetInt("company") == 1)
			{
				PhotonNetwork.ConnectUsingSettings("v" + GlobalGameController.AppVersion + "company");
			}
			else
			{
				PhotonNetwork.ConnectUsingSettings("v" + GlobalGameController.AppVersion);
			}
		}
	}

	private void OnFailedToConnectToPhoton(object parameters)
	{
		Debug.Log("OnFailedToConnectToPhoton. StatusCode: " + parameters);
		if (!isCancelReConnect)
		{
			Invoke("ConnectToPhoton", 3f);
		}
	}

	public void OnJoinedLobby()
	{
		ConnectToRoom();
	}

	private void ConnectToRoom()
	{
		Debug.Log("OnJoinedLobby " + PlayerPrefs.GetString("RoomName"));
		if (!isCancelReConnect)
		{
			PhotonNetwork.JoinRoom(PlayerPrefs.GetString("RoomName"));
		}
	}

	private void OnPhotonJoinRoomFailed()
	{
		countConnectToRoom++;
		Debug.Log("OnPhotonJoinRoomFailed - init");
		isNotConnectRoom = true;
		if (countConnectToRoom < 6)
		{
			Invoke("ConnectToRoom", 3f);
		}
		else
		{
			timerShowNotConnectToRoom = 3f;
		}
	}

	private void OnJoinedRoom()
	{
		_purchaseActivityIndicator.SetActive(false);
		Debug.Log("OnJoinedRoom - init");
		if (isDisconnect)
		{
			if (PlayerPrefs.GetInt("company", 0) == 0)
			{
				PlayerPrefs.SetInt("StartAfterDisconnect", 1);
			}
			_weaponManager.myTable = PhotonNetwork.Instantiate("NetworkTable", base.transform.position, base.transform.rotation, 0);
		}
		isDisconnect = false;
	}

	public void OnPhotonPlayerConnected(PhotonPlayer player)
	{
	}

	public void OnPhotonPlayerDisconnected(PhotonPlayer player)
	{
		Debug.Log("OnPlayerDisconnecedinit: " + player);
	}

	public void OnReceivedRoomList()
	{
		Debug.Log("OnReceivedRoomListinit");
	}

	public void OnReceivedRoomListUpdate()
	{
		Debug.Log("OnReceivedRoomListUpdateinit");
	}

	public void OnConnectedToPhoton()
	{
		Debug.Log("OnConnectedToPhotoninit");
	}

	public void OnFailedToConnectToPhoton()
	{
		Debug.Log("OnFailedToConnectToPhotoninit");
	}

	public void OnPhotonInstantiate(PhotonMessageInfo info)
	{
		Debug.Log("OnPhotonInstantiate init" + info.sender);
	}
}
