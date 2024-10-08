using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;

public static class PhotonNetwork
{
	public const string versionPUN = "1.24";

	public const string serverSettingsAssetFile = "PhotonServerSettings";

	public const string serverSettingsAssetPath = "Assets/Photon Unity Networking/Resources/PhotonServerSettings.asset";

	internal static readonly PhotonHandler photonMono;

	internal static readonly NetworkingPeer networkingPeer;

	public static readonly int MAX_VIEW_IDS;

	public static ServerSettings PhotonServerSettings;

	public static float precisionForVectorSynchronization;

	public static float precisionForQuaternionSynchronization;

	public static float precisionForFloatSynchronization;

	public static PhotonLogLevel logLevel;

	public static bool UsePrefabCache;

	public static Dictionary<string, GameObject> PrefabCache;

	private static bool isOfflineMode;

	private static bool offlineMode_inRoom;

	public static HashSet<GameObject> SendMonoMessageTargets;

	private static bool _mAutomaticallySyncScene;

	private static bool m_autoCleanUpPlayerObjects;

	private static bool autoJoinLobbyField;

	private static int sendInterval;

	private static int sendIntervalOnSerialize;

	private static bool m_isMessageQueueRunning;

	internal static int lastUsedViewSubId;

	internal static int lastUsedViewSubIdStatic;

	internal static List<int> manuallyAllocatedViewIds;

	public static string ServerAddress
	{
		get
		{
			return (networkingPeer == null) ? "<not connected>" : networkingPeer.ServerAddress;
		}
	}

	public static bool connected
	{
		get
		{
			if (offlineMode)
			{
				return true;
			}
			return connectionState == ConnectionState.Connected;
		}
	}

	public static ConnectionState connectionState
	{
		get
		{
			if (offlineMode)
			{
				return ConnectionState.Connected;
			}
			if (networkingPeer == null)
			{
				return ConnectionState.Disconnected;
			}
			switch (networkingPeer.PeerState)
			{
			case PeerStateValue.Disconnected:
				return ConnectionState.Disconnected;
			case PeerStateValue.Connecting:
				return ConnectionState.Connecting;
			case PeerStateValue.Connected:
				return ConnectionState.Connected;
			case PeerStateValue.Disconnecting:
				return ConnectionState.Disconnecting;
			case PeerStateValue.InitializingApplication:
				return ConnectionState.InitializingApplication;
			default:
				return ConnectionState.Disconnected;
			}
		}
	}

	public static PeerState connectionStateDetailed
	{
		get
		{
			if (offlineMode)
			{
				return PeerState.Connected;
			}
			if (networkingPeer == null)
			{
				return PeerState.Disconnected;
			}
			return networkingPeer.State;
		}
	}

	public static AuthenticationValues AuthValues
	{
		get
		{
			return (networkingPeer == null) ? null : networkingPeer.AuthValues;
		}
		set
		{
			if (networkingPeer != null)
			{
				networkingPeer.AuthValues = value;
			}
		}
	}

	public static Room room
	{
		get
		{
			if (isOfflineMode)
			{
				if (offlineMode_inRoom)
				{
					return new Room("OfflineRoom", new Hashtable());
				}
				return null;
			}
			return networkingPeer.mCurrentGame;
		}
	}

	public static PhotonPlayer player
	{
		get
		{
			if (networkingPeer == null)
			{
				return null;
			}
			return networkingPeer.mLocalActor;
		}
	}

	public static PhotonPlayer masterClient
	{
		get
		{
			if (networkingPeer == null)
			{
				return null;
			}
			return networkingPeer.mMasterClient;
		}
	}

	public static string playerName
	{
		get
		{
			return networkingPeer.PlayerName;
		}
		set
		{
			networkingPeer.PlayerName = value;
		}
	}

	public static PhotonPlayer[] playerList
	{
		get
		{
			if (networkingPeer == null)
			{
				return new PhotonPlayer[0];
			}
			return networkingPeer.mPlayerListCopy;
		}
	}

	public static PhotonPlayer[] otherPlayers
	{
		get
		{
			if (networkingPeer == null)
			{
				return new PhotonPlayer[0];
			}
			return networkingPeer.mOtherPlayerListCopy;
		}
	}

	public static List<FriendInfo> Friends { get; set; }

	public static int FriendsListAge
	{
		get
		{
			return (networkingPeer != null) ? networkingPeer.FriendsListAge : 0;
		}
	}

	public static bool offlineMode
	{
		get
		{
			return isOfflineMode;
		}
		set
		{
			if (value == isOfflineMode)
			{
				return;
			}
			if (value && connected)
			{
				Debug.LogError("Can't start OFFLINE mode while connected!");
				return;
			}
			networkingPeer.Disconnect();
			isOfflineMode = value;
			if (isOfflineMode)
			{
				NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnConnectedToPhoton);
				networkingPeer.ChangeLocalID(1);
				networkingPeer.mMasterClient = player;
			}
			else
			{
				networkingPeer.ChangeLocalID(-1);
				networkingPeer.mMasterClient = null;
			}
		}
	}

	[Obsolete("Used for compatibility with Unity networking only.")]
	public static int maxConnections
	{
		get
		{
			if (room == null)
			{
				return 0;
			}
			return room.maxPlayers;
		}
		set
		{
			room.maxPlayers = value;
		}
	}

	public static bool automaticallySyncScene
	{
		get
		{
			return _mAutomaticallySyncScene;
		}
		set
		{
			_mAutomaticallySyncScene = value;
			if (_mAutomaticallySyncScene && room != null)
			{
				networkingPeer.AutomaticallySyncScene();
			}
		}
	}

	public static bool autoCleanUpPlayerObjects
	{
		get
		{
			return m_autoCleanUpPlayerObjects;
		}
		set
		{
			if (room != null)
			{
				Debug.LogError("Setting autoCleanUpPlayerObjects while in a room is not supported.");
			}
			m_autoCleanUpPlayerObjects = value;
		}
	}

	public static bool autoJoinLobby
	{
		get
		{
			return autoJoinLobbyField;
		}
		set
		{
			autoJoinLobbyField = value;
		}
	}

	public static bool insideLobby
	{
		get
		{
			return networkingPeer.insideLobby;
		}
	}

	public static int sendRate
	{
		get
		{
			return 1000 / sendInterval;
		}
		set
		{
			sendInterval = 1000 / value;
			if (photonMono != null)
			{
				photonMono.updateInterval = sendInterval;
			}
			if (value < sendRateOnSerialize)
			{
				sendRateOnSerialize = value;
			}
		}
	}

	public static int sendRateOnSerialize
	{
		get
		{
			return 1000 / sendIntervalOnSerialize;
		}
		set
		{
			if (value > sendRate)
			{
				Debug.LogError("Error, can not set the OnSerialize SendRate more often then the overall SendRate");
				value = sendRate;
			}
			sendIntervalOnSerialize = 1000 / value;
			if (photonMono != null)
			{
				photonMono.updateIntervalOnSerialize = sendIntervalOnSerialize;
			}
		}
	}

	public static bool isMessageQueueRunning
	{
		get
		{
			return m_isMessageQueueRunning;
		}
		set
		{
			if (value)
			{
				PhotonHandler.StartFallbackSendAckThread();
			}
			networkingPeer.IsSendingOnlyAcks = !value;
			m_isMessageQueueRunning = value;
		}
	}

	public static int unreliableCommandsLimit
	{
		get
		{
			return networkingPeer.LimitOfUnreliableCommands;
		}
		set
		{
			networkingPeer.LimitOfUnreliableCommands = value;
		}
	}

	public static double time
	{
		get
		{
			if (offlineMode)
			{
				return Time.time;
			}
			return (double)(uint)networkingPeer.ServerTimeInMilliSeconds / 1000.0;
		}
	}

	public static bool isMasterClient
	{
		get
		{
			if (offlineMode)
			{
				return true;
			}
			return networkingPeer.mMasterClient == networkingPeer.mLocalActor;
		}
	}

	public static bool isNonMasterClientInRoom
	{
		get
		{
			return !isMasterClient && room != null;
		}
	}

	public static int countOfPlayersOnMaster
	{
		get
		{
			return networkingPeer.mPlayersOnMasterCount;
		}
	}

	public static int countOfPlayersInRooms
	{
		get
		{
			return networkingPeer.mPlayersInRoomsCount;
		}
	}

	public static int countOfPlayers
	{
		get
		{
			return networkingPeer.mPlayersInRoomsCount + networkingPeer.mPlayersOnMasterCount;
		}
	}

	public static int countOfRooms
	{
		get
		{
			if (insideLobby)
			{
				return GetRoomList().Length;
			}
			return networkingPeer.mGameCount;
		}
	}

	public static bool NetworkStatisticsEnabled
	{
		get
		{
			return networkingPeer.TrafficStatsEnabled;
		}
		set
		{
			networkingPeer.TrafficStatsEnabled = value;
		}
	}

	public static int ResentReliableCommands
	{
		get
		{
			return networkingPeer.ResentReliableCommands;
		}
	}

	static PhotonNetwork()
	{
		MAX_VIEW_IDS = 1000;
		PhotonServerSettings = (ServerSettings)Resources.Load("PhotonServerSettings", typeof(ServerSettings));
		precisionForVectorSynchronization = 9.9E-05f;
		precisionForQuaternionSynchronization = 1f;
		precisionForFloatSynchronization = 0.01f;
		logLevel = PhotonLogLevel.ErrorsOnly;
		UsePrefabCache = true;
		PrefabCache = new Dictionary<string, GameObject>();
		isOfflineMode = false;
		offlineMode_inRoom = false;
		_mAutomaticallySyncScene = false;
		m_autoCleanUpPlayerObjects = true;
		autoJoinLobbyField = true;
		sendInterval = 50;
		sendIntervalOnSerialize = 100;
		m_isMessageQueueRunning = true;
		lastUsedViewSubId = 0;
		lastUsedViewSubIdStatic = 0;
		manuallyAllocatedViewIds = new List<int>();
		Application.runInBackground = true;
		GameObject gameObject = new GameObject();
		photonMono = gameObject.AddComponent<PhotonHandler>();
		gameObject.AddComponent<PingCloudRegions>();
		gameObject.name = "PhotonMono";
		gameObject.hideFlags = HideFlags.HideInHierarchy;
		networkingPeer = new NetworkingPeer(photonMono, string.Empty, ConnectionProtocol.Udp);
		networkingPeer.LimitOfUnreliableCommands = 40;
		CustomTypes.Register();
	}

	public static bool SetMasterClient(PhotonPlayer player)
	{
		if (!VerifyCanUseNetwork() || !isMasterClient)
		{
			return false;
		}
		return networkingPeer.SetMasterClient(player.ID, true);
	}

	public static void NetworkStatisticsReset()
	{
		networkingPeer.TrafficStatsReset();
	}

	public static string NetworkStatisticsToString()
	{
		if (networkingPeer == null || offlineMode)
		{
			return "Offline or in OfflineMode. No VitalStats available.";
		}
		return networkingPeer.VitalStatsToString(false);
	}

	public static void InternalCleanPhotonMonoFromSceneIfStuck()
	{
		PhotonHandler[] array = UnityEngine.Object.FindObjectsOfType(typeof(PhotonHandler)) as PhotonHandler[];
		if (array == null || array.Length <= 0)
		{
			return;
		}
		Debug.Log("Cleaning up hidden PhotonHandler instances in scene. Please save it. This is not an issue.");
		PhotonHandler[] array2 = array;
		foreach (PhotonHandler photonHandler in array2)
		{
			photonHandler.gameObject.hideFlags = HideFlags.None;
			if (photonHandler.gameObject != null && photonHandler.gameObject.name == "PhotonMono")
			{
				UnityEngine.Object.DestroyImmediate(photonHandler.gameObject);
			}
			UnityEngine.Object.DestroyImmediate(photonHandler);
		}
	}

	public static void ConnectUsingSettings(string gameVersion)
	{
		if (PhotonServerSettings == null)
		{
			Debug.LogError("Can't connect: Loading settings failed. ServerSettings asset must be in any 'Resources' folder as: PhotonServerSettings");
		}
		else if (PhotonServerSettings.HostType == ServerSettings.HostingOption.OfflineMode)
		{
			offlineMode = true;
		}
		else
		{
			Connect(PhotonServerSettings.ServerAddress, PhotonServerSettings.ServerPort, PhotonServerSettings.AppID, gameVersion);
		}
	}

	public static void ConnectToBestCloudServer(string gameVersion)
	{
		if (PhotonServerSettings == null)
		{
			Debug.LogError("Can't connect: Loading settings failed. ServerSettings asset must be in any 'Resources' folder as: PhotonServerSettings");
		}
		else if (PhotonServerSettings.HostType == ServerSettings.HostingOption.OfflineMode)
		{
			offlineMode = true;
		}
		else
		{
			PingCloudRegions.ConnectToBestRegion(gameVersion);
		}
	}

	public static void OverrideBestCloudServer(CloudServerRegion region)
	{
		PingCloudRegions.OverrideRegion(region);
	}

	public static void RefreshCloudServerRating()
	{
		PingCloudRegions.RefreshCloudServerRating();
	}

	public static void Connect(string serverAddress, int port, string appID, string gameVersion)
	{
		if (serverAddress.Length <= 2)
		{
			Debug.LogError("Aborted Connect: invalid serverAddress: " + serverAddress);
			return;
		}
		if (networkingPeer.PeerState != 0)
		{
			Debug.LogWarning("Connect() only works when disconnected. Current state: " + networkingPeer.PeerState);
			return;
		}
		if (offlineMode)
		{
			offlineMode = false;
			Debug.LogWarning("Shut down offline mode due to a connect attempt");
		}
		if (!isMessageQueueRunning)
		{
			isMessageQueueRunning = true;
			Debug.LogWarning("Forced enabling of isMessageQueueRunning because of a Connect()");
		}
		networkingPeer.mAppVersion = gameVersion + "_1.24";
		networkingPeer.MasterServerAddress = serverAddress + ":" + port;
		networkingPeer.Connect(networkingPeer.MasterServerAddress, appID);
	}

	public static void Disconnect()
	{
		if (offlineMode)
		{
			offlineMode = false;
			networkingPeer.State = PeerState.Disconnecting;
			networkingPeer.OnStatusChanged(StatusCode.Disconnect);
		}
		else if (networkingPeer != null)
		{
			networkingPeer.Disconnect();
		}
	}

	[Obsolete("Used for compatibility with Unity networking only. Encryption is automatically initialized while connecting.")]
	public static void InitializeSecurity()
	{
	}

	public static bool FindFriends(string[] friendsToFind)
	{
		if (networkingPeer == null || isOfflineMode)
		{
			return false;
		}
		return networkingPeer.OpFindFriends(friendsToFind);
	}

	public static void CreateRoom(string roomName)
	{
		CreateRoom(roomName, true, true, 0, null, null);
	}

	public static void CreateRoom(string roomName, bool isVisible, bool isOpen, int maxPlayers)
	{
		CreateRoom(roomName, isVisible, isOpen, maxPlayers, null, null);
	}

	public static void CreateRoom(string roomName, bool isVisible, bool isOpen, int maxPlayers, Hashtable customRoomProperties, string[] propsToListInLobby)
	{
		if (connectionStateDetailed == PeerState.Joining || connectionStateDetailed == PeerState.Joined || connectionStateDetailed == PeerState.ConnectedToGameserver)
		{
			Debug.LogError("CreateRoom aborted: You can only create a room while not currently connected/connecting to a room.");
			return;
		}
		if (room != null)
		{
			Debug.LogError("CreateRoom aborted: You are already in a room!");
			return;
		}
		if (offlineMode)
		{
			offlineMode_inRoom = true;
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnCreatedRoom);
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnJoinedRoom);
			return;
		}
		if (maxPlayers > 255)
		{
			Debug.LogError("Error: CreateRoom called with " + maxPlayers + " maxplayers. This has been reverted to the max of 255 players because internally a 'byte' is used.");
			maxPlayers = 255;
		}
		networkingPeer.OpCreateGame(roomName, isVisible, isOpen, (byte)maxPlayers, autoCleanUpPlayerObjects, customRoomProperties, propsToListInLobby);
	}

	public static void JoinRoom(string roomName)
	{
		JoinRoom(roomName, false);
	}

	public static void JoinRoom(string roomName, bool createIfNotExists)
	{
		if (connectionStateDetailed == PeerState.Joining || connectionStateDetailed == PeerState.Joined || connectionStateDetailed == PeerState.ConnectedToGameserver)
		{
			Debug.LogError("JoinRoom aborted: You can only join a room while not currently connected/connecting to a room.");
		}
		else if (room != null)
		{
			Debug.LogError("JoinRoom aborted: You are already in a room!");
		}
		else if (roomName == string.Empty)
		{
			Debug.LogError("JoinRoom aborted: You must specifiy a room name!");
		}
		else if (offlineMode)
		{
			offlineMode_inRoom = true;
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnJoinedRoom);
		}
		else
		{
			networkingPeer.OpJoin(roomName, createIfNotExists);
		}
	}

	public static void JoinRandomRoom()
	{
		JoinRandomRoom(null, 0);
	}

	public static void JoinRandomRoom(Hashtable expectedCustomRoomProperties, byte expectedMaxPlayers)
	{
		JoinRandomRoom(expectedCustomRoomProperties, expectedMaxPlayers, MatchmakingMode.FillRoom);
	}

	public static void JoinRandomRoom(Hashtable expectedCustomRoomProperties, byte expectedMaxPlayers, MatchmakingMode matchingType)
	{
		if (connectionStateDetailed == PeerState.Joining || connectionStateDetailed == PeerState.Joined || connectionStateDetailed == PeerState.ConnectedToGameserver)
		{
			Debug.LogError("JoinRandomRoom aborted: You can only join a room while not currently connected/connecting to a room.");
			return;
		}
		if (room != null)
		{
			Debug.LogError("JoinRandomRoom aborted: You are already in a room!");
			return;
		}
		if (offlineMode)
		{
			offlineMode_inRoom = true;
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnJoinedRoom);
			return;
		}
		Hashtable hashtable = new Hashtable();
		hashtable.MergeStringKeys(expectedCustomRoomProperties);
		if (expectedMaxPlayers > 0)
		{
			hashtable[byte.MaxValue] = expectedMaxPlayers;
		}
		networkingPeer.OpJoinRandomRoom(hashtable, 0, null, matchingType);
	}

	public static void LeaveRoom()
	{
		if (!offlineMode && connectionStateDetailed != PeerState.Joined)
		{
			Debug.LogError("PhotonNetwork: Error, you cannot leave a room if you're not in a room!(1)");
		}
		else if (room == null)
		{
			Debug.LogError("PhotonNetwork: Error, you cannot leave a room if you're not in a room!(2)");
		}
		else if (offlineMode)
		{
			offlineMode_inRoom = false;
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnLeftRoom);
		}
		else
		{
			networkingPeer.OpLeave();
		}
	}

	public static RoomInfo[] GetRoomList()
	{
		if (offlineMode)
		{
			return new RoomInfo[0];
		}
		if (networkingPeer == null)
		{
			return new RoomInfo[0];
		}
		return networkingPeer.mGameListCopy;
	}

	public static void SetPlayerCustomProperties(Hashtable customProperties)
	{
		if (customProperties == null)
		{
			customProperties = new Hashtable();
			foreach (object key in player.customProperties.Keys)
			{
				customProperties[(string)key] = null;
			}
		}
		if (room != null && room.isLocalClientInside)
		{
			player.SetCustomProperties(customProperties);
		}
		else
		{
			player.InternalCacheProperties(customProperties);
		}
	}

	public static int AllocateViewID()
	{
		int num = AllocateViewID(player.ID);
		manuallyAllocatedViewIds.Add(num);
		return num;
	}

	public static void UnAllocateViewID(int viewID)
	{
		manuallyAllocatedViewIds.Remove(viewID);
		if (networkingPeer.photonViewList.ContainsKey(viewID))
		{
			Debug.LogWarning(string.Format("Unallocated manually used viewID: {0} but found it used still in a PhotonView: {1}", viewID, networkingPeer.photonViewList[viewID]));
		}
	}

	private static int AllocateViewID(int ownerId)
	{
		if (ownerId == 0)
		{
			int num = lastUsedViewSubIdStatic;
			int num2 = ownerId * MAX_VIEW_IDS;
			for (int i = 1; i < MAX_VIEW_IDS; i++)
			{
				num = (num + 1) % MAX_VIEW_IDS;
				if (num != 0)
				{
					int num3 = num + num2;
					if (!networkingPeer.photonViewList.ContainsKey(num3))
					{
						lastUsedViewSubIdStatic = num;
						return num3;
					}
				}
			}
			throw new Exception(string.Format("AllocateViewID() failed. Room (user {0}) is out of subIds, as all room viewIDs are used.", ownerId));
		}
		int num4 = lastUsedViewSubId;
		int num5 = ownerId * MAX_VIEW_IDS;
		for (int j = 1; j < MAX_VIEW_IDS; j++)
		{
			num4 = (num4 + 1) % MAX_VIEW_IDS;
			if (num4 != 0)
			{
				int num6 = num4 + num5;
				if (!networkingPeer.photonViewList.ContainsKey(num6) && !manuallyAllocatedViewIds.Contains(num6))
				{
					lastUsedViewSubId = num4;
					return num6;
				}
			}
		}
		throw new Exception(string.Format("AllocateViewID() failed. User {0} is out of subIds, as all viewIDs are used.", ownerId));
	}

	private static int[] AllocateSceneViewIDs(int countOfNewViews)
	{
		int[] array = new int[countOfNewViews];
		for (int i = 0; i < countOfNewViews; i++)
		{
			array[i] = AllocateViewID(0);
		}
		return array;
	}

	public static GameObject Instantiate(string prefabName, Vector3 position, Quaternion rotation, int group)
	{
		return Instantiate(prefabName, position, rotation, group, null);
	}

	public static GameObject Instantiate(string prefabName, Vector3 position, Quaternion rotation, int group, object[] data)
	{
		if (!VerifyCanUseNetwork())
		{
			Debug.LogError("PhotonNetwork error: Could not Instantiate the prefab [" + prefabName + "] as the game is not connected.");
			return null;
		}
		GameObject value;
		if (!UsePrefabCache || !PrefabCache.TryGetValue(prefabName, out value))
		{
			value = (GameObject)Resources.Load(prefabName, typeof(GameObject));
			if (UsePrefabCache)
			{
				PrefabCache.Add(prefabName, value);
			}
		}
		if (value == null)
		{
			Debug.LogError("PhotonNetwork error: Could not Instantiate the prefab [" + prefabName + "]. Please verify you have this gameobject in a Resources folder (and not in a subfolder)");
			return null;
		}
		if (value.GetComponent<PhotonView>() == null)
		{
			Debug.LogError("PhotonNetwork error: Could not Instantiate the prefab [" + prefabName + "] as it has no PhotonView attached to the root.");
			return null;
		}
		Component[] componentsInChildren = value.GetComponentsInChildren<PhotonView>(true);
		int[] array = new int[componentsInChildren.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = AllocateViewID(player.ID);
		}
		Hashtable evData = networkingPeer.SendInstantiate(prefabName, position, rotation, group, array, data, false);
		return networkingPeer.DoInstantiate(evData, networkingPeer.mLocalActor, value);
	}

	public static GameObject InstantiateSceneObject(string prefabName, Vector3 position, Quaternion rotation, int group, object[] data)
	{
		if (!VerifyCanUseNetwork())
		{
			return null;
		}
		if (!isMasterClient)
		{
			Debug.LogError("PhotonNetwork error [InstantiateSceneObject]: Only the master client can Instantiate scene objects");
			return null;
		}
		GameObject value;
		if (!UsePrefabCache || !PrefabCache.TryGetValue(prefabName, out value))
		{
			value = (GameObject)Resources.Load(prefabName, typeof(GameObject));
			if (UsePrefabCache)
			{
				PrefabCache.Add(prefabName, value);
			}
		}
		if (value == null)
		{
			Debug.LogError("PhotonNetwork error [InstantiateSceneObject]: Could not Instantiate the prefab [" + prefabName + "]. Please verify you have this gameobject in a Resources folder (and not in a subfolder)");
			return null;
		}
		if (value.GetComponent<PhotonView>() == null)
		{
			Debug.LogError("PhotonNetwork error [InstantiateSceneObject]: Could not Instantiate the prefab [" + prefabName + "] as it has no PhotonView attached to the root.");
			return null;
		}
		Component[] photonViewsInChildren = value.GetPhotonViewsInChildren();
		int[] array = AllocateSceneViewIDs(photonViewsInChildren.Length);
		if (array == null)
		{
			Debug.LogError("PhotonNetwork error [InstantiateSceneObject]: Could not Instantiate the prefab [" + prefabName + "] as no ViewIDs are free to use. Max is: " + MAX_VIEW_IDS);
			return null;
		}
		Hashtable evData = networkingPeer.SendInstantiate(prefabName, position, rotation, group, array, data, true);
		return networkingPeer.DoInstantiate(evData, networkingPeer.mLocalActor, value);
	}

	public static int GetPing()
	{
		return networkingPeer.RoundTripTime;
	}

	public static void FetchServerTimestamp()
	{
		if (networkingPeer != null)
		{
			networkingPeer.FetchServerTimestamp();
		}
	}

	public static void SendOutgoingCommands()
	{
		if (VerifyCanUseNetwork())
		{
			while (networkingPeer.SendOutgoingCommands())
			{
			}
		}
	}

	public static void CloseConnection(PhotonPlayer kickPlayer)
	{
		if (VerifyCanUseNetwork())
		{
			if (!player.isMasterClient)
			{
				Debug.LogError("CloseConnection: Only the masterclient can kick another player.");
			}
			if (kickPlayer == null)
			{
				Debug.LogError("CloseConnection: No such player connected!");
				return;
			}
			int[] targetActors = new int[1] { kickPlayer.ID };
			networkingPeer.OpRaiseEvent(203, null, true, 0, targetActors);
		}
	}

	public static void Destroy(PhotonView targetView)
	{
		if (targetView != null)
		{
			networkingPeer.RemoveInstantiatedGO(targetView.gameObject, false);
		}
		else
		{
			Debug.LogError("Destroy(targetPhotonView) failed, cause targetPhotonView is null.");
		}
	}

	public static void Destroy(GameObject targetGo)
	{
		networkingPeer.RemoveInstantiatedGO(targetGo, false);
	}

	public static void DestroyPlayerObjects(PhotonPlayer targetPlayer)
	{
		if (player == null)
		{
			Debug.LogError("DestroyPlayerObjects() failed, cause parameter 'targetPlayer' was null.");
		}
		DestroyPlayerObjects(targetPlayer.ID);
	}

	public static void DestroyPlayerObjects(int targetPlayerId)
	{
		if (VerifyCanUseNetwork())
		{
			if (player.isMasterClient || targetPlayerId == player.ID)
			{
				networkingPeer.DestroyPlayerObjects(targetPlayerId, false);
			}
			else
			{
				Debug.LogError("DestroyPlayerObjects() failed, cause players can only destroy their own GameObjects. A Master Client can destroy anyone's. This is master: " + isMasterClient);
			}
		}
	}

	public static void DestroyAll()
	{
		if (isMasterClient)
		{
			networkingPeer.DestroyAll(false);
		}
		else
		{
			Debug.LogError("Couldn't call RemoveAllInstantiatedObjects as only the master client is allowed to call this.");
		}
	}

	public static void RemoveRPCs(PhotonPlayer targetPlayer)
	{
		if (VerifyCanUseNetwork())
		{
			if (!targetPlayer.isLocal && !isMasterClient)
			{
				Debug.LogError("Error; Only the MasterClient can call RemoveRPCs for other players.");
			}
			else
			{
				networkingPeer.OpCleanRpcBuffer(targetPlayer.ID);
			}
		}
	}

	public static void RemoveRPCs(PhotonView targetPhotonView)
	{
		if (VerifyCanUseNetwork())
		{
			networkingPeer.CleanRpcBufferIfMine(targetPhotonView);
		}
	}

	public static void RemoveRPCsInGroup(int targetGroup)
	{
		if (VerifyCanUseNetwork())
		{
			networkingPeer.RemoveRPCsInGroup(targetGroup);
		}
	}

	internal static void RPC(PhotonView view, string methodName, PhotonTargets target, params object[] parameters)
	{
		if (VerifyCanUseNetwork())
		{
			if (room == null)
			{
				Debug.LogWarning("Cannot send RPCs in Lobby! RPC dropped.");
			}
			else if (networkingPeer != null)
			{
				networkingPeer.RPC(view, methodName, target, parameters);
			}
			else
			{
				Debug.LogWarning("Could not execute RPC " + methodName + ". Possible scene loading in progress?");
			}
		}
	}

	internal static void RPC(PhotonView view, string methodName, PhotonPlayer targetPlayer, params object[] parameters)
	{
		if (!VerifyCanUseNetwork())
		{
			return;
		}
		if (room == null)
		{
			Debug.LogWarning("Cannot send RPCs in Lobby, only processed locally");
			return;
		}
		if (player == null)
		{
			Debug.LogError("Error; Sending RPC to player null! Aborted \"" + methodName + "\"");
		}
		if (networkingPeer != null)
		{
			networkingPeer.RPC(view, methodName, targetPlayer, parameters);
		}
		else
		{
			Debug.LogWarning("Could not execute RPC " + methodName + ". Possible scene loading in progress?");
		}
	}

	public static void SetReceivingEnabled(int group, bool enabled)
	{
		if (VerifyCanUseNetwork())
		{
			networkingPeer.SetReceivingEnabled(group, enabled);
		}
	}

	public static void SetSendingEnabled(int group, bool enabled)
	{
		if (VerifyCanUseNetwork())
		{
			networkingPeer.SetSendingEnabled(group, enabled);
		}
	}

	public static void SetLevelPrefix(short prefix)
	{
		if (VerifyCanUseNetwork())
		{
			networkingPeer.SetLevelPrefix(prefix);
		}
	}

	private static bool VerifyCanUseNetwork()
	{
		if (networkingPeer != null && (offlineMode || connected))
		{
			return true;
		}
		Debug.LogError("Cannot send messages when not connected; Either connect to Photon OR use offline mode!");
		return false;
	}

	public static void LoadLevel(int levelNumber)
	{
		isMessageQueueRunning = false;
		networkingPeer.loadingLevelAndPausedNetwork = true;
		Application.LoadLevel(levelNumber);
	}

	public static void LoadLevel(string levelTitle)
	{
		isMessageQueueRunning = false;
		networkingPeer.loadingLevelAndPausedNetwork = true;
		Application.LoadLevel(levelTitle);
	}
}
