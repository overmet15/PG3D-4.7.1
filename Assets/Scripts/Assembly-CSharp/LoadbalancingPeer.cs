using System.Collections.Generic;
using ExitGames.Client.Photon;
using ExitGames.Client.Photon.Lite;

internal class LoadbalancingPeer : PhotonPeer
{
	public LoadbalancingPeer(IPhotonPeerListener listener, ConnectionProtocol protocolType)
		: base(listener, protocolType)
	{
	}

	public virtual bool OpJoinLobby()
	{
		if ((int)base.DebugOut >= 3)
		{
			base.Listener.DebugReturn(DebugLevel.INFO, "OpJoinLobby()");
		}
		return OpCustom(229, null, true);
	}

	public virtual bool OpLeaveLobby()
	{
		if ((int)base.DebugOut >= 3)
		{
			base.Listener.DebugReturn(DebugLevel.INFO, "OpLeaveLobby()");
		}
		return OpCustom(228, null, true);
	}

	public virtual bool OpCreateRoom(string gameID, bool isVisible, bool isOpen, byte maxPlayers, bool autoCleanUp, Hashtable customGameProperties, Hashtable customPlayerProperties, string[] customRoomPropertiesForLobby)
	{
		if ((int)base.DebugOut >= 3)
		{
			base.Listener.DebugReturn(DebugLevel.INFO, "OpCreateRoom()");
		}
		Hashtable hashtable = new Hashtable();
		hashtable[(byte)253] = isOpen;
		hashtable[(byte)254] = isVisible;
		hashtable[(byte)250] = customRoomPropertiesForLobby;
		hashtable.MergeStringKeys(customGameProperties);
		if (maxPlayers > 0)
		{
			hashtable[byte.MaxValue] = maxPlayers;
		}
		Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
		dictionary[248] = hashtable;
		dictionary[250] = true;
		if (customPlayerProperties != null)
		{
			dictionary[249] = customPlayerProperties;
		}
		if (!string.IsNullOrEmpty(gameID))
		{
			dictionary[byte.MaxValue] = gameID;
		}
		if (autoCleanUp)
		{
			dictionary[241] = autoCleanUp;
			hashtable[(byte)249] = autoCleanUp;
		}
		return OpCustom(227, dictionary, true);
	}

	public virtual bool OpJoinRoom(string roomName, Hashtable playerProperties, bool createIfNotExists)
	{
		if ((int)base.DebugOut >= 3)
		{
			base.Listener.DebugReturn(DebugLevel.INFO, "OpJoinRoom()");
		}
		if (string.IsNullOrEmpty(roomName))
		{
			base.Listener.DebugReturn(DebugLevel.ERROR, "OpJoinRoom() failed. Please specify a roomname.");
			return false;
		}
		Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
		dictionary[byte.MaxValue] = roomName;
		dictionary[250] = true;
		if (createIfNotExists)
		{
			dictionary[215] = createIfNotExists;
		}
		if (playerProperties != null)
		{
			dictionary[249] = playerProperties;
		}
		return OpCustom(226, dictionary, true);
	}

	public virtual bool OpJoinRandomRoom(Hashtable expectedCustomRoomProperties, byte expectedMaxPlayers, Hashtable playerProperties, MatchmakingMode matchingType)
	{
		if ((int)base.DebugOut >= 3)
		{
			base.Listener.DebugReturn(DebugLevel.INFO, "OpJoinRandomRoom()");
		}
		Hashtable hashtable = new Hashtable();
		hashtable.MergeStringKeys(expectedCustomRoomProperties);
		if (expectedMaxPlayers > 0)
		{
			hashtable[byte.MaxValue] = expectedMaxPlayers;
		}
		Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
		if (hashtable.Count > 0)
		{
			dictionary[248] = hashtable;
		}
		if (playerProperties != null && playerProperties.Count > 0)
		{
			dictionary[249] = playerProperties;
		}
		if (matchingType != 0)
		{
			dictionary[223] = (byte)matchingType;
		}
		return OpCustom(225, dictionary, true);
	}

	public virtual bool OpFindFriends(string[] friendsToFind)
	{
		Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
		if (friendsToFind != null && friendsToFind.Length > 0)
		{
			dictionary[1] = friendsToFind;
		}
		return OpCustom(222, dictionary, true);
	}

	public bool OpSetCustomPropertiesOfActor(int actorNr, Hashtable actorProperties, bool broadcast, byte channelId)
	{
		return OpSetPropertiesOfActor(actorNr, actorProperties.StripToStringKeys(), broadcast, channelId);
	}

	protected bool OpSetPropertiesOfActor(int actorNr, Hashtable actorProperties, bool broadcast, byte channelId)
	{
		if ((int)base.DebugOut >= 3)
		{
			base.Listener.DebugReturn(DebugLevel.INFO, "OpSetPropertiesOfActor()");
		}
		if (actorNr <= 0 || actorProperties == null)
		{
			if ((int)base.DebugOut >= 3)
			{
				base.Listener.DebugReturn(DebugLevel.INFO, "OpSetPropertiesOfActor not sent. ActorNr must be > 0 and actorProperties != null.");
			}
			return false;
		}
		Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
		dictionary.Add(251, actorProperties);
		dictionary.Add(254, actorNr);
		if (broadcast)
		{
			dictionary.Add(250, broadcast);
		}
		return OpCustom(252, dictionary, broadcast, channelId);
	}

	protected void OpSetPropertyOfRoom(byte propCode, object value)
	{
		Hashtable hashtable = new Hashtable();
		hashtable[propCode] = value;
		OpSetPropertiesOfRoom(hashtable, true, 0);
	}

	public bool OpSetCustomPropertiesOfRoom(Hashtable gameProperties, bool broadcast, byte channelId)
	{
		return OpSetPropertiesOfRoom(gameProperties.StripToStringKeys(), broadcast, channelId);
	}

	public bool OpSetPropertiesOfRoom(Hashtable gameProperties, bool broadcast, byte channelId)
	{
		if ((int)base.DebugOut >= 3)
		{
			base.Listener.DebugReturn(DebugLevel.INFO, "OpSetPropertiesOfRoom()");
		}
		Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
		dictionary.Add(251, gameProperties);
		if (broadcast)
		{
			dictionary.Add(250, broadcast);
		}
		return OpCustom(252, dictionary, broadcast, channelId);
	}

	public virtual bool OpAuthenticate(string appId, string appVersion, string userId, AuthenticationValues authValues)
	{
		if ((int)base.DebugOut >= 3)
		{
			base.Listener.DebugReturn(DebugLevel.INFO, "OpAuthenticate()");
		}
		Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
		dictionary[220] = appVersion;
		dictionary[224] = appId;
		if (!string.IsNullOrEmpty(userId))
		{
			dictionary[225] = userId;
		}
		if (authValues != null && authValues.AuthType != CustomAuthenticationType.None)
		{
			if (!base.IsEncryptionAvailable)
			{
				base.Listener.DebugReturn(DebugLevel.ERROR, "OpAuthenticate() failed. When you want Custom Authentication encryption is mandatory.");
				return false;
			}
			dictionary[217] = (byte)authValues.AuthType;
			if (!string.IsNullOrEmpty(authValues.Secret))
			{
				dictionary[221] = authValues.Secret;
			}
			else
			{
				if (!string.IsNullOrEmpty(authValues.AuthParameters))
				{
					dictionary[216] = authValues.AuthParameters;
				}
				if (authValues.AuthPostData != null)
				{
					dictionary[214] = authValues.AuthPostData;
				}
			}
		}
		return OpCustom(230, dictionary, true, 0, base.IsEncryptionAvailable);
	}

	public virtual bool OpChangeGroups(byte[] groupsToRemove, byte[] groupsToAdd)
	{
		if ((int)base.DebugOut >= 5)
		{
			base.Listener.DebugReturn(DebugLevel.ALL, "OpChangeGroups()");
		}
		Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
		if (groupsToRemove != null)
		{
			dictionary[239] = groupsToRemove;
		}
		if (groupsToAdd != null)
		{
			dictionary[238] = groupsToAdd;
		}
		return OpCustom(248, dictionary, true, 0);
	}

	public virtual bool OpRaiseEvent(byte eventCode, bool sendReliable, object customEventContent)
	{
		return OpRaiseEvent(eventCode, sendReliable, customEventContent, 0, EventCaching.DoNotCache, null, ReceiverGroup.Others, 0);
	}

	public virtual bool OpRaiseEvent(byte eventCode, bool sendReliable, object customEventContent, byte channelId, EventCaching cache, int[] targetActors, ReceiverGroup receivers, byte interestGroup)
	{
		Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
		dictionary[244] = eventCode;
		if (customEventContent != null)
		{
			dictionary[245] = customEventContent;
		}
		if (cache != 0)
		{
			dictionary[247] = (byte)cache;
		}
		if (receivers != 0)
		{
			dictionary[246] = (byte)receivers;
		}
		if (interestGroup != 0)
		{
			dictionary[240] = interestGroup;
		}
		if (targetActors != null)
		{
			dictionary[252] = targetActors;
		}
		return OpCustom(253, dictionary, sendReliable, channelId, false);
	}

	public virtual bool OpRaiseEvent(byte eventCode, byte interestGroup, Hashtable customEventContent, bool sendReliable, byte channelId)
	{
		Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
		dictionary[245] = customEventContent;
		dictionary[244] = eventCode;
		if (interestGroup != 0)
		{
			dictionary[240] = interestGroup;
		}
		return OpCustom(253, dictionary, sendReliable, channelId);
	}

	public virtual bool OpRaiseEvent(byte eventCode, Hashtable evData, bool sendReliable, byte channelId)
	{
		return OpRaiseEvent(eventCode, evData, sendReliable, channelId, EventCaching.DoNotCache, ReceiverGroup.Others);
	}

	public virtual bool OpRaiseEvent(byte eventCode, Hashtable evData, bool sendReliable, byte channelId, int[] targetActors)
	{
		return OpRaiseEvent(eventCode, evData, sendReliable, channelId, targetActors, EventCaching.DoNotCache);
	}

	public virtual bool OpRaiseEvent(byte eventCode, Hashtable evData, bool sendReliable, byte channelId, int[] targetActors, EventCaching cache)
	{
		if ((int)base.DebugOut >= 3)
		{
			base.Listener.DebugReturn(DebugLevel.INFO, "OpRaiseEvent()");
		}
		Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
		dictionary[245] = evData;
		dictionary[244] = eventCode;
		if (cache != 0)
		{
			dictionary[247] = (byte)cache;
		}
		if (targetActors != null)
		{
			dictionary[252] = targetActors;
		}
		return OpCustom(253, dictionary, sendReliable, channelId);
	}

	public virtual bool OpRaiseEvent(byte eventCode, Hashtable evData, bool sendReliable, byte channelId, EventCaching cache, ReceiverGroup receivers)
	{
		if ((int)base.DebugOut >= 3)
		{
			base.Listener.DebugReturn(DebugLevel.INFO, "OpRaiseEvent()");
		}
		Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
		dictionary[245] = evData;
		dictionary[244] = eventCode;
		if (receivers != 0)
		{
			dictionary[246] = (byte)receivers;
		}
		if (cache != 0)
		{
			dictionary[247] = (byte)cache;
		}
		return OpCustom(253, dictionary, sendReliable, channelId);
	}
}
