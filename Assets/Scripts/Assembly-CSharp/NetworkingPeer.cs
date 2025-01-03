using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using ExitGames.Client.Photon;
using ExitGames.Client.Photon.Lite;
using UnityEngine;

internal class NetworkingPeer : LoadbalancingPeer, IPhotonPeerListener
{
	protected internal const string CurrentSceneProperty = "curScn";

	public string mAppVersion;

	private string mAppId;

	private string playername = string.Empty;

	private IPhotonPeerListener externalListener;

	private JoinType mLastJoinType;

	private bool mPlayernameHasToBeUpdated;

	public Dictionary<int, PhotonPlayer> mActors = new Dictionary<int, PhotonPlayer>();

	public PhotonPlayer[] mOtherPlayerListCopy = new PhotonPlayer[0];

	public PhotonPlayer[] mPlayerListCopy = new PhotonPlayer[0];

	public PhotonPlayer mMasterClient;

	public bool hasSwitchedMC;

	public bool requestSecurity = true;

	private Dictionary<Type, List<MethodInfo>> monoRPCMethodsCache = new Dictionary<Type, List<MethodInfo>>();

	public static bool UsePrefabCache = true;

	public static Dictionary<string, GameObject> PrefabCache = new Dictionary<string, GameObject>();

	public Dictionary<string, RoomInfo> mGameList = new Dictionary<string, RoomInfo>();

	public RoomInfo[] mGameListCopy = new RoomInfo[0];

	public bool insideLobby;

	public Dictionary<int, GameObject> instantiatedObjects = new Dictionary<int, GameObject>();

	private HashSet<int> allowedReceivingGroups = new HashSet<int>();

	private HashSet<int> blockSendingGroups = new HashSet<int>();

	protected internal Dictionary<int, PhotonView> photonViewList = new Dictionary<int, PhotonView>();

	protected internal short currentLevelPrefix;

	private readonly Dictionary<string, int> rpcShortcuts;

	private int friendListTimestamp;

	private bool isFetchingFriends;

	private Dictionary<int, object[]> tempInstantiationData = new Dictionary<int, object[]>();

	protected internal bool loadingLevelAndPausedNetwork;

	public AuthenticationValues AuthValues { get; set; }

	public string MasterServerAddress { get; protected internal set; }

	public string PlayerName
	{
		get
		{
			return playername;
		}
		set
		{
			if (!string.IsNullOrEmpty(value) && !value.Equals(playername))
			{
				if (mLocalActor != null)
				{
					mLocalActor.name = value;
				}
				playername = value;
				if (mCurrentGame != null)
				{
					SendPlayerName();
				}
			}
		}
	}

	public PeerState State { get; internal set; }

	public Room mCurrentGame
	{
		get
		{
			if (mRoomToGetInto != null && mRoomToGetInto.isLocalClientInside)
			{
				return mRoomToGetInto;
			}
			return null;
		}
	}

	internal Room mRoomToGetInto { get; set; }

	public PhotonPlayer mLocalActor { get; internal set; }

	public string mGameserver { get; internal set; }

	public int mQueuePosition { get; internal set; }

	public int mPlayersOnMasterCount { get; internal set; }

	public int mGameCount { get; internal set; }

	public int mPlayersInRoomsCount { get; internal set; }

	protected internal int FriendsListAge
	{
		get
		{
			return (!isFetchingFriends && friendListTimestamp != 0) ? (Environment.TickCount - friendListTimestamp) : 0;
		}
	}

	public NetworkingPeer(IPhotonPeerListener listener, string playername, ConnectionProtocol connectionProtocol)
		: base(listener, connectionProtocol)
	{
		base.Listener = this;
		externalListener = listener;
		PlayerName = playername;
		mLocalActor = new PhotonPlayer(true, -1, this.playername);
		AddNewPlayer(mLocalActor.ID, mLocalActor);
		rpcShortcuts = new Dictionary<string, int>(PhotonNetwork.PhotonServerSettings.RpcList.Count);
		for (int i = 0; i < PhotonNetwork.PhotonServerSettings.RpcList.Count; i++)
		{
			string key = PhotonNetwork.PhotonServerSettings.RpcList[i];
			rpcShortcuts[key] = i;
		}
		State = global::PeerState.PeerCreated;
	}

	public override bool Connect(string serverAddress, string appID)
	{
		if (PhotonNetwork.connectionStateDetailed == global::PeerState.Disconnecting)
		{
			Debug.LogError("ERROR: Cannot connect to Photon while Disconnecting. Connection failed.");
			return false;
		}
		if (string.IsNullOrEmpty(MasterServerAddress))
		{
			MasterServerAddress = serverAddress;
		}
		mAppId = appID.Trim();
		bool flag = base.Connect(serverAddress, string.Empty);
		State = ((!flag) ? global::PeerState.Disconnected : global::PeerState.Connecting);
		return flag;
	}

	public override void Disconnect()
	{
		if (base.PeerState == PeerStateValue.Disconnected)
		{
			if ((int)base.DebugOut >= 2)
			{
				DebugReturn(DebugLevel.WARNING, string.Format("Can't execute Disconnect() while not connected. Nothing changed. State: {0}", State));
			}
		}
		else
		{
			State = global::PeerState.Disconnecting;
			base.Disconnect();
			LeftRoomCleanup();
			LeftLobbyCleanup();
		}
	}

	private void DisconnectFromMaster()
	{
		State = global::PeerState.DisconnectingFromMasterserver;
		base.Disconnect();
		LeftLobbyCleanup();
	}

	private void DisconnectFromGameServer()
	{
		State = global::PeerState.DisconnectingFromGameserver;
		base.Disconnect();
		LeftRoomCleanup();
	}

	private void LeftLobbyCleanup()
	{
		if (insideLobby)
		{
			SendMonoMessage(PhotonNetworkingMessage.OnLeftLobby);
			insideLobby = false;
			isFetchingFriends = false;
		}
	}

	private void LeftRoomCleanup()
	{
		bool flag = mRoomToGetInto != null;
		bool flag2 = ((mRoomToGetInto == null) ? PhotonNetwork.autoCleanUpPlayerObjects : mRoomToGetInto.autoCleanUp);
		hasSwitchedMC = false;
		mRoomToGetInto = null;
		mActors = new Dictionary<int, PhotonPlayer>();
		mPlayerListCopy = new PhotonPlayer[0];
		mOtherPlayerListCopy = new PhotonPlayer[0];
		mMasterClient = null;
		allowedReceivingGroups = new HashSet<int>();
		blockSendingGroups = new HashSet<int>();
		mGameList = new Dictionary<string, RoomInfo>();
		mGameListCopy = new RoomInfo[0];
		isFetchingFriends = false;
		ChangeLocalID(-1);
		if (flag2)
		{
			LocalCleanupAnythingInstantiated(true);
			PhotonNetwork.manuallyAllocatedViewIds = new List<int>();
		}
		if (flag)
		{
			SendMonoMessage(PhotonNetworkingMessage.OnLeftRoom);
		}
	}

	protected internal void LocalCleanupAnythingInstantiated(bool destroyInstantiatedGameObjects)
	{
		if (tempInstantiationData.Count > 0)
		{
			Debug.LogWarning("It seems some instantiation is not completed, as instantiation data is used. You should make sure instantiations are paused when calling this method. Cleaning now, despite this.");
		}
		if (destroyInstantiatedGameObjects)
		{
			HashSet<GameObject> hashSet = new HashSet<GameObject>(instantiatedObjects.Values);
			foreach (GameObject item in hashSet)
			{
				RemoveInstantiatedGO(item, true);
			}
		}
		tempInstantiationData.Clear();
		instantiatedObjects = new Dictionary<int, GameObject>();
		PhotonNetwork.lastUsedViewSubId = 0;
		PhotonNetwork.lastUsedViewSubIdStatic = 0;
	}

	private void ReadoutProperties(ExitGames.Client.Photon.Hashtable gameProperties, ExitGames.Client.Photon.Hashtable pActorProperties, int targetActorNr)
	{
		if (mCurrentGame != null && gameProperties != null)
		{
			mCurrentGame.CacheProperties(gameProperties);
			SendMonoMessage(PhotonNetworkingMessage.OnPhotonCustomRoomPropertiesChanged);
			if (PhotonNetwork.automaticallySyncScene)
			{
				AutomaticallySyncScene();
			}
		}
		if (pActorProperties == null || pActorProperties.Count <= 0)
		{
			return;
		}
		if (targetActorNr > 0)
		{
			PhotonPlayer playerWithID = GetPlayerWithID(targetActorNr);
			if (playerWithID != null)
			{
				playerWithID.InternalCacheProperties(GetActorPropertiesForActorNr(pActorProperties, targetActorNr));
				SendMonoMessage(PhotonNetworkingMessage.OnPhotonPlayerPropertiesChanged, playerWithID);
			}
			return;
		}
		foreach (object key in pActorProperties.Keys)
		{
			int num = (int)key;
			ExitGames.Client.Photon.Hashtable hashtable = (ExitGames.Client.Photon.Hashtable)pActorProperties[key];
			string name = (string)hashtable[byte.MaxValue];
			PhotonPlayer photonPlayer = GetPlayerWithID(num);
			if (photonPlayer == null)
			{
				photonPlayer = new PhotonPlayer(false, num, name);
				AddNewPlayer(num, photonPlayer);
			}
			photonPlayer.InternalCacheProperties(hashtable);
			SendMonoMessage(PhotonNetworkingMessage.OnPhotonPlayerPropertiesChanged, photonPlayer);
		}
	}

	private void AddNewPlayer(int ID, PhotonPlayer player)
	{
		if (!mActors.ContainsKey(ID))
		{
			mActors[ID] = player;
			RebuildPlayerListCopies();
		}
		else
		{
			Debug.LogError("Adding player twice: " + ID);
		}
	}

	private void RemovePlayer(int ID, PhotonPlayer player)
	{
		mActors.Remove(ID);
		if (!player.isLocal)
		{
			RebuildPlayerListCopies();
		}
	}

	private void RebuildPlayerListCopies()
	{
		mPlayerListCopy = new PhotonPlayer[mActors.Count];
		mActors.Values.CopyTo(mPlayerListCopy, 0);
		List<PhotonPlayer> list = new List<PhotonPlayer>();
		PhotonPlayer[] array = mPlayerListCopy;
		foreach (PhotonPlayer photonPlayer in array)
		{
			if (!photonPlayer.isLocal)
			{
				list.Add(photonPlayer);
			}
		}
		mOtherPlayerListCopy = list.ToArray();
	}

	private void ResetPhotonViewsOnSerialize()
	{
		foreach (PhotonView value in photonViewList.Values)
		{
			value.lastOnSerializeDataSent = null;
		}
	}

	private void HandleEventLeave(int actorID)
	{
		if ((int)base.DebugOut >= 3)
		{
			DebugReturn(DebugLevel.INFO, "HandleEventLeave actorNr: " + actorID);
		}
		if (actorID < 0 || !mActors.ContainsKey(actorID))
		{
			if ((int)base.DebugOut >= 1)
			{
				DebugReturn(DebugLevel.ERROR, string.Format("Received event Leave for unknown actorNumber: {0}", actorID));
			}
			return;
		}
		PhotonPlayer playerWithID = GetPlayerWithID(actorID);
		if (playerWithID == null)
		{
			Debug.LogError("Error: HandleEventLeave for actorID=" + actorID + " has no PhotonPlayer!");
		}
		CheckMasterClient(actorID);
		if (mCurrentGame != null && mCurrentGame.autoCleanUp)
		{
			DestroyPlayerObjects(actorID, true);
		}
		RemovePlayer(actorID, playerWithID);
		SendMonoMessage(PhotonNetworkingMessage.OnPhotonPlayerDisconnected, playerWithID);
	}

	private void CheckMasterClient(int leavingPlayerId)
	{
		bool flag = mMasterClient != null && mMasterClient.ID == leavingPlayerId;
		bool flag2 = leavingPlayerId > 0;
		if (flag2 && !flag)
		{
			return;
		}
		if (mActors.Count <= 1)
		{
			mMasterClient = mLocalActor;
		}
		else
		{
			int num = int.MaxValue;
			foreach (int key in mActors.Keys)
			{
				if (key < num && key != leavingPlayerId)
				{
					num = key;
				}
			}
			mMasterClient = mActors[num];
		}
		if (flag2)
		{
			SendMonoMessage(PhotonNetworkingMessage.OnMasterClientSwitched, mMasterClient);
		}
	}

	private static int ReturnLowestPlayerId(PhotonPlayer[] players, int playerIdToIgnore)
	{
		if (players == null || players.Length == 0)
		{
			return -1;
		}
		int num = int.MaxValue;
		foreach (PhotonPlayer photonPlayer in players)
		{
			if (photonPlayer.ID != playerIdToIgnore && photonPlayer.ID < num)
			{
				num = photonPlayer.ID;
			}
		}
		return num;
	}

	protected internal bool SetMasterClient(int playerId, bool sync)
	{
		if (mMasterClient == null || mMasterClient.ID == playerId || !mActors.ContainsKey(playerId))
		{
			return false;
		}
		if (sync && !OpRaiseEvent(208, new ExitGames.Client.Photon.Hashtable { 
		{
			(byte)1,
			playerId
		} }, true, 0))
		{
			return false;
		}
		hasSwitchedMC = true;
		mMasterClient = mActors[playerId];
		SendMonoMessage(PhotonNetworkingMessage.OnMasterClientSwitched, mMasterClient);
		return true;
	}

	private ExitGames.Client.Photon.Hashtable GetActorPropertiesForActorNr(ExitGames.Client.Photon.Hashtable actorProperties, int actorNr)
	{
		if (actorProperties.ContainsKey(actorNr))
		{
			return (ExitGames.Client.Photon.Hashtable)actorProperties[actorNr];
		}
		return actorProperties;
	}

	private PhotonPlayer GetPlayerWithID(int number)
	{
		if (mActors != null && mActors.ContainsKey(number))
		{
			return mActors[number];
		}
		return null;
	}

	private void SendPlayerName()
	{
		if (State == global::PeerState.Joining)
		{
			mPlayernameHasToBeUpdated = true;
		}
		else if (mLocalActor != null)
		{
			mLocalActor.name = PlayerName;
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable[byte.MaxValue] = PlayerName;
			if (mLocalActor.ID > 0)
			{
				OpSetPropertiesOfActor(mLocalActor.ID, hashtable, true, 0);
				mPlayernameHasToBeUpdated = false;
			}
		}
	}

	private void GameEnteredOnGameServer(OperationResponse operationResponse)
	{
		if (operationResponse.ReturnCode != 0)
		{
			switch (operationResponse.OperationCode)
			{
			case 227:
				DebugReturn(DebugLevel.ERROR, "Create failed on GameServer. Changing back to MasterServer. Msg: " + operationResponse.DebugMessage);
				SendMonoMessage(PhotonNetworkingMessage.OnPhotonCreateRoomFailed);
				break;
			case 226:
				DebugReturn(DebugLevel.WARNING, "Join failed on GameServer. Changing back to MasterServer. Msg: " + operationResponse.DebugMessage);
				if (operationResponse.ReturnCode == 32758)
				{
					Debug.Log("Most likely the game became empty during the switch to GameServer.");
				}
				SendMonoMessage(PhotonNetworkingMessage.OnPhotonJoinRoomFailed);
				break;
			case 225:
				DebugReturn(DebugLevel.WARNING, "Join failed on GameServer. Changing back to MasterServer. Msg: " + operationResponse.DebugMessage);
				if (operationResponse.ReturnCode == 32758)
				{
					Debug.Log("Most likely the game became empty during the switch to GameServer.");
				}
				SendMonoMessage(PhotonNetworkingMessage.OnPhotonRandomJoinFailed);
				break;
			}
			DisconnectFromGameServer();
		}
		else
		{
			State = global::PeerState.Joined;
			mRoomToGetInto.isLocalClientInside = true;
			ExitGames.Client.Photon.Hashtable pActorProperties = (ExitGames.Client.Photon.Hashtable)operationResponse[249];
			ExitGames.Client.Photon.Hashtable gameProperties = (ExitGames.Client.Photon.Hashtable)operationResponse[248];
			ReadoutProperties(gameProperties, pActorProperties, 0);
			int newID = (int)operationResponse[254];
			ChangeLocalID(newID);
			CheckMasterClient(-1);
			if (mPlayernameHasToBeUpdated)
			{
				SendPlayerName();
			}
			switch (operationResponse.OperationCode)
			{
			case 227:
				SendMonoMessage(PhotonNetworkingMessage.OnCreatedRoom);
				break;
			case 225:
			case 226:
				break;
			}
		}
	}

	private ExitGames.Client.Photon.Hashtable GetLocalActorProperties()
	{
		if (PhotonNetwork.player != null)
		{
			return PhotonNetwork.player.allProperties;
		}
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable[byte.MaxValue] = PlayerName;
		return hashtable;
	}

	public void ChangeLocalID(int newID)
	{
		if (mLocalActor == null)
		{
			Debug.LogWarning(string.Format("Local actor is null or not in mActors! mLocalActor: {0} mActors==null: {1} newID: {2}", mLocalActor, mActors == null, newID));
		}
		if (mActors.ContainsKey(mLocalActor.ID))
		{
			mActors.Remove(mLocalActor.ID);
		}
		mLocalActor.InternalChangeLocalID(newID);
		mActors[mLocalActor.ID] = mLocalActor;
		RebuildPlayerListCopies();
	}

	public bool OpCreateGame(string gameID, bool isVisible, bool isOpen, byte maxPlayers, bool autoCleanUp, ExitGames.Client.Photon.Hashtable customGameProperties, string[] propsListedInLobby)
	{
		mRoomToGetInto = new Room(gameID, customGameProperties, isVisible, isOpen, maxPlayers, autoCleanUp, propsListedInLobby);
		mLastJoinType = JoinType.CreateGame;
		bool flag = State == global::PeerState.Joining;
		return base.OpCreateRoom(gameID, isVisible, isOpen, maxPlayers, autoCleanUp, customGameProperties, (!flag) ? null : GetLocalActorProperties(), propsListedInLobby);
	}

	public bool OpJoin(string gameID, bool createIfNotExists)
	{
		mRoomToGetInto = new Room(gameID, null);
		mLastJoinType = ((!createIfNotExists) ? JoinType.JoinGame : JoinType.JoinOrCreateOnDemand);
		bool flag = State == global::PeerState.Joining;
		return OpJoinRoom(gameID, (!flag) ? null : GetLocalActorProperties(), createIfNotExists);
	}

	public override bool OpJoinRandomRoom(ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties, byte expectedMaxPlayers, ExitGames.Client.Photon.Hashtable playerProperties, MatchmakingMode matchingType)
	{
		mRoomToGetInto = new Room(null, null);
		mLastJoinType = JoinType.JoinRandomGame;
		return base.OpJoinRandomRoom(expectedCustomRoomProperties, expectedMaxPlayers, playerProperties, matchingType);
	}

	public virtual bool OpLeave()
	{
		if (State != global::PeerState.Joined)
		{
			DebugReturn(DebugLevel.ERROR, "NetworkingPeer::leaveGame() - ERROR: no game is currently joined");
			return false;
		}
		return OpCustom(254, null, true, 0);
	}

	public override bool OpRaiseEvent(byte eventCode, bool sendReliable, object customEventContent)
	{
		if (PhotonNetwork.offlineMode)
		{
			return false;
		}
		return base.OpRaiseEvent(eventCode, sendReliable, customEventContent);
	}

	public override bool OpRaiseEvent(byte eventCode, bool sendReliable, object customEventContent, byte channelId, EventCaching cache, int[] targetActors, ReceiverGroup receivers, byte interestGroup)
	{
		if (PhotonNetwork.offlineMode)
		{
			return false;
		}
		return base.OpRaiseEvent(eventCode, sendReliable, customEventContent, channelId, cache, targetActors, receivers, interestGroup);
	}

	public override bool OpRaiseEvent(byte eventCode, byte interestGroup, ExitGames.Client.Photon.Hashtable evData, bool sendReliable, byte channelId)
	{
		if (PhotonNetwork.offlineMode)
		{
			return false;
		}
		return base.OpRaiseEvent(eventCode, interestGroup, evData, sendReliable, channelId);
	}

	public override bool OpRaiseEvent(byte eventCode, ExitGames.Client.Photon.Hashtable evData, bool sendReliable, byte channelId, int[] targetActors, EventCaching cache)
	{
		if (PhotonNetwork.offlineMode)
		{
			return false;
		}
		return base.OpRaiseEvent(eventCode, evData, sendReliable, channelId, targetActors, cache);
	}

	public override bool OpRaiseEvent(byte eventCode, ExitGames.Client.Photon.Hashtable evData, bool sendReliable, byte channelId, EventCaching cache, ReceiverGroup receivers)
	{
		if (PhotonNetwork.offlineMode)
		{
			return false;
		}
		return base.OpRaiseEvent(eventCode, evData, sendReliable, channelId, cache, receivers);
	}

	public void DebugReturn(DebugLevel level, string message)
	{
		externalListener.DebugReturn(level, message);
	}

	public void OnOperationResponse(OperationResponse operationResponse)
	{
		if (PhotonNetwork.networkingPeer.State == global::PeerState.Disconnecting)
		{
			if ((int)base.DebugOut >= 3)
			{
				DebugReturn(DebugLevel.INFO, "OperationResponse ignored while disconnecting: " + operationResponse.OperationCode);
			}
			return;
		}
		if (operationResponse.ReturnCode == 0)
		{
			if ((int)base.DebugOut >= 3)
			{
				DebugReturn(DebugLevel.INFO, operationResponse.ToString());
			}
		}
		else if ((int)base.DebugOut >= 2)
		{
			if (operationResponse.ReturnCode == -3)
			{
				DebugReturn(DebugLevel.WARNING, "Operation could not be executed yet. Wait for state JoinedLobby or ConnectedToMaster and their respective callbacks before calling OPs. Client must be authorized.");
			}
			DebugReturn(DebugLevel.WARNING, operationResponse.ToStringFull());
		}
		switch (operationResponse.OperationCode)
		{
		case 230:
			if (operationResponse.ReturnCode != 0)
			{
				if (operationResponse.ReturnCode == -2)
				{
					DebugReturn(DebugLevel.ERROR, string.Format("If you host Photon yourself, make sure to start the 'Instance LoadBalancing'"));
				}
				else if (operationResponse.ReturnCode == short.MaxValue)
				{
					DebugReturn(DebugLevel.ERROR, string.Format("The appId this client sent is unknown on the server (Cloud). Check settings. If using the Cloud, check account."));
				}
				else if (operationResponse.ReturnCode == 32755)
				{
					DebugReturn(DebugLevel.ERROR, string.Format("Custom Authentication failed (either due to user-input or configuration or AuthParameter string format). Calling: OnCustomAuthenticationFailed()"));
					SendMonoMessage(PhotonNetworkingMessage.OnCustomAuthenticationFailed, operationResponse.DebugMessage);
				}
				else if ((int)base.DebugOut >= 1)
				{
					DebugReturn(DebugLevel.ERROR, string.Format("Authentication failed: '{0}' Code: {1}", operationResponse.DebugMessage, operationResponse.ReturnCode));
				}
				Disconnect();
				State = global::PeerState.Disconnecting;
				if (operationResponse.ReturnCode == 32757)
				{
					DebugReturn(DebugLevel.ERROR, string.Format("Currently, the limit of users is reached for this title. Try again later. Disconnecting"));
					SendMonoMessage(PhotonNetworkingMessage.OnPhotonMaxCccuReached);
					SendMonoMessage(PhotonNetworkingMessage.OnConnectionFail, DisconnectCause.MaxCcuReached);
				}
				else if (operationResponse.ReturnCode == 32756)
				{
					DebugReturn(DebugLevel.ERROR, string.Format("The used master server address is not available with the subscription currently used. Got to Photon Cloud Dashboard or change URL. Disconnecting"));
					SendMonoMessage(PhotonNetworkingMessage.OnConnectionFail, DisconnectCause.InvalidRegion);
				}
			}
			else if (State == global::PeerState.Connected || State == global::PeerState.ConnectedComingFromGameserver)
			{
				if (operationResponse.Parameters.ContainsKey(221))
				{
					if (AuthValues != null)
					{
						AuthValues.Secret = operationResponse[221] as string;
					}
					else if ((int)base.DebugOut >= 2)
					{
						DebugReturn(DebugLevel.WARNING, "Server returned secret but AuthValues are null. Won't use this.");
					}
				}
				if (PhotonNetwork.autoJoinLobby)
				{
					OpJoinLobby();
					State = global::PeerState.Authenticated;
				}
				else
				{
					State = global::PeerState.ConnectedToMaster;
					SendMonoMessage(PhotonNetworkingMessage.OnConnectedToMaster);
				}
			}
			else if (State == global::PeerState.ConnectedToGameserver)
			{
				State = global::PeerState.Joining;
				if (AuthValues != null)
				{
					AuthValues.Secret = null;
				}
				if (mLastJoinType == JoinType.JoinGame || mLastJoinType == JoinType.JoinRandomGame || mLastJoinType == JoinType.JoinOrCreateOnDemand)
				{
					OpJoin(mRoomToGetInto.name, mLastJoinType == JoinType.JoinOrCreateOnDemand);
				}
				else if (mLastJoinType == JoinType.CreateGame)
				{
					OpCreateGame(mRoomToGetInto.name, mRoomToGetInto.visible, mRoomToGetInto.open, (byte)mRoomToGetInto.maxPlayers, mRoomToGetInto.autoCleanUp, mRoomToGetInto.customProperties, mRoomToGetInto.propertiesListedInLobby);
				}
			}
			break;
		case 227:
			if (State != global::PeerState.Joining)
			{
				if (operationResponse.ReturnCode != 0)
				{
					if ((int)base.DebugOut >= 1)
					{
						DebugReturn(DebugLevel.ERROR, string.Format("createGame failed, client stays on masterserver: {0}.", operationResponse.ToStringFull()));
					}
					SendMonoMessage(PhotonNetworkingMessage.OnPhotonCreateRoomFailed);
					break;
				}
				string text = (string)operationResponse[byte.MaxValue];
				if (!string.IsNullOrEmpty(text))
				{
					mRoomToGetInto.name = text;
				}
				mGameserver = (string)operationResponse[230];
				DisconnectFromMaster();
			}
			else
			{
				GameEnteredOnGameServer(operationResponse);
			}
			break;
		case 226:
			if (State != global::PeerState.Joining)
			{
				if (operationResponse.ReturnCode != 0)
				{
					SendMonoMessage(PhotonNetworkingMessage.OnPhotonJoinRoomFailed);
					if ((int)base.DebugOut >= 2)
					{
						DebugReturn(DebugLevel.WARNING, string.Format("JoinRoom failed (room maybe closed by now). Client stays on masterserver: {0}. State: {1}", operationResponse.ToStringFull(), State));
					}
				}
				else
				{
					mGameserver = (string)operationResponse[230];
					DisconnectFromMaster();
				}
			}
			else
			{
				GameEnteredOnGameServer(operationResponse);
			}
			break;
		case 225:
			if (operationResponse.ReturnCode != 0)
			{
				if (operationResponse.ReturnCode == 32760)
				{
					DebugReturn(DebugLevel.INFO, "JoinRandom failed: No open game. Calling: OnPhotonRandomJoinFailed() and staying on master server.");
				}
				else if ((int)base.DebugOut >= 1)
				{
					DebugReturn(DebugLevel.ERROR, string.Format("JoinRandom failed: {0}.", operationResponse.ToStringFull()));
				}
				SendMonoMessage(PhotonNetworkingMessage.OnPhotonRandomJoinFailed);
			}
			else
			{
				string name = (string)operationResponse[byte.MaxValue];
				mRoomToGetInto.name = name;
				mGameserver = (string)operationResponse[230];
				DisconnectFromMaster();
			}
			break;
		case 229:
			State = global::PeerState.JoinedLobby;
			insideLobby = true;
			SendMonoMessage(PhotonNetworkingMessage.OnJoinedLobby);
			break;
		case 228:
			State = global::PeerState.Authenticated;
			LeftLobbyCleanup();
			break;
		case 254:
			DisconnectFromGameServer();
			break;
		case 251:
		{
			ExitGames.Client.Photon.Hashtable pActorProperties = (ExitGames.Client.Photon.Hashtable)operationResponse[249];
			ExitGames.Client.Photon.Hashtable gameProperties = (ExitGames.Client.Photon.Hashtable)operationResponse[248];
			ReadoutProperties(gameProperties, pActorProperties, 0);
			break;
		}
		case 222:
		{
			bool[] array = operationResponse[1] as bool[];
			string[] array2 = operationResponse[2] as string[];
			if (array != null && array2 != null && PhotonNetwork.Friends != null && array.Length == PhotonNetwork.Friends.Count)
			{
				for (int i = 0; i < PhotonNetwork.Friends.Count; i++)
				{
					FriendInfo friendInfo = PhotonNetwork.Friends[i];
					friendInfo.Room = array2[i];
					friendInfo.IsOnline = array[i];
				}
			}
			else
			{
				DebugReturn(DebugLevel.ERROR, "FindFriends failed to apply the result, as a required value wasn't provided or the friend list length differed from result.");
			}
			isFetchingFriends = false;
			friendListTimestamp = Environment.TickCount;
			if (friendListTimestamp == 0)
			{
				friendListTimestamp = 1;
			}
			SendMonoMessage(PhotonNetworkingMessage.OnUpdatedFriendList);
			break;
		}
		default:
			if ((int)base.DebugOut >= 1)
			{
				DebugReturn(DebugLevel.ERROR, string.Format("operationResponse unhandled: {0}", operationResponse.ToString()));
			}
			break;
		case 252:
		case 253:
			break;
		}
		externalListener.OnOperationResponse(operationResponse);
	}

	public override bool OpFindFriends(string[] friendsToFind)
	{
		if (isFetchingFriends)
		{
			return false;
		}
		isFetchingFriends = true;
		PhotonNetwork.Friends = new List<FriendInfo>(friendsToFind.Length);
		foreach (string name in friendsToFind)
		{
			PhotonNetwork.Friends.Add(new FriendInfo
			{
				Name = name
			});
		}
		return base.OpFindFriends(friendsToFind);
	}

	public void OnStatusChanged(StatusCode statusCode)
	{
		if ((int)base.DebugOut >= 3)
		{
			DebugReturn(DebugLevel.INFO, string.Format("OnStatusChanged: {0}", statusCode.ToString()));
		}
		switch (statusCode)
		{
		case StatusCode.Connect:
			if (State == global::PeerState.ConnectingToGameserver)
			{
				if ((int)base.DebugOut >= 5)
				{
					DebugReturn(DebugLevel.ALL, "Connected to gameserver.");
				}
				State = global::PeerState.ConnectedToGameserver;
			}
			else
			{
				if ((int)base.DebugOut >= 5)
				{
					DebugReturn(DebugLevel.ALL, "Connected to masterserver.");
				}
				if (State == global::PeerState.Connecting)
				{
					SendMonoMessage(PhotonNetworkingMessage.OnConnectedToPhoton);
					State = global::PeerState.Connected;
				}
				else
				{
					State = global::PeerState.ConnectedComingFromGameserver;
				}
			}
			if (requestSecurity || AuthValues != null)
			{
				EstablishEncryption();
			}
			else if (!OpAuthenticate(mAppId, mAppVersion, PlayerName, AuthValues))
			{
				externalListener.DebugReturn(DebugLevel.ERROR, "Error calling OpAuthenticate! Did not work. Check log output, AuthValues and if you're connected. State: " + State);
			}
			break;
		case StatusCode.Disconnect:
			if (State == global::PeerState.DisconnectingFromMasterserver)
			{
				if (Connect(mGameserver, mAppId))
				{
					State = global::PeerState.ConnectingToGameserver;
				}
			}
			else if (State == global::PeerState.DisconnectingFromGameserver)
			{
				if (Connect(MasterServerAddress, mAppId))
				{
					State = global::PeerState.ConnectingToMasterserver;
				}
			}
			else
			{
				LeftRoomCleanup();
				State = global::PeerState.PeerCreated;
				SendMonoMessage(PhotonNetworkingMessage.OnDisconnectedFromPhoton);
			}
			break;
		case StatusCode.SecurityExceptionOnConnect:
		case StatusCode.ExceptionOnConnect:
		{
			State = global::PeerState.PeerCreated;
			DisconnectCause disconnectCause = (DisconnectCause)statusCode;
			SendMonoMessage(PhotonNetworkingMessage.OnFailedToConnectToPhoton, disconnectCause);
			break;
		}
		case StatusCode.Exception:
			if (State == global::PeerState.Connecting)
			{
				DebugReturn(DebugLevel.WARNING, "Exception while connecting to: " + base.ServerAddress + ". Check if the server is available.");
				if (base.ServerAddress == null || base.ServerAddress.StartsWith("127.0.0.1"))
				{
					DebugReturn(DebugLevel.WARNING, "The server address is 127.0.0.1 (localhost): Make sure the server is running on this machine. Android and iOS emulators have their own localhost.");
					if (base.ServerAddress == mGameserver)
					{
						DebugReturn(DebugLevel.WARNING, "This might be a misconfiguration in the game server config. You need to edit it to a (public) address.");
					}
				}
				State = global::PeerState.PeerCreated;
				DisconnectCause disconnectCause = (DisconnectCause)statusCode;
				SendMonoMessage(PhotonNetworkingMessage.OnFailedToConnectToPhoton, disconnectCause);
			}
			else
			{
				State = global::PeerState.PeerCreated;
				DisconnectCause disconnectCause = (DisconnectCause)statusCode;
				SendMonoMessage(PhotonNetworkingMessage.OnConnectionFail, disconnectCause);
			}
			Disconnect();
			break;
		case StatusCode.ExceptionOnReceive:
		case StatusCode.TimeoutDisconnect:
		case StatusCode.DisconnectByServer:
		case StatusCode.DisconnectByServerUserLimit:
		case StatusCode.DisconnectByServerLogic:
			if (State == global::PeerState.Connecting)
			{
				DebugReturn(DebugLevel.WARNING, string.Concat(statusCode, " while connecting to: ", base.ServerAddress, ". Check if the server is available."));
				State = global::PeerState.PeerCreated;
				DisconnectCause disconnectCause = (DisconnectCause)statusCode;
				SendMonoMessage(PhotonNetworkingMessage.OnFailedToConnectToPhoton, disconnectCause);
			}
			else
			{
				State = global::PeerState.PeerCreated;
				DisconnectCause disconnectCause = (DisconnectCause)statusCode;
				SendMonoMessage(PhotonNetworkingMessage.OnConnectionFail, disconnectCause);
			}
			Disconnect();
			break;
		case StatusCode.EncryptionEstablished:
			if (!OpAuthenticate(mAppId, mAppVersion, PlayerName, AuthValues))
			{
				externalListener.DebugReturn(DebugLevel.ERROR, "Error calling OpAuthenticate! Did not work. Check log output, AuthValues and if you're connected. State: " + State);
			}
			break;
		case StatusCode.EncryptionFailedToEstablish:
			externalListener.DebugReturn(DebugLevel.ERROR, string.Concat("Encryption wasn't established: ", statusCode, ". Going to authenticate anyways."));
			if (!OpAuthenticate(mAppId, mAppVersion, PlayerName, AuthValues))
			{
				externalListener.DebugReturn(DebugLevel.ERROR, "Error calling OpAuthenticate! Did not work. Check log output, AuthValues and if you're connected. State: " + State);
			}
			break;
		default:
			DebugReturn(DebugLevel.ERROR, "Received unknown status code: " + statusCode);
			break;
		case StatusCode.QueueOutgoingReliableWarning:
		case StatusCode.QueueOutgoingUnreliableWarning:
		case StatusCode.SendError:
		case StatusCode.QueueOutgoingAcksWarning:
		case StatusCode.QueueSentWarning:
			break;
		}
		externalListener.OnStatusChanged(statusCode);
	}

	public void OnEvent(EventData photonEvent)
	{
		if ((int)base.DebugOut >= 3)
		{
			DebugReturn(DebugLevel.INFO, string.Format("OnEvent: {0}", photonEvent.ToString()));
		}
		int num = -1;
		PhotonPlayer photonPlayer = null;
		if (photonEvent.Parameters.ContainsKey(254))
		{
			num = (int)photonEvent[254];
			if (mActors.ContainsKey(num))
			{
				photonPlayer = mActors[num];
			}
		}
		switch (photonEvent.Code)
		{
		case 230:
		{
			mGameList = new Dictionary<string, RoomInfo>();
			ExitGames.Client.Photon.Hashtable hashtable2 = (ExitGames.Client.Photon.Hashtable)photonEvent[222];
			foreach (DictionaryEntry item in hashtable2)
			{
				string text = (string)item.Key;
				mGameList[text] = new RoomInfo(text, (ExitGames.Client.Photon.Hashtable)item.Value);
			}
			mGameListCopy = new RoomInfo[mGameList.Count];
			mGameList.Values.CopyTo(mGameListCopy, 0);
			SendMonoMessage(PhotonNetworkingMessage.OnReceivedRoomListUpdate);
			break;
		}
		case 229:
		{
			ExitGames.Client.Photon.Hashtable hashtable3 = (ExitGames.Client.Photon.Hashtable)photonEvent[222];
			foreach (DictionaryEntry item2 in hashtable3)
			{
				string text2 = (string)item2.Key;
				Room room = new Room(text2, (ExitGames.Client.Photon.Hashtable)item2.Value);
				if (room.removedFromList)
				{
					mGameList.Remove(text2);
				}
				else
				{
					mGameList[text2] = room;
				}
			}
			mGameListCopy = new RoomInfo[mGameList.Count];
			mGameList.Values.CopyTo(mGameListCopy, 0);
			SendMonoMessage(PhotonNetworkingMessage.OnReceivedRoomListUpdate);
			break;
		}
		case 228:
			if (photonEvent.Parameters.ContainsKey(223))
			{
				mQueuePosition = (int)photonEvent[223];
			}
			else
			{
				DebugReturn(DebugLevel.ERROR, "Event QueueState must contain position!");
			}
			if (mQueuePosition == 0)
			{
				if (PhotonNetwork.autoJoinLobby)
				{
					OpJoinLobby();
					State = global::PeerState.Authenticated;
				}
				else
				{
					State = global::PeerState.ConnectedToMaster;
					SendMonoMessage(PhotonNetworkingMessage.OnConnectedToMaster);
				}
			}
			break;
		case 226:
			mPlayersInRoomsCount = (int)photonEvent[229];
			mPlayersOnMasterCount = (int)photonEvent[227];
			mGameCount = (int)photonEvent[228];
			break;
		case byte.MaxValue:
		{
			ExitGames.Client.Photon.Hashtable properties = (ExitGames.Client.Photon.Hashtable)photonEvent[249];
			if (photonPlayer == null)
			{
				bool isLocal = mLocalActor.ID == num;
				AddNewPlayer(num, new PhotonPlayer(isLocal, num, properties));
				ResetPhotonViewsOnSerialize();
			}
			if (mActors[num] == mLocalActor)
			{
				int[] array = (int[])photonEvent[252];
				int[] array2 = array;
				foreach (int num3 in array2)
				{
					if (mLocalActor.ID != num3 && !mActors.ContainsKey(num3))
					{
						AddNewPlayer(num3, new PhotonPlayer(false, num3, string.Empty));
					}
				}
				if (mLastJoinType == JoinType.JoinOrCreateOnDemand && mLocalActor.ID == 1)
				{
					SendMonoMessage(PhotonNetworkingMessage.OnCreatedRoom);
				}
				else
				{
					SendMonoMessage(PhotonNetworkingMessage.OnJoinedRoom);
				}
			}
			else
			{
				SendMonoMessage(PhotonNetworkingMessage.OnPhotonPlayerConnected, mActors[num]);
			}
			break;
		}
		case 254:
			HandleEventLeave(num);
			break;
		case 253:
		{
			int num5 = (int)photonEvent[253];
			ExitGames.Client.Photon.Hashtable gameProperties = null;
			ExitGames.Client.Photon.Hashtable pActorProperties = null;
			if (num5 == 0)
			{
				gameProperties = (ExitGames.Client.Photon.Hashtable)photonEvent[251];
			}
			else
			{
				pActorProperties = (ExitGames.Client.Photon.Hashtable)photonEvent[251];
			}
			ReadoutProperties(gameProperties, pActorProperties, num5);
			break;
		}
		case 200:
			ExecuteRPC(photonEvent[245] as ExitGames.Client.Photon.Hashtable, photonPlayer);
			break;
		case 201:
		case 206:
		{
			ExitGames.Client.Photon.Hashtable hashtable4 = (ExitGames.Client.Photon.Hashtable)photonEvent[245];
			int networkTime = (int)hashtable4[(byte)0];
			short correctPrefix = -1;
			short num6 = 1;
			if (hashtable4.ContainsKey((byte)1))
			{
				correctPrefix = (short)hashtable4[(byte)1];
				num6 = 2;
			}
			for (short num7 = num6; num7 < hashtable4.Count; num7++)
			{
				OnSerializeRead(hashtable4[num7] as ExitGames.Client.Photon.Hashtable, photonPlayer, networkTime, correctPrefix);
			}
			break;
		}
		case 202:
			DoInstantiate((ExitGames.Client.Photon.Hashtable)photonEvent[245], photonPlayer, null);
			break;
		case 203:
			if (photonPlayer == null || !photonPlayer.isMasterClient)
			{
				Debug.LogError(string.Concat("Error: Someone else(", photonPlayer, ") then the masterserver requests a disconnect!"));
			}
			else
			{
				PhotonNetwork.LeaveRoom();
			}
			break;
		case 207:
		{
			ExitGames.Client.Photon.Hashtable hashtable = (ExitGames.Client.Photon.Hashtable)photonEvent[245];
			int num4 = (int)hashtable[(byte)0];
			if (num4 >= 0)
			{
				DestroyPlayerObjects(num4, true);
				break;
			}
			Debug.Log("Ev DestroyAll! By PlayerId: " + num);
			DestroyAll(true);
			break;
		}
		case 204:
		{
			ExitGames.Client.Photon.Hashtable hashtable = (ExitGames.Client.Photon.Hashtable)photonEvent[245];
			int num2 = (int)hashtable[(byte)0];
			GameObject value = null;
			instantiatedObjects.TryGetValue(num2, out value);
			if (value == null || photonPlayer == null)
			{
				Debug.LogError(string.Concat("Can't execute received Destroy request for view ID=", num2, " as GO can't be foudn. From player/actorNr: ", num, " goToDestroyLocally=", value, "  originating Player=", photonPlayer));
			}
			else
			{
				RemoveInstantiatedGO(value, true);
			}
			break;
		}
		case 208:
		{
			ExitGames.Client.Photon.Hashtable hashtable = (ExitGames.Client.Photon.Hashtable)photonEvent[245];
			int playerId = (int)hashtable[(byte)1];
			SetMasterClient(playerId, false);
			break;
		}
		default:
			Debug.LogError("Error. Unhandled event: " + photonEvent);
			break;
		}
		externalListener.OnEvent(photonEvent);
	}

	public static void SendMonoMessage(PhotonNetworkingMessage methodString, params object[] parameters)
	{
		HashSet<GameObject> hashSet;
		if (PhotonNetwork.SendMonoMessageTargets != null)
		{
			hashSet = PhotonNetwork.SendMonoMessageTargets;
		}
		else
		{
			hashSet = new HashSet<GameObject>();
			Component[] array = (Component[])UnityEngine.Object.FindObjectsOfType(typeof(MonoBehaviour));
			for (int i = 0; i < array.Length; i++)
			{
				hashSet.Add(array[i].gameObject);
			}
		}
		string methodName = methodString.ToString();
		foreach (GameObject item in hashSet)
		{
			if (parameters != null && parameters.Length == 1)
			{
				item.SendMessage(methodName, parameters[0], SendMessageOptions.DontRequireReceiver);
			}
			else
			{
				item.SendMessage(methodName, parameters, SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	public void ExecuteRPC(ExitGames.Client.Photon.Hashtable rpcData, PhotonPlayer sender)
	{
		if (rpcData == null || !rpcData.ContainsKey((byte)0))
		{
			DebugReturn(DebugLevel.ERROR, "Malformed RPC; this should never occur.");
			return;
		}
		int num = (int)rpcData[(byte)0];
		int num2 = 0;
		if (rpcData.ContainsKey((byte)1))
		{
			num2 = (short)rpcData[(byte)1];
		}
		string text;
		if (rpcData.ContainsKey((byte)5))
		{
			int num3 = (byte)rpcData[(byte)5];
			if (num3 > PhotonNetwork.PhotonServerSettings.RpcList.Count - 1)
			{
				Debug.LogError("Could not find RPC with index: " + num3 + ". Going to ignore! Check PhotonServerSettings.RpcList");
				return;
			}
			text = PhotonNetwork.PhotonServerSettings.RpcList[num3];
		}
		else
		{
			text = (string)rpcData[(byte)3];
		}
		object[] array = null;
		if (rpcData.ContainsKey((byte)4))
		{
			array = (object[])rpcData[(byte)4];
		}
		if (array == null)
		{
			array = new object[0];
		}
		PhotonView photonView = GetPhotonView(num);
		if (photonView == null)
		{
			int num4 = num / PhotonNetwork.MAX_VIEW_IDS;
			bool flag = num4 == mLocalActor.ID;
			bool flag2 = num4 == sender.ID;
			if (flag)
			{
				Debug.LogWarning("Received RPC \"" + text + "\" for viewID " + num + " but this PhotonView does not exist! View was/is ours." + ((!flag2) ? " Remote called." : " Owner called."));
			}
			else
			{
				Debug.LogError("Received RPC \"" + text + "\" for viewID " + num + " but this PhotonView does not exist! Was remote PV." + ((!flag2) ? " Remote called." : " Owner called."));
			}
			return;
		}
		if (photonView.prefix != num2)
		{
			Debug.LogError("Received RPC \"" + text + "\" on viewID " + num + " with a prefix of " + num2 + ", our prefix is " + photonView.prefix + ". The RPC has been ignored.");
			return;
		}
		if (text == string.Empty)
		{
			DebugReturn(DebugLevel.ERROR, "Malformed RPC; this should never occur.");
			return;
		}
		if ((int)base.DebugOut >= 5)
		{
			DebugReturn(DebugLevel.ALL, "Received RPC; " + text);
		}
		if (photonView.group != 0 && !allowedReceivingGroups.Contains(photonView.group))
		{
			return;
		}
		Type[] array2 = new Type[0];
		if (array.Length > 0)
		{
			array2 = new Type[array.Length];
			int num5 = 0;
			foreach (object obj in array)
			{
				if (obj == null)
				{
					array2[num5] = null;
				}
				else
				{
					array2[num5] = obj.GetType();
				}
				num5++;
			}
		}
		int num6 = 0;
		int num7 = 0;
		MonoBehaviour[] components = photonView.GetComponents<MonoBehaviour>();
		foreach (MonoBehaviour monoBehaviour in components)
		{
			if (monoBehaviour == null)
			{
				Debug.LogError("ERROR You have missing MonoBehaviours on your gameobjects!");
				continue;
			}
			Type type = monoBehaviour.GetType();
			List<MethodInfo> list = null;
			if (monoRPCMethodsCache.ContainsKey(type))
			{
				list = monoRPCMethodsCache[type];
			}
			if (list == null)
			{
				List<MethodInfo> methods = SupportClass.GetMethods(type, typeof(RPC));
				monoRPCMethodsCache[type] = methods;
				list = methods;
			}
			if (list == null)
			{
				continue;
			}
			for (int k = 0; k < list.Count; k++)
			{
				MethodInfo methodInfo = list[k];
				if (!(methodInfo.Name == text))
				{
					continue;
				}
				num7++;
				ParameterInfo[] parameters = methodInfo.GetParameters();
				if (parameters.Length == array2.Length)
				{
					if (CheckTypeMatch(parameters, array2))
					{
						num6++;
						object obj2 = methodInfo.Invoke(monoBehaviour, array);
						if (methodInfo.ReturnType == typeof(IEnumerator))
						{
							PhotonHandler.SP.StartCoroutine((IEnumerator)obj2);
						}
					}
				}
				else if (parameters.Length - 1 == array2.Length)
				{
					if (CheckTypeMatch(parameters, array2) && parameters[parameters.Length - 1].ParameterType == typeof(PhotonMessageInfo))
					{
						num6++;
						int timestamp = (int)rpcData[(byte)2];
						object[] array3 = new object[array.Length + 1];
						array.CopyTo(array3, 0);
						array3[array3.Length - 1] = new PhotonMessageInfo(sender, timestamp, photonView);
						object obj3 = methodInfo.Invoke(monoBehaviour, array3);
						if (methodInfo.ReturnType == typeof(IEnumerator))
						{
							PhotonHandler.SP.StartCoroutine((IEnumerator)obj3);
						}
					}
				}
				else if (parameters.Length == 1 && parameters[0].ParameterType.IsArray)
				{
					num6++;
					object obj4 = methodInfo.Invoke(monoBehaviour, new object[1] { array });
					if (methodInfo.ReturnType == typeof(IEnumerator))
					{
						PhotonHandler.SP.StartCoroutine((IEnumerator)obj4);
					}
				}
			}
		}
		if (num6 == 1)
		{
			return;
		}
		string text2 = string.Empty;
		foreach (Type type2 in array2)
		{
			if (text2 != string.Empty)
			{
				text2 += ", ";
			}
			text2 = ((type2 != null) ? (text2 + type2.Name) : (text2 + "null"));
		}
		if (num6 == 0)
		{
			if (num7 == 0)
			{
				DebugReturn(DebugLevel.ERROR, "PhotonView with ID " + num + " has no method \"" + text + "\" marked with the [RPC](C#) or @RPC(JS) property! Args: " + text2);
			}
			else
			{
				DebugReturn(DebugLevel.ERROR, "PhotonView with ID " + num + " has no method \"" + text + "\" that takes " + array2.Length + " argument(s): " + text2);
			}
		}
		else
		{
			DebugReturn(DebugLevel.ERROR, "PhotonView with ID " + num + " has " + num6 + " methods \"" + text + "\" that takes " + array2.Length + " argument(s): " + text2 + ". Should be just one?");
		}
	}

	private bool CheckTypeMatch(ParameterInfo[] methodParameters, Type[] callParameterTypes)
	{
		if (methodParameters.Length < callParameterTypes.Length)
		{
			return false;
		}
		for (int i = 0; i < callParameterTypes.Length; i++)
		{
			Type parameterType = methodParameters[i].ParameterType;
			if (callParameterTypes[i] != null && !parameterType.Equals(callParameterTypes[i]))
			{
				return false;
			}
		}
		return true;
	}

	internal ExitGames.Client.Photon.Hashtable SendInstantiate(string prefabName, Vector3 position, Quaternion rotation, int group, int[] viewIDs, object[] data, bool isGlobalObject)
	{
		int num = viewIDs[0];
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable[(byte)0] = prefabName;
		if (position != Vector3.zero)
		{
			hashtable[(byte)1] = position;
		}
		if (rotation != Quaternion.identity)
		{
			hashtable[(byte)2] = rotation;
		}
		if (group != 0)
		{
			hashtable[(byte)3] = group;
		}
		if (viewIDs.Length > 1)
		{
			hashtable[(byte)4] = viewIDs;
		}
		if (data != null)
		{
			hashtable[(byte)5] = data;
		}
		if (currentLevelPrefix > 0)
		{
			hashtable[(byte)8] = currentLevelPrefix;
		}
		hashtable[(byte)6] = base.ServerTimeInMilliSeconds;
		hashtable[(byte)7] = num;
		EventCaching cache = ((!isGlobalObject) ? EventCaching.AddToRoomCache : EventCaching.AddToRoomCacheGlobal);
		OpRaiseEvent(202, hashtable, true, 0, cache, ReceiverGroup.Others);
		return hashtable;
	}

	internal GameObject DoInstantiate(ExitGames.Client.Photon.Hashtable evData, PhotonPlayer photonPlayer, GameObject resourceGameObject)
	{
		string text = (string)evData[(byte)0];
		int timestamp = (int)evData[(byte)6];
		int num = (int)evData[(byte)7];
		Vector3 position = ((!evData.ContainsKey((byte)1)) ? Vector3.zero : ((Vector3)evData[(byte)1]));
		Quaternion rotation = Quaternion.identity;
		if (evData.ContainsKey((byte)2))
		{
			rotation = (Quaternion)evData[(byte)2];
		}
		int num2 = 0;
		if (evData.ContainsKey((byte)3))
		{
			num2 = (int)evData[(byte)3];
		}
		short prefix = 0;
		if (evData.ContainsKey((byte)8))
		{
			prefix = (short)evData[(byte)8];
		}
		int[] array = ((!evData.ContainsKey((byte)4)) ? new int[1] { num } : ((int[])evData[(byte)4]));
		object[] instantiationData = ((!evData.ContainsKey((byte)5)) ? null : ((object[])evData[(byte)5]));
		if (num2 != 0 && !allowedReceivingGroups.Contains(num2))
		{
			return null;
		}
		if (resourceGameObject == null)
		{
			if (!UsePrefabCache || !PrefabCache.TryGetValue(text, out resourceGameObject))
			{
				resourceGameObject = (GameObject)Resources.Load(text, typeof(GameObject));
				if (UsePrefabCache)
				{
					PrefabCache.Add(text, resourceGameObject);
				}
			}
			if (resourceGameObject == null)
			{
				Debug.LogError("PhotonNetwork error: Could not Instantiate the prefab [" + text + "]. Please verify you have this gameobject in a Resources folder.");
				return null;
			}
		}
		PhotonView[] photonViewsInChildren = resourceGameObject.GetPhotonViewsInChildren();
		if (photonViewsInChildren.Length != array.Length)
		{
			throw new Exception("Error in Instantiation! The resource's PhotonView count is not the same as in incoming data.");
		}
		for (int i = 0; i < array.Length; i++)
		{
			photonViewsInChildren[i].viewID = array[i];
			photonViewsInChildren[i].prefix = prefix;
			photonViewsInChildren[i].instantiationId = num;
		}
		StoreInstantiationData(num, instantiationData);
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(resourceGameObject, position, rotation);
		for (int j = 0; j < array.Length; j++)
		{
			photonViewsInChildren[j].viewID = 0;
			photonViewsInChildren[j].prefix = -1;
			photonViewsInChildren[j].prefixBackup = -1;
			photonViewsInChildren[j].instantiationId = -1;
		}
		RemoveInstantiationData(num);
		if (instantiatedObjects.ContainsKey(num))
		{
			GameObject gameObject2 = instantiatedObjects[num];
			string text2 = string.Empty;
			if (gameObject2 != null)
			{
				PhotonView[] photonViewsInChildren2 = gameObject2.GetPhotonViewsInChildren();
				PhotonView[] array2 = photonViewsInChildren2;
				foreach (PhotonView photonView in array2)
				{
					if (!(photonView == null))
					{
						text2 = text2 + photonView.ToString() + ", ";
					}
				}
			}
			Debug.LogError(string.Format("DoInstantiate re-defines a GameObject. Destroying old entry! New: '{0}' (instantiationID: {1}) Old: {3}. PhotonViews on old: {4}. instantiatedObjects.Count: {2}. PhotonNetwork.lastUsedViewSubId: {5} PhotonNetwork.lastUsedViewSubIdStatic: {6} this.photonViewList.Count {7}.)", gameObject, num, instantiatedObjects.Count, gameObject2, text2, PhotonNetwork.lastUsedViewSubId, PhotonNetwork.lastUsedViewSubIdStatic, photonViewList.Count));
			RemoveInstantiatedGO(gameObject2, true);
		}
		instantiatedObjects.Add(num, gameObject);
		object[] parameters = new object[1]
		{
			new PhotonMessageInfo(photonPlayer, timestamp, null)
		};
		MonoBehaviour[] componentsInChildren = gameObject.GetComponentsInChildren<MonoBehaviour>();
		foreach (MonoBehaviour monoBehaviour in componentsInChildren)
		{
			MethodInfo mi;
			if (GetMethod(monoBehaviour, PhotonNetworkingMessage.OnPhotonInstantiate.ToString(), out mi))
			{
				object obj = mi.Invoke(monoBehaviour, parameters);
				if (mi.ReturnType == typeof(IEnumerator))
				{
					PhotonHandler.SP.StartCoroutine((IEnumerator)obj);
				}
			}
		}
		return gameObject;
	}

	private void StoreInstantiationData(int instantiationId, object[] instantiationData)
	{
		tempInstantiationData[instantiationId] = instantiationData;
	}

	public object[] FetchInstantiationData(int instantiationId)
	{
		object[] value = null;
		if (instantiationId == 0)
		{
			return null;
		}
		tempInstantiationData.TryGetValue(instantiationId, out value);
		return value;
	}

	private void RemoveInstantiationData(int instantiationId)
	{
		tempInstantiationData.Remove(instantiationId);
	}

	public void RemoveAllInstantiatedObjects()
	{
		GameObject[] array = new GameObject[instantiatedObjects.Count];
		instantiatedObjects.Values.CopyTo(array, 0);
		foreach (GameObject gameObject in array)
		{
			if (!(gameObject == null))
			{
				RemoveInstantiatedGO(gameObject, false);
			}
		}
		if (instantiatedObjects.Count > 0)
		{
			Debug.LogError("RemoveAllInstantiatedObjects() this.instantiatedObjects.Count should be 0 by now.");
		}
		instantiatedObjects = new Dictionary<int, GameObject>();
	}

	public void DestroyPlayerObjects(int playerId, bool localOnly)
	{
		if (playerId <= 0)
		{
			Debug.LogError("Failed to Destroy objects of playerId: " + playerId);
			return;
		}
		if (!localOnly)
		{
			OpRemoveFromServerInstantiationsOfPlayer(playerId);
			OpCleanRpcBuffer(playerId);
			SendDestroyOfPlayer(playerId);
		}
		Queue<GameObject> queue = new Queue<GameObject>();
		int num = playerId * PhotonNetwork.MAX_VIEW_IDS;
		int num2 = num + PhotonNetwork.MAX_VIEW_IDS;
		foreach (KeyValuePair<int, GameObject> instantiatedObject in instantiatedObjects)
		{
			if (instantiatedObject.Key > num && instantiatedObject.Key < num2)
			{
				queue.Enqueue(instantiatedObject.Value);
			}
		}
		foreach (GameObject item in queue)
		{
			RemoveInstantiatedGO(item, true);
		}
	}

	public void DestroyAll(bool localOnly)
	{
		if (!localOnly)
		{
			OpRemoveCompleteCache();
			SendDestroyOfAll();
		}
		LocalCleanupAnythingInstantiated(true);
	}

	public void RemoveInstantiatedGO(GameObject go, bool localOnly)
	{
		if (go == null)
		{
			if (base.DebugOut == DebugLevel.ERROR)
			{
				DebugReturn(DebugLevel.ERROR, "Failed to 'network-remove' GameObject because it's null.");
			}
			return;
		}
		PhotonView[] componentsInChildren = go.GetComponentsInChildren<PhotonView>();
		if (componentsInChildren == null || componentsInChildren.Length <= 0)
		{
			if (base.DebugOut == DebugLevel.ERROR)
			{
				DebugReturn(DebugLevel.ERROR, "Failed to 'network-remove' GameObject because has no PhotonView components: " + go);
			}
			return;
		}
		PhotonView photonView = componentsInChildren[0];
		int ownerActorNr = photonView.OwnerActorNr;
		int instantiationId = photonView.instantiationId;
		if (!localOnly && !photonView.isMine && (!mLocalActor.isMasterClient || mActors.ContainsKey(ownerActorNr)))
		{
			if (base.DebugOut == DebugLevel.ERROR)
			{
				DebugReturn(DebugLevel.ERROR, "Failed to 'network-remove' GameObject. Client is neither owner nor masterClient taking over for owner who left: " + photonView);
			}
			return;
		}
		if (instantiationId < 1)
		{
			if (base.DebugOut == DebugLevel.ERROR)
			{
				DebugReturn(DebugLevel.ERROR, string.Concat("Failed to 'network-remove' GameObject because it is missing a valid InstantiationId on view: ", photonView, ". Not Destroying GameObject or PhotonViews!"));
			}
			return;
		}
		if (!localOnly)
		{
			ServerCleanInstantiateAndDestroy(instantiationId, ownerActorNr);
		}
		instantiatedObjects.Remove(instantiationId);
		for (int num = componentsInChildren.Length - 1; num >= 0; num--)
		{
			PhotonView photonView2 = componentsInChildren[num];
			if (!(photonView2 == null))
			{
				if (photonView2.instantiationId >= 1)
				{
					LocalCleanPhotonView(photonView2);
				}
				if (!localOnly)
				{
					OpCleanRpcBuffer(photonView2);
				}
			}
		}
		if ((int)base.DebugOut >= 5)
		{
			DebugReturn(DebugLevel.ALL, "Network destroy Instantiated GO: " + go.name);
		}
		UnityEngine.Object.Destroy(go);
	}

	public int GetInstantiatedObjectsId(GameObject go)
	{
		int result = -1;
		if (go == null)
		{
			DebugReturn(DebugLevel.ERROR, "GetInstantiatedObjectsId() for GO == null.");
			return result;
		}
		PhotonView[] photonViewsInChildren = go.GetPhotonViewsInChildren();
		if (photonViewsInChildren != null && photonViewsInChildren.Length > 0 && photonViewsInChildren[0] != null)
		{
			return photonViewsInChildren[0].instantiationId;
		}
		if (base.DebugOut == DebugLevel.ALL)
		{
			DebugReturn(DebugLevel.ALL, "GetInstantiatedObjectsId failed for GO: " + go);
		}
		return result;
	}

	private void ServerCleanInstantiateAndDestroy(int instantiateId, int actorNr)
	{
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable[(byte)7] = instantiateId;
		OpRaiseEvent(202, hashtable, true, 0, new int[1] { actorNr }, EventCaching.RemoveFromRoomCache);
		ExitGames.Client.Photon.Hashtable hashtable2 = new ExitGames.Client.Photon.Hashtable();
		hashtable2[(byte)0] = instantiateId;
		OpRaiseEvent(204, hashtable2, true, 0, EventCaching.DoNotCache, ReceiverGroup.Others);
	}

	private void SendDestroyOfPlayer(int actorNr)
	{
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable[(byte)0] = actorNr;
		OpRaiseEvent(207, hashtable, true, 0, EventCaching.DoNotCache, ReceiverGroup.Others);
	}

	private void SendDestroyOfAll()
	{
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable[(byte)0] = -1;
		OpRaiseEvent(207, hashtable, true, 0, EventCaching.DoNotCache, ReceiverGroup.Others);
	}

	private void OpRemoveFromServerInstantiationsOfPlayer(int actorNr)
	{
		OpRaiseEvent(202, null, true, 0, new int[1] { actorNr }, EventCaching.RemoveFromRoomCache);
	}

	public void LocalCleanPhotonView(PhotonView view)
	{
		view.destroyedByPhotonNetworkOrQuit = true;
		photonViewList.Remove(view.viewID);
	}

	public PhotonView GetPhotonView(int viewID)
	{
		PhotonView value = null;
		photonViewList.TryGetValue(viewID, out value);
		if (value == null)
		{
			PhotonView[] array = UnityEngine.Object.FindObjectsOfType(typeof(PhotonView)) as PhotonView[];
			PhotonView[] array2 = array;
			foreach (PhotonView photonView in array2)
			{
				if (photonView.viewID == viewID)
				{
					Debug.LogWarning("Had to lookup view that wasn't in dict: " + photonView);
					return photonView;
				}
			}
		}
		return value;
	}

	public void RegisterPhotonView(PhotonView netView)
	{
		if (!Application.isPlaying)
		{
			photonViewList = new Dictionary<int, PhotonView>();
		}
		else
		{
			if (netView.subId == 0)
			{
				return;
			}
			if (photonViewList.ContainsKey(netView.viewID))
			{
				if (netView != photonViewList[netView.viewID])
				{
					Debug.LogError(string.Format("PhotonView ID duplicate found: {0}. New: {1} old: {2}. Maybe one wasn't destroyed on scene load?! Check for 'DontDestroyOnLoad'. Destroying old entry, adding new.", netView.viewID, netView, photonViewList[netView.viewID]));
				}
				RemoveInstantiatedGO(photonViewList[netView.viewID].gameObject, true);
			}
			photonViewList.Add(netView.viewID, netView);
			if ((int)base.DebugOut >= 5)
			{
				DebugReturn(DebugLevel.ALL, "Registered PhotonView: " + netView.viewID);
			}
		}
	}

	public void OpCleanRpcBuffer(int actorNumber)
	{
		OpRaiseEvent(200, null, true, 0, new int[1] { actorNumber }, EventCaching.RemoveFromRoomCache);
	}

	public void OpRemoveCompleteCacheOfPlayer(int actorNumber)
	{
		OpRaiseEvent(0, null, true, 0, new int[1] { actorNumber }, EventCaching.RemoveFromRoomCache);
	}

	public void OpRemoveCompleteCache()
	{
		OpRaiseEvent(0, null, true, 0, EventCaching.RemoveFromRoomCache, ReceiverGroup.MasterClient);
	}

	private void RemoveCacheOfLeftPlayers()
	{
		Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
		dictionary[244] = (byte)0;
		dictionary[247] = (byte)7;
		OpCustom(253, dictionary, true, 0);
	}

	public void CleanRpcBufferIfMine(PhotonView view)
	{
		if (view.ownerId != mLocalActor.ID && !mLocalActor.isMasterClient)
		{
			Debug.LogError(string.Concat("Cannot remove cached RPCs on a PhotonView thats not ours! ", view.owner, " scene: ", view.isSceneView));
		}
		else
		{
			OpCleanRpcBuffer(view);
		}
	}

	public void OpCleanRpcBuffer(PhotonView view)
	{
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable[(byte)0] = view.viewID;
		OpRaiseEvent(200, hashtable, true, 0, EventCaching.RemoveFromRoomCache, ReceiverGroup.Others);
	}

	public void RemoveRPCsInGroup(int group)
	{
		foreach (KeyValuePair<int, PhotonView> photonView in photonViewList)
		{
			PhotonView value = photonView.Value;
			if (value.group == group)
			{
				CleanRpcBufferIfMine(value);
			}
		}
	}

	public void SetLevelPrefix(short prefix)
	{
		currentLevelPrefix = prefix;
	}

	internal void RPC(PhotonView view, string methodName, PhotonPlayer player, params object[] parameters)
	{
		if (!blockSendingGroups.Contains(view.group))
		{
			if (view.viewID < 1)
			{
				Debug.LogError("Illegal view ID:" + view.viewID + " method: " + methodName + " GO:" + view.gameObject.name);
			}
			if ((int)base.DebugOut >= 3)
			{
				DebugReturn(DebugLevel.INFO, string.Concat("Sending RPC \"", methodName, "\" to player[", player, "]"));
			}
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable[(byte)0] = view.viewID;
			if (view.prefix > 0)
			{
				hashtable[(byte)1] = (short)view.prefix;
			}
			hashtable[(byte)2] = base.ServerTimeInMilliSeconds;
			int value = 0;
			if (rpcShortcuts.TryGetValue(methodName, out value))
			{
				hashtable[(byte)5] = (byte)value;
			}
			else
			{
				hashtable[(byte)3] = methodName;
			}
			if (parameters != null && parameters.Length > 0)
			{
				hashtable[(byte)4] = parameters;
			}
			if (mLocalActor == player)
			{
				ExecuteRPC(hashtable, player);
				return;
			}
			int[] targetActors = new int[1] { player.ID };
			OpRaiseEvent(200, hashtable, true, 0, targetActors);
		}
	}

	internal void RPC(PhotonView view, string methodName, PhotonTargets target, params object[] parameters)
	{
		if (blockSendingGroups.Contains(view.group))
		{
			return;
		}
		if (view.viewID < 1)
		{
			Debug.LogError("Illegal view ID:" + view.viewID + " method: " + methodName + " GO:" + view.gameObject.name);
		}
		if ((int)base.DebugOut >= 3)
		{
			DebugReturn(DebugLevel.INFO, "Sending RPC \"" + methodName + "\" to " + target);
		}
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable[(byte)0] = view.viewID;
		if (view.prefix > 0)
		{
			hashtable[(byte)1] = (short)view.prefix;
		}
		hashtable[(byte)2] = base.ServerTimeInMilliSeconds;
		int value = 0;
		if (rpcShortcuts.TryGetValue(methodName, out value))
		{
			hashtable[(byte)5] = (byte)value;
		}
		else
		{
			hashtable[(byte)3] = methodName;
		}
		if (parameters != null && parameters.Length > 0)
		{
			hashtable[(byte)4] = parameters;
		}
		switch (target)
		{
		case PhotonTargets.All:
			OpRaiseEvent(200, (byte)view.group, hashtable, true, 0);
			ExecuteRPC(hashtable, mLocalActor);
			break;
		case PhotonTargets.Others:
			OpRaiseEvent(200, (byte)view.group, hashtable, true, 0);
			break;
		case PhotonTargets.AllBuffered:
			OpRaiseEvent(200, hashtable, true, 0, EventCaching.AddToRoomCache, ReceiverGroup.Others);
			ExecuteRPC(hashtable, mLocalActor);
			break;
		case PhotonTargets.OthersBuffered:
			OpRaiseEvent(200, hashtable, true, 0, EventCaching.AddToRoomCache, ReceiverGroup.Others);
			break;
		case PhotonTargets.MasterClient:
			if (mMasterClient == mLocalActor)
			{
				ExecuteRPC(hashtable, mLocalActor);
			}
			else
			{
				OpRaiseEvent(200, hashtable, true, 0, EventCaching.DoNotCache, ReceiverGroup.MasterClient);
			}
			break;
		default:
			Debug.LogError("Unsupported target enum: " + target);
			break;
		}
	}

	public void SetReceivingEnabled(int group, bool enabled)
	{
		if (group <= 0)
		{
			Debug.LogError("Error: PhotonNetwork.SetReceivingEnabled was called with an illegal group number: " + group + ". The group number should be at least 1.");
		}
		else if (enabled)
		{
			if (!allowedReceivingGroups.Contains(group))
			{
				allowedReceivingGroups.Add(group);
				byte[] groupsToAdd = new byte[1] { (byte)group };
				OpChangeGroups(null, groupsToAdd);
			}
		}
		else if (allowedReceivingGroups.Contains(group))
		{
			allowedReceivingGroups.Remove(group);
			byte[] groupsToRemove = new byte[1] { (byte)group };
			OpChangeGroups(groupsToRemove, null);
		}
	}

	public void SetSendingEnabled(int group, bool enabled)
	{
		if (!enabled)
		{
			blockSendingGroups.Add(group);
		}
		else
		{
			blockSendingGroups.Remove(group);
		}
	}

	public void NewSceneLoaded()
	{
		if (loadingLevelAndPausedNetwork && !PhotonNetwork.isMessageQueueRunning)
		{
			loadingLevelAndPausedNetwork = false;
			PhotonNetwork.isMessageQueueRunning = true;
		}
		List<int> list = new List<int>();
		foreach (KeyValuePair<int, PhotonView> photonView in photonViewList)
		{
			PhotonView value = photonView.Value;
			if (value == null)
			{
				list.Add(photonView.Key);
			}
		}
		for (int i = 0; i < list.Count; i++)
		{
			int key = list[i];
			photonViewList.Remove(key);
		}
		if (list.Count > 0 && (int)base.DebugOut >= 3)
		{
			DebugReturn(DebugLevel.INFO, "Removed " + list.Count + " scene view IDs from last scene.");
		}
	}

	public void RunViewUpdate()
	{
		if (!PhotonNetwork.connected || PhotonNetwork.offlineMode || mActors == null || mActors.Count <= 1)
		{
			return;
		}
		Dictionary<int, ExitGames.Client.Photon.Hashtable> dictionary = new Dictionary<int, ExitGames.Client.Photon.Hashtable>();
		Dictionary<int, ExitGames.Client.Photon.Hashtable> dictionary2 = new Dictionary<int, ExitGames.Client.Photon.Hashtable>();
		foreach (KeyValuePair<int, PhotonView> photonView in photonViewList)
		{
			PhotonView value = photonView.Value;
			if (!(value.observed != null) || value.synchronization == ViewSynchronization.Off || (value.owner != mLocalActor && (!value.isSceneView || mMasterClient != mLocalActor)) || !value.gameObject.activeInHierarchy || blockSendingGroups.Contains(value.group))
			{
				continue;
			}
			ExitGames.Client.Photon.Hashtable hashtable = OnSerializeWrite(value);
			if (hashtable == null)
			{
				continue;
			}
			if (value.synchronization == ViewSynchronization.ReliableDeltaCompressed || value.mixedModeIsReliable)
			{
				if (!hashtable.ContainsKey((byte)1) && !hashtable.ContainsKey((byte)2))
				{
					continue;
				}
				if (!dictionary.ContainsKey(value.group))
				{
					dictionary[value.group] = new ExitGames.Client.Photon.Hashtable();
					dictionary[value.group][(byte)0] = base.ServerTimeInMilliSeconds;
					if (currentLevelPrefix >= 0)
					{
						dictionary[value.group][(byte)1] = currentLevelPrefix;
					}
				}
				ExitGames.Client.Photon.Hashtable hashtable2 = dictionary[value.group];
				hashtable2.Add((short)hashtable2.Count, hashtable);
				continue;
			}
			if (!dictionary2.ContainsKey(value.group))
			{
				dictionary2[value.group] = new ExitGames.Client.Photon.Hashtable();
				dictionary2[value.group][(byte)0] = base.ServerTimeInMilliSeconds;
				if (currentLevelPrefix >= 0)
				{
					dictionary2[value.group][(byte)1] = currentLevelPrefix;
				}
			}
			ExitGames.Client.Photon.Hashtable hashtable3 = dictionary2[value.group];
			hashtable3.Add((short)hashtable3.Count, hashtable);
		}
		foreach (KeyValuePair<int, ExitGames.Client.Photon.Hashtable> item in dictionary)
		{
			OpRaiseEvent(206, (byte)item.Key, item.Value, true, 0);
		}
		foreach (KeyValuePair<int, ExitGames.Client.Photon.Hashtable> item2 in dictionary2)
		{
			OpRaiseEvent(201, (byte)item2.Key, item2.Value, false, 0);
		}
	}

	private ExitGames.Client.Photon.Hashtable OnSerializeWrite(PhotonView view)
	{
		List<object> list = new List<object>();
		if (view.observed is MonoBehaviour)
		{
			PhotonStream photonStream = new PhotonStream(true, null);
			PhotonMessageInfo info = new PhotonMessageInfo(mLocalActor, base.ServerTimeInMilliSeconds, view);
			view.ExecuteOnSerialize(photonStream, info);
			if (photonStream.Count == 0)
			{
				return null;
			}
			list = photonStream.data;
		}
		else if (view.observed is Transform)
		{
			Transform transform = (Transform)view.observed;
			if (view.onSerializeTransformOption == OnSerializeTransform.OnlyPosition || view.onSerializeTransformOption == OnSerializeTransform.PositionAndRotation || view.onSerializeTransformOption == OnSerializeTransform.All)
			{
				list.Add(transform.localPosition);
			}
			else
			{
				list.Add(null);
			}
			if (view.onSerializeTransformOption == OnSerializeTransform.OnlyRotation || view.onSerializeTransformOption == OnSerializeTransform.PositionAndRotation || view.onSerializeTransformOption == OnSerializeTransform.All)
			{
				list.Add(transform.localRotation);
			}
			else
			{
				list.Add(null);
			}
			if (view.onSerializeTransformOption == OnSerializeTransform.OnlyScale || view.onSerializeTransformOption == OnSerializeTransform.All)
			{
				list.Add(transform.localScale);
			}
		}
		else
		{
			if (!(view.observed is Rigidbody))
			{
				Debug.LogError("Observed type is not serializable: " + view.observed.GetType());
				return null;
			}
			Rigidbody rigidbody = (Rigidbody)view.observed;
			if (view.onSerializeRigidBodyOption != OnSerializeRigidBody.OnlyAngularVelocity)
			{
				list.Add(rigidbody.velocity);
			}
			else
			{
				list.Add(null);
			}
			if (view.onSerializeRigidBodyOption != 0)
			{
				list.Add(rigidbody.angularVelocity);
			}
		}
		object[] array = list.ToArray();
		if (view.synchronization == ViewSynchronization.UnreliableOnChange)
		{
			if (AlmostEquals(array, view.lastOnSerializeDataSent))
			{
				if (view.mixedModeIsReliable)
				{
					return null;
				}
				view.mixedModeIsReliable = true;
				view.lastOnSerializeDataSent = array;
			}
			else
			{
				view.mixedModeIsReliable = false;
				view.lastOnSerializeDataSent = array;
			}
		}
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable[(byte)0] = view.viewID;
		hashtable[(byte)1] = array;
		if (view.synchronization == ViewSynchronization.ReliableDeltaCompressed)
		{
			bool flag = DeltaCompressionWrite(view, hashtable);
			view.lastOnSerializeDataSent = array;
			if (!flag)
			{
				return null;
			}
		}
		return hashtable;
	}

	private void OnSerializeRead(ExitGames.Client.Photon.Hashtable data, PhotonPlayer sender, int networkTime, short correctPrefix)
	{
		int num = (int)data[(byte)0];
		PhotonView photonView = GetPhotonView(num);
		if (photonView == null)
		{
			Debug.LogWarning("Received OnSerialization for view ID " + num + ". We have no such PhotonView! Ignored this if you're leaving a room. State: " + State);
		}
		else if (photonView.prefix > 0 && correctPrefix != photonView.prefix)
		{
			Debug.LogError("Received OnSerialization for view ID " + num + " with prefix " + correctPrefix + ". Our prefix is " + photonView.prefix);
		}
		else
		{
			if (photonView.group != 0 && !allowedReceivingGroups.Contains(photonView.group))
			{
				return;
			}
			if (photonView.synchronization == ViewSynchronization.ReliableDeltaCompressed)
			{
				if (!DeltaCompressionRead(photonView, data))
				{
					DebugReturn(DebugLevel.INFO, "Skipping packet for " + photonView.name + " [" + photonView.viewID + "] as we haven't received a full packet for delta compression yet. This is OK if it happens for the first few frames after joining a game.");
					return;
				}
				photonView.lastOnSerializeDataReceived = data[(byte)1] as object[];
			}
			if (photonView.observed is MonoBehaviour)
			{
				object[] incomingData = data[(byte)1] as object[];
				PhotonStream pStream = new PhotonStream(false, incomingData);
				PhotonMessageInfo info = new PhotonMessageInfo(sender, networkTime, photonView);
				photonView.ExecuteOnSerialize(pStream, info);
			}
			else if (photonView.observed is Transform)
			{
				object[] array = data[(byte)1] as object[];
				Transform transform = (Transform)photonView.observed;
				if (array.Length >= 1 && array[0] != null)
				{
					transform.localPosition = (Vector3)array[0];
				}
				if (array.Length >= 2 && array[1] != null)
				{
					transform.localRotation = (Quaternion)array[1];
				}
				if (array.Length >= 3 && array[2] != null)
				{
					transform.localScale = (Vector3)array[2];
				}
			}
			else if (photonView.observed is Rigidbody)
			{
				object[] array2 = data[(byte)1] as object[];
				Rigidbody rigidbody = (Rigidbody)photonView.observed;
				if (array2.Length >= 1 && array2[0] != null)
				{
					rigidbody.velocity = (Vector3)array2[0];
				}
				if (array2.Length >= 2 && array2[1] != null)
				{
					rigidbody.angularVelocity = (Vector3)array2[1];
				}
			}
			else
			{
				Debug.LogError("Type of observed is unknown when receiving.");
			}
		}
	}

	private bool AlmostEquals(object[] lastData, object[] currentContent)
	{
		if (lastData == null && currentContent == null)
		{
			return true;
		}
		if (lastData == null || currentContent == null || lastData.Length != currentContent.Length)
		{
			return false;
		}
		for (int i = 0; i < currentContent.Length; i++)
		{
			object one = currentContent[i];
			object two = lastData[i];
			if (!ObjectIsSameWithInprecision(one, two))
			{
				return false;
			}
		}
		return true;
	}

	private bool DeltaCompressionWrite(PhotonView view, ExitGames.Client.Photon.Hashtable data)
	{
		if (view.lastOnSerializeDataSent == null)
		{
			return true;
		}
		object[] lastOnSerializeDataSent = view.lastOnSerializeDataSent;
		object[] array = data[(byte)1] as object[];
		if (array == null)
		{
			return false;
		}
		if (lastOnSerializeDataSent.Length != array.Length)
		{
			return true;
		}
		object[] array2 = new object[array.Length];
		int num = 0;
		List<int> list = new List<int>();
		for (int i = 0; i < array2.Length; i++)
		{
			object obj = array[i];
			object two = lastOnSerializeDataSent[i];
			if (ObjectIsSameWithInprecision(obj, two))
			{
				num++;
				continue;
			}
			array2[i] = array[i];
			if (obj == null)
			{
				list.Add(i);
			}
		}
		if (num > 0)
		{
			data.Remove((byte)1);
			if (num == array.Length)
			{
				return false;
			}
			data[(byte)2] = array2;
			if (list.Count > 0)
			{
				data[(byte)3] = list.ToArray();
			}
		}
		return true;
	}

	private bool DeltaCompressionRead(PhotonView view, ExitGames.Client.Photon.Hashtable data)
	{
		if (data.ContainsKey((byte)1))
		{
			return true;
		}
		if (view.lastOnSerializeDataReceived == null)
		{
			return false;
		}
		object[] array = data[(byte)2] as object[];
		if (array == null)
		{
			return false;
		}
		int[] array2 = data[(byte)3] as int[];
		if (array2 == null)
		{
			array2 = new int[0];
		}
		object[] lastOnSerializeDataReceived = view.lastOnSerializeDataReceived;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] == null && !array2.Contains(i))
			{
				object obj = lastOnSerializeDataReceived[i];
				array[i] = obj;
			}
		}
		data[(byte)1] = array;
		return true;
	}

	private bool ObjectIsSameWithInprecision(object one, object two)
	{
		if (one == null || two == null)
		{
			return one == null && two == null;
		}
		if (!one.Equals(two))
		{
			if (one is Vector3)
			{
				Vector3 target = (Vector3)one;
				Vector3 second = (Vector3)two;
				if (target.AlmostEquals(second, PhotonNetwork.precisionForVectorSynchronization))
				{
					return true;
				}
			}
			else if (one is Vector2)
			{
				Vector2 target2 = (Vector2)one;
				Vector2 second2 = (Vector2)two;
				if (target2.AlmostEquals(second2, PhotonNetwork.precisionForVectorSynchronization))
				{
					return true;
				}
			}
			else if (one is Quaternion)
			{
				Quaternion target3 = (Quaternion)one;
				Quaternion second3 = (Quaternion)two;
				if (target3.AlmostEquals(second3, PhotonNetwork.precisionForQuaternionSynchronization))
				{
					return true;
				}
			}
			else if (one is float)
			{
				float target4 = (float)one;
				float second4 = (float)two;
				if (target4.AlmostEquals(second4, PhotonNetwork.precisionForFloatSynchronization))
				{
					return true;
				}
			}
			return false;
		}
		return true;
	}

	protected internal static bool GetMethod(MonoBehaviour monob, string methodType, out MethodInfo mi)
	{
		mi = null;
		if (monob == null || string.IsNullOrEmpty(methodType))
		{
			return false;
		}
		List<MethodInfo> methods = SupportClass.GetMethods(monob.GetType(), null);
		for (int i = 0; i < methods.Count; i++)
		{
			MethodInfo methodInfo = methods[i];
			if (methodInfo.Name.Equals(methodType))
			{
				mi = methodInfo;
				return true;
			}
		}
		return false;
	}

	protected internal void AutomaticallySyncScene()
	{
		if (PhotonNetwork.room == null || !PhotonNetwork.automaticallySyncScene || PhotonNetwork.isMasterClient)
		{
			return;
		}
		string text = (string)PhotonNetwork.room.customProperties["curScn"];
		if (!string.IsNullOrEmpty(text))
		{
			if (text != Application.loadedLevelName)
			{
				PhotonNetwork.LoadLevel(text);
			}
			else if ((int)base.DebugOut >= 2)
			{
				DebugReturn(DebugLevel.WARNING, "Skipped re-loading level due to scene syncing. Level already loaded.");
			}
		}
	}
}
