using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;

public sealed class ConnectGUI : MonoBehaviour
{
	public struct infoServer
	{
		public string ipAddress;

		public int port;

		public string name;

		public string map;

		public int playerLimit;

		public int connectedPlayers;

		public string coments;
	}

	public struct infoClient
	{
		public string ipAddress;

		public string name;

		public string coments;
	}

	public static Rect LeftButtonRect = new Rect((float)Screen.width * 0.12f - 99.5f * (float)Screen.height / 768f, (float)Screen.height * 0.9f - 43.5f * (float)Screen.height / 768f, (float)(199 * Screen.height) / 768f, (float)(87 * Screen.height) / 768f);

	public static Rect RightButtonRect = new Rect((float)Screen.width * 0.88f - 99.5f * (float)Screen.height / 768f, LeftButtonRect.y + LeftButtonRect.height - (float)(193 * Screen.height) / 768f, (float)(199 * Screen.height) / 768f, (float)(193 * Screen.height) / 768f);

	private bool isVozvratMap;

	private bool scrollEnabled;

	private Vector2 scrollStartTouch;

	private Texture loadingToDraw;

	private bool showStatistik;

	public GameObject _purchaseActivityIndicator;

	public Texture[] popularTextures;

	public new string name = "NameName";

	public string myName = "MyName";

	public string goMapName = string.Empty;

	public Texture tLocalIsNotAvailable;

	public Texture swipeTexture;

	public string limitsPlayer = "4";

	public string killToWin = "15";

	private string password = string.Empty;

	public string commentsServer = "Comments";

	public string mapServer = "map";

	public Texture fonTexture;

	public Texture fon2Texture;

	public GUISkin tableSkin;

	public Texture tLocked;

	public Texture head_pass;

	public Texture head_enter_password;

	public Texture head_password;

	public Texture head_connection;

	public Texture head_local;

	public Texture head_maps;

	public Texture head_players;

	public Texture head_profile;

	public Texture head_serv_name;

	public Texture head_worldwide;

	public Texture head_listOfGames;

	public Texture head_search;

	public GUIStyle randomStyle;

	public GUIStyle sPasswordJoin;

	public GUIStyle clearStyle;

	public GUIStyle sSearch;

	public GUIStyle sPasswordOk;

	public GUIStyle sPasswordCancel;

	public GUIStyle back;

	public GUIStyle sSetPassword;

	public GUIStyle connect;

	public GUIStyle connect_small;

	public GUIStyle create;

	public GUIStyle local;

	public GUIStyle minus;

	public GUIStyle next_conn;

	public GUIStyle plus;

	public GUIStyle profile;

	public GUIStyle seach;

	public GUIStyle setStyle;

	public GUIStyle start;

	public GUIStyle worldwide;

	public GUIStyle nameStyle;

	public GUIStyle countStyle;

	public GUIStyle commentStyle;

	public GUIStyle mapNameStyle;

	public GUIStyle numberofplayersStyle;

	public GUIStyle playersWindow;

	public GUIStyle closeServer;

	public GUIStyle openServer;

	public GUIStyle openServerText;

	public GUIStyle closeServerText;

	public GUIStyle openServerFont;

	public GUIStyle closeServerFont;

	public GUIStyle bigStart;

	public GUIStyle customGame;

	public GUIStyle company2x2;

	public GUIStyle company3x3;

	public GUIStyle company4x4;

	public Texture[] masMap;

	public Texture[] masLoading;

	public string[] masMapName;

	public Texture[] masMapCOOP;

	public Texture[] masLoadingCOOP;

	public string[] masMapNameCOOP;

	public Texture[] masMapCompany;

	public Texture[] masLoadingCompany;

	public string[] masMapNameCompany;

	private Vector2 rowSize;

	public bool isFirstFrame = true;

	private bool showServerFull;

	private float timerShowServerFull;

	private float timerShowFaledJoin;

	private bool isRandomSelectMap;

	private Rect rScrollFrame;

	private int ratingSelectMap;

	private int ratingSelectMapDO;

	private int ratingSelectMapPOSLE;

	public List<infoClient> players = new List<infoClient>();

	public List<infoServer> servers = new List<infoServer>();

	private Vector2 scrollPosition = Vector2.zero;

	private Vector2 pressPoint;

	private Vector2 startPoint;

	private Vector2 pointMap;

	private Vector2 sizeMap = new Vector2(909f, 600f);

	private int selectMapIndex;

	private int typeGame;

	private int typeConnect;

	private bool pressBack;

	private bool isMoveMap;

	private bool isSetMap;

	private int regimGUIServer;

	private int regimGUIClient;

	private bool isScanServers;

	private bool showPasswordForm;

	private bool isPasswordIncorrect;

	private bool showPasswordEnterForm;

	private bool connectingFoton;

	private bool showNotConnectingFoton;

	private float timerShowNotConnecting;

	private float timerShowErrorName;

	private bool showFilterForm;

	private bool isPressed2x2;

	private bool isPressed3x3;

	private bool isPressed4x4 = true;

	private RoomInfo connectGame;

	private float koofScreen = (float)Screen.height / 768f;

	private float windowWidth;

	public Texture worldwideTx;

	public Texture head_first;

	private HostData[] hostData;

	public WeaponManager _weaponManager;

	private bool showLoading;

	private bool isLocalAvailable;

	private bool isConSuccess;

	private bool startingGame;

	public GUIStyle timeSurvivalCoop;

	public GUIStyle teamFight;

	public Texture gameModeHeader;

	public Texture bossBattle;

	private string gameFilter = string.Empty;

	private string gameFilterTemp = string.Empty;

	private List<RoomInfo> filteredRoomList = new List<RoomInfo>();

	private bool firstUpdate;

	public static void Local()
	{
		if (PlayerPrefs.GetString("TypeConnect").Equals("local"))
		{
			PlayerPrefs.SetInt("typeConnect__", 2);
		}
		else
		{
			PlayerPrefs.SetInt("typeConnect__", 1);
		}
		LoadConnectScene.textureToShow = null;
		LoadConnectScene.sceneToLoad = "ConnectScene";
		Application.LoadLevel(Defs.PromSceneName);
	}

	private void guiIncorrectPassword()
	{
		if (isPasswordIncorrect)
		{
			GUI.Label(new Rect(0f, (float)Screen.height * 0.15f, Screen.width, (float)Screen.height * 0.2f), "Incorrect Password", openServerText);
		}
	}

	private void disableGuiIncorrectPassword()
	{
		isPasswordIncorrect = false;
	}

	public static void GoToProfile()
	{
		PlayerPrefs.SetInt(Defs.SkinEditorMode, 1);
		Application.LoadLevel("SkinEditor");
	}

	private void Awake()
	{
		PlayerPrefs.SetInt("CustomGame", 1);
		typeConnect = PlayerPrefs.GetInt("typeConnect__", 0);
		PlayerPrefs.SetString("TypeGame", "client");
		if (PlayerPrefs.GetString("TypeConnect").Equals("local"))
		{
			typeGame = 2;
		}
		else
		{
			typeGame = 3;
		}
		if (typeConnect == 1 && PhotonNetwork.connected)
		{
			updateFilteredRoomList(string.Empty);
		}
		windowWidth = (float)playersWindow.normal.background.width * koofScreen;
		checkLocalAvailability();
		openServerText.fontSize = Mathf.RoundToInt(23f * Defs.Coef);
	}

	private void checkLocalAvailability()
	{
		if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
		{
			isLocalAvailable = true;
		}
		else
		{
			isLocalAvailable = false;
		}
	}

	private void OnApplicationPause(bool pause)
	{
		if (!pause)
		{
			checkLocalAvailability();
		}
	}

	private void checkConSuccess()
	{
		if (!isConSuccess)
		{
			MonoBehaviour.print("BEDA!");
		}
	}

	private void guiSetPassword()
	{
		if (!password.Equals(string.Empty))
		{
			GUI.DrawTexture(new Rect((float)Screen.width * 0.5f - (float)sSetPassword.normal.background.width * koofScreen * 0.5f - (float)tLocked.width * koofScreen * 1.1f, (float)Screen.height - (21f + (float)tLocked.height) * koofScreen, (float)tLocked.width * koofScreen, (float)tLocked.height * koofScreen), tLocked);
		}
		if (GUI.Button(new Rect((float)Screen.width * 0.5f - (float)sSetPassword.normal.background.width * koofScreen * 0.5f, (float)Screen.height - (21f + (float)sSetPassword.normal.background.height) * koofScreen, (float)sSetPassword.normal.background.width * koofScreen, (float)sSetPassword.normal.background.height * koofScreen), string.Empty, sSetPassword))
		{
			password = string.Empty;
			showPasswordForm = true;
		}
	}

	private void guiPasswordForm()
	{
		GUI.DrawTexture(new Rect((float)Screen.width / 2f - (float)head_password.width / 2f * koofScreen, (float)Screen.height * 0.1f - (float)head_password.height / 2f * (float)Screen.height / 768f, (float)head_password.width * koofScreen, (float)head_password.height * koofScreen), head_password);
		Rect position = new Rect((float)Screen.width * 0.5f - (float)nameStyle.normal.background.width * 0.5f * koofScreen, (float)Screen.height * 0.23f - (float)nameStyle.normal.background.height * 0.5f * koofScreen, (float)nameStyle.normal.background.width * koofScreen, (float)nameStyle.normal.background.height * koofScreen);
		password = GUI.TextField(position, password, nameStyle);
		if (GUI.Button(new Rect(21f * koofScreen, (float)Screen.height - (21f + (float)sPasswordCancel.active.background.height) * (float)Screen.height / 768f, (float)sPasswordCancel.normal.background.width * koofScreen, (float)sPasswordCancel.normal.background.height * koofScreen), string.Empty, sPasswordCancel))
		{
			showPasswordForm = false;
			password = string.Empty;
		}
		if (GUI.Button(new Rect((float)Screen.width - (21f + (float)sPasswordOk.normal.background.width) * koofScreen, (float)Screen.height - (float)((21 + sPasswordOk.active.background.height) * Screen.height) / 768f, (float)sPasswordOk.normal.background.width * koofScreen, (float)sPasswordOk.normal.background.height * koofScreen), string.Empty, sPasswordOk))
		{
			showPasswordForm = false;
		}
	}

	private void guiPasswordEnter()
	{
		GUI.DrawTexture(new Rect((float)Screen.width / 2f - (float)head_password.width / 2f * koofScreen, (float)Screen.height * 0.1f - (float)head_password.height / 2f * (float)Screen.height / 768f, (float)head_password.width * koofScreen, (float)head_password.height * koofScreen), head_enter_password);
		Rect position = new Rect((float)Screen.width * 0.5f - (float)nameStyle.normal.background.width * 0.5f * koofScreen, (float)Screen.height * 0.23f - (float)nameStyle.normal.background.height * 0.5f * koofScreen, (float)nameStyle.normal.background.width * koofScreen, (float)nameStyle.normal.background.height * koofScreen);
		password = GUI.TextField(position, password, nameStyle);
		if (GUI.Button(new Rect(21f * (float)Screen.height / 768f, (float)Screen.height - (21f + (float)sPasswordCancel.active.background.height) * (float)Screen.height / 768f, (float)sPasswordCancel.normal.background.width * koofScreen, (float)sPasswordCancel.normal.background.height * koofScreen), string.Empty, sPasswordCancel))
		{
			showPasswordEnterForm = false;
			password = string.Empty;
		}
		if (!GUI.Button(new Rect((float)Screen.width - (21f + (float)sPasswordJoin.normal.background.width) * (float)Screen.height / 768f, (float)Screen.height - (float)((21 + sPasswordJoin.active.background.height) * Screen.height) / 768f, (float)sPasswordJoin.normal.background.width * koofScreen, (float)sPasswordJoin.normal.background.height * koofScreen), string.Empty, sPasswordJoin))
		{
			return;
		}
		showPasswordEnterForm = false;
		if (connectGame.customProperties["pass"].Equals(string.Empty) || connectGame.customProperties["pass"].Equals(password))
		{
			PlayerPrefs.SetString("MaxKill", connectGame.customProperties["MaxKill"].ToString());
			Debug.Log("setMaxKil client local = " + connectGame.customProperties["MaxKill"].ToString());
			PlayerPrefs.SetString("MapName", (PlayerPrefs.GetInt("COOP", 0) == 1) ? masMapNameCOOP[(int)connectGame.customProperties["map"]] : ((PlayerPrefs.GetInt("company", 0) != 1) ? masMapName[(int)connectGame.customProperties["map"]] : masMapNameCompany[(int)connectGame.customProperties["map"]]));
			goMapName = ((PlayerPrefs.GetInt("COOP", 0) == 1) ? masMapNameCOOP[(int)connectGame.customProperties["map"]] : ((PlayerPrefs.GetInt("company", 0) != 1) ? masMapName[(int)connectGame.customProperties["map"]] : masMapNameCompany[(int)connectGame.customProperties["map"]]));
			showLoading = true;
			setFonLoading(goMapName);
			PhotonNetwork.JoinRoom(connectGame.name);
			if (PlayerPrefs.GetString("TypeConnect").Equals("local"))
			{
				Application.LoadLevel((PlayerPrefs.GetInt("COOP", 0) == 1) ? masMapNameCOOP[(int)connectGame.customProperties["map"]] : ((PlayerPrefs.GetInt("company", 0) != 1) ? masMapName[(int)connectGame.customProperties["map"]] : masMapNameCompany[(int)connectGame.customProperties["map"]]));
			}
		}
		else if (!connectGame.customProperties["pass"].Equals(password))
		{
			showPasswordEnterForm = false;
			isPasswordIncorrect = true;
			Invoke("disableGuiIncorrectPassword", 2f);
		}
	}

	private void guiWiFiEnabled()
	{
		if (!isLocalAvailable)
		{
			GUI.DrawTexture(new Rect((float)Screen.width * 0.5f - (float)tLocalIsNotAvailable.width * koofScreen * 0.5f, (float)Screen.height * 0.13f, (float)tLocalIsNotAvailable.width * koofScreen, (float)tLocalIsNotAvailable.height * koofScreen), tLocalIsNotAvailable);
		}
	}

	private void setEnabledGUI()
	{
		isFirstFrame = false;
	}

	private void Start()
	{
		RightButtonRect = new Rect((float)Screen.width - (21f + (float)profile.normal.background.width) * (float)Screen.height / 768f, (float)Screen.height - (21f + (float)profile.normal.background.height) * (float)Screen.height / 768f, (float)(199 * Screen.height) / 768f, (float)(193 * Screen.height) / 768f);
		string @string = PlayerPrefs.GetString(Defs.ShouldReoeatActionSett, string.Empty);
		if (@string.Equals(Defs.GoToProfileAction))
		{
			PlayerPrefs.SetString(Defs.ShouldReoeatActionSett, string.Empty);
			PlayerPrefs.Save();
		}
		if (coinsPlashka.thisScript != null)
		{
			coinsPlashka.thisScript.enabled = false;
		}
		if (PlayerPrefs.GetInt("COOP", 0) == 0 && PlayerPrefs.GetInt("company", 0) != 1)
		{
			for (int i = 0; i < masMapCOOP.Length; i++)
			{
				masMapCOOP[i] = null;
				masLoadingCOOP[i] = null;
			}
			for (int j = 0; j < masLoadingCompany.Length; j++)
			{
				masMapCompany[j] = null;
				masLoadingCompany[j] = null;
			}
		}
		else
		{
			for (int k = 0; k < masMap.Length; k++)
			{
				masMap[k] = null;
				masLoading[k] = null;
			}
		}
		Resources.UnloadUnusedAssets();
		Invoke("setEnabledGUI", 0.1f);
		_purchaseActivityIndicator = StoreKitEventListener.purchaseActivityInd;
		if (typeConnect == 2)
		{
			connectToServer();
		}
		name = PlayerPrefs.GetString("nameServerStart", "Enter server name");
		pointMap = new Vector2((float)Screen.width / 2f, (float)Screen.height / 2f);
		_weaponManager = GameObject.FindGameObjectWithTag("WeaponManager").GetComponent<WeaponManager>();
		myName = PlayerPrefs.GetString("NamePlayer", Defs.defaultPlayerName);
	}

	private void Update()
	{
#if UNITY_STANDALONE
		float axis = Input.GetAxis("Mouse ScrollWheel");
			if (axis != 0f)
			{
				Debug.Log("Scroll wheel count: " + axis);
				if (axis > 0f)
				{
					selectMapIndex--;
				}
				else if (axis < 0f)
				{
					selectMapIndex++;
				}
				if (selectMapIndex < 0)
				{
					selectMapIndex = ((PlayerPrefs.GetInt("COOP", 0) == 1) ? masMapCOOP.Length : ((PlayerPrefs.GetInt("company", 0) != 1) ? masMap.Length : masMapCompany.Length)) - 1;
				}
				if (selectMapIndex == ((PlayerPrefs.GetInt("COOP", 0) == 1) ? masMapCOOP.Length : ((PlayerPrefs.GetInt("company", 0) != 1) ? masMap.Length : masMapCompany.Length)))
				{
					selectMapIndex = 0;
            }
#endif
	}
		slideScroll();
		if (typeConnect == 1 && PhotonNetwork.connectionState == ConnectionState.Disconnected)
		{
			Debug.Log("OnDisconnectedFromServer");
			typeGame = 3;
			typeConnect = 0;
			PlayerPrefs.SetString("TypeGame", "client");
			regimGUIClient = 0;
			regimGUIServer = 0;
			isSetMap = false;
			servers.Clear();
			players.Clear();
			showLoading = false;
		}
	}

	private void OnGUI()
	{
		if (showLoading)
		{
			Rect position = new Rect(((float)Screen.width - 2048f * (float)Screen.height / 1154f) / 2f, 0f, 2048f * (float)Screen.height / 1154f, Screen.height);
			GUI.DrawTexture(position, loadingToDraw, ScaleMode.StretchToFill);
			return;
		}
		GUI.skin = tableSkin;
		GUI.DrawTexture(new Rect((float)Screen.width / 2f - 683f * koofScreen, 0f, 1366f * koofScreen, Screen.height), fon2Texture);
		if (showStatistik)
		{
			if (_purchaseActivityIndicator == null)
			{
				Debug.LogWarning("_purchaseActivityIndicator == null");
			}
			else
			{
				_purchaseActivityIndicator.SetActive(false);
			}
			int[] array = new int[(PlayerPrefs.GetInt("COOP", 0) != 1) ? masMapName.Length : masMapNameCOOP.Length];
			for (int i = 0; i < ((PlayerPrefs.GetInt("COOP", 0) == 1) ? masMapNameCOOP.Length : ((PlayerPrefs.GetInt("company", 0) != 1) ? masMapName.Length : masMapNameCompany.Length)); i++)
			{
				array[i] = 0;
			}
			RoomInfo[] roomList = PhotonNetwork.GetRoomList();
			RoomInfo[] array2 = roomList;
			foreach (RoomInfo roomInfo in array2)
			{
				array[(int)roomInfo.customProperties["map"]]++;
			}
			playersWindow.fontSize = Mathf.RoundToInt(30f * koofScreen);
			GUILayout.Space((float)Screen.height * 0.5f - (float)playersWindow.normal.background.height * 0.5f * koofScreen);
			GUILayout.BeginHorizontal(GUILayout.Height((float)playersWindow.normal.background.height * koofScreen));
			GUILayout.Space((float)Screen.width * 0.5f - (float)playersWindow.normal.background.width * 0.5f * koofScreen);
			scrollPosition = GUILayout.BeginScrollView(scrollPosition, playersWindow);
			for (int k = 0; k < ((PlayerPrefs.GetInt("COOP", 0) == 1) ? masMapNameCOOP.Length : ((PlayerPrefs.GetInt("company", 0) != 1) ? masMapName.Length : masMapNameCompany.Length)); k++)
			{
				GUILayout.Space(20f * koofScreen);
				GUILayout.BeginHorizontal();
				GUILayout.Space(20f * koofScreen);
				GUILayout.Label((PlayerPrefs.GetInt("COOP", 0) != 1) ? ((PlayerPrefs.GetInt("company", 0) != 1) ? masMapName[k] : masMapNameCompany[k]) : masMapNameCOOP[k], playersWindow, GUILayout.Width((float)playersWindow.normal.background.width * koofScreen * 0.7f));
				GUILayout.Label(string.Empty + array[k], playersWindow, GUILayout.Width((float)playersWindow.normal.background.width * koofScreen * 0.2f));
				GUILayout.Space(20f * koofScreen);
				GUILayout.EndHorizontal();
			}
			GUILayout.EndScrollView();
			GUILayout.EndHorizontal();
			if (GUI.Button(new Rect(21f * koofScreen, (float)Screen.height - (21f + (float)back.active.background.height) * (float)Screen.height / 768f, (float)(back.active.background.width * Screen.height) / 768f, (float)(back.active.background.height * Screen.height) / 768f), string.Empty, back))
			{
				typeConnect = 0;
				showStatistik = false;
				PhotonNetwork.Disconnect();
			}
			return;
		}
		guiIncorrectPassword();
		if (_purchaseActivityIndicator == null)
		{
			Debug.LogWarning("_purchaseActivityIndicator == null");
		}
		else
		{
			_purchaseActivityIndicator.SetActive(connectingFoton || startingGame);
		}
		GUI.enabled = !showNotConnectingFoton && timerShowNotConnecting <= 0f && !connectingFoton;
		if (typeConnect == 0)
		{
			float top = (float)Screen.height * 0.1f - (float)head_connection.height / 2f * (float)Screen.height / 768f;
			if (PlayerPrefs.GetInt("COOP", 0) == 1 || PlayerPrefs.GetInt("company", 0) == 1)
			{
				float num = (float)Screen.height / 20f;
				Rect position2 = new Rect((float)Screen.width / 2f - (float)gameModeHeader.width / 2f * koofScreen, top, (float)gameModeHeader.width * koofScreen, (float)gameModeHeader.height * koofScreen);
				GUI.DrawTexture(position2, gameModeHeader);
				float num2 = (float)timeSurvivalCoop.normal.background.height * Defs.Coef;
				Rect position3 = new Rect((float)Screen.width * 0.5f - (float)timeSurvivalCoop.normal.background.width * Defs.Coef * 0.5f, (float)Screen.height / 2f - num - num2 * 1.5f, (float)timeSurvivalCoop.normal.background.width * Defs.Coef, num2);
				if (GUI.Button(position3, string.Empty, timeSurvivalCoop))
				{
					PlayerPrefs.SetInt("COOP", 1);
					PlayerPrefs.SetInt("company", 0);
					_initializeWorldwide();
				}
				position3.y += num2 + num;
				if (GUI.Button(position3, string.Empty, teamFight))
				{
					killToWin = "20";
					PlayerPrefs.SetInt("COOP", 0);
					PlayerPrefs.SetInt("company", 1);
					_initializeWorldwide();
				}
				position3.y += num2 + num;
				GUI.DrawTexture(position3, bossBattle);
			}
			else
			{
				Rect position4 = new Rect((float)Screen.width / 2f - (float)head_connection.width / 2f * koofScreen, top, (float)head_connection.width * koofScreen, (float)head_connection.height * koofScreen);
				GUI.DrawTexture(position4, head_connection);
				if (GUI.Button(new Rect((float)Screen.width / 2f - (float)worldwide.active.background.width / 2f * (float)Screen.height / 768f, (float)Screen.height * 0.4f - (float)worldwide.active.background.height / 2f * (float)Screen.height / 768f, (float)(worldwide.active.background.width * Screen.height) / 768f, (float)(worldwide.active.background.height * Screen.height) / 768f), string.Empty, worldwide))
				{
					FlurryPluginWrapper.LogMultiplayeWorldwideEvent();
					PlayerPrefs.SetInt("company", 0);
					killToWin = "15";
					_initializeWorldwide();
				}
				if (GUI.Button(new Rect((float)Screen.width / 2f - (float)local.active.background.width / 2f * (float)Screen.height / 768f, (float)Screen.height * 0.6f - (float)local.active.background.height / 2f * (float)Screen.height / 768f, (float)(local.active.background.width * Screen.height) / 768f, (float)(local.active.background.height * Screen.height) / 768f), string.Empty, local) && !isFirstFrame)
				{
					FlurryPluginWrapper.LogMultiplayeLocalEvent();
					killToWin = "15";
					typeConnect = 2;
					PlayerPrefs.SetString("TypeConnect", "local");
					typeGame = 2;
					PlayerPrefs.SetString("TypeGame", "client");
					connectToServer();
					showPasswordEnterForm = false;
					showFilterForm = false;
				}
			}
			if (GUI.RepeatButton(new Rect(21f * koofScreen, (float)Screen.height - (21f + (float)back.active.background.height) * (float)Screen.height / 768f, (float)(back.active.background.width * Screen.height) / 768f, (float)(back.active.background.height * Screen.height) / 768f), string.Empty, back))
			{
				GUIHelper.DrawLoading();
				typeConnect = 0;
				FlurryPluginWrapper.LogEvent("Back to Main Menu");
				MenuBackgroundMusic.keepPlaying = true;
				LoadConnectScene.textureToShow = null;
				LoadConnectScene.sceneToLoad = Defs.MainMenuScene;
				Application.LoadLevel(Defs.PromSceneName);
			}
		}
		else if (typeConnect == 1)
		{
			if (typeGame == 0)
			{
				GUI.DrawTexture(new Rect((float)Screen.width / 2f - (float)head_worldwide.width / 2f * koofScreen, (float)Screen.height * 0.1f - (float)head_worldwide.height / 2f * (float)Screen.height / 768f, (float)head_worldwide.width * koofScreen, (float)head_worldwide.height * koofScreen), head_worldwide);
				if (GUI.Button(new Rect((float)Screen.width / 2f - (float)create.active.background.width / 2f * (float)Screen.height / 768f, (float)Screen.height * 0.4f - (float)create.active.background.height / 2f * (float)Screen.height / 768f, (float)(create.active.background.width * Screen.height) / 768f, (float)(create.active.background.height * Screen.height) / 768f), string.Empty, create))
				{
					typeGame = 1;
				}
				if (GUI.Button(new Rect((float)Screen.width / 2f - (float)connect.active.background.width / 2f * (float)Screen.height / 768f, (float)Screen.height * 0.6f - (float)connect.active.background.height / 2f * (float)Screen.height / 768f, (float)(connect.active.background.width * Screen.height) / 768f, (float)(connect.active.background.height * Screen.height) / 768f), string.Empty, connect))
				{
					typeGame = 2;
					PlayerPrefs.SetString("TypeGame", "client");
				}
				if (GUI.Button(new Rect(21f * koofScreen, (float)Screen.height - (21f + (float)back.active.background.height) * (float)Screen.height / 768f, (float)(back.active.background.width * Screen.height) / 768f, (float)(back.active.background.height * Screen.height) / 768f), string.Empty, back))
				{
					typeConnect = 0;
					pressBack = true;
					PhotonNetwork.Disconnect();
				}
			}
			if (typeGame == 1)
			{
				if (regimGUIServer == 0)
				{
					settingsServerGUI();
				}
				if (regimGUIServer == 1)
				{
					playersTable();
				}
			}
			if (typeGame == 2)
			{
				if (regimGUIClient == 0)
				{
					if (!showPasswordEnterForm)
					{
						clientTableGUI();
						if (!showFilterForm)
						{
							GUI.DrawTexture(new Rect((float)Screen.width / 2f - (float)head_listOfGames.width / 2f * koofScreen, (float)Screen.height * 0.1f - (float)head_listOfGames.height / 2f * (float)Screen.height / 768f, (float)head_listOfGames.width * koofScreen, (float)head_listOfGames.height * koofScreen), head_listOfGames);
							if (GUI.Button(new Rect((float)Screen.width / 2f - (float)create.active.background.width / 2f * (float)Screen.height / 768f, (float)Screen.height - (21f + (float)create.active.background.height) * (float)Screen.height / 768f, (float)(create.active.background.width * Screen.height) / 768f, (float)(create.active.background.height * Screen.height) / 768f), string.Empty, create))
							{
								typeGame = 1;
							}
							GUI.DrawTexture(new Rect((float)Screen.width * 0.5f + (float)playersWindow.normal.background.width * 0.7f * koofScreen, (float)Screen.height * 0.5f - (float)swipeTexture.height * 0.5f * koofScreen, (float)swipeTexture.width * koofScreen, (float)swipeTexture.height * koofScreen), swipeTexture);
							if (GUI.Button(new Rect(21f * koofScreen, (float)Screen.height - (21f + (float)back.active.background.height) * (float)Screen.height / 768f, (float)(back.active.background.width * Screen.height) / 768f, (float)(back.active.background.height * Screen.height) / 768f), string.Empty, back))
							{
								typeGame = 3;
								gameFilter = string.Empty;
								updateFilteredRoomList(gameFilter);
							}
							if (GUI.Button(new Rect((float)Screen.width - (21f + (float)sSearch.active.background.width) * (float)Screen.height / 768f, 21f * (float)Screen.height / 768f, (float)(sSearch.active.background.width * Screen.height) / 768f, (float)(sSearch.active.background.height * Screen.height) / 768f), string.Empty, sSearch))
							{
								showFilterForm = true;
								gameFilterTemp = string.Empty;
							}
						}
					}
					else
					{
						guiPasswordEnter();
					}
				}
				if (regimGUIClient == 2)
				{
					GUI.Label(new Rect((float)Screen.width * 0.35f, (float)Screen.height * 0.5f, (float)Screen.width * 0.3f, (float)Screen.height * 0.05f), "Connected...");
				}
				if (regimGUIClient == 3)
				{
					playersTable();
				}
			}
			if (typeGame == 3)
			{
				GUI.DrawTexture(new Rect((float)Screen.width / 2f - (float)head_first.width / 2f * koofScreen, (float)Screen.height * 0.1f - (float)head_first.height / 2f * (float)Screen.height / 768f, (float)head_first.width * koofScreen, (float)head_first.height * koofScreen), head_first);
				selectMap();
				if (GUI.Button(new Rect((float)Screen.width * 0.5f - (float)bigStart.normal.background.width / 2f * (float)Screen.height / 768f, (float)Screen.height - (21f + (float)bigStart.normal.background.height) * (float)Screen.height / 768f, (float)(bigStart.normal.background.width * Screen.height) / 768f, (float)(bigStart.normal.background.height * Screen.height) / 768f), string.Empty, bigStart))
				{
					isRandomSelectMap = true;
					ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
					hashtable["pass"] = string.Empty;
					hashtable["map"] = selectMapIndex;
					PlayerPrefs.SetString("TypeGame", "client");
					PlayerPrefs.SetInt("CustomGame", 0);
					PhotonNetwork.JoinRandomRoom(hashtable, 2);
					if (StoreKitEventListener.purchaseActivityInd == null)
					{
						Debug.LogWarning("StoreKitEventListener.purchaseActivityInd");
					}
					else
					{
						StoreKitEventListener.purchaseActivityInd.SetActive(true);
					}
					startingGame = true;
					FlurryPluginWrapper.LogEnteringMap(typeConnect, (PlayerPrefs.GetInt("COOP", 0) != 0) ? masMapNameCOOP[selectMapIndex] : masMapName[selectMapIndex]);
					FlurryPluginWrapper.LogMultiplayerWayStart();
				}
				if (GUI.Button(new Rect(21f * koofScreen, (float)Screen.height - (21f + (float)back.active.background.height) * (float)Screen.height / 768f, (float)(back.active.background.width * Screen.height) / 768f, (float)(back.active.background.height * Screen.height) / 768f), string.Empty, back))
				{
					typeConnect = 0;
					pressBack = true;
					PhotonNetwork.Disconnect();
				}
				Rect position5 = new Rect((float)Screen.width - (21f + (float)customGame.active.background.width) * (float)Screen.height / 768f, (float)Screen.height - (21f + (float)customGame.active.background.height) * (float)Screen.height / 768f, (float)(customGame.active.background.width * Screen.height) / 768f, (float)(customGame.active.background.height * Screen.height) / 768f);
				if (GUI.Button(new Rect((float)Screen.width - (21f + (float)randomStyle.active.background.width) * (float)Screen.height / 768f, 21f * (float)Screen.height / 768f, (float)randomStyle.normal.background.width * koofScreen, (float)randomStyle.normal.background.height * koofScreen), string.Empty, randomStyle))
				{
					isRandomSelectMap = false;
					ExitGames.Client.Photon.Hashtable hashtable2 = new ExitGames.Client.Photon.Hashtable();
					hashtable2["pass"] = string.Empty;
					PlayerPrefs.SetString("TypeGame", "client");
					PlayerPrefs.SetInt("CustomGame", 0);
					PhotonNetwork.JoinRandomRoom(hashtable2, 2);
					StoreKitEventListener.purchaseActivityInd.SetActive(true);
					startingGame = true;
					FlurryPluginWrapper.LogMultiplayerWayQuckRandGame();
				}
				if (GUI.Button(position5, string.Empty, customGame))
				{
					typeGame = 2;
					FlurryPluginWrapper.LogMultiplayerWayCustom();
				}
			}
		}
		else if (typeGame == 0)
		{
			if (typeGame == 0)
			{
				GUI.DrawTexture(new Rect((float)Screen.width / 2f - (float)head_worldwide.width / 2f * koofScreen, (float)Screen.height * 0.1f - (float)head_worldwide.height / 2f * (float)Screen.height / 768f, (float)head_worldwide.width * koofScreen, (float)head_worldwide.height * koofScreen), head_local);
				if (GUI.Button(new Rect((float)Screen.width / 2f - (float)create.active.background.width / 2f * (float)Screen.height / 768f, (float)Screen.height * 0.4f - (float)create.active.background.height / 2f * (float)Screen.height / 768f, (float)(create.active.background.width * Screen.height) / 768f, (float)(create.active.background.height * Screen.height) / 768f), string.Empty, create))
				{
					typeGame = 1;
				}
				if (GUI.Button(new Rect((float)Screen.width / 2f - (float)connect.active.background.width / 2f * (float)Screen.height / 768f, (float)Screen.height * 0.6f - (float)connect.active.background.height / 2f * (float)Screen.height / 768f, (float)(connect.active.background.width * Screen.height) / 768f, (float)(connect.active.background.height * Screen.height) / 768f), string.Empty, connect))
				{
					typeGame = 2;
					PlayerPrefs.SetString("TypeGame", "client");
					connectToServer();
				}
				guiWiFiEnabled();
				if (GUI.Button(new Rect(21f * koofScreen, (float)Screen.height - (21f + (float)back.active.background.height) * (float)Screen.height / 768f, (float)(back.active.background.width * Screen.height) / 768f, (float)(back.active.background.height * Screen.height) / 768f), string.Empty, back))
				{
					typeConnect = 0;
				}
			}
		}
		else
		{
			if (typeGame == 1)
			{
				if (regimGUIServer == 0)
				{
					settingsServerGUI();
				}
				if (regimGUIServer == 1)
				{
					playersTable();
				}
			}
			if (typeGame == 2)
			{
				if (regimGUIClient == 0)
				{
					clientTableGUI();
					GUI.DrawTexture(new Rect((float)Screen.width / 2f - (float)head_listOfGames.width / 2f * koofScreen, (float)Screen.height * 0.1f - (float)head_listOfGames.height / 2f * (float)Screen.height / 768f, (float)head_listOfGames.width * koofScreen, (float)head_listOfGames.height * koofScreen), head_listOfGames);
					GUI.DrawTexture(new Rect((float)Screen.width * 0.5f + (float)playersWindow.normal.background.width * 0.7f * koofScreen, (float)Screen.height * 0.5f - (float)swipeTexture.height * 0.5f * koofScreen, (float)swipeTexture.width * koofScreen, (float)swipeTexture.height * koofScreen), swipeTexture);
					if (GUI.Button(new Rect((float)Screen.width / 2f - (float)create.active.background.width / 2f * (float)Screen.height / 768f, (float)Screen.height - (21f + (float)create.active.background.height) * (float)Screen.height / 768f, (float)(create.active.background.width * Screen.height) / 768f, (float)(create.active.background.height * Screen.height) / 768f), string.Empty, create))
					{
						LANBroadcastService component = GetComponent<LANBroadcastService>();
						component.StopBroadCasting();
						typeGame = 1;
					}
					guiWiFiEnabled();
					if (GUI.Button(new Rect(21f * koofScreen, (float)Screen.height - (21f + (float)back.active.background.height) * (float)Screen.height / 768f, (float)(back.active.background.width * Screen.height) / 768f, (float)(back.active.background.height * Screen.height) / 768f), string.Empty, back))
					{
						typeGame = 0;
						typeConnect = 0;
						LANBroadcastService component2 = GetComponent<LANBroadcastService>();
						component2.StopBroadCasting();
					}
				}
				if (regimGUIClient == 2)
				{
					GUI.Label(new Rect((float)Screen.width * 0.35f, (float)Screen.height * 0.5f, (float)Screen.width * 0.3f, (float)Screen.height * 0.05f), "Connected...");
				}
				if (regimGUIClient == 3)
				{
					playersTable();
				}
			}
		}
		if (timerShowServerFull > 0f)
		{
			Debug.Log("------");
			GUI.Label(new Rect(0f, (float)Screen.height * 0.15f, Screen.width, (float)Screen.height * 0.2f), "Game is full ...", openServerText);
			timerShowServerFull -= Time.deltaTime;
		}
		if (timerShowFaledJoin > 0f)
		{
			GUI.Label(new Rect(0f, (float)Screen.height * 0.15f, Screen.width, (float)Screen.height * 0.2f), "Can't connect ...", openServerText);
			timerShowFaledJoin -= Time.deltaTime;
		}
		if (timerShowErrorName > 0f)
		{
			GUI.Label(new Rect(0f, (float)Screen.height * 0.15f, Screen.width, (float)Screen.height * 0.2f), "Name is already used!", openServerText);
			timerShowErrorName -= Time.deltaTime;
		}
		GUI.enabled = true;
		if (timerShowNotConnecting > 0f)
		{
			GUI.Label(new Rect(0f, (float)Screen.height - (21f + (float)back.normal.background.height) * koofScreen, Screen.width, (float)back.normal.background.height * koofScreen), "Server is not available,\ncheck the internet connection.", openServerText);
			timerShowNotConnecting -= Time.deltaTime;
			if (timerShowNotConnecting <= 0f)
			{
				connectingFoton = false;
			}
		}
	}

	private void setRationg(int _selectMap)
	{
		RoomInfo[] roomList = PhotonNetwork.GetRoomList();
		if (roomList.Length == 0)
		{
			ratingSelectMap = 0;
			ratingSelectMapDO = 0;
			ratingSelectMapPOSLE = 0;
			return;
		}
		int num = _selectMap - 1;
		int num2 = _selectMap + 1;
		if (num < 0)
		{
			num = ((PlayerPrefs.GetInt("COOP", 0) == 1) ? masMapCOOP.Length : ((PlayerPrefs.GetInt("company", 0) != 1) ? masMap.Length : masMapCompany.Length)) - 1;
		}
		if (num2 == ((PlayerPrefs.GetInt("COOP", 0) == 1) ? masMapCOOP.Length : ((PlayerPrefs.GetInt("company", 0) != 1) ? masMap.Length : masMapCompany.Length)))
		{
			num2 = 0;
		}
		int[] array = new int[(PlayerPrefs.GetInt("COOP", 0) == 1) ? masMapNameCOOP.Length : ((PlayerPrefs.GetInt("company", 0) != 1) ? masMapName.Length : masMapNameCompany.Length)];
		for (int i = 0; i < ((PlayerPrefs.GetInt("COOP", 0) == 1) ? masMapNameCOOP.Length : ((PlayerPrefs.GetInt("company", 0) != 1) ? masMapName.Length : masMapNameCompany.Length)); i++)
		{
			array[i] = 0;
		}
		RoomInfo[] array2 = roomList;
		foreach (RoomInfo roomInfo in array2)
		{
			array[(int)roomInfo.customProperties["map"]]++;
		}
		float num3 = (float)array[_selectMap] / (float)roomList.Length * 100f;
		float num4 = (float)array[num] / (float)roomList.Length * 100f;
		float num5 = (float)array[num2] / (float)roomList.Length * 100f;
		for (int k = 0; k < 3; k++)
		{
			int num6 = 0;
			float num7 = num3;
			if (k == 1)
			{
				num7 = num4;
			}
			if (k == 2)
			{
				num7 = num5;
			}
			if (num7 >= 3f && num7 < 6f)
			{
				num6 = 1;
			}
			if (num7 >= 6f && num7 < 12f)
			{
				num6 = 2;
			}
			if (num7 >= 12f && num7 < 15f)
			{
				num6 = 3;
			}
			if (num7 >= 15f)
			{
				num6 = 4;
			}
			if (k == 0)
			{
				ratingSelectMap = num6;
			}
			if (k == 1)
			{
				ratingSelectMapDO = num6;
			}
			if (k == 2)
			{
				ratingSelectMapPOSLE = num6;
			}
		}
		Debug.Log(ratingSelectMap + string.Empty);
	}

	private void selectMap()
	{
		float num = -0.04f;
		mapNameStyle.fontSize = Mathf.RoundToInt(30f * koofScreen);
		if (!isVozvratMap && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
		{
			pressPoint = Input.GetTouch(0).position;
			startPoint = pointMap;
			isMoveMap = true;
		}
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved && isMoveMap)
		{
			pointMap = new Vector2(startPoint.x + Input.GetTouch(0).position.x - pressPoint.x, pointMap.y);
		}
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended && isMoveMap)
		{
			isMoveMap = false;
			if (pointMap.x < (float)Screen.width * 0.3f)
			{
				selectMapIndex++;
				pointMap.x += sizeMap.x * (0.5f + num) * koofScreen;
			}
			if (pointMap.x > (float)Screen.width * 0.7f)
			{
				selectMapIndex--;
				pointMap.x -= sizeMap.x * (0.5f + num) * koofScreen;
			}
			if (selectMapIndex < 0)
			{
				selectMapIndex = ((PlayerPrefs.GetInt("COOP", 0) == 1) ? masMapCOOP.Length : ((PlayerPrefs.GetInt("company", 0) != 1) ? masMap.Length : masMapCompany.Length)) - 1;
			}
			if (selectMapIndex == ((PlayerPrefs.GetInt("COOP", 0) == 1) ? masMapCOOP.Length : ((PlayerPrefs.GetInt("company", 0) != 1) ? masMap.Length : masMapCompany.Length)))
			{
				selectMapIndex = 0;
			}
			setRationg(selectMapIndex);
			mapServer = ((PlayerPrefs.GetInt("COOP", 0) == 1) ? masMapNameCOOP[selectMapIndex] : ((PlayerPrefs.GetInt("company", 0) != 1) ? masMapName[selectMapIndex] : masMapNameCompany[selectMapIndex]));
		}
		if (!isMoveMap && (pointMap.x > (float)Screen.width * 0.5f + 1f || pointMap.x < (float)Screen.width * 0.5f - 1f))
		{
			isVozvratMap = true;
			float num2 = Time.deltaTime * (float)Screen.width * 0.7f;
			if (Mathf.Abs((float)Screen.width * 0.5f - pointMap.x) > num2)
			{
				if (pointMap.x > (float)Screen.width * 0.5f)
				{
					pointMap.x -= num2;
				}
				else
				{
					pointMap.x += num2;
				}
			}
			else
			{
				pointMap.x = (float)Screen.width * 0.5f;
				isVozvratMap = false;
			}
		}
		else
		{
			isVozvratMap = false;
		}
		int num3 = selectMapIndex - 1;
		int num4 = selectMapIndex + 1;
		if (num3 < 0)
		{
			num3 = ((PlayerPrefs.GetInt("COOP", 0) == 1) ? masMapCOOP.Length : ((PlayerPrefs.GetInt("company", 0) != 1) ? masMap.Length : masMapCompany.Length)) - 1;
		}
		if (num4 == ((PlayerPrefs.GetInt("COOP", 0) == 1) ? masMapCOOP.Length : ((PlayerPrefs.GetInt("company", 0) != 1) ? masMap.Length : masMapCompany.Length)))
		{
			num4 = 0;
		}
		GUI.DrawTexture(new Rect(pointMap.x - sizeMap.x * (1.5f + num) * koofScreen, pointMap.y - sizeMap.y * 0.5f * koofScreen, sizeMap.x * koofScreen, sizeMap.y * koofScreen), (PlayerPrefs.GetInt("COOP", 0) == 1) ? masMapCOOP[num3] : ((PlayerPrefs.GetInt("company", 0) != 1) ? masMap[num3] : masMapCompany[num3]));
		GUI.DrawTexture(new Rect(pointMap.x - sizeMap.x * 0.5f * koofScreen, pointMap.y - sizeMap.y * 0.5f * koofScreen, sizeMap.x * koofScreen, sizeMap.y * koofScreen), (PlayerPrefs.GetInt("COOP", 0) == 1) ? masMapCOOP[selectMapIndex] : ((PlayerPrefs.GetInt("company", 0) != 1) ? masMap[selectMapIndex] : masMapCompany[selectMapIndex]));
		GUI.DrawTexture(new Rect(pointMap.x + sizeMap.x * (0.5f + num) * koofScreen, pointMap.y - sizeMap.y * 0.5f * koofScreen, sizeMap.x * koofScreen, sizeMap.y * koofScreen), (PlayerPrefs.GetInt("COOP", 0) == 1) ? masMapCOOP[num4] : ((PlayerPrefs.GetInt("company", 0) != 1) ? masMap[num4] : masMapCompany[num4]));
		if (typeGame == 3)
		{
			GUI.DrawTexture(new Rect(pointMap.x - sizeMap.x * 0.43f * koofScreen, pointMap.y - sizeMap.y * 0.35f * koofScreen, (float)popularTextures[ratingSelectMap].width * koofScreen, (float)popularTextures[ratingSelectMap].height * koofScreen), popularTextures[ratingSelectMap]);
			GUI.DrawTexture(new Rect(pointMap.x - sizeMap.x * 1.39f * koofScreen, pointMap.y - sizeMap.y * 0.35f * koofScreen, (float)popularTextures[ratingSelectMap].width * koofScreen, (float)popularTextures[ratingSelectMap].height * koofScreen), popularTextures[ratingSelectMapDO]);
			GUI.DrawTexture(new Rect(pointMap.x + sizeMap.x * 0.53f * koofScreen, pointMap.y - sizeMap.y * 0.35f * koofScreen, (float)popularTextures[ratingSelectMap].width * koofScreen, (float)popularTextures[ratingSelectMap].height * koofScreen), popularTextures[ratingSelectMapPOSLE]);
		}
	}

	private void settingsServerGUI()
	{
		if (!isSetMap)
		{
			GUI.DrawTexture(new Rect((float)Screen.width / 2f - (float)head_serv_name.width / 2f * koofScreen, (float)Screen.height * 0.1f - (float)head_serv_name.height / 2f * (float)Screen.height / 768f, (float)head_serv_name.width * koofScreen, (float)head_serv_name.height * koofScreen), head_maps);
			selectMap();
			if (GUI.Button(new Rect((float)Screen.width - (21f + (float)next_conn.active.background.width) * (float)Screen.height / 768f, (float)Screen.height - (21f + (float)next_conn.active.background.height) * (float)Screen.height / 768f, (float)(next_conn.active.background.width * Screen.height) / 768f, (float)(next_conn.active.background.height * Screen.height) / 768f), string.Empty, next_conn))
			{
				isSetMap = true;
			}
			if (GUI.Button(new Rect(21f * koofScreen, (float)Screen.height - (21f + (float)back.active.background.height) * (float)Screen.height / 768f, (float)(back.active.background.width * Screen.height) / 768f, (float)(back.active.background.height * Screen.height) / 768f), string.Empty, back))
			{
				typeGame = 2;
				PlayerPrefs.SetString("TypeGame", "client");
				if (typeConnect == 2)
				{
					connectToServer();
				}
			}
		}
		else if (!showPasswordForm)
		{
			GUI.DrawTexture(new Rect((float)Screen.width / 2f - (float)head_serv_name.width / 2f * koofScreen, (float)Screen.height * 0.1f - (float)head_serv_name.height / 2f * (float)Screen.height / 768f, (float)head_serv_name.width * koofScreen, (float)head_serv_name.height * koofScreen), head_serv_name);
			numberofplayersStyle.fontSize = Mathf.RoundToInt(30f * koofScreen);
			countStyle.fontSize = Mathf.RoundToInt(30f * koofScreen);
			nameStyle.fontSize = Mathf.RoundToInt(30f * koofScreen);
			name = GUI.TextField(new Rect((float)Screen.width * 0.5f - (float)nameStyle.normal.background.width * 0.5f * koofScreen, (float)Screen.height * 0.23f - (float)nameStyle.normal.background.height * 0.5f * koofScreen, (float)nameStyle.normal.background.width * koofScreen, (float)nameStyle.normal.background.height * koofScreen), name, nameStyle);
			if (name.Length > 22)
			{
				name = name.Substring(0, 22);
			}
			PlayerPrefs.SetString("nameServerStart", name);
			GUI.Label(new Rect(0f, (float)Screen.height * 0.25f, Screen.width, (float)Screen.height * 0.2f), "NUMBER OF PLAYERS", numberofplayersStyle);
			if (PlayerPrefs.GetInt("company", 0) == 1)
			{
				if (GUI.Toggle(new Rect((float)Screen.width * 0.5f - (float)company2x2.normal.background.width * koofScreen * 1.5f, (float)Screen.height * 0.45f - (float)company2x2.normal.background.height * koofScreen * 0.5f, (float)company2x2.normal.background.width * koofScreen, (float)company2x2.normal.background.height * koofScreen), isPressed2x2, string.Empty, company2x2))
				{
					isPressed2x2 = true;
					isPressed3x3 = false;
					isPressed4x4 = false;
				}
				if (GUI.Toggle(new Rect((float)Screen.width * 0.5f - (float)company2x2.normal.background.width * koofScreen * 0.5f, (float)Screen.height * 0.45f - (float)company2x2.normal.background.height * koofScreen * 0.5f, (float)company2x2.normal.background.width * koofScreen, (float)company2x2.normal.background.height * koofScreen), isPressed3x3, string.Empty, company3x3))
				{
					isPressed2x2 = false;
					isPressed3x3 = true;
					isPressed4x4 = false;
				}
				if (GUI.Toggle(new Rect((float)Screen.width * 0.5f + (float)company2x2.normal.background.width * koofScreen * 0.5f, (float)Screen.height * 0.45f - (float)company2x2.normal.background.height * koofScreen * 0.5f, (float)company2x2.normal.background.width * koofScreen, (float)company2x2.normal.background.height * koofScreen), isPressed4x4, string.Empty, company4x4))
				{
					isPressed2x2 = false;
					isPressed3x3 = false;
					isPressed4x4 = true;
				}
				if (isPressed2x2)
				{
					limitsPlayer = "4";
				}
				if (isPressed3x3)
				{
					limitsPlayer = "6";
				}
				if (isPressed4x4)
				{
					limitsPlayer = "8";
				}
			}
			else
			{
				GUI.Label(new Rect((float)Screen.width * 0.5f - (float)countStyle.normal.background.width * koofScreen, (float)Screen.height * 0.45f - (float)countStyle.normal.background.height * koofScreen * 0.5f, (float)countStyle.normal.background.width * koofScreen, (float)countStyle.normal.background.height * koofScreen), limitsPlayer, countStyle);
				if (GUI.Button(new Rect((float)Screen.width * 0.5f + (float)minus.normal.background.width * koofScreen, (float)Screen.height * 0.45f - (float)plus.normal.background.height * koofScreen * 0.5f, (float)plus.normal.background.width * koofScreen, (float)plus.normal.background.height * koofScreen), string.Empty, plus))
				{
					int num = int.Parse(limitsPlayer) + 1;
					if (num > ((PlayerPrefs.GetInt("COOP", 0) != 1) ? 10 : 4))
					{
						num = ((PlayerPrefs.GetInt("COOP", 0) != 1) ? 10 : 4);
					}
					limitsPlayer = num.ToString();
				}
				if (GUI.Button(new Rect((float)Screen.width * 0.5f, (float)Screen.height * 0.45f - (float)minus.normal.background.height * koofScreen * 0.5f, (float)minus.normal.background.width * koofScreen, (float)minus.normal.background.height * koofScreen), string.Empty, minus))
				{
					int num2 = int.Parse(limitsPlayer) - 1;
					if (num2 < 2)
					{
						num2 = 2;
					}
					limitsPlayer = num2.ToString();
				}
			}
			if (PlayerPrefs.GetInt("COOP", 0) != 1)
			{
				GUI.Label(new Rect(0f, (float)Screen.height * 0.5f, Screen.width, (float)Screen.height * 0.2f), "KILLS TO WIN", numberofplayersStyle);
				GUI.Label(new Rect((float)Screen.width * 0.5f - (float)countStyle.normal.background.width * koofScreen, (float)Screen.height * 0.7f - (float)countStyle.normal.background.height / 2f * koofScreen, (float)countStyle.normal.background.width * koofScreen, (float)countStyle.normal.background.height * koofScreen), killToWin, countStyle);
				if (GUI.Button(new Rect((float)Screen.width * 0.5f + (float)minus.normal.background.width * koofScreen, (float)Screen.height * 0.7f - (float)plus.normal.background.height / 2f * koofScreen, (float)plus.normal.background.width * koofScreen, (float)plus.normal.background.height * koofScreen), string.Empty, plus))
				{
					int num3 = int.Parse(killToWin) + 1;
					if (num3 > 50)
					{
						num3 = 50;
					}
					killToWin = num3.ToString();
				}
				if (GUI.Button(new Rect((float)Screen.width * 0.5f, (float)Screen.height * 0.7f - (float)minus.normal.background.height / 2f * koofScreen, (float)minus.normal.background.width * koofScreen, (float)minus.normal.background.height * koofScreen), string.Empty, minus))
				{
					int num4 = int.Parse(killToWin) - 1;
					if (num4 < ((PlayerPrefs.GetInt("company", 0) != 1) ? 10 : 15))
					{
						num4 = ((PlayerPrefs.GetInt("company", 0) != 1) ? 10 : 15);
					}
					killToWin = num4.ToString();
				}
			}
			commentsServer = killToWin;
			if (PlayerPrefs.GetString("TypeConnect").Equals("inet"))
			{
				guiSetPassword();
			}
			if (isLocalAvailable || PlayerPrefs.GetString("TypeConnect").Equals("inet"))
			{
				if (GUI.Button(new Rect((float)Screen.width - (21f + (float)next_conn.active.background.width) * (float)Screen.height / 768f, (float)Screen.height - (21f + (float)next_conn.active.background.height) * (float)Screen.height / 768f, (float)(next_conn.active.background.width * Screen.height) / 768f, (float)(next_conn.active.background.height * Screen.height) / 768f), string.Empty, next_conn))
				{
					bool flag = false;
					if (PlayerPrefs.GetString("TypeConnect").Equals("inet"))
					{
						RoomInfo[] roomList = PhotonNetwork.GetRoomList();
						for (int i = 0; i < roomList.Length; i++)
						{
							if (roomList[i].name.Equals(name))
							{
								flag = true;
								break;
							}
						}
					}
					if (!flag)
					{
						registerServer();
						GlobalGameController.currentLevel = 7;
						goMapName = ((PlayerPrefs.GetInt("COOP", 0) == 1) ? masMapNameCOOP[selectMapIndex] : ((PlayerPrefs.GetInt("company", 0) != 1) ? masMapName[selectMapIndex] : masMapNameCompany[selectMapIndex]));
						Debug.Log("goMapName=" + goMapName + " PlayerPrefs.GetString(TypeConnect)=" + PlayerPrefs.GetString("TypeConnect"));
						if (PlayerPrefs.GetString("TypeConnect").Equals("local"))
						{
							Application.LoadLevel((PlayerPrefs.GetInt("COOP", 0) == 1) ? masMapNameCOOP[selectMapIndex] : ((PlayerPrefs.GetInt("company", 0) != 1) ? masMapName[selectMapIndex] : masMapNameCompany[selectMapIndex]));
						}
					}
					else
					{
						timerShowErrorName = 5f;
					}
				}
			}
			else
			{
				guiWiFiEnabled();
			}
			if (GUI.Button(new Rect(21f * koofScreen, (float)Screen.height - (21f + (float)back.active.background.height) * (float)Screen.height / 768f, (float)(back.active.background.width * Screen.height) / 768f, (float)(back.active.background.height * Screen.height) / 768f), string.Empty, back))
			{
				isSetMap = false;
			}
		}
		else
		{
			guiPasswordForm();
		}
	}

	private void registerServer()
	{
		PlayerPrefs.SetString("TypeGame", "server");
		if (typeConnect == 1)
		{
			string[] propsToListInLobby = new string[3] { "map", "MaxKill", "pass" };
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable.Add("map", selectMapIndex);
			hashtable.Add("MaxKill", int.Parse(commentsServer));
			hashtable.Add("pass", password);
			ExitGames.Client.Photon.Hashtable customRoomProperties = hashtable;
			PlayerPrefs.SetString("MapName", (PlayerPrefs.GetInt("COOP", 0) == 1) ? masMapNameCOOP[selectMapIndex] : ((PlayerPrefs.GetInt("company", 0) != 1) ? masMapName[selectMapIndex] : masMapNameCompany[selectMapIndex]));
			PlayerPrefs.SetString("MaxKill", commentsServer);
			showLoading = true;
			setFonLoading((PlayerPrefs.GetInt("COOP", 0) == 1) ? masMapNameCOOP[selectMapIndex] : ((PlayerPrefs.GetInt("company", 0) != 1) ? masMapName[selectMapIndex] : masMapNameCompany[selectMapIndex]));
			string roomName = _weaponManager.gameObject.GetComponent<FilterBadWorld>().FilterString(name);
			PhotonNetwork.CreateRoom(roomName, true, true, int.Parse(limitsPlayer), customRoomProperties, propsToListInLobby);
		}
		if (typeConnect == 2)
		{
			bool useNat = Network.HavePublicAddress();
			Network.InitializeServer(int.Parse(limitsPlayer) - 1, 25002, useNat);
			PlayerPrefs.SetString("ServerName", name);
			PlayerPrefs.SetString("MapName", (PlayerPrefs.GetInt("COOP", 0) == 1) ? masMapNameCOOP[selectMapIndex] : ((PlayerPrefs.GetInt("company", 0) != 1) ? masMapName[selectMapIndex] : masMapNameCompany[selectMapIndex]));
			PlayerPrefs.SetString("PlayersLimits", limitsPlayer);
			PlayerPrefs.SetString("MaxKill", commentsServer);
			showLoading = true;
			setFonLoading((PlayerPrefs.GetInt("COOP", 0) == 1) ? masMapNameCOOP[selectMapIndex] : ((PlayerPrefs.GetInt("company", 0) != 1) ? masMapName[selectMapIndex] : masMapNameCompany[selectMapIndex]));
			Debug.Log("setMaxKil server local = " + commentsServer);
			Debug.Log("password: " + password);
		}
		FlurryPluginWrapper.LogEnteringMap(typeConnect, PlayerPrefs.GetString("MapName"));
		Debug.Log("registerServer");
		PlayerPrefs.SetString("NamePlayer", myName);
	}

	private void connectToServer()
	{
		LANBroadcastService component = GetComponent<LANBroadcastService>();
		component.StartSearchBroadCasting(seachServer, isNotServer);
		Invoke("checkConSuccess", 1f);
	}

	private void playersTable()
	{
		GUI.DrawTexture(new Rect((float)Screen.width / 2f - (float)head_players.width / 2f * koofScreen, (float)Screen.height * 0.1f - (float)head_players.height / 2f * (float)Screen.height / 768f, (float)head_players.width * koofScreen, (float)head_players.height * koofScreen), head_players);
		if (typeGame == 1 && GUI.Button(new Rect((float)Screen.width * 0.88f - (float)start.normal.background.width / 2f * (float)Screen.height / 768f, (float)Screen.height - (21f + (float)start.normal.background.height) * (float)Screen.height / 768f, (float)(start.normal.background.width * Screen.height) / 768f, (float)(start.normal.background.height * Screen.height) / 768f), string.Empty, start))
		{
			GlobalGameController.currentLevel = 7;
			if (PlayerPrefs.GetInt("MultyPlayer") == 1)
			{
				base.GetComponent<NetworkView>().RPC("goLevel", RPCMode.AllBuffered, mapServer);
			}
		}
		playersWindow.fontSize = Mathf.RoundToInt(30f * koofScreen);
		GUILayout.Space((float)Screen.height * 0.5f - (float)playersWindow.normal.background.height * 0.5f * koofScreen);
		GUILayout.BeginHorizontal(GUILayout.Height((float)playersWindow.normal.background.height * koofScreen));
		GUILayout.Space((float)Screen.width * 0.5f - (float)playersWindow.normal.background.width * 0.5f * koofScreen);
		scrollPosition = GUILayout.BeginScrollView(scrollPosition, playersWindow);
		if (players.Count > 0)
		{
			foreach (infoClient player in players)
			{
				GUILayout.Space(20f * koofScreen);
				GUILayout.BeginHorizontal();
				GUILayout.Space(20f * koofScreen);
				GUILayout.Label(player.name, playersWindow, GUILayout.Width((float)playersWindow.normal.background.width * koofScreen * 0.8f));
				GUILayout.Space(20f * koofScreen);
				GUILayout.EndHorizontal();
			}
		}
		GUILayout.EndScrollView();
		GUILayout.EndHorizontal();
		if (GUI.Button(new Rect(21f * koofScreen, (float)Screen.height - (21f + (float)back.active.background.height) * (float)Screen.height / 768f, (float)(back.active.background.width * Screen.height) / 768f, (float)(back.active.background.height * Screen.height) / 768f), string.Empty, back))
		{
			disconnectGame();
		}
	}

	[RPC]
	private void addPlayer(string _name, string _ip, NetworkMessageInfo info)
	{
		infoClient item = default(infoClient);
		item.name = _name;
		item.ipAddress = _ip;
		players.Add(item);
		Debug.Log("playerCount " + players.Count);
	}

	[RPC]
	private void goLevel(string _mapName, NetworkMessageInfo info)
	{
		Application.LoadLevel(_mapName);
	}

	[RPC]
	private void delPlayer(string _ip, NetworkMessageInfo info)
	{
		Debug.Log("delPlayer " + _ip);
		for (int i = 0; i < players.Count; i++)
		{
			if (players[i].ipAddress.Equals(_ip))
			{
				players.RemoveAt(i);
				break;
			}
		}
	}

	private void disconnectGame()
	{
		if (typeConnect == 1)
		{
			if (typeGame == 1)
			{
				Network.Disconnect(200);
			}
			if (typeGame == 2 && Network.connections.Length == 1)
			{
				Debug.Log("Disconnecting: " + Network.connections[0].ipAddress + ":" + Network.connections[0].port);
				Network.CloseConnection(Network.connections[0], true);
			}
			typeGame = 0;
			typeConnect = 0;
			regimGUIClient = 0;
			regimGUIServer = 0;
			servers.Clear();
			players.Clear();
		}
		if (typeConnect == 2)
		{
			if (typeGame == 1)
			{
				LANBroadcastService component = GetComponent<LANBroadcastService>();
				component.StopBroadCasting();
				Network.Disconnect(200);
			}
			if (typeGame == 2 && Network.connections.Length == 1)
			{
				Debug.Log("Disconnecting: " + Network.connections[0].ipAddress + ":" + Network.connections[0].port);
				Network.CloseConnection(Network.connections[0], true);
			}
			typeGame = 0;
			regimGUIClient = 0;
			regimGUIServer = 0;
			servers.Clear();
			players.Clear();
		}
	}

	private void seachServer(string ipServerSeaches)
	{
		Debug.Log(ipServerSeaches);
		bool flag = false;
		if (servers.Count > 0)
		{
			foreach (infoServer server in servers)
			{
				if (server.ipAddress.Equals(ipServerSeaches))
				{
					flag = true;
				}
			}
		}
		if (!flag)
		{
			infoServer item = default(infoServer);
			item.ipAddress = ipServerSeaches;
			servers.Add(item);
		}
	}

	private void isNotServer()
	{
		Debug.Log("Not servers");
		typeGame = 0;
	}

	private void settingsClientGUI()
	{
		name = GUI.TextField(new Rect((float)Screen.width * 0.5f, (float)Screen.height * 0.2f, (float)Screen.width * 0.3f, (float)Screen.height * 0.05f), name, nameStyle);
		if (GUI.Button(new Rect((float)Screen.width * 0.7f, (float)Screen.height * 0.6f, (float)Screen.width * 0.1f, (float)Screen.height * 0.05f), " Next"))
		{
			regimGUIClient = 1;
		}
		if (GUI.Button(new Rect((float)Screen.width * 0.5f, (float)Screen.height * 0.6f, (float)Screen.width * 0.1f, (float)Screen.height * 0.05f), " Back"))
		{
			typeGame = 0;
		}
	}

	private int hostDataComparison(HostData host1, HostData host2)
	{
		string text = string.Empty;
		string text2 = string.Empty;
		string[] ip = host1.ip;
		foreach (string text3 in ip)
		{
			text += text3;
		}
		string[] ip2 = host2.ip;
		foreach (string text4 in ip2)
		{
			text2 += text4;
		}
		return text.CompareTo(text2);
	}

	private int localServerComparison(LANBroadcastService.ReceivedMessage msg1, LANBroadcastService.ReceivedMessage msg2)
	{
		return msg1.ipAddress.CompareTo(msg2.ipAddress);
	}

	private void sortRoomByGameName(ref RoomInfo[] rooms)
	{
		for (int i = 1; i < rooms.Length; i++)
		{
			RoomInfo roomInfo = rooms[i];
			int num = i - 1;
			while (num >= 0 && string.Compare(rooms[num].name, roomInfo.name) > 0)
			{
				rooms[num + 1] = rooms[num];
				num--;
			}
			rooms[num + 1] = roomInfo;
		}
	}

	private void goGame()
	{
		Application.LoadLevel(PlayerPrefs.GetString("MapName"));
	}

	private void setFonLoading(string _mapName = "")
	{
		for (int i = 0; i < ((PlayerPrefs.GetInt("COOP", 0) == 1) ? masMapNameCOOP.Length : ((PlayerPrefs.GetInt("company", 0) != 1) ? masMapName.Length : masMapNameCompany.Length)); i++)
		{
			if ((PlayerPrefs.GetInt("COOP", 0) == 1) ? masMapNameCOOP[i].Equals(_mapName) : ((PlayerPrefs.GetInt("company", 0) != 1) ? masMapName[i].Equals(_mapName) : masMapNameCompany[i].Equals(_mapName)))
			{
				loadingToDraw = ((PlayerPrefs.GetInt("COOP", 0) == 1) ? masLoadingCOOP[i] : ((PlayerPrefs.GetInt("company", 0) != 1) ? masLoading[i] : masLoadingCompany[i]));
			}
		}
	}

	private void slideScroll()
	{
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
		{
			scrollEnabled = true;
			scrollStartTouch = Input.GetTouch(0).position;
		}
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved && scrollEnabled)
		{
			Vector2 position = Input.GetTouch(0).position;
			scrollPosition.y += position.y - scrollStartTouch.y;
			scrollStartTouch = position;
		}
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended && scrollEnabled)
		{
			scrollEnabled = false;
		}
	}

	private void DoWindow(int windowID)
	{
		if (typeConnect == 1)
		{
			RoomInfo[] roomList = PhotonNetwork.GetRoomList();
			if (roomList.Length <= 0)
			{
				return;
			}
			float num = (float)playersWindow.normal.background.width * koofScreen;
			rScrollFrame = new Rect(0f, 0f, (float)playersWindow.normal.background.width * koofScreen, (float)playersWindow.normal.background.height * koofScreen);
			Vector2 vector = new Vector2((float)openServer.normal.background.width * koofScreen, (float)(openServer.normal.background.height + 3) * koofScreen);
			Vector2 vector2 = new Vector2((float)openServer.normal.background.width * koofScreen * 0.97f, (float)(openServer.normal.background.height + 3) * koofScreen);
			Rect viewRect = new Rect(0f, 0f, rowSize.x, (float)filteredRoomList.Count * rowSize.y);
			if (viewRect.height > rScrollFrame.height)
			{
				rowSize = vector2;
			}
			else
			{
				rowSize = vector;
			}
			scrollPosition = GUI.BeginScrollView(rScrollFrame, scrollPosition, viewRect, false, false);
			Rect rect = new Rect(0f, 0f, rowSize.x, rowSize.y);
			float num2 = (windowWidth - num) * 0.5f;
			foreach (RoomInfo filteredRoom in filteredRoomList)
			{
				if (rect.yMax >= scrollPosition.y && rect.yMin <= scrollPosition.y + rScrollFrame.height)
				{
					if (GUI.Button(new Rect(num2, rect.y, rowSize.x, rowSize.y - 4f * koofScreen), string.Empty, (!filteredRoom.customProperties["pass"].Equals(string.Empty)) ? closeServer : openServer))
					{
						if (filteredRoom.playerCount == filteredRoom.maxPlayers)
						{
							timerShowServerFull = 5f;
							if (timerShowFaledJoin > 0f)
							{
								timerShowFaledJoin = 0f;
							}
							showServerFull = true;
						}
						else
						{
							if (filteredRoom.customProperties["pass"].Equals(string.Empty) || filteredRoom.customProperties["pass"].Equals(password))
							{
								PlayerPrefs.SetString("MaxKill", filteredRoom.customProperties["MaxKill"].ToString());
								Debug.Log("setMaxKil client local = " + filteredRoom.customProperties["MaxKill"].ToString());
								PlayerPrefs.SetString("MapName", (PlayerPrefs.GetInt("COOP", 0) == 1) ? masMapNameCOOP[(int)filteredRoom.customProperties["map"]] : ((PlayerPrefs.GetInt("company", 0) != 1) ? masMapName[(int)filteredRoom.customProperties["map"]] : masMapNameCompany[(int)filteredRoom.customProperties["map"]]));
								goMapName = ((PlayerPrefs.GetInt("COOP", 0) == 1) ? masMapNameCOOP[(int)filteredRoom.customProperties["map"]] : ((PlayerPrefs.GetInt("company", 0) != 1) ? masMapName[(int)filteredRoom.customProperties["map"]] : masMapNameCompany[(int)filteredRoom.customProperties["map"]]));
								showLoading = true;
								setFonLoading((PlayerPrefs.GetInt("COOP", 0) == 1) ? masMapNameCOOP[(int)filteredRoom.customProperties["map"]] : ((PlayerPrefs.GetInt("company", 0) != 1) ? masMapName[(int)filteredRoom.customProperties["map"]] : masMapNameCompany[(int)filteredRoom.customProperties["map"]]));
								PlayerPrefs.SetString("TypeGame", "client");
								PhotonNetwork.JoinRoom(filteredRoom.name);
							}
							else if (!filteredRoom.customProperties["pass"].Equals(password))
							{
								connectGame = filteredRoom;
								showPasswordEnterForm = true;
							}
							timerShowServerFull = 0f;
						}
						FlurryPluginWrapper.LogEnteringMap(typeConnect, (PlayerPrefs.GetInt("COOP", 0) == 1) ? masMapNameCOOP[(int)filteredRoom.customProperties["map"]] : ((PlayerPrefs.GetInt("company", 0) != 1) ? masMapName[(int)filteredRoom.customProperties["map"]] : masMapNameCompany[(int)filteredRoom.customProperties["map"]]));
					}
					string text = filteredRoom.name;
					if (text.Length == 36 && text.IndexOf("-") == 8 && filteredRoom.name.LastIndexOf("-") == 23)
					{
						text = "Random Server";
					}
					GUI.Label(new Rect(num2 + 25f * koofScreen, rect.y + 30f * koofScreen, 445f * koofScreen, 61f * koofScreen), text, openServerText);
					string text2 = ((PlayerPrefs.GetInt("COOP", 0) == 1) ? masMapNameCOOP[(int)filteredRoom.customProperties["map"]] : ((PlayerPrefs.GetInt("company", 0) != 1) ? masMapName[(int)filteredRoom.customProperties["map"]] : masMapNameCompany[(int)filteredRoom.customProperties["map"]]));
					if (Defs.mapNamesForUser.ContainsKey((PlayerPrefs.GetInt("COOP", 0) == 1) ? masMapNameCOOP[(int)filteredRoom.customProperties["map"]] : ((PlayerPrefs.GetInt("company", 0) != 1) ? masMapName[(int)filteredRoom.customProperties["map"]] : masMapNameCompany[(int)filteredRoom.customProperties["map"]])))
					{
						text2 = Defs.mapNamesForUser[(PlayerPrefs.GetInt("COOP", 0) == 1) ? masMapNameCOOP[(int)filteredRoom.customProperties["map"]] : ((PlayerPrefs.GetInt("company", 0) != 1) ? masMapName[(int)filteredRoom.customProperties["map"]] : masMapNameCompany[(int)filteredRoom.customProperties["map"]])];
					}
					GUI.Label(new Rect(num2 + 165f * koofScreen, rect.y + 96f * koofScreen, 402f * koofScreen, 55f * koofScreen), "Map: " + text2, openServerText);
					GUI.Label(new Rect(num2 - 20f * koofScreen, rect.y + 96f * koofScreen, 240f * koofScreen, 55f * koofScreen), filteredRoom.playerCount + "/" + filteredRoom.maxPlayers, openServerText);
				}
				rect.y += rowSize.y;
			}
			GUI.EndScrollView();
			return;
		}
		LANBroadcastService component = GetComponent<LANBroadcastService>();
		if (component.lstReceivedMessages.Count <= 0)
		{
			return;
		}
		LANBroadcastService.ReceivedMessage[] array = component.lstReceivedMessages.ToArray();
		Array.Sort(array, localServerComparison);
		float num3 = (float)playersWindow.normal.background.width * koofScreen;
		rScrollFrame = new Rect(0f, 0f, windowWidth, (float)playersWindow.normal.background.height * koofScreen);
		Vector2 vector3 = new Vector2((float)openServer.normal.background.width * koofScreen, (float)openServer.normal.background.height * koofScreen);
		Vector2 vector4 = new Vector2((float)openServer.normal.background.width * koofScreen * 0.97f, (float)openServer.normal.background.height * koofScreen);
		Rect viewRect2 = new Rect(0f, 0f, rowSize.x, (float)component.lstReceivedMessages.Count * rowSize.y);
		if (viewRect2.height > rScrollFrame.height)
		{
			rowSize = vector4;
		}
		else
		{
			rowSize = vector3;
		}
		scrollPosition = GUI.BeginScrollView(rScrollFrame, scrollPosition, viewRect2, false, false);
		Rect rect2 = new Rect(0f, 0f, rowSize.x, rowSize.y);
		float num4 = (windowWidth - num3) * 0.5f;
		LANBroadcastService.ReceivedMessage[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			LANBroadcastService.ReceivedMessage receivedMessage = array2[i];
			if (rect2.yMax >= scrollPosition.y && rect2.yMin <= scrollPosition.y + rScrollFrame.height)
			{
				if (GUI.Button(new Rect(num4, rect2.y, rowSize.x, rowSize.y), string.Empty, openServer))
				{
					if (receivedMessage.connectedPlayers == receivedMessage.playerLimit)
					{
						timerShowServerFull = 5f;
						if (timerShowFaledJoin > 0f)
						{
							timerShowFaledJoin = 0f;
						}
						showServerFull = true;
					}
					else
					{
						_weaponManager.ServerIp = receivedMessage.ipAddress;
						GlobalGameController.currentLevel = 7;
						PlayerPrefs.SetString("MaxKill", receivedMessage.comment);
						Debug.Log("setMaxKil client local = " + receivedMessage.comment);
						PlayerPrefs.SetString("MapName", receivedMessage.map);
						showLoading = true;
						setFonLoading(receivedMessage.map);
						Invoke("goGame", 0.1f);
						component.StopBroadCasting();
					}
					FlurryPluginWrapper.LogEnteringMap(typeConnect, receivedMessage.map);
				}
				GUI.Label(new Rect(num4 + 25f * koofScreen, rect2.y + 30f * koofScreen, 445f * koofScreen, 61f * koofScreen), receivedMessage.name, openServerText);
				string text3 = receivedMessage.map;
				if (Defs.mapNamesForUser.ContainsKey(receivedMessage.map))
				{
					text3 = Defs.mapNamesForUser[receivedMessage.map];
				}
				GUI.Label(new Rect(num4 + 165f * koofScreen, rect2.y + 96f * koofScreen, 402f * koofScreen, 55f * koofScreen), "Map: " + text3, openServerText);
				GUI.Label(new Rect(num4 - 20f * koofScreen, rect2.y + 96f * koofScreen, 240f * koofScreen, 55f * koofScreen), receivedMessage.connectedPlayers + "/" + receivedMessage.playerLimit, openServerText);
			}
			rect2.y += rowSize.y;
		}
		GUI.EndScrollView();
	}

	private void guiFilterForm()
	{
		GUI.DrawTexture(new Rect((float)Screen.width / 2f - (float)head_search.width / 2f * koofScreen, (float)Screen.height * 0.1f - (float)head_search.height / 2f * (float)Screen.height / 768f, (float)head_search.width * koofScreen, (float)head_search.height * koofScreen), head_search);
		nameStyle.fontSize = Mathf.RoundToInt(30f * koofScreen);
		Rect position = new Rect((float)Screen.width * 0.5f - (float)nameStyle.normal.background.width * 0.5f * koofScreen, (float)Screen.height * 0.23f - (float)nameStyle.normal.background.height * 0.5f * koofScreen, (float)nameStyle.normal.background.width * koofScreen, (float)nameStyle.normal.background.height * koofScreen);
		gameFilter = GUI.TextField(position, gameFilter, nameStyle);
		if (GUI.Button(new Rect(21f * koofScreen, (float)Screen.height - (21f + (float)sPasswordCancel.active.background.height) * (float)Screen.height / 768f, (float)sPasswordCancel.normal.background.width * koofScreen, (float)sPasswordCancel.normal.background.height * koofScreen), string.Empty, sPasswordCancel))
		{
			showFilterForm = false;
			gameFilter = string.Empty;
			updateFilteredRoomList(gameFilter);
		}
		if (GUI.Button(new Rect((float)Screen.width * 0.5f - (float)clearStyle.normal.background.width * koofScreen / 2f, (float)Screen.height * 0.3f, (float)clearStyle.normal.background.width * koofScreen, (float)clearStyle.normal.background.height * koofScreen), string.Empty, clearStyle))
		{
			gameFilter = string.Empty;
		}
		if (GUI.Button(new Rect((float)Screen.width - (21f + (float)sSearch.normal.background.width) * (float)Screen.height / 768f, (float)Screen.height - (21f + (float)sSearch.active.background.height) * (float)Screen.height / 768f, (float)sSearch.normal.background.width * koofScreen, (float)sSearch.normal.background.height * koofScreen), string.Empty, sSearch))
		{
			showFilterForm = false;
			updateFilteredRoomList(gameFilter);
		}
	}

	private void clientTableGUI()
	{
		if (showPasswordEnterForm)
		{
			guiPasswordEnter();
		}
		else if (showFilterForm)
		{
			guiFilterForm();
		}
		else
		{
			GUI.Window(0, new Rect((float)Screen.width * 0.5f - windowWidth * 0.5f, (float)Screen.height * 0.5f - (float)playersWindow.normal.background.height * 0.5f * koofScreen, windowWidth, (float)playersWindow.normal.background.height * koofScreen), DoWindow, string.Empty);
		}
	}

	private void connectTo(string _serverIP)
	{
		bool useNat = !Network.HavePublicAddress();
		Network.useNat = useNat;
		Network.Connect(_serverIP, 25002);
	}

	private void OnConnectedToServer()
	{
		regimGUIClient = 3;
		Debug.Log("OnConnectedToServer");
		if (PlayerPrefs.GetInt("MultyPlayer") == 1)
		{
			base.GetComponent<NetworkView>().RPC("addPlayer", RPCMode.AllBuffered, name, Network.player.ipAddress);
		}
		isConSuccess = true;
	}

	private void OnDisconnectedFromServer(NetworkDisconnection info)
	{
		Debug.Log("OnDisconnectedFromServer");
		regimGUIClient = 0;
		regimGUIServer = 0;
		isSetMap = false;
		servers.Clear();
		players.Clear();
	}

	private void OnPlayerDisconnected(NetworkPlayer player)
	{
		Debug.Log("Clean up after player " + player.ipAddress);
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
		base.GetComponent<NetworkView>().RPC("delPlayer", RPCMode.All, player.ipAddress);
	}

	private void OnFailedToConnectToMasterServer(NetworkConnectionError info)
	{
		Debug.Log("Could not connect to master server: " + info);
		typeConnect = 0;
		typeGame = 0;
		regimGUIClient = 0;
		regimGUIServer = 0;
		isSetMap = false;
	}

	private void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if (msEvent == MasterServerEvent.RegistrationSucceeded)
		{
			Debug.Log("Server registered");
		}
		if (msEvent == MasterServerEvent.RegistrationFailedNoServer)
		{
			Debug.Log("Server RegistrationFailedNoServer");
			typeConnect = 0;
			typeGame = 0;
			regimGUIClient = 0;
			regimGUIServer = 0;
			isSetMap = false;
		}
		if (msEvent == MasterServerEvent.RegistrationFailedGameName)
		{
			Debug.Log("Server RegistrationFailedGameName");
		}
	}

	public void updateFilteredRoomList(string gFilter)
	{
		filteredRoomList.Clear();
		RoomInfo[] roomList = PhotonNetwork.GetRoomList();
		for (int i = 0; i < roomList.Length; i++)
		{
			if (roomList[i].name.StartsWith(gFilter, true, null))
			{
				filteredRoomList.Add(roomList[i]);
			}
		}
	}

	private void OnPhotonRandomJoinFailed()
	{
		startingGame = false;
		Debug.Log("OnPhotonJoinRoomFailed");
		PlayerPrefs.SetString("TypeGame", "server");
		string[] propsToListInLobby = new string[3] { "map", "MaxKill", "pass" };
		if (!isRandomSelectMap)
		{
			selectMapIndex = UnityEngine.Random.Range(0, ((PlayerPrefs.GetInt("COOP", 0) == 1) ? masMapNameCOOP.Length : ((PlayerPrefs.GetInt("company", 0) != 1) ? masMapName.Length : masMapNameCompany.Length)) - 1);
		}
		goMapName = ((PlayerPrefs.GetInt("COOP", 0) == 1) ? masMapNameCOOP[selectMapIndex] : ((PlayerPrefs.GetInt("company", 0) != 1) ? masMapName[selectMapIndex] : masMapNameCompany[selectMapIndex]));
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable.Add("map", selectMapIndex);
		hashtable.Add("MaxKill", (PlayerPrefs.GetInt("company", 0) != 1) ? 15 : 20);
		hashtable.Add("pass", string.Empty);
		ExitGames.Client.Photon.Hashtable customRoomProperties = hashtable;
		PlayerPrefs.SetString("MapName", (PlayerPrefs.GetInt("COOP", 0) == 1) ? masMapNameCOOP[selectMapIndex] : ((PlayerPrefs.GetInt("company", 0) != 1) ? masMapName[selectMapIndex] : masMapNameCompany[selectMapIndex]));
		PlayerPrefs.SetString("MaxKill", (PlayerPrefs.GetInt("company", 0) != 1) ? "15" : "20");
		showLoading = true;
		setFonLoading((PlayerPrefs.GetInt("COOP", 0) == 1) ? masMapNameCOOP[selectMapIndex] : ((PlayerPrefs.GetInt("company", 0) != 1) ? masMapName[selectMapIndex] : masMapNameCompany[selectMapIndex]));
		PhotonNetwork.CreateRoom(null, true, true, (PlayerPrefs.GetInt("COOP", 0) == 1) ? 4 : ((PlayerPrefs.GetInt("company", 0) != 1) ? 10 : 8), customRoomProperties, propsToListInLobby);
	}

	private void OnPhotonJoinRoomFailed()
	{
		Debug.Log("OnPhotonJoinRoomFailed");
		showLoading = false;
		timerShowFaledJoin = 5f;
		if (timerShowServerFull > 0f)
		{
			timerShowServerFull = 0f;
		}
	}

	private void OnJoinedRoom()
	{
		startingGame = false;
		selectMapIndex = int.Parse(PhotonNetwork.room.customProperties["map"].ToString());
		PlayerPrefs.SetString("MapName", (PlayerPrefs.GetInt("COOP", 0) == 1) ? masMapNameCOOP[selectMapIndex] : ((PlayerPrefs.GetInt("company", 0) != 1) ? masMapName[selectMapIndex] : masMapNameCompany[selectMapIndex]));
		Debug.Log("OnJoinedRoom" + PhotonNetwork.room);
		PlayerPrefs.SetString("RoomName", PhotonNetwork.room.name);
		goMapName = ((PlayerPrefs.GetInt("COOP", 0) == 1) ? masMapNameCOOP[(int)PhotonNetwork.room.customProperties["map"]] : ((PlayerPrefs.GetInt("company", 0) != 1) ? masMapName[(int)PhotonNetwork.room.customProperties["map"]] : masMapNameCompany[(int)PhotonNetwork.room.customProperties["map"]]));
		PhotonNetwork.isMessageQueueRunning = false;
		StartCoroutine(MoveToGameScene());
	}

	private void OnCreatedRoom()
	{
		Debug.Log("OnCreatedRoom");
		PlayerPrefs.SetString("RoomName", PhotonNetwork.room.name);
		StartCoroutine(MoveToGameScene());
	}

	private void OnDisconnectedFromPhoton()
	{
		if (pressBack)
		{
			pressBack = false;
		}
		else
		{
			timerShowNotConnecting = 3f;
		}
		typeConnect = 0;
		typeGame = 0;
		regimGUIClient = 0;
		regimGUIServer = 0;
		isSetMap = false;
		connectingFoton = false;
		showLoading = false;
		Debug.Log("Disconnected from Photon.");
	}

	private void OnFailedToConnectToPhoton(object parameters)
	{
		timerShowNotConnecting = 3f;
		connectingFoton = false;
		Debug.Log("OnFailedToConnectToPhoton. StatusCode: " + parameters);
	}

	private IEnumerator MoveToGameScene()
	{
		Debug.Log("MoveToGameScene");
		GlobalGameController.countKillsBlue = 0;
		GlobalGameController.countKillsRed = 0;
		while (PhotonNetwork.room == null)
		{
			yield return 0;
		}
		PhotonNetwork.isMessageQueueRunning = false;
		for (int k = 0; k < masMap.Length; k++)
		{
			masMap[k] = null;
		}
		for (int j = 0; j < masMapCOOP.Length; j++)
		{
			masMapCOOP[j] = null;
		}
		for (int i = 0; i < masMapCompany.Length; i++)
		{
			masMapCompany[i] = null;
		}
		head_local = null;
		head_maps = null;
		head_pass = null;
		head_password = null;
		head_players = null;
		head_profile = null;
		head_search = null;
		head_serv_name = null;
		head_worldwide = null;
		yield return Resources.UnloadUnusedAssets();
		LoadConnectScene.textureToShow = ((PlayerPrefs.GetInt("COOP", 0) == 1) ? masLoadingCOOP[selectMapIndex] : ((PlayerPrefs.GetInt("company", 0) != 1) ? masLoading[selectMapIndex] : masLoadingCompany[selectMapIndex]));
		LoadConnectScene.sceneToLoad = goMapName;
		yield return Application.LoadLevelAsync("PromScene");
	}

	public void OnPhotonPlayerConnected(PhotonPlayer player)
	{
		Debug.Log("OnPhotonPlayerConnected: " + player);
	}

	public void OnPhotonPlayerDisconnected(PhotonPlayer player)
	{
		Debug.Log("OnPlayerDisconneced: " + player);
	}

	public void OnReceivedRoomListUpdate()
	{
		updateFilteredRoomList(gameFilter);
		if (firstUpdate)
		{
			firstUpdate = false;
			setRationg(selectMapIndex);
		}
	}

	public void OnJoinedLobby()
	{
		firstUpdate = true;
		Debug.Log("OnConnectedToPhoton");
		typeConnect = 1;
		PlayerPrefs.SetString("TypeGame", "client");
		typeGame = 3;
		connectingFoton = false;
		setRationg(selectMapIndex);
	}

	public void OnConnectedToPhoton()
	{
	}

	public void OnFailedToConnectToPhoton()
	{
		Debug.Log("OnFailedToConnectToPhoton");
	}

	public void OnPhotonInstantiate(PhotonMessageInfo info)
	{
		Debug.Log("OnPhotonInstantiate" + info.sender);
	}

	private void _initializeWorldwide()
	{
		if (!isFirstFrame)
		{
			PlayerPrefs.SetString("TypeConnect", "inet");
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
			connectingFoton = true;
			showPasswordEnterForm = false;
			showFilterForm = false;
		}
	}
}
