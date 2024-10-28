using System;
using System.Collections.Generic;
using Prime31;
using Fuckhead.PixlGun3D;
using UnityEngine;
using System.Linq.Expressions;

public sealed class NetworkStartTable : MonoBehaviour
{
	public struct infoClient
	{
		public string ipAddress;

		public string name;

		public string coments;
	}

	public Texture mySkin;

	public Texture blueNotTexture;

	public Texture redNotTexture;

	public Texture plashkaStartMultu;

	public Texture plashkaStartCoop;

	public Texture plashkaStartCompany;

	public Texture kubochek;

	public Texture zagolovokWiner;

	public Texture zagolovokWiner_2;

	public Texture zagolovokStart;

	public Texture zagolovokStartCOOP;

	public Texture zagolovogTeam;

	public List<GameObject> zombiePrefabs = new List<GameObject>();

	private GameObject _playerPrefab;

	public GameObject tempCam;

	public GameObject zombieManagerPrefab;

	public Texture2D serverLeftTheGame;

	public ExperienceController experienceController;

	private int addCoins;

	private bool showMessagFacebook;

	private bool showMessagTiwtter;

	private bool clickButtonFacebook;

	public bool isIwin;

	public int myCommand;

	public int myCommandOld;

	public GUIStyle blueButton;

	public GUIStyle redButton;

	private bool isLocal;

	private bool isMine;

	private bool isCOOP;

	private bool isServer;

	private bool isCompany;

	private bool isMulti;

	private bool isInet;

	public Texture tableFon;

	public Texture tableFonCOOP;

	public Texture tableFonCommand;

	private float timeNotRunZombiManager;

	private bool isSendZaprosZombiManager;

	private bool isGetZaprosZombiManager;

	private ExperienceController expController;

	public List<infoClient> players = new List<infoClient>();

	public GUIStyle back;

	public GUIStyle start;

	public GUIStyle restart;

	public GUIStyle playersWindow;

	public GUIStyle playersWindowFrags;

	public GUIStyle twitterStyle;

	public GUIStyle facebookStyle;

	public GUIStyle labelStyle;

	public GUIStyle messagesStyle;

	public Texture head_players;

	public Texture nicksStyle;

	public Texture killsStyle;

	public Texture scoreTexture;

	private Vector2 scrollPosition = Vector2.zero;

	public GameObject _purchaseActivityIndicator;

	private float koofScreen = (float)Screen.height / 768f;

	public WeaponManager _weaponManager;

	public bool showTable;

	public string nickPobeditelya;

	public bool isShowNickTable;

	public bool runGame = true;

	public GUIStyle zagolovokStyle;

	public GameObject[] zoneCreatePlayer;

	private GameObject _cam;

	public bool showDisconnectFromServer;

	public bool showDisconnectFromMasterServer;

	private float timerShow = -1f;

	public string NamePlayer = "Unname";

	public int CountKills;

	public int oldCountKills;

	public string[] oldSpisokName;

	public string[] oldCountLilsSpisok;

	public int[] oldSpisokRanks;

	public string[] oldSpisokNameBlue;

	public string[] oldCountLilsSpisokBlue;

	public int[] oldSpisokRanksBlue;

	public string[] oldSpisokNameRed;

	public string[] oldCountLilsSpisokRed;

	public int[] oldSpisokRanksRed;

	public int oldIndexMy;

	private GameObject tc;

	public float score;

	public float scoreOld;

	private PhotonView photonView;

	private float timeTomig = 0.5f;

	private int countMigZagolovok;

	private int commandWinner;

	private bool isMigZag;

	private bool _canUserUseFacebookComposer;

	private bool _hasPublishPermission;

	private bool _hasPublishActions;

	public int myRanks = 1;

	private string _SocialMessage()
	{
		if (!Storager.hasKey(Defs.COOPScore))
		{
			Storager.setInt(Defs.COOPScore, 0, false);
		}
		int @int = Storager.getInt(Defs.COOPScore, false);
		bool flag = PlayerPrefs.GetInt("COOP", 0) == 1;
		int int2 = PlayerPrefs.GetInt("Rating", 0);
		string applicationUrl = Defs.ApplicationUrl;
		if (isIwin)
		{
			return (!flag) ? string.Format("Now I have {0} wins  in Pixel Gun 3D! Try it right now! {1}", int2, applicationUrl) : string.Format("Now I have {0} score in Pixel Gun 3D! Try it right now! {1}", @int, applicationUrl);
		}
		return (!flag) ? string.Format("I won {0} matches in Pixel Gun 3D! Try it right now! {1}", int2, applicationUrl) : string.Format("I received {0} points in Pixel Gun 3D! Try it right now! {1}", @int, applicationUrl);
	}

	private string _SocialSentSuccess(string SocialName)
	{
		return "Message was sent to " + SocialName;
	}

	private void completionHandler(string error, object result)
	{
		if (error != null)
		{
			Debug.LogError(error);
		}
		else
		{
			Utils.logObject(result);
		}
	}

	private void Awake()
	{
		isLocal = PlayerPrefs.GetString("TypeConnect").Equals("local");
		isInet = PlayerPrefs.GetString("TypeConnect").Equals("inet");
		isCOOP = PlayerPrefs.GetInt("COOP", 0) == 1;
		isServer = PlayerPrefs.GetString("TypeGame").Equals("server");
		isMulti = PlayerPrefs.GetInt("MultyPlayer") == 1;
		isCompany = isInet && PlayerPrefs.GetInt("company", 0) == 1;
		experienceController = FindObjectOfType<ExperienceController>();
		NamePlayer = "Unname";
		string[] array = null;
		array = new string[10] { "1", "15", "14", "2", "3", "9", "11", "12", "10", "16" };
		string[] array2 = array;
		foreach (string text in array2)
		{
			GameObject item = Resources.Load("Enemies/Enemy" + text + "_go") as GameObject;
			zombiePrefabs.Add(item);
		}
	}

	public void setScoreFromGlobalGameController()
	{
		score = GlobalGameController.Score;
		synchState();
	}

	[RPC]
	private void addPlayer(string _name, string _ip)
	{
		_weaponManager = FindObjectOfType<WeaponManager>();
		WeaponManager.infoClient item = default(WeaponManager.infoClient);
		item.name = _name;
		item.ipAddress = _ip;
		_weaponManager.players.Add(item);
	}

	[RPC]
	private void RunGame()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("NetworkTable");
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			gameObject.GetComponent<NetworkStartTable>().runGame = true;
		}
	}

	[RPC]
	private void delPlayer(string _name)
	{
		_weaponManager = GameObject.FindGameObjectWithTag("WeaponManager").GetComponent<WeaponManager>();
		for (int i = 0; i < _weaponManager.players.Count; i++)
		{
			if (_weaponManager.players[i].name.Equals(_name))
			{
				_weaponManager.players.RemoveAt(i);
			}
		}
	}

	public void sendDelMyPlayer()
	{
		if (!isInet)
		{
			if (base.GetComponent<NetworkView>().isMine)
			{
				base.GetComponent<NetworkView>().RPC("delPlayer", RPCMode.Others, PlayerPrefs.GetString("NamePlayer", Defs.defaultPlayerName));
			}
		}
		else if (photonView.isMine)
		{
			photonView.RPC("delPlayer", PhotonTargets.Others, PlayerPrefs.GetString("NamePlayer", Defs.defaultPlayerName));
		}
	}

	private void playersTable()
	{
		GUIStyle gUIStyle = ((!isShowNickTable) ? start : restart);
		if (isCOOP)
		{
			Texture texture = scoreTexture;
		}
		else
		{
			Texture texture = killsStyle;
		}
		if ((isServer || runGame || isInet) && (!isShowNickTable || isInet))
		{
			if (isInet && isCompany)
			{
				int num = 0;
				int num2 = 0;
				GameObject[] array = GameObject.FindGameObjectsWithTag("NetworkTable");
				foreach (GameObject gameObject in array)
				{
					if (gameObject.GetComponent<NetworkStartTable>().myCommand == 1)
					{
						num++;
					}
					if (gameObject.GetComponent<NetworkStartTable>().myCommand == 2)
					{
						num2++;
					}
				}
				bool flag = true;
				bool flag2 = true;
				if (PhotonNetwork.room != null && (num >= PhotonNetwork.room.maxPlayers / 2 || num - num2 > 1))
				{
					flag = false;
				}
				if (PhotonNetwork.room != null && (num2 >= PhotonNetwork.room.maxPlayers / 2 || num2 - num > 1))
				{
					flag2 = false;
				}
				if (flag)
				{
					if (GUI.Button(new Rect((float)Screen.width * 0.5f - (float)blueButton.normal.background.width * 1.1f * koofScreen, (float)Screen.height * 0.9f - (float)blueButton.normal.background.height * 0.5f * koofScreen, (float)blueButton.normal.background.width * koofScreen, (float)blueButton.normal.background.height * koofScreen), string.Empty, blueButton))
					{
						isShowNickTable = false;
						CountKills = 0;
						score = 0f;
						GlobalGameController.Score = 0;
						myCommand = 1;
						startPlayer();
						countMigZagolovok = 0;
						timeTomig = 0.7f;
						isMigZag = false;
						synchState();
						return;
					}
				}
				else
				{
					GUI.DrawTexture(new Rect((float)Screen.width * 0.5f - (float)blueButton.normal.background.width * 1.1f * koofScreen, (float)Screen.height * 0.9f - (float)blueButton.normal.background.height * 0.5f * koofScreen, (float)blueButton.normal.background.width * koofScreen, (float)blueButton.normal.background.height * koofScreen), blueNotTexture);
				}
				if (flag2)
				{
					if (GUI.Button(new Rect((float)Screen.width * 0.5f + (float)blueButton.normal.background.width * 0.1f * koofScreen, (float)Screen.height * 0.9f - (float)blueButton.normal.background.height * 0.5f * koofScreen, (float)blueButton.normal.background.width * koofScreen, (float)blueButton.normal.background.height * koofScreen), string.Empty, redButton))
					{
						isShowNickTable = false;
						CountKills = 0;
						score = 0f;
						GlobalGameController.Score = 0;
						myCommand = 2;
						startPlayer();
						countMigZagolovok = 0;
						timeTomig = 0.7f;
						isMigZag = false;
						synchState();
						return;
					}
				}
				else
				{
					GUI.DrawTexture(new Rect((float)Screen.width * 0.5f + (float)blueButton.normal.background.width * 0.1f * koofScreen, (float)Screen.height * 0.9f - (float)blueButton.normal.background.height * 0.5f * koofScreen, (float)blueButton.normal.background.width * koofScreen, (float)blueButton.normal.background.height * koofScreen), redNotTexture);
				}
			}
			else if (GUI.Button(new Rect((float)Screen.width * 0.9f - (float)gUIStyle.normal.background.width / 2f * (float)Screen.height / 768f, (float)Screen.height * 0.9f - (float)gUIStyle.normal.background.height / 2f * (float)Screen.height / 768f, (float)(gUIStyle.normal.background.width * Screen.height) / 768f, (float)(gUIStyle.normal.background.height * Screen.height) / 768f), string.Empty, gUIStyle))
			{
				isShowNickTable = false;
				CountKills = 0;
				score = 0f;
				GlobalGameController.Score = 0;
				startPlayer();
				countMigZagolovok = 0;
				timeTomig = 0.7f;
				isMigZag = false;
				synchState();
				return;
			}
		}
		Rect position = new Rect((float)Screen.width * 0.5f - 320f * koofScreen, (float)Screen.height * 0.515f - 222.5f * koofScreen, 640f * koofScreen, 445f * koofScreen);
		float num3 = 49f * koofScreen;
		float num4 = position.height * 0.5f + 45f * koofScreen;
		float num5 = 17f * koofScreen;
		float num6 = 26f * koofScreen;
		float num7 = 22f * koofScreen;
		float num8 = 26f * koofScreen;
		playersWindow.fontSize = Mathf.RoundToInt(num7);
		playersWindowFrags.fontSize = Mathf.RoundToInt(num7);
		playersWindowFrags.alignment = TextAnchor.MiddleRight;
		playersWindow.alignment = TextAnchor.MiddleLeft;
		GUI.DrawTexture(position, isCompany ? tableFonCommand : ((!isCOOP) ? tableFon : tableFonCOOP));
		if (showTable)
		{
			GameObject[] array2 = GameObject.FindGameObjectsWithTag("NetworkTable");
			List<GameObject> list = new List<GameObject>();
			List<GameObject> list2 = new List<GameObject>();
			List<GameObject> list3 = new List<GameObject>();
			for (int j = 1; j < array2.Length; j++)
			{
				GameObject gameObject2 = array2[j];
				int num9 = j - 1;
				while (num9 >= 0 && ((!isCOOP) ? ((float)array2[num9].GetComponent<NetworkStartTable>().CountKills) : array2[num9].GetComponent<NetworkStartTable>().score) < ((!isCOOP) ? ((float)gameObject2.GetComponent<NetworkStartTable>().CountKills) : gameObject2.GetComponent<NetworkStartTable>().score))
				{
					array2[num9 + 1] = array2[num9];
					num9--;
				}
				array2[num9 + 1] = gameObject2;
			}
			for (int k = 0; k < array2.Length; k++)
			{
				if (array2[k].GetComponent<NetworkStartTable>().myCommand == 0)
				{
					list3.Add(array2[k]);
				}
				if (array2[k].GetComponent<NetworkStartTable>().myCommand == 1)
				{
					list.Add(array2[k]);
				}
				if (array2[k].GetComponent<NetworkStartTable>().myCommand == 2)
				{
					list2.Add(array2[k]);
				}
			}
			for (int l = 0; l < array2.Length; l++)
			{
				if (l < list.Count)
				{
					array2[l] = list[l];
				}
				if (l >= list.Count && l < list.Count + list2.Count)
				{
					array2[l] = list2[l - list.Count];
				}
				if (!isCompany && l >= list.Count + list2.Count)
				{
					array2[l] = list3[l - list.Count - list2.Count];
				}
			}
			if (array2.Length > 0)
			{
				for (int m = 0; m < array2.Length; m++)
				{
					GameObject gameObject3 = array2[m];
					if (_weaponManager == null) _weaponManager = FindObjectOfType<WeaponManager>();
					if (gameObject3 == _weaponManager.myTable)
					{
						Color textColor = new Color(1f, 1f, 0f, 1f);
						if (myCommand == 1)
						{
							textColor = new Color(1f, 1f, 0f, 1f);
						}
						if (myCommand == 2)
						{
							textColor = new Color(1f, 1f, 0f, 1f);
						}
						playersWindow.normal.textColor = textColor;
						playersWindowFrags.normal.textColor = textColor;
					}
					else if (isCompany)
					{
						if (gameObject3.GetComponent<NetworkStartTable>().myCommand == 0)
						{
							playersWindow.normal.textColor = new Color(0.7843f, 0.7843f, 0.7843f, 1f);
							playersWindowFrags.normal.textColor = new Color(0.7843f, 0.7843f, 0.7843f, 1f);
						}
						else if (gameObject3.GetComponent<NetworkStartTable>().myCommand == 1)
						{
							playersWindow.normal.textColor = new Color(0f, 0f, 1f, 1f);
							playersWindowFrags.normal.textColor = new Color(0f, 0f, 1f, 1f);
						}
						else
						{
							playersWindow.normal.textColor = new Color(1f, 0f, 0f, 1f);
							playersWindowFrags.normal.textColor = new Color(1f, 0f, 0f, 1f);
						}
					}
					else
					{
						playersWindow.normal.textColor = new Color(0.7843f, 0.7843f, 0.7843f, 1f);
						playersWindowFrags.normal.textColor = new Color(0.7843f, 0.7843f, 0.7843f, 1f);
					}
					int num10 = ((!isCOOP) ? gameObject3.GetComponent<NetworkStartTable>().CountKills : Mathf.RoundToInt(gameObject3.GetComponent<NetworkStartTable>().score));
					float num11 = 1000000f;
					if (isCompany)
					{
						if (m < list.Count)
						{
							num11 = num3 + num6 * (float)m;
						}
						if (m >= list.Count && m < list.Count + list2.Count)
						{
							num11 = num4 + num6 * (float)(m - list.Count);
						}
						if (m >= list.Count + list2.Count)
						{
							num11 = 1000000f;
						}
					}
					else
					{
						num11 = num3 + num6 * (float)m;
					}
					GUI.Label(new Rect(position.x + num5, position.y + num11, position.width * 0.75f, num7), gameObject3.GetComponent<NetworkStartTable>().NamePlayer, playersWindow);
					GUI.Label(new Rect(position.x + position.width - num5 - position.width * 0.1f, position.y + num11, position.width * 0.1f, num7), (num10 != -1) ? num10.ToString() : "0", playersWindowFrags);
					Texture2D image = expController.marks[gameObject3.GetComponent<NetworkStartTable>().myRanks];
					GUI.DrawTexture(new Rect(position.x - num8, position.y + num11, num8, num8), image);
				}
			}
		}
		else if (oldSpisokName.Length + oldSpisokNameBlue.Length + oldSpisokNameRed.Length > 0)
		{
			if (isCompany)
			{
				for (int n = 0; n < oldSpisokNameBlue.Length; n++)
				{
					if (oldIndexMy == n && myCommandOld == 1)
					{
						playersWindow.normal.textColor = new Color(1f, 1f, 0f, 1f);
						playersWindowFrags.normal.textColor = new Color(1f, 1f, 0f, 1f);
					}
					else
					{
						playersWindow.normal.textColor = new Color(0f, 0f, 1f, 1f);
						playersWindowFrags.normal.textColor = new Color(0f, 0f, 1f, 1f);
					}
					float num12 = 1000000f;
					num12 = num3 + num6 * (float)n;
					string text = oldCountLilsSpisokBlue[n];
					GUI.Label(new Rect(position.x + num5, position.y + num12, position.width * 0.75f, num7), oldSpisokNameBlue[n], playersWindow);
					GUI.Label(new Rect(position.x + position.width - num5 - position.width * 0.1f, position.y + num12, position.width * 0.1f, num7), (!text.Equals("-1")) ? text.ToString() : "0", playersWindowFrags);
					Texture2D image2 = expController.marks[oldSpisokRanksBlue[n]];
					GUI.DrawTexture(new Rect(position.x - num8, position.y + num12, num8, num8), image2);
				}
				for (int num13 = 0; num13 < oldSpisokNameRed.Length; num13++)
				{
					if (oldIndexMy == num13 && myCommandOld == 2)
					{
						playersWindow.normal.textColor = new Color(1f, 1f, 0f, 1f);
						playersWindowFrags.normal.textColor = new Color(1f, 1f, 0f, 1f);
					}
					else
					{
						playersWindow.normal.textColor = new Color(1f, 0f, 0f, 1f);
						playersWindowFrags.normal.textColor = new Color(1f, 0f, 0f, 1f);
					}
					float num14 = 1000000f;
					num14 = num4 + num6 * (float)num13;
					string text2 = oldCountLilsSpisokRed[num13];
					GUI.Label(new Rect(position.x + num5, position.y + num14, position.width * 0.75f, num7), oldSpisokNameRed[num13], playersWindow);
					GUI.Label(new Rect(position.x + position.width - num5 - position.width * 0.1f, position.y + num14, position.width * 0.1f, num7), (!text2.Equals("-1")) ? text2.ToString() : "0", playersWindowFrags);
					Texture2D image3 = expController.marks[oldSpisokRanksRed[num13]];
					GUI.DrawTexture(new Rect(position.x - num8, position.y + num14, num8, num8), image3);
				}
			}
			else
			{
				for (int num15 = 0; num15 < oldSpisokName.Length; num15++)
				{
					if (oldIndexMy == num15)
					{
						playersWindow.normal.textColor = new Color(1f, 1f, 0f, 1f);
						playersWindowFrags.normal.textColor = new Color(1f, 1f, 0f, 1f);
					}
					else
					{
						playersWindow.normal.textColor = new Color(0.7843f, 0.7843f, 0.7843f, 1f);
						playersWindowFrags.normal.textColor = new Color(0.7843f, 0.7843f, 0.7843f, 1f);
					}
					float num16 = 1000000f;
					num16 = num3 + num6 * (float)num15;
					string text3 = oldCountLilsSpisok[num15];
					GUI.Label(new Rect(position.x + num5, position.y + num16, position.width * 0.75f, num7), oldSpisokName[num15], playersWindow);
					GUI.Label(new Rect(position.x + position.width - num5 - position.width * 0.1f, position.y + num16, position.width * 0.1f, num7), (!text3.Equals("-1")) ? text3.ToString() : "0", playersWindowFrags);
					Texture2D image4 = expController.marks[oldSpisokRanks[num15]];
					GUI.DrawTexture(new Rect(position.x - num8, position.y + num16, num8, num8), image4);
				}
			}
		}
		if (isShowNickTable)
		{
			if (addCoins > 0 && oldIndexMy == 0)
			{
				if (countMigZagolovok < 6)
				{
					timeTomig -= Time.deltaTime;
					if (timeTomig < 0f)
					{
						isMigZag = !isMigZag;
						timeTomig = 0.7f;
						if (!isMigZag)
						{
							countMigZagolovok++;
						}
					}
				}
				GUI.DrawTexture(new Rect((float)Screen.width / 2f - (float)zagolovokWiner.width / 2f * koofScreen * 0.8f, 0f, (float)zagolovokWiner.width * koofScreen * 0.8f, (float)zagolovokWiner.height * koofScreen * 0.8f), (!isMigZag) ? zagolovokWiner_2 : zagolovokWiner);
			}
			else
			{
				zagolovokStyle.fontSize = Mathf.RoundToInt(45f * (float)Screen.height / 768f);
				zagolovokStyle.normal.textColor = new Color(0f, 0f, 0f, 1f);
				string text4 = ((!isCompany) ? ("WINNER:\n" + nickPobeditelya) : ((commandWinner != 1) ? "RED TEAM\nWON!" : "BLUE TEAM\nWON!"));
				GUI.Label(new Rect((float)Screen.width / 2f - ((float)head_players.width / 2f + 4f) * koofScreen, (float)Screen.height * 0.15f - (float)head_players.height / 2f * (float)Screen.height / 768f, (float)head_players.width * koofScreen, (float)head_players.height * koofScreen), text4, zagolovokStyle);
				GUI.Label(new Rect((float)Screen.width / 2f - ((float)head_players.width / 2f - 4f) * koofScreen, (float)Screen.height * 0.15f - (float)head_players.height / 2f * (float)Screen.height / 768f, (float)head_players.width * koofScreen, (float)head_players.height * koofScreen), text4, zagolovokStyle);
				GUI.Label(new Rect((float)Screen.width / 2f - ((float)head_players.width / 2f + 4f) * koofScreen, (float)Screen.height * 0.15f - ((float)head_players.height / 2f + 4f) * (float)Screen.height / 768f, (float)head_players.width * koofScreen, (float)head_players.height * koofScreen), text4, zagolovokStyle);
				GUI.Label(new Rect((float)Screen.width / 2f - ((float)head_players.width / 2f + 4f) * koofScreen, (float)Screen.height * 0.15f - ((float)head_players.height / 2f - 4f) * (float)Screen.height / 768f, (float)head_players.width * koofScreen, (float)head_players.height * koofScreen), text4, zagolovokStyle);
				GUI.Label(new Rect((float)Screen.width / 2f - ((float)head_players.width / 2f - 4f) * koofScreen, (float)Screen.height * 0.15f - ((float)head_players.height / 2f + 4f) * (float)Screen.height / 768f, (float)head_players.width * koofScreen, (float)head_players.height * koofScreen), text4, zagolovokStyle);
				GUI.Label(new Rect((float)Screen.width / 2f - ((float)head_players.width / 2f - 4f) * koofScreen, (float)Screen.height * 0.15f - ((float)head_players.height / 2f - 4f) * (float)Screen.height / 768f, (float)head_players.width * koofScreen, (float)head_players.height * koofScreen), text4, zagolovokStyle);
				GUI.Label(new Rect((float)Screen.width / 2f - (float)head_players.width / 2f * koofScreen, (float)Screen.height * 0.15f - ((float)head_players.height / 2f + 4f) * (float)Screen.height / 768f, (float)head_players.width * koofScreen, (float)head_players.height * koofScreen), text4, zagolovokStyle);
				GUI.Label(new Rect((float)Screen.width / 2f - (float)head_players.width / 2f * koofScreen, (float)Screen.height * 0.15f - ((float)head_players.height / 2f - 4f) * (float)Screen.height / 768f, (float)head_players.width * koofScreen, (float)head_players.height * koofScreen), text4, zagolovokStyle);
				zagolovokStyle.normal.textColor = new Color(1f, 1f, 0f, 1f);
				GUI.Label(new Rect((float)Screen.width / 2f - (float)head_players.width / 2f * koofScreen, (float)Screen.height * 0.15f - (float)head_players.height / 2f * (float)Screen.height / 768f, (float)head_players.width * koofScreen, (float)head_players.height * koofScreen), text4, zagolovokStyle);
			}
			Rect position2 = new Rect((float)Screen.width * 0.1f - (float)facebookStyle.normal.background.width * 0.5f * koofScreen, (float)Screen.height * 0.5f - (float)facebookStyle.normal.background.height * koofScreen * 1.1f, (float)facebookStyle.normal.background.width * koofScreen, (float)facebookStyle.normal.background.height * koofScreen);
			Rect position3 = new Rect((float)Screen.width * 0.1f - (float)facebookStyle.normal.background.width * 0.5f * koofScreen, (float)Screen.height * 0.5f + (float)facebookStyle.normal.background.height * koofScreen * 0.1f, (float)facebookStyle.normal.background.width * koofScreen, (float)facebookStyle.normal.background.height * koofScreen);
			if (!Application.isEditor)
			{
				if (GUI.Button(position2, string.Empty, twitterStyle))
				{
					FlurryPluginWrapper.LogTwitter();
					InitTwitter();
				}
				if (GUI.Button(position3, string.Empty, facebookStyle))
				{
					FlurryPluginWrapper.LogFacebook();
					InitFacebook();
				}
			}
		}
		else
		{
			if (PlayerPrefs.GetInt("CustomGame", 0) == 1)
			{
				GUI.DrawTexture(new Rect((float)Screen.width / 2f - (float)head_players.width / 2f * koofScreen * 0.8f, 0f, (float)head_players.width * koofScreen * 0.8f, (float)head_players.height * koofScreen * 0.8f), head_players);
			}
			else if (isCompany)
			{
				GUI.DrawTexture(new Rect((float)Screen.width / 2f - (float)zagolovogTeam.width / 2f * koofScreen * 0.8f, 0f, (float)zagolovogTeam.width * koofScreen * 0.8f, (float)zagolovogTeam.height * koofScreen * 0.8f), zagolovogTeam);
			}
			else if (isCOOP)
			{
				GUI.DrawTexture(new Rect((float)Screen.width / 2f - (float)zagolovokStartCOOP.width / 2f * koofScreen * 0.8f, 0f, (float)zagolovokStartCOOP.width * koofScreen * 0.8f, (float)zagolovokStartCOOP.height * koofScreen * 0.8f), zagolovokStartCOOP);
			}
			else
			{
				GUI.DrawTexture(new Rect((float)Screen.width / 2f - (float)zagolovokStart.width / 2f * koofScreen * 0.8f, 0f, (float)zagolovokStart.width * koofScreen * 0.8f, (float)zagolovokStart.height * koofScreen * 0.8f), zagolovokStart);
			}
			if (!isCompany)
			{
				if (isCOOP)
				{
					GUI.DrawTexture(new Rect((float)Screen.width / 2f - (float)plashkaStartCoop.width * 0.5f * koofScreen, (float)Screen.height - (float)plashkaStartCoop.height * koofScreen, (float)plashkaStartCoop.width * koofScreen, (float)plashkaStartCoop.height * koofScreen), plashkaStartCoop);
				}
				else
				{
					GUI.DrawTexture(new Rect((float)Screen.width / 2f - (float)plashkaStartMultu.width * 0.5f * koofScreen, (float)Screen.height - (float)plashkaStartMultu.height * koofScreen, (float)plashkaStartMultu.width * koofScreen, (float)plashkaStartMultu.height * koofScreen), plashkaStartMultu);
				}
			}
		}
		if (!GUI.Button(new Rect((float)Screen.width * 0.1f - (float)back.active.background.width / 2f * (float)Screen.height / 768f, (float)Screen.height * 0.9f - (float)back.active.background.height / 2f * (float)Screen.height / 768f, (float)(back.active.background.width * Screen.height) / 768f, (float)(back.active.background.height * Screen.height) / 768f), string.Empty, back))
		{
			return;
		}
		if (!isInet)
		{
			sendDelMyPlayer();
			if (isServer)
			{
				Network.Disconnect(200);
				GameObject.FindGameObjectWithTag("NetworkTable").GetComponent<LANBroadcastService>().StopBroadCasting();
			}
			else if (Network.connections.Length == 1)
			{
				Debug.Log("Disconnecting: " + Network.connections[0].ipAddress + ":" + Network.connections[0].port);
				Network.CloseConnection(Network.connections[0], true);
			}
			if (_purchaseActivityIndicator == null)
			{
				Debug.LogWarning("_purchaseActivityIndicator == null");
			}
			else
			{
				_purchaseActivityIndicator.SetActive(false);
			}
			ConnectGUI.Local();
		}
		else
		{
			sendDelMyPlayer();
			if (_purchaseActivityIndicator == null)
			{
				Debug.LogWarning("_purchaseActivityIndicator == null");
			}
			else
			{
				_purchaseActivityIndicator.SetActive(false);
			}
			PlayerPrefs.SetInt("ExitGame", 1);
			PhotonNetwork.LeaveRoom();
		}
	}

	public void startPlayer()
	{
		myRanks = expController.currentLevel;
		_playerPrefab = Resources.Load("Player") as GameObject;
		_cam = GameObject.FindGameObjectWithTag("CamTemp");
		_cam.SetActive(false);
		_weaponManager.useCam = null;
		zoneCreatePlayer = GameObject.FindGameObjectsWithTag(isCOOP ? "MultyPlayerCreateZoneCOOP" : ((!isCompany) ? "MultyPlayerCreateZone" : ("MultyPlayerCreateZoneCommand" + myCommand)));
		GameObject gameObject = zoneCreatePlayer[UnityEngine.Random.Range(0, zoneCreatePlayer.Length)];
		BoxCollider component = gameObject.GetComponent<BoxCollider>();
		Vector2 vector = new Vector2(component.size.x * gameObject.transform.localScale.x, component.size.z * gameObject.transform.localScale.z);
		Rect rect = new Rect(gameObject.transform.position.x - vector.x / 2f, gameObject.transform.position.z - vector.y / 2f, vector.x, vector.y);
		Vector3 position = new Vector3(rect.x + UnityEngine.Random.Range(0f, rect.width), gameObject.transform.position.y + 2f, rect.y + UnityEngine.Random.Range(0f, rect.height));
		Quaternion rotation = gameObject.transform.rotation;
		if (PlayerPrefs.GetInt("StartAfterDisconnect") == 1)
		{
			position = GlobalGameController.posMyPlayer;
		}
		GameObject myPlayer;
		if (isInet)
		{
			myPlayer = PhotonNetwork.Instantiate("Player", position, rotation, 0);
			GameObject.FindGameObjectWithTag("GameController").GetComponent<BonusCreator>().BeginCreateBonuses();
		}
		else
		{
			_playerPrefab = Resources.Load("Player") as GameObject;
			myPlayer = (GameObject)Network.Instantiate(_playerPrefab, position, rotation, 0);
		}
		ObjectLabel.currentCamera = Camera.main;
		_weaponManager.myPlayer = myPlayer;
		if (!isInet && isServer)
		{
			Debug.Log("networkView.RPC(RunGame, RPCMode.OthersBuffered);");
			base.GetComponent<NetworkView>().RPC("RunGame", RPCMode.OthersBuffered);
			GameObject.FindGameObjectWithTag("GameController").GetComponent<BonusCreator>().BeginCreateBonuses();
		}
		GameObject.FindGameObjectWithTag("GameController").GetComponent<Initializer>().SetupObjectThatNeedsPlayer();
		showTable = false;
	}

	[RPC]
	private void setState(string _namePlayer, int _countKills, int _oldCountLills, float _score, int _command, int _comandOld, int _myRanks)
	{
		NamePlayer = _namePlayer;
		CountKills = _countKills;
		oldCountKills = _oldCountLills;
		score = _score;
		myCommand = _command;
		myCommandOld = _command;
		myRanks = _myRanks;
	}
	public void addZombiManager()
	{
        int num = PhotonNetwork.AllocateViewID();
        photonView.RPC("addZombiManagerRPC", PhotonTargets.All, base.transform.position, base.transform.rotation, num);
    }

	[RPC]
	private void addZombiManagerRPC(Vector3 pos, Quaternion rot, int id1)
	{
        ZombiManager zombiManager = FindObjectOfType<ZombiManager>();
		GameObject gameObject = null;
		if (zombiManager == null)
		{
			gameObject = Instantiate(zombieManagerPrefab, pos, rot);
			zombiManager = gameObject.GetComponent<ZombiManager>();
		}
		else gameObject = zombiManager.gameObject;
		PhotonView component = gameObject.GetComponent<PhotonView>();
		component.viewID = id1;
	}

	public void addBonus(int _id, int _type, Vector3 _pos, Quaternion rot)
	{
		photonView.RPC("addBonusPhoton", PhotonTargets.Others, _id, _type, _pos, rot);
	}

	[RPC]
	public void addBonusPhoton(int _id, int _type, Vector3 _pos, Quaternion rot)
	{
		GameObject.FindGameObjectWithTag("GameController").GetComponent<BonusCreator>().addBonusFromPhotonRPC(_id, _type, _pos, rot);
	}

	[RPC]
	private void addBonusPhotonNewClientRPC(int playerId, int _id, int _type, Vector3 _pos, Quaternion rot)
	{
		if (playerId == PhotonNetwork.player.ID)
		{
			GameObject.FindGameObjectWithTag("GameController").GetComponent<BonusCreator>().addBonusFromPhotonRPC(_id, _type, _pos, rot);
		}
	}

	[RPC]
	private void addZombiManagerNewClientRPC(int playerId, Vector3 pos, Quaternion rot, int id1)
	{
		if (playerId == PhotonNetwork.player.ID)
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("ZombiCreator");
			if (!(gameObject != null) || id1 != gameObject.GetComponent<PhotonView>().viewID)
			{
				GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(zombieManagerPrefab, pos, rot);
				PhotonView component = gameObject2.GetComponent<PhotonView>();
				component.viewID = id1;
			}
		}
	}

	public void OnPhotonPlayerConnected(PhotonPlayer player)
	{
		if (isCOOP && (bool)photonView && photonView.isMine && GameObject.FindGameObjectWithTag("ZombiCreator") != null && GameObject.FindGameObjectWithTag("ZombiCreator").GetComponent<PhotonView>().owner.ID == PhotonNetwork.player.ID)
		{
			photonView.RPC("addZombiManagerNewClientRPC", PhotonTargets.Others, player.ID, base.transform.position, base.transform.rotation, GameObject.FindGameObjectWithTag("ZombiCreator").GetComponent<PhotonView>().viewID);
			GameObject[] array = GameObject.FindGameObjectsWithTag("Enemy");
			GameObject[] array2 = array;
			foreach (GameObject gameObject in array2)
			{
				if (!gameObject.GetComponent<ZombiUpravlenie>().deaded)
				{
					photonView.RPC("addZombiNewClientRPC", PhotonTargets.Others, player.ID, gameObject.GetComponent<ZombiUpravlenie>().typeZombInMas, gameObject.transform.position, gameObject.GetComponent<PhotonView>().viewID);
				}
			}
			GameObject[] array3 = GameObject.FindGameObjectsWithTag("Bonus");
			GameObject[] array4 = array3;
			foreach (GameObject gameObject2 in array4)
			{
				photonView.RPC("addBonusPhotonNewClientRPC", PhotonTargets.Others, player.ID, gameObject2.GetComponent<PhotonView>().viewID, gameObject2.GetComponent<SettingBonus>().typeOfMass, gameObject2.transform.position, gameObject2.transform.rotation);
			}
		}
		if ((bool)photonView && photonView.isMine)
		{
			synchState();
		}
	}

	[RPC]
	private void addZombiNewClientRPC(int _playerId, int typeOfZomb, Vector3 pos, int _id)
	{
		Debug.Log(string.Empty + GetComponent<PhotonView>().owner.ID + " " + PhotonNetwork.player.ID);
		if (_playerId != PhotonNetwork.player.ID)
		{
			return;
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag("Enemy");
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			if (gameObject.GetComponent<PhotonView>().viewID == _id)
			{
				return;
			}
		}
		GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(zombiePrefabs[typeOfZomb], pos, Quaternion.identity);
		PhotonView component = gameObject2.GetComponent<PhotonView>();
		component.viewID = _id;
	}

	public void synchState()
	{
		if (isInet)
		{
			if (!isCOOP)
			{
				GlobalGameController.Score = CountKills;
			}
			else
			{
				GlobalGameController.Score = Mathf.RoundToInt(score);
			}
			if (photonView != null)
			{
				photonView.RPC("setState", PhotonTargets.Others, NamePlayer, CountKills, oldCountKills, score, myCommand, myCommandOld, myRanks);
			}
			if (!isCOOP)
			{
			}
		}
		else
		{
			base.GetComponent<NetworkView>().RPC("setState", RPCMode.OthersBuffered, NamePlayer, CountKills, oldCountKills, 0, 0);
		}
	}

	private void InitTwitter()
	{
		string empty = string.Empty;
		string empty2 = string.Empty;
		if (GlobalGameController.isFullVersion)
		{
			empty = "cuMbTHM8izr9Mb3bIfcTxA";
			empty2 = "mpTLWIku4kIaQq7sTTi91wRLlvAxADhalhlEresnuI";
		}
		else
		{
			empty = "Jb7CwCaMgCQQiMViQRNHw";
			empty2 = "zGVrax4vqgs3CYf04O7glsoRbNT3vhIafte6lfm8w";
		}
		ServiceLocator.TwitterFacade.Init(empty, empty2);
		if (!ServiceLocator.TwitterFacade.IsLoggedIn())
		{
			TwitterLogin();
		}
		else
		{
			TwitterPost();
		}
	}

	private void TwitterLogin()
	{
		TwitterManager.loginSucceededEvent += OnTwitterLogin;
		TwitterManager.loginFailedEvent += OnTwitterLoginFailed;
		ServiceLocator.TwitterFacade.ShowLoginDialog();
	}

	private void OnTwitterLogin(string s)
	{
		TwitterManager.loginSucceededEvent -= OnTwitterLogin;
		TwitterManager.loginFailedEvent -= OnTwitterLoginFailed;
		TwitterPost();
	}

	private void OnTwitterLoginFailed(string _error)
	{
		TwitterManager.loginSucceededEvent -= OnTwitterLogin;
		TwitterManager.loginFailedEvent -= OnTwitterLoginFailed;
	}

	private void TwitterPost()
	{
		TwitterManager.requestDidFinishEvent += OnTwitterPost;
		ServiceLocator.TwitterFacade.PostStatusUpdate(_SocialMessage());
	}

	private void OnTwitterPost(object result)
	{
		if (result != null)
		{
			TwitterManager.requestDidFinishEvent -= OnTwitterPost;
			showMessagTiwtter = true;
			Invoke("hideMessagTwitter", 3f);
		}
	}

	private void OnTwitterPostFailed(string _error)
	{
		TwitterManager.requestDidFinishEvent -= OnTwitterPost;
	}

	private void hideMessag()
	{
		showMessagFacebook = false;
	}

	private void hideMessagTwitter()
	{
		showMessagTiwtter = false;
	}

	private void InitFacebook()
	{
		if (!Application.isEditor)
		{
			clickButtonFacebook = true;
			if (!ServiceLocator.FacebookFacade.IsSessionValid())
			{
				Debug.Log("!isSessionValid");
				string[] permissions = new string[1] { "email" };
				ServiceLocator.FacebookFacade.LoginWithReadPermissions(permissions);
			}
			else
			{
				Debug.Log("isSessionValid");
				OnEventFacebookLogin();
			}
		}
	}

	private void InitFacebookEvents()
	{
		if (!Application.isEditor)
		{
			FacebookManager.reauthorizationSucceededEvent += OnEventFacebookLogin;
			FacebookManager.loginFailedEvent += OnEventFacebookLoginFailed;
			FacebookManager.sessionOpenedEvent += OnEventFacebookLogin;
		}
	}

	private void CleanFacebookEvents()
	{
		if (!Application.isEditor)
		{
			FacebookManager.reauthorizationSucceededEvent -= OnEventFacebookLogin;
			FacebookManager.loginFailedEvent -= OnEventFacebookLoginFailed;
			FacebookManager.sessionOpenedEvent -= OnEventFacebookLogin;
		}
	}

	private void OnEventFacebookLogin()
	{
		if (Application.isEditor || !clickButtonFacebook)
		{
			return;
		}
		Debug.Log("OnEventFacebookLogin");
		if (!ServiceLocator.FacebookFacade.IsSessionValid())
		{
			return;
		}
		if (_hasPublishPermission)
		{
			Debug.Log("sendMessag");
			clickButtonFacebook = false;
			showMessagFacebook = true;
			Invoke("hideMessag", 3f);
			Facebook.instance.postMessage(_SocialMessage(), completionHandler);
			return;
		}
		Debug.Log("poluchau permissions");
		string[] permissions = new string[2] { "publish_actions", "publish_stream" };
		ServiceLocator.FacebookFacade.ReauthorizeWithPublishPermissions(permissions, FacebookSessionDefaultAudience.Everyone);
		if (Application.platform == RuntimePlatform.Android)
		{
			_hasPublishPermission = true;
			_hasPublishActions = true;
		}
	}

	private void facebookGraphReqCompl(object result)
	{
		Utils.logObject(result);
	}

	private void facebookSessionOpened()
	{
		if (!Application.isEditor)
		{
			_hasPublishPermission = ServiceLocator.FacebookFacade.GetSessionPermissions().Contains("publish_stream");
			_hasPublishActions = ServiceLocator.FacebookFacade.GetSessionPermissions().Contains("publish_actions");
		}
	}

	private void facebookreauthorizationSucceededEvent()
	{
		if (!Application.isEditor)
		{
			_hasPublishPermission = ServiceLocator.FacebookFacade.GetSessionPermissions().Contains("publish_stream");
			_hasPublishActions = ServiceLocator.FacebookFacade.GetSessionPermissions().Contains("publish_actions");
		}
	}

	private void OnEventFacebookLoginFailed(P31Error error)
	{
		clickButtonFacebook = false;
		Debug.Log("OnEventFacebookLoginFailed=" + error);
	}

	private void Start()
	{
		photonView = PhotonView.Get(this);
		if (isMulti)
		{
			if (isLocal)
			{
				isMine = base.GetComponent<NetworkView>().isMine;
			}
			else
			{
				isMine = photonView.isMine;
			}
		}
		expController = FindObjectOfType<ExperienceController>();
		expController.posRanks = new Vector2(21f * Defs.Coef, 21f * Defs.Coef);
		InitFacebookEvents();
		if (!Application.isEditor)
		{
			ServiceLocator.FacebookFacade.Init();
			FacebookSessionLoginBehavior sessionLoginBehavior = FacebookSessionLoginBehavior.SSO_WITH_FALLBACK;
			ServiceLocator.FacebookFacade.SetSessionLoginBehavior(sessionLoginBehavior);
		}
		if (!Application.isEditor)
		{
			FacebookManager.graphRequestCompletedEvent += facebookGraphReqCompl;
			FacebookManager.sessionOpenedEvent += facebookSessionOpened;
			FacebookManager.reauthorizationSucceededEvent += facebookreauthorizationSucceededEvent;
			_canUserUseFacebookComposer = ServiceLocator.FacebookFacade.CanUserUseFacebookComposer();
		}
		if (isCOOP && isInet && photonView.isMine && PlayerPrefs.GetString("TypeGame").Equals("server"))
		{
			addZombiManager();
		}
		_purchaseActivityIndicator = StoreKitEventListener.purchaseActivityInd;
		zoneCreatePlayer = GameObject.FindGameObjectsWithTag((!isCOOP) ? "MultyPlayerCreateZone" : "MultyPlayerCreateZoneCOOP");
		_weaponManager = GameObject.FindGameObjectWithTag("WeaponManager").GetComponent<WeaponManager>();
		if (isMulti && isMine)
		{
			if (PlayerPrefs.GetInt("StartAfterDisconnect") == 0)
			{
				showTable = true;
			}
			else
			{
				showTable = GlobalGameController.showTableMyPlayer;
				if (!showTable)
				{
					Invoke("startPlayer", 0.1f);
				}
			}
			ObjectLabel.currentCamera = GameObject.FindGameObjectWithTag("GameController").GetComponent<Initializer>().tc.GetComponent<Camera>();
			tempCam.SetActive(true);
			_weaponManager = GameObject.FindGameObjectWithTag("WeaponManager").GetComponent<WeaponManager>();
			string text = (NamePlayer = _weaponManager.gameObject.GetComponent<FilterBadWorld>().FilterString(PlayerPrefs.GetString("NamePlayer", Defs.defaultPlayerName)));
			if (isServer)
			{
				addPlayer(PlayerPrefs.GetString("NamePlayer", Defs.defaultPlayerName), Network.player.ipAddress);
				if (isMulti)
				{
					if (!isInet)
					{
						base.GetComponent<NetworkView>().RPC("addPlayer", RPCMode.OthersBuffered, text, Network.player.ipAddress);
					}
					else
					{
						photonView.RPC("addPlayer", PhotonTargets.OthersBuffered, text, Network.player.ipAddress);
					}
				}
				if (!isInet)
				{
					LANBroadcastService component = GetComponent<LANBroadcastService>();
					component.serverMessage.name = PlayerPrefs.GetString("ServerName");
					component.serverMessage.map = PlayerPrefs.GetString("MapName");
					component.serverMessage.connectedPlayers = 0;
					component.serverMessage.playerLimit = int.Parse(PlayerPrefs.GetString("PlayersLimits"));
					component.serverMessage.comment = PlayerPrefs.GetString("MaxKill");
					component.StartAnnounceBroadCasting();
				}
			}
			else if (!isInet)
			{
				base.GetComponent<NetworkView>().RPC("addPlayer", RPCMode.AllBuffered, text, Network.player.ipAddress);
			}
			else
			{
				Debug.Log("addPlayer client  " + photonView);
				photonView.RPC("addPlayer", PhotonTargets.AllBuffered, text, Network.player.ipAddress);
			}
			if (PlayerPrefs.GetInt("StartAfterDisconnect") == 1)
			{
				CountKills = GlobalGameController.Score;
				score = GlobalGameController.Score;
				Invoke("synchState", 1f);
			}
			else
			{
				CountKills = -1;
				score = -1f;
				Invoke("synchState", 1f);
			}
			expController = FindObjectOfType<ExperienceController>();
			myRanks = expController.currentLevel;
			synchState();
			mySkin = SkinsManager.currentMultiplayerSkin();
			sendMySkin();
		}
		else
		{
			showTable = false;
		}
	}

	[RPC]
	private void setMySkin(string str)
	{
		if (base.transform.GetComponent<PhotonView>() == null)
		{
			return;
		}
		byte[] data = Convert.FromBase64String(str);
		Texture2D texture2D = new Texture2D(64, 32);
		texture2D.LoadImage(data);
		texture2D.filterMode = FilterMode.Point;
		texture2D.Apply();
		mySkin = texture2D;
		GameObject[] array = GameObject.FindGameObjectsWithTag("PlayerGun");
		if (array == null)
		{
			return;
		}
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			if ((isInet && (bool)gameObject && gameObject.GetComponent<PhotonView>() != null && gameObject.GetComponent<PhotonView>().owner != null && (bool)base.transform && base.transform.GetComponent<PhotonView>() != null && gameObject.GetComponent<PhotonView>().owner.Equals(base.transform.GetComponent<PhotonView>().owner)) || (!isInet && gameObject.GetComponent<NetworkView>().owner.ipAddress.Equals(base.transform.GetComponent<NetworkView>().owner.ipAddress)))
			{
				gameObject.GetComponent<Player_move_c>().setMyTamble(base.gameObject);
				break;
			}
		}
	}

	[RPC]
	private void setMySkinLocal(string str1, string str2)
	{
		Debug.Log("setMySkin");
		byte[] data = Convert.FromBase64String(str1 + str2);
		Texture2D texture2D = new Texture2D(64, 32);
		texture2D.LoadImage(data);
		texture2D.filterMode = FilterMode.Point;
		texture2D.Apply();
		mySkin = texture2D;
		GameObject[] array = GameObject.FindGameObjectsWithTag("PlayerGun");
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			if ((isInet && gameObject.GetComponent<PhotonView>().owner.ID == base.transform.GetComponent<PhotonView>().owner.ID) || (!isInet && gameObject.GetComponent<NetworkView>().owner.ipAddress.Equals(base.transform.GetComponent<NetworkView>().owner.ipAddress)))
			{
				gameObject.GetComponent<Player_move_c>().setMyTamble(base.gameObject);
				break;
			}
		}
	}

	private void sendMySkin()
	{
		Texture2D texture2D = mySkin as Texture2D;
		byte[] inArray = texture2D.EncodeToPNG();
		string text = Convert.ToBase64String(inArray);
		if (isInet)
		{
			photonView.RPC("setMySkin", PhotonTargets.AllBuffered, text);
			return;
		}
		Debug.Log(text.Length + " " + text.Length / 2 + " " + (text.Length / 2 + text.Length / 2));
		base.GetComponent<NetworkView>().RPC("setMySkinLocal", RPCMode.AllBuffered, text.Substring(0, text.Length / 2), text.Substring(text.Length / 2, text.Length / 2));
	}

	[RPC]
	private void ZombiManagerZamenaIdRPC(int _id)
	{
		if (GameObject.FindGameObjectWithTag("ZombiCreator") == null)
		{
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(zombieManagerPrefab, base.transform.position, base.transform.rotation);
			PhotonView component = gameObject.GetComponent<PhotonView>();
			component.viewID = _id;
		}
		else
		{
			GameObject.FindGameObjectWithTag("ZombiCreator").GetComponent<PhotonView>().viewID = _id;
		}
	}

	[RPC]
	private void ZombiZamenaIdRPC(int _idOld, int _idNew)
	{
		Debug.Log("ZombiZamenaIdRPC  " + _idOld + " " + _idNew);
		GameObject[] array = GameObject.FindGameObjectsWithTag("Enemy");
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			if (gameObject.GetComponent<PhotonView>().viewID == _idOld)
			{
				gameObject.GetComponent<PhotonView>().viewID = _idNew;
				break;
			}
		}
	}

	private void Update()
    {
        if (expController == null) expController = FindObjectOfType<ExperienceController>();
        if (isMine)
		{
			if (showTable || isShowNickTable)
			{
				expController.isShowRanks = true;
			}
			else
			{
				expController.isShowRanks = false;
			}
		}
		if (!isLocal && isCOOP && photonView.isMine)
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("ZombiCreator");
			if (gameObject != null)
			{
				if (gameObject.GetComponent<PhotonView>().owner == null)
				{
					GameObject[] array = GameObject.FindGameObjectsWithTag("NetworkTable");
					int viewID = array[0].GetComponent<PhotonView>().viewID;
					GameObject gameObject2 = array[0];
					GameObject[] array2 = array;
					foreach (GameObject gameObject3 in array2)
					{
						if (gameObject3.GetComponent<PhotonView>().viewID > viewID)
						{
							gameObject2 = gameObject3;
							viewID = gameObject3.GetComponent<PhotonView>().viewID;
						}
					}
					if (gameObject2 == base.gameObject)
					{
						int num = PhotonNetwork.AllocateViewID();
						gameObject.GetComponent<PhotonView>().viewID = num;
						photonView.RPC("ZombiManagerZamenaIdRPC", PhotonTargets.Others, num);
						GameObject[] array3 = GameObject.FindGameObjectsWithTag("Enemy");
						GameObject[] array4 = array3;
						foreach (GameObject gameObject4 in array4)
						{
							int viewID2 = gameObject4.GetComponent<PhotonView>().viewID;
							int num2 = PhotonNetwork.AllocateViewID();
							photonView.RPC("ZombiZamenaIdRPC", PhotonTargets.Others, viewID2, num2);
							gameObject4.GetComponent<PhotonView>().viewID = num2;
						}
						Debug.Log("Set My Upravlenie");
					}
				}
			}
			else
			{
				timeNotRunZombiManager += Time.deltaTime;
				if (timeNotRunZombiManager > 15f && !isSendZaprosZombiManager)
				{
					photonView.RPC("zaprosZombiManager", PhotonTargets.Others);
					isSendZaprosZombiManager = false;
				}
				if (timeNotRunZombiManager > 20f)
				{
					addZombiManager();
				}
			}
		}
		if (!isLocal && isMine)
		{
			GlobalGameController.showTableMyPlayer = showTable;
		}
		if (isLocal && isServer)
		{
			LANBroadcastService component = GetComponent<LANBroadcastService>();
			if (component != null)
			{
				component.serverMessage.connectedPlayers = GameObject.FindGameObjectsWithTag("NetworkTable").Length;
			}
		}
		if (!(timerShow >= 0f))
		{
			return;
		}
		timerShow -= Time.deltaTime;
		if (timerShow < 0f)
		{
			if (_purchaseActivityIndicator == null)
			{
				Debug.LogWarning("_purchaseActivityIndicator == null");
			}
			else
			{
				_purchaseActivityIndicator.SetActive(false);
			}
			ConnectGUI.Local();
		}
	}

	[RPC]
	private void zaprosZombiManager()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("ZombiCreator");
		if (gameObject != null && gameObject.GetComponent<PhotonView>().owner != null && photonView != null)
		{
			photonView.RPC("otvetNAzaprosZombiManager", PhotonTargets.MasterClient, gameObject.GetComponent<PhotonView>().viewID);
		}
	}

	[RPC]
	private void otvetNAzaprosZombiManager(int ID)
	{
		if (!isGetZaprosZombiManager)
		{
			isGetZaprosZombiManager = false;
			addZombiManagerRPC(base.transform.position, base.transform.rotation, ID);
		}
	}

	private void OnDisconnectedFromServer(NetworkDisconnection info)
	{
		Debug.Log("OnDisconnectedFromServer");
		showDisconnectFromServer = true;
		timerShow = 3f;
	}

	private void OnPlayerDisconnected(NetworkPlayer player)
	{
		Debug.Log("Clean up after player " + player.ipAddress);
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
		GameObject[] array = GameObject.FindGameObjectsWithTag("PlayerGun");
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			if (!player.ipAddress.Equals(gameObject.GetComponent<Player_move_c>().myIp))
			{
				continue;
			}
			GameObject[] array3 = GameObject.FindGameObjectsWithTag("ObjectLabel");
			GameObject[] array4 = array3;
			foreach (GameObject gameObject2 in array4)
			{
				if (gameObject2.GetComponent<ObjectLabel>().target == gameObject.transform)
				{
					UnityEngine.Object.Destroy(gameObject2);
					break;
				}
			}
			UnityEngine.Object.Destroy(gameObject.transform.parent.transform.gameObject);
		}
	}

	private void OnFailedToConnectToMasterServer(NetworkConnectionError info)
	{
		Debug.Log("Could not connect to master server: " + info);
		showDisconnectFromMasterServer = true;
		timerShow = 3f;
	}

	public void win(string winner, int _commandWin = 0)
	{
		commandWinner = _commandWin;
		GlobalGameController.countKillsBlue = 0;
		GlobalGameController.countKillsRed = 0;
		if (!Storager.hasKey(Defs.COOPScore))
		{
			Storager.setInt(Defs.COOPScore, 0, false);
		}
		int @int = Storager.getInt(Defs.COOPScore, false);
		if (GlobalGameController.Score > @int)
		{
			Storager.setInt(Defs.COOPScore, GlobalGameController.Score, false);
		}
		nickPobeditelya = winner;
		GameObject[] array = GameObject.FindGameObjectsWithTag("NetworkTable");
		List<GameObject> list = new List<GameObject>();
		List<GameObject> list2 = new List<GameObject>();
		List<GameObject> list3 = new List<GameObject>();
		for (int i = 1; i < array.Length; i++)
		{
			GameObject gameObject = array[i];
			int num = i - 1;
			while (num >= 0 && ((!isCOOP) ? ((float)array[num].GetComponent<NetworkStartTable>().CountKills) : array[num].GetComponent<NetworkStartTable>().score) < ((!isCOOP) ? ((float)gameObject.GetComponent<NetworkStartTable>().CountKills) : gameObject.GetComponent<NetworkStartTable>().score))
			{
				array[num + 1] = array[num];
				num--;
			}
			array[num + 1] = gameObject;
		}
		for (int j = 0; j < array.Length; j++)
		{
			int num2 = array[j].GetComponent<NetworkStartTable>().myCommand;
			if (num2 == -1)
			{
				num2 = array[j].GetComponent<NetworkStartTable>().myCommandOld;
			}
			if (num2 == 0)
			{
				list3.Add(array[j]);
			}
			if (num2 == 1)
			{
				list.Add(array[j]);
			}
			if (num2 == 2)
			{
				list2.Add(array[j]);
			}
		}
		if (list3.Count > 0)
		{
			oldSpisokName = new string[list3.Count];
			oldCountLilsSpisok = new string[list3.Count];
			oldSpisokRanks = new int[list3.Count];
		}
		if (list.Count > 0)
		{
			oldSpisokNameBlue = new string[list.Count];
			oldCountLilsSpisokBlue = new string[list.Count];
			oldSpisokRanksBlue = new int[list.Count];
		}
		if (list2.Count > 0)
		{
			oldSpisokNameRed = new string[list2.Count];
			oldCountLilsSpisokRed = new string[list2.Count];
			oldSpisokRanksRed = new int[list2.Count];
		}
		addCoins = 0;
		int num3 = 0;
		int num4 = int.Parse(PhotonNetwork.room.customProperties["MaxKill"].ToString());
		if ((bool)_weaponManager && PlayerPrefs.GetInt("CustomGame", 0) == 0 && !isCOOP && !isCompany && array[0].Equals(_weaponManager.myTable))
		{
			if (num4 <= 20)
			{
				addCoins = 2;
				num3 = 5;
			}
			if (num4 > 20 && num4 <= 30)
			{
				addCoins = 4;
				num3 = 10;
			}
			if (num4 > 30 && num4 <= 40)
			{
				addCoins = 6;
				num3 = 15;
			}
			if (num4 > 40)
			{
				addCoins = 10;
				num3 = 20;
			}
		}
		if ((bool)_weaponManager && PlayerPrefs.GetInt("CustomGame", 0) == 0 && !isCOOP && isCompany && myCommand == _commandWin)
		{
			float num5 = (float)CountKills / (float)num4;
			if (num5 >= 0.1f && num5 < 0.3f)
			{
				addCoins = 1;
				num3 = 10;
			}
			if (num5 >= 0.3f && num5 < 0.6f)
			{
				addCoins = 3;
				num3 = 15;
			}
			if (num5 >= 0.6f)
			{
				addCoins = 5;
				num3 = 25;
			}
		}
		if ((bool)_weaponManager && PlayerPrefs.GetInt("CustomGame", 0) == 0 && isCOOP && score >= 2000f)
		{
			addCoins = 1;
			if (array[0].Equals(_weaponManager.myTable))
			{
				addCoins += 4;
				num3 = 15;
			}
			if (array.Length > 2 && array[1].Equals(_weaponManager.myTable))
			{
				addCoins += 2;
				num3 = 10;
			}
			if (array.Length > 3 && array[2].Equals(_weaponManager.myTable))
			{
				addCoins++;
				num3 = 5;
			}
		}
		if (addCoins > 0)
		{
			if (!Storager.hasKey(Defs.Coins))
			{
				Storager.setInt(Defs.Coins, 0, false);
			}
			int int2 = Storager.getInt(Defs.Coins, false);
			Storager.setInt(Defs.Coins, int2 + addCoins, false);
			PlayerPrefs.SetInt("AddCoins", addCoins);
			zagolovokWiner = Resources.Load("Win_Coins_Head/got" + addCoins + "w") as Texture;
			zagolovokWiner_2 = Resources.Load("Win_Coins_Head/got" + addCoins) as Texture;
		}
		if (num3 > 0)
		{
			experienceController.addExperience(num3);
		}
		for (int k = 0; k < list3.Count; k++)
		{
			if ((bool)_weaponManager && list3[k].Equals(_weaponManager.myTable))
			{
				oldIndexMy = k;
			}
			oldSpisokName[k] = list3[k].GetComponent<NetworkStartTable>().NamePlayer;
			oldSpisokRanks[k] = list3[k].GetComponent<NetworkStartTable>().myRanks;
			if (isCOOP)
			{
				oldCountLilsSpisok[k] = ((list3[k].GetComponent<NetworkStartTable>().score == -1f) ? (string.Empty + list3[k].GetComponent<NetworkStartTable>().scoreOld) : (string.Empty + list3[k].GetComponent<NetworkStartTable>().score));
			}
			else
			{
				oldCountLilsSpisok[k] = ((list3[k].GetComponent<NetworkStartTable>().CountKills == -1) ? (string.Empty + list3[k].GetComponent<NetworkStartTable>().oldCountKills) : (string.Empty + list3[k].GetComponent<NetworkStartTable>().CountKills));
			}
		}
		for (int l = 0; l < list.Count; l++)
		{
			if ((bool)_weaponManager && list[l].Equals(_weaponManager.myTable))
			{
				oldIndexMy = l;
			}
			oldSpisokNameBlue[l] = list[l].GetComponent<NetworkStartTable>().NamePlayer;
			oldSpisokRanksBlue[l] = list[l].GetComponent<NetworkStartTable>().myRanks;
			if (isCOOP)
			{
				oldCountLilsSpisokBlue[l] = ((list[l].GetComponent<NetworkStartTable>().score == -1f) ? (string.Empty + list[l].GetComponent<NetworkStartTable>().scoreOld) : (string.Empty + list[l].GetComponent<NetworkStartTable>().score));
			}
			else
			{
				oldCountLilsSpisokBlue[l] = ((list[l].GetComponent<NetworkStartTable>().CountKills == -1) ? (string.Empty + list[l].GetComponent<NetworkStartTable>().oldCountKills) : (string.Empty + list[l].GetComponent<NetworkStartTable>().CountKills));
			}
		}
		for (int m = 0; m < list2.Count; m++)
		{
			if ((bool)_weaponManager && list2[m].Equals(_weaponManager.myTable))
			{
				oldIndexMy = m;
			}
			oldSpisokNameRed[m] = list2[m].GetComponent<NetworkStartTable>().NamePlayer;
			oldSpisokRanksRed[m] = list2[m].GetComponent<NetworkStartTable>().myRanks;
			if (isCOOP)
			{
				oldCountLilsSpisokRed[m] = ((list2[m].GetComponent<NetworkStartTable>().score == -1f) ? (string.Empty + list2[m].GetComponent<NetworkStartTable>().scoreOld) : (string.Empty + list2[m].GetComponent<NetworkStartTable>().score));
			}
			else
			{
				oldCountLilsSpisokRed[m] = ((list2[m].GetComponent<NetworkStartTable>().CountKills == -1) ? (string.Empty + list2[m].GetComponent<NetworkStartTable>().oldCountKills) : (string.Empty + list2[m].GetComponent<NetworkStartTable>().CountKills));
			}
		}
		myCommandOld = myCommand;
		oldCountKills = CountKills;
		scoreOld = score;
		score = -1f;
		GlobalGameController.Score = -1;
		CountKills = -1;
		if (isCompany)
		{
			myCommand = -1;
		}
		synchState();
		ObjectLabel.currentCamera = GameObject.FindGameObjectWithTag("GameController").GetComponent<Initializer>().tc.GetComponent<Camera>();
		if (!isInet)
		{
			UnityEngine.Object.DestroyObject(_weaponManager.myPlayer);
		}
		else
		{
			if ((bool)_weaponManager && (bool)_weaponManager.myPlayer)
			{
				PhotonNetwork.Destroy(_weaponManager.myPlayer);
			}
			if (isCOOP)
			{
				GameObject[] array2 = GameObject.FindGameObjectsWithTag("Enemy");
				for (int n = 0; n < array2.Length; n++)
				{
					UnityEngine.Object.Destroy(array2[n]);
				}
			}
		}
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("DamageFrame");
		if (gameObject2 != null)
		{
			UnityEngine.Object.Destroy(gameObject2);
		}
		if (_cam != null)
		{
			_cam.SetActive(true);
		}
		isShowNickTable = true;
	}

	private void end()
	{
		Debug.Log("end");
		if (!isInet)
		{
			if (isServer)
			{
				Network.Disconnect(200);
				GameObject.FindGameObjectWithTag("NetworkTable").GetComponent<LANBroadcastService>().StopBroadCasting();
			}
			else if (Network.connections.Length == 1)
			{
				Debug.Log("Disconnecting: " + Network.connections[0].ipAddress + ":" + Network.connections[0].port);
				Network.CloseConnection(Network.connections[0], true);
			}
			_purchaseActivityIndicator.SetActive(false);
			ConnectGUI.Local();
		}
		else
		{
			PhotonNetwork.LeaveRoom();
		}
	}

	private void finishTable()
	{
		playersTable();
	}

	private void OnGUI()
	{
		Rect rect = new Rect(0f, (float)Screen.height * 0.6f, Screen.width, (float)Screen.height - (float)Screen.height * 4f / 5f);
		messagesStyle.fontSize = Mathf.RoundToInt((float)(30 * Screen.height) / 768f);
		if (experienceController.isShowAdd)
		{
			GUI.enabled = false;
		}
		if (showDisconnectFromServer)
		{
			GUI.DrawTexture(new Rect((float)(Screen.width / 2) - (float)serverLeftTheGame.width * 0.5f * koofScreen, (float)(Screen.height / 2) - (float)serverLeftTheGame.height * 0.5f * koofScreen, (float)serverLeftTheGame.width * koofScreen, (float)serverLeftTheGame.height * koofScreen), serverLeftTheGame);
			GUI.enabled = false;
		}
		if (showDisconnectFromMasterServer)
		{
			GUI.DrawTexture(new Rect((float)(Screen.width / 2) - (float)serverLeftTheGame.width * 0.5f * koofScreen, (float)(Screen.height / 2) - (float)serverLeftTheGame.height * 0.5f * koofScreen, (float)serverLeftTheGame.width * koofScreen, (float)serverLeftTheGame.height * koofScreen), serverLeftTheGame);
		}
		if (showTable)
		{
			playersTable();
		}
		if (isShowNickTable)
		{
			finishTable();
		}
		if (showMessagFacebook)
		{
			labelStyle.fontSize = Player_move_c.FontSizeForMessages;
			GUI.Label(Player_move_c.SuccessMessageRect(), _SocialSentSuccess("Facebook"), labelStyle);
		}
		if (showMessagTiwtter)
		{
			labelStyle.fontSize = Player_move_c.FontSizeForMessages;
			GUI.Label(Player_move_c.SuccessMessageRect(), _SocialSentSuccess("Twitter"), labelStyle);
		}
		GUI.enabled = true;
	}

	private void OnConnectedToServer()
	{
		Debug.Log("OnConnectedToServer");
	}

	private void OnDestroy()
	{
		if (expController != null)
		{
			expController.isShowRanks = false;
		}
		CleanFacebookEvents();
	}
}
