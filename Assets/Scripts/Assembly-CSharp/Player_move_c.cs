using System;
using System.Collections;
using System.Collections.Generic;
using Holoville.HOTween;
using Holoville.HOTween.Core;
using UnityEngine;

public sealed class Player_move_c : MonoBehaviour
{
	public struct MessageChat
	{
		public string text;

		public float time;

		public int ID;

		public int command;

		public NetworkViewID IDLocal;
	}

	public Camera myCamera;

	public Camera gunCamera;

	public GameObject hatsPoint;

	public GameObject capesPoint;

	public bool isZooming;

	public AudioClip headShotSound;

	public GameObject headShotParticle;

	public GameObject chatViewer;

	public GUISkin MySkin;

	public GameObject myTable;

	public Texture2D ammoTexture;

	public Texture2D scoreTexture;

	public Texture2D enemiesTxture;

	public Texture HeartTexture;

	public Texture buyTexture;

	public Texture head_players;

	public Texture nicksStyle;

	public Texture killsStyle;

	public Texture scoreTextureCOOP;

	public Texture timeTexture;

	public Texture2D AimTexture;

	public int AimTextureWidth = 50;

	public int AimTextureHeight = 50;

	public Transform GunFlash;

	public bool showGUIUnlockFullVersion;

	public Texture fonChat;

	public Texture tapChat;

	public GUIStyle noStyle;

	public GUIStyle fullVerStyle;

	public GUIStyle unlockStyle;

	public GUIStyle playersWindow;

	public GUIStyle playersWindowFrags;

	public GUIStyle closeRanksStyle;

	public GUIStyle labelChatStyle;

	public GUIStyle labelGameChatStyle;

	public Texture tableFon;

	public Texture tableFonCommand;

	public int BulletForce = 5000;

	public GameObject renderAllObjectPrefab;

	private Texture zaglushkaTexture;

	public GUIStyle labelStyle;

	private bool productPurchased;

	public bool killed;

	private Vector2 scrollPosition = Vector2.zero;

	public ZombiManager zombiManager;

	public GameObject hole;

	public GameObject bloodParticle;

	public GameObject wallParticle;

	public string textChat;

	public bool showGUI = true;

	public bool showRanks;

	public Texture minerWeaponSoldTexture;

	public Texture swordSoldTexture;

	public Texture hasElixirTexture;

	public Texture combatRifleSoldTexture;

	public Texture goldenEagleSoldTexture;

	public Texture magicBowSoldTexture;

	public Texture axeBoughtTexture;

	public Texture spasBoughtTexture;

	public Texture chainsawOffTexture;

	public Texture famasOffTexture;

	public Texture GlockOffTexture;

	public Texture scytheOffTexture;

	public Texture shovelOffTexture;

	public Texture elixir;

	public Texture multiplayerInappFon;

	public Texture ranksFon;

	public string[] killedSpisok = new string[3]
	{
		string.Empty,
		string.Empty,
		string.Empty
	};

	public GUIStyle elixirsCountStyle;

	public GUIStyle ranksStyle;

	public GUIStyle chatStyle;

	public GUIStyle shopFromPauseStyle;

	public GUIStyle killedStyle;

	public GUIStyle combatRifleStyle;

	public GUIStyle goldenEagleInappStyle;

	public GUIStyle magicBowInappStyle;

	public GUIStyle spasStyle;

	public GUIStyle axeStyle;

	public GUIStyle famasStyle;

	public GUIStyle glockStyle;

	public GUIStyle chainsawStyle;

	public GUIStyle scytheStyle;

	public GUIStyle shovelStyle;

	private Vector3 camPosition;

	private Quaternion camRotaion;

	public bool showChat;

	public bool showChatOld;

	public bool showRanksOld;

	private bool isDeadFrame;

	public int myCommand;

	private string[] productIdentifiers = StoreKitEventListener.idsForSingle;

	public string myIp = string.Empty;

	public TrainingController trainigController;

	public bool isKilled;

	public bool theEnd;

	public string nickPobeditel;

	private bool _flashing;

	public Texture hitTexture;

	public Texture _skin;

	public float showNoInetTimer = 5f;

	public int countKills;

	public int maxCountKills;

	public GameObject _leftJoystick;

	public GameObject _rightJoystick;

	public float _curHealth;

	private float _timeWhenPurchShown;

	private bool inAppOpenedFromPause;

	public Texture sendTek;

	public Texture sendUstanovlenii;

	public GUIStyle cancelEindButStyle;

	public bool isMulti;

	public bool isInet;

	public bool isMine;

	public bool isCompany;

	public bool isCOOP;

	private ExperienceController expController;

	private float inGameTime;

	private int multiKill;

	private float timerShowMultyKill = -1f;

	private int[] porogiMultyKill = new int[5] { 2, 3, 5, 7, 9 };

	public Texture2D[] multyKillTextures;

	private GameObject _label;

	public float MaxHealth = MaxPlayerHealth;

	public float curArmor;

	public float MaxArmor;

	public int AmmoBoxWidth = 100;

	public int AmmoBoxHeight = 100;

	public int AmmoBoxOffset = 10;

	public int ScoreBoxWidth = 100;

	public int ScoreBoxHeight = 100;

	public int ScoreBoxOffset = 10;

	public float[] timerShow = new float[3] { -1f, -1f, -1f };

	public AudioClip deadPlayerSound;

	public AudioClip damagePlayerSound;

	private float GunFlashLifetime;

	public GameObject[] zoneCreatePlayer;

	public GUIStyle ScoreBox;

	public GUIStyle EnemiesBox;

	public GUIStyle AmmoBox;

	public GUIStyle HealthBox;

	public GUIStyle pauseStyle;

	public GUIStyle menuStyle;

	public GUIStyle soundStyle;

	public GUIStyle musicStyle;

	public GUIStyle buyStyle;

	public GUIStyle resumePauseStyle;

	public Texture sensitPausePlashka;

	public Texture slow_fast;

	public Texture polzunok;

	private float mySens;

	public GUIStyle sliderSensStyle;

	public GUIStyle thumbSensStyle;

	public GUIStyle enemiesLeftStyle;

	private GameObject damage;

	private bool damageShown;

	public Texture pauseFon;

	private Pauser _pauser;

	public Texture pauseTitle;

	private GameObject _gameController;

	private bool _backWasPressed;

	private GameObject _player;

	public GameObject bulletPrefab;

	public GameObject _bulletSpawnPoint;

	public GameObject _purchaseActivityIndicator;

	private GameObject _inAppGameObject;

	public StoreKitEventListener _listener;

	public GUIStyle puliInApp;

	public GUIStyle healthInApp;

	public GUIStyle pulemetInApp;

	public GUIStyle crystalSwordInapp;

	public GUIStyle elixirInapp;

	public bool isInappWinOpen;

	public InGameGUI inGameGUI;

	private Dictionary<string, KeyValuePair<Action, GUIStyle>> _actionsForPurchasedItems = new Dictionary<string, KeyValuePair<Action, GUIStyle>>();

	private bool scrollEnabled;

	private Vector2 scrollStartTouch;

	private float otstupMejduProd = 10f;

	private float widthPoduct;

	private readonly List<object> _products = new List<object>();

	private readonly ICollection<object> _productsFull = new object[0];

	private ZombieCreator _zombieCreator;

	private WeaponManager ___weaponManager;

	public GUIStyle armorStyle;

	public Texture armorShield;

	public int countKillsCommandBlue;

	public int countKillsCommandRed;

	private Vector2 leftFingerPos = Vector2.zero;

	private Vector2 leftFingerLastPos = Vector2.zero;

	private Vector2 leftFingerMovedBy = Vector2.zero;

	private bool canReceiveSwipes = true;

	public float slideMagnitudeX;

	public float slideMagnitudeY;

	public AudioClip ChangeWeaponClip;

	private PhotonView photonView;

	private float height = (float)Screen.height * 0.078f;

	private float _width = 125f;

	public GUIStyle sword_2_Style;

	public GUIStyle hammerStyle;

	public GUIStyle staffStyle;

	public GUIStyle laserStyle;

	public GUIStyle lightSwordStyle;

	public GUIStyle berettaStyle;

	public Texture sword_2_off_text;

	public Texture hammer_off_text;

	public Texture staffOff_text;

	public Texture laserOff_text;

	public Texture lightSwordOff_text;

	public Texture berettaOff_text;

	public GUIStyle maceStyle;

	public GUIStyle crossbowStyle;

	public GUIStyle minigunStyle;

	public Texture mace_off;

	public Texture crossbow_off;

	public Texture minigun_off;

	public AudioClip clickShop;

	public List<MessageChat> messages = new List<MessageChat>();

	public int _armorType;

	private bool _showArt;

	private Texture _campaignWeaponTexture;

	public static int MaxPlayerHealth
	{
		get
		{
			return 9;
		}
	}

	public float CurHealth
	{
		get
		{
			return _curHealth;
		}
		set
		{
			_curHealth = value;
		}
	}

	public float curHealthProp
	{
		get
		{
			return CurHealth;
		}
		set
		{
			if (CurHealth > value && !damageShown)
			{
				StartCoroutine(FlashWhenHit());
			}
			CurHealth = value;
		}
	}

	public static int FontSizeForMessages
	{
		get
		{
			return Mathf.RoundToInt((float)Screen.height * 0.03f);
		}
	}

	public WeaponManager _weaponManager
	{
		get
		{
			return ___weaponManager;
		}
		set
		{
			___weaponManager = value;
		}
	}

	private void OnGUI()
	{
		if (!showGUI || (coinsShop.thisScript != null && coinsShop.thisScript.enabled))
		{
			return;
		}
		GUI.enabled = !showGUIUnlockFullVersion;
		float num = (float)Screen.height / 768f;
		if (showChat)
		{
			if (!showChatOld)
			{
				inGameGUI.gameObject.SetActive(false);
			}
			_leftJoystick.SetActive(false);
			_rightJoystick.SetActive(false);
			showChatOld = showChat;
			return;
		}
		if (showChatOld)
		{
			inGameGUI.gameObject.SetActive(true);
			_leftJoystick.SetActive(true);
			_rightJoystick.SetActive(true);
			_weaponManager.currentWeaponSounds.gameObject.SetActive(true);
		}
		showChatOld = showChat;
		if (showRanks)
		{
			if (!showRanksOld)
			{
				inGameGUI.gameObject.SetActive(false);
			}
			_leftJoystick.SetActive(false);
			_rightJoystick.SetActive(false);
			showRanksOld = showRanks;
		}
		else
		{
			if (showRanksOld)
			{
				inGameGUI.gameObject.SetActive(true);
				_leftJoystick.SetActive(true);
				_rightJoystick.SetActive(true);
				_weaponManager.currentWeaponSounds.gameObject.SetActive(true);
			}
			showRanksOld = showRanks;
		}
		if (showRanks)
		{
			GUI.DrawTexture(new Rect(((float)Screen.width - 2048f * (float)Screen.height / 1154f) / 2f, 0f, 2048f * (float)Screen.height / 1154f, Screen.height), ranksFon, ScaleMode.StretchToFill);
			GUI.DrawTexture(new Rect((float)Screen.width / 2f - (float)head_players.width / 2f * num, (float)Screen.height * 0.1f - (float)head_players.height / 2f * (float)Screen.height / 768f, (float)head_players.width * num, (float)head_players.height * num), head_players);
			Rect position = new Rect((float)Screen.width * 0.5f - 310f * num, (float)Screen.height * 0.5f - 222.5f * num, 620f * num, 445f * num);
			float num2 = 49f * num;
			float num3 = position.height * 0.5f + 45f * num;
			float num4 = 17f * num;
			float num5 = 26f * num;
			float f = 22f * num;
			float num6 = 26f * num;
			playersWindow.fontSize = Mathf.RoundToInt(f);
			playersWindowFrags.fontSize = Mathf.RoundToInt(f);
			playersWindowFrags.alignment = TextAnchor.MiddleRight;
			playersWindow.alignment = TextAnchor.MiddleLeft;
			GUI.DrawTexture(position, (PlayerPrefs.GetInt("company", 0) != 1 || PlayerPrefs.GetInt("COOP", 0) != 0) ? tableFon : tableFonCommand);
			GameObject[] array = GameObject.FindGameObjectsWithTag("NetworkTable");
			List<GameObject> list = new List<GameObject>();
			List<GameObject> list2 = new List<GameObject>();
			List<GameObject> list3 = new List<GameObject>();
			for (int i = 1; i < array.Length; i++)
			{
				GameObject gameObject = array[i];
				int num7 = i - 1;
				while (num7 >= 0 && ((PlayerPrefs.GetInt("COOP", 0) != 1) ? ((float)array[num7].GetComponent<NetworkStartTable>().CountKills) : array[num7].GetComponent<NetworkStartTable>().score) < ((PlayerPrefs.GetInt("COOP", 0) != 1) ? ((float)gameObject.GetComponent<NetworkStartTable>().CountKills) : gameObject.GetComponent<NetworkStartTable>().score))
				{
					array[num7 + 1] = array[num7];
					num7--;
				}
				array[num7 + 1] = gameObject;
			}
			if (PlayerPrefs.GetInt("company", 0) == 1)
			{
				for (int j = 0; j < array.Length; j++)
				{
					if (array[j].GetComponent<NetworkStartTable>().myCommand == 1)
					{
						list.Add(array[j]);
					}
					else if (array[j].GetComponent<NetworkStartTable>().myCommand == 2)
					{
						list2.Add(array[j]);
					}
					else
					{
						list3.Add(array[j]);
					}
				}
				for (int k = 0; k < array.Length; k++)
				{
					if (k < list.Count)
					{
						array[k] = list[k];
					}
					if (k >= list.Count && k < list.Count + list2.Count)
					{
						array[k] = list2[k - list.Count];
					}
					if (k >= list.Count + list2.Count)
					{
						array[k] = list3[k - list.Count - list2.Count];
					}
				}
			}
			if (array.Length > 0)
			{
				for (int l = 0; l < array.Length; l++)
				{
					GameObject gameObject2 = array[l];
					if (gameObject2.Equals(_weaponManager.myTable))
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
					else if (PlayerPrefs.GetInt("company") == 1)
					{
						if (gameObject2.GetComponent<NetworkStartTable>().myCommand == 0)
						{
							playersWindow.normal.textColor = new Color(0.7843f, 0.7843f, 0.7843f, 1f);
							playersWindowFrags.normal.textColor = new Color(0.7843f, 0.7843f, 0.7843f, 1f);
						}
						else if (gameObject2.GetComponent<NetworkStartTable>().myCommand == 1)
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
					int num8 = ((PlayerPrefs.GetInt("COOP", 0) != 1) ? gameObject2.GetComponent<NetworkStartTable>().CountKills : Mathf.RoundToInt(gameObject2.GetComponent<NetworkStartTable>().score));
					float num9 = 1000000f;
					if (PlayerPrefs.GetInt("company", 0) == 1 && PlayerPrefs.GetInt("COOP", 0) == 0)
					{
						if (l < list.Count)
						{
							num9 = num2 + num5 * (float)l;
						}
						if (l >= list.Count && l < list.Count + list2.Count)
						{
							num9 = num3 + num5 * (float)(l - list.Count);
						}
						if (l >= list.Count + list2.Count)
						{
							num9 = 1000000f;
						}
					}
					else
					{
						num9 = num2 + num5 * (float)l;
					}
					GUI.Label(new Rect(position.x + num4, position.y + num9, position.width * 0.75f, f), gameObject2.GetComponent<NetworkStartTable>().NamePlayer, playersWindow);
					GUI.Label(new Rect(position.x + position.width - num4 - position.width * 0.1f, position.y + num9, position.width * 0.1f, f), (num8 != -1) ? num8.ToString() : "0", playersWindowFrags);
					Texture2D image = expController.marks[gameObject2.GetComponent<NetworkStartTable>().myRanks];
					GUI.DrawTexture(new Rect(position.x - num6, position.y + num9, num6, num6), image);
				}
			}
			if (!GUI.Button(new Rect(21f * num, (float)Screen.height - (21f + (float)closeRanksStyle.normal.background.height) * num, (float)closeRanksStyle.normal.background.width * num, (float)closeRanksStyle.normal.background.height * num), string.Empty, closeRanksStyle))
			{
				return;
			}
			AddButtonHandlers();
			showRanks = false;
		}
		GUI.depth = 2;
		if (isMulti && multiKill > 1 && timerShowMultyKill > 0f)
		{
			GUI.DrawTexture(new Rect((float)Screen.width * 0.5f - 177f * num, 0f, 354f * num, 354f * num), multyKillTextures[(multiKill >= 6) ? 4 : (multiKill - 2)]);
			timerShowMultyKill -= Time.deltaTime;
		}
		GUI.skin = MySkin;
		if (!isZooming && (!Defs.IsTraining || !TrainingController.isPressSkip) && _weaponManager.currentWeaponSounds.aimTextureV != null && _weaponManager.currentWeaponSounds.aimTextureH != null)
		{
			AimTexture = _weaponManager.currentWeaponSounds.aimTextureV;
			GUI.DrawTexture(new Rect((float)Screen.width * 0.5f - (float)AimTexture.width * num * 0.5f, (float)Screen.height * 0.5f - (float)AimTexture.height * num - _weaponManager.currentWeaponSounds.tekKoof * _weaponManager.currentWeaponSounds.startZone.y * 0.5f * num, (float)AimTexture.width * num, (float)AimTexture.height * num), AimTexture);
			GUI.DrawTexture(new Rect((float)Screen.width * 0.5f - (float)AimTexture.width * num * 0.5f, (float)Screen.height * 0.5f + _weaponManager.currentWeaponSounds.tekKoof * _weaponManager.currentWeaponSounds.startZone.y * 0.5f * num, (float)AimTexture.width * num, (float)AimTexture.height * num), AimTexture);
			AimTexture = _weaponManager.currentWeaponSounds.aimTextureH;
			GUI.DrawTexture(new Rect((float)Screen.width * 0.5f - (float)AimTexture.width * num - _weaponManager.currentWeaponSounds.tekKoof * _weaponManager.currentWeaponSounds.startZone.x * 0.5f * num, (float)Screen.height * 0.5f - (float)AimTexture.height * num * 0.5f, (float)AimTexture.width * num, (float)AimTexture.height * num), AimTexture);
			GUI.DrawTexture(new Rect((float)Screen.width * 0.5f + _weaponManager.currentWeaponSounds.tekKoof * _weaponManager.currentWeaponSounds.startZone.x * 0.5f * num, (float)Screen.height * 0.5f - (float)AimTexture.height * num * 0.5f, (float)AimTexture.width * num, (float)AimTexture.height * num), AimTexture);
		}
		float num10 = (float)Screen.height * 0.080729164f;
		float num11 = num10 * ((float)buyStyle.normal.background.width / (float)buyStyle.normal.background.height);
		Rect position2 = new Rect((float)Screen.width - num11, 0f, num11, num10);
		float num12 = num10;
		float num13 = num12 * ((float)ranksStyle.normal.background.width / (float)ranksStyle.normal.background.height);
		Rect position3 = new Rect(position2.x - num13, 0f, num13, num10);
		float num14 = num12 * ((float)chatStyle.normal.background.width / (float)chatStyle.normal.background.height);
		Rect position4 = new Rect(position3.x - num14, 0f, num14, num10);
		if (PlayerPrefs.GetInt("MultyPlayer") == 1 && !_pauser.paused && PlayerPrefs.GetInt("ChatOn", 1) == 1 && GUI.Button(position4, string.Empty, chatStyle))
		{
			showChat = true;
			_weaponManager.currentWeaponSounds.gameObject.SetActive(false);
			GameObject gameObject3 = (GameObject)UnityEngine.Object.Instantiate(chatViewer);
			gameObject3.GetComponent<ChatViewrController>().PlayerObject = base.gameObject;
		}
		Rect rect = new Rect(0f, 0f, 73f * (float)Screen.width / 1024f, 73f * (float)Screen.width / 1024f);
		AmmoBox.fontSize = Mathf.RoundToInt(18f * (float)Screen.width / 1024f);
		Rect position5 = new Rect((float)Screen.width - 264f * (float)Screen.width / 1024f, 94f * (float)Screen.width / 1024f, 264f * (float)Screen.width / 1024f, 95f * (float)Screen.width / 1024f);
		Rect position6 = new Rect((float)Screen.width - 172f * (float)Screen.width / 1024f, 186f * (float)Screen.width / 1024f, (float)(40 * Screen.width) / 1024f, (float)(28 * Screen.width) / 1024f);
		Rect position7 = new Rect((float)Screen.width - 135f * (float)Screen.width / 1024f, 186f * (float)Screen.width / 1024f, 130f * (float)Screen.width / 1024f, (float)(28 * Screen.width) / 1024f);
		if (_weaponManager == null)
		{
			Debug.LogWarning("OnGUI(): _weaponManager is null.");
		}
		else
		{
			if (_weaponManager.playerWeapons == null)
			{
				Debug.LogWarning("OnGUI(): _weaponManager.playerWeapons is null.");
			}
			if (_weaponManager.currentWeaponSounds == null)
			{
				Debug.LogWarning("OnGUI(): _weaponManager.currentWeaponSounds is null.");
			}
		}
		if (_weaponManager != null && _weaponManager.CurrentWeaponIndex >= 0 && _weaponManager.CurrentWeaponIndex < _weaponManager.playerWeapons.Count && !_weaponManager.currentWeaponSounds.isMelee && (!Defs.IsTraining || TrainingController.stepTraining >= TrainingController.stepTrainingList["SwipeWeapon"]))
		{
			GUI.DrawTexture(position6, ammoTexture);
			GUI.Box(position7, ((Weapon)_weaponManager.playerWeapons[_weaponManager.CurrentWeaponIndex]).currentAmmoInClip + "/" + ((Weapon)_weaponManager.playerWeapons[_weaponManager.CurrentWeaponIndex]).currentAmmoInBackpack, AmmoBox);
		}
		if (_campaignWeaponTexture != null)
		{
			Rect position8 = new Rect((float)Screen.width / 2f - (float)_campaignWeaponTexture.width / 2f * Defs.Coef, (float)Screen.height / 3f, (float)_campaignWeaponTexture.width * Defs.Coef, (float)_campaignWeaponTexture.height * Defs.Coef);
			GUI.DrawTexture(position8, _campaignWeaponTexture);
		}
		ScoreBox.fontSize = Mathf.RoundToInt((float)Screen.height * 0.035f);
		float num15 = (float)(enemiesTxture.width * Screen.width) / 1024f;
		float num16 = num15 * ((float)enemiesTxture.height / (float)enemiesTxture.width);
		float num17 = 13f;
		EnemiesBox.fontSize = Mathf.RoundToInt((float)Screen.height * 0.035f);
		if (PlayerPrefs.GetInt("MultyPlayer") == 1)
		{
			killedStyle.fontSize = Mathf.RoundToInt(20f * num);
			killedStyle.normal.textColor = new Color(1f, 1f, 1f, 1f);
			labelGameChatStyle.fontSize = Mathf.RoundToInt(20f * num);
			if (timerShow[0] > 0f)
			{
				GUI.Label(new Rect((float)Screen.height * 0.04f, (float)Screen.height * 0.12f, (float)Screen.width * 0.5f, killedStyle.fontSize), killedSpisok[0], killedStyle);
			}
			if (timerShow[1] > 0f)
			{
				GUI.Label(new Rect((float)Screen.height * 0.04f, (float)Screen.height * 0.12f + (float)killedStyle.fontSize, (float)Screen.width * 0.5f, killedStyle.fontSize), killedSpisok[1], killedStyle);
			}
			if (timerShow[2] > 0f)
			{
				GUI.Label(new Rect((float)Screen.height * 0.04f, (float)Screen.height * 0.12f + (float)(killedStyle.fontSize * 2), (float)Screen.width * 0.5f, killedStyle.fontSize), killedSpisok[2], killedStyle);
			}
			if (PlayerPrefs.GetInt("ChatOn", 1) == 1)
			{
				int num18 = messages.Count - 1;
				while (num18 >= 0 && messages.Count - num18 - 1 < 3)
				{
					if (Time.time - messages[num18].time < 10f)
					{
						if ((PlayerPrefs.GetString("TypeConnect").Equals("local") && messages[num18].IDLocal == _weaponManager.myPlayer.GetComponent<NetworkView>().viewID) || (PlayerPrefs.GetString("TypeConnect").Equals("inet") && messages[num18].ID == _weaponManager.myPlayer.GetComponent<PhotonView>().viewID))
						{
							labelGameChatStyle.normal.textColor = new Color(0f, 1f, 0.15f, 1f);
						}
						else
						{
							if (messages[num18].command == 0)
							{
								labelGameChatStyle.normal.textColor = new Color(1f, 1f, 0.15f, 1f);
							}
							if (messages[num18].command == 1)
							{
								labelGameChatStyle.normal.textColor = new Color(0f, 0f, 1f, 1f);
							}
							if (messages[num18].command == 2)
							{
								labelGameChatStyle.normal.textColor = new Color(1f, 0f, 0f, 1f);
							}
						}
						GUI.Label(new Rect((float)Screen.height * 0.04f, (float)Screen.height * 0.12f + (float)(killedStyle.fontSize * (messages.Count - 1 - num18 + 3)), (float)Screen.width * 0.75f, killedStyle.fontSize), messages[num18].text, labelGameChatStyle);
					}
					num18--;
				}
			}
			if (PlayerPrefs.GetInt("COOP", 0) == 1)
			{
				ScoreBox.fontSize = Mathf.RoundToInt((float)Screen.height * 0.025f);
			}
		}
		bool flag = true;
		float left = (float)Screen.width * 0.271f;
		float width = (float)Screen.width * 0.521f;
		if (_weaponManager == null)
		{
			Debug.LogWarning("OnGUI(): _weaponManager is null.");
		}
		else if (_weaponManager.currentWeaponSounds == null)
		{
			Debug.LogWarning("OnGUI(): _weaponManager.currentWeaponSounds is null.");
		}
		else if (!Defs.IsTraining || TrainingController.stepTraining >= TrainingController.stepTrainingList["SwipeWeapon"])
		{
			GUI.DrawTexture(position5, _weaponManager.currentWeaponSounds.preview);
		}
		if ((bool)_weaponManager && _weaponManager.playerWeapons != null && _weaponManager.playerWeapons.Count > 1 && (!Defs.IsTraining || TrainingController.stepTraining >= TrainingController.stepTrainingList["SwipeWeapon"]))
		{
			GUI.Box(new Rect((float)Screen.width - 186f * (float)Screen.width / 1024f, 94f * (float)Screen.width / 1024f, 186f * (float)Screen.width / 1024f, 23f * (float)Screen.width / 1024f), "< SWIPE >", ScoreBox);
		}
		bool flag2 = false;
		if (Application.platform == RuntimePlatform.Android)
		{
			if (Input.GetKey(KeyCode.Escape))
			{
				_backWasPressed = true;
			}
			else
			{
				if (_backWasPressed)
				{
					flag2 = true;
				}
				_backWasPressed = false;
			}
		}
		if (_pauser == null)
		{
			Debug.LogWarning("OnGUI(): _pauser is null.");
		}
		else if (flag2 && !_pauser.paused && !_pauser.paused)
		{
			flag2 = false;
			SwitchPause();
		}
		if (PlayerPrefs.GetInt("MultyPlayer") != 1)
		{
			int num19 = GlobalGameController.EnemiesToKill - _zombieCreator.NumOfDeadZombies;
			if (!Defs.IsSurvival && num19 == 0)
			{
				enemiesLeftStyle.fontSize = Mathf.RoundToInt((float)Screen.height * 0.035f);
				string text = ((!LevelBox.weaponsFromBosses.ContainsKey(Application.loadedLevelName)) ? "Enter the Portal..." : "Get the gun...");
				if (_zombieCreator.bossShowm)
				{
					text = "Defeat the Boss!";
				}
				GUI.Box(new Rect(left, height + (float)(enemiesLeftStyle.fontSize * ((!Defs.IsTraining) ? 2 : 3)), width, height), text, enemiesLeftStyle);
			}
		}
		else if (GUI.Button(position3, string.Empty, ranksStyle) && !_pauser.paused)
		{
			RemoveButtonHandelrs();
			showRanks = true;
		}
		bool flag3 = true;
		if (Defs.IsTraining && TrainingController.stepTraining != TrainingController.stepTrainingList["InterTheShop"])
		{
			flag3 = false;
		}
		if (flag3)
		{
			GUI.enabled = !isInappWinOpen;
			if (GUI.Button(position2, string.Empty, buyStyle) && !_pauser.paused)
			{
				if (Defs.IsTraining)
				{
					TrainingController.isNextStep = TrainingController.stepTrainingList["InterTheShop"];
				}
				if (CurHealth > 0f)
				{
					SetInApp();
					SetPause();
					if (PlayerPrefsX.GetBool(PlayerPrefsX.SoundFXSetting, true))
					{
						NGUITools.PlaySound(clickShop);
					}
				}
				GUI.enabled = true;
			}
		}
		Shop.sharedShop.SetHatsAndCapesEnabled(false);
		Shop.sharedShop.SetGearCatEnabled(true);
		Shop.sharedShop.ShowShop(isInappWinOpen);
		if (Time.realtimeSinceStartup - _timeWhenPurchShown >= GUIHelper.Int)
		{
			productPurchased = false;
		}
		if (productPurchased)
		{
			labelStyle.fontSize = FontSizeForMessages;
		}
		if ((bool)_pauser && _pauser.paused && !isInappWinOpen)
		{
			Rect position9 = new Rect(((float)Screen.width - 2048f * (float)Screen.height / 1154f) / 2f, 0f, 2048f * (float)Screen.height / 1154f, Screen.height);
			GUI.DrawTexture(position9, pauseFon, ScaleMode.StretchToFill);
			float num20 = 15f;
			Vector2 vector = new Vector2(176f, 150f - num20);
			float num21 = (float)Screen.height * 0.4781f;
			Rect position10 = new Rect((float)Screen.width * 0.5f - num21 * 0.5f, (float)Screen.height * 0.05f, num21, (float)Screen.height * 0.1343f);
			GUI.DrawTexture(position10, pauseTitle);
			float num22 = (float)resumePauseStyle.normal.background.width * Defs.Coef;
			float num23 = num22 * ((float)resumePauseStyle.normal.background.height / (float)resumePauseStyle.normal.background.width);
			float num24 = num23 * 0.01f;
			if (GUI.Button(new Rect(position10.x + position10.width / 2f - num22 / 2f, 187f * (float)Screen.height / 768f, num22, num23), string.Empty, resumePauseStyle) || flag2)
			{
				SetPause();
			}
			Rect position11 = new Rect(position10.x + position10.width / 2f - num22 / 2f, 316f * (float)Screen.height / 768f, num22, num23);
			if (PlayerPrefs.GetInt(Defs.TrainingCompleted_4_4_Sett, 0) != 0 && GUI.Button(new Rect(position11.x + position11.width / 2f - (float)menuStyle.active.background.width / 2f * (float)Screen.height / 768f, 445f * (float)Screen.height / 768f, (float)(menuStyle.active.background.width * Screen.height) / 768f, (float)(menuStyle.active.background.height * Screen.height) / 768f), string.Empty, menuStyle))
			{
				Time.timeScale = 1f;
				Time.timeScale = 1f;
				if (PlayerPrefs.GetInt("MultyPlayer") == 1)
				{
					GameObject[] array2 = GameObject.FindGameObjectsWithTag("NetworkTable");
					GameObject[] array3 = array2;
					foreach (GameObject gameObject4 in array3)
					{
						gameObject4.GetComponent<NetworkStartTable>().sendDelMyPlayer();
					}
					if (PlayerPrefs.GetString("TypeConnect").Equals("local"))
					{
						if (PlayerPrefs.GetString("TypeGame").Equals("server"))
						{
							Network.Disconnect(200);
							GameObject.FindGameObjectWithTag("NetworkTable").GetComponent<LANBroadcastService>().StopBroadCasting();
						}
						else if (Network.connections.Length == 1)
						{
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
						coinsShop.hideCoinsShop();
						coinsPlashka.hidePlashka();
						ConnectGUI.Local();
					}
					else
					{
						coinsShop.hideCoinsShop();
						coinsPlashka.hidePlashka();
						PlayerPrefs.SetInt("ExitGame", 1);
						PhotonNetwork.LeaveRoom();
					}
				}
				else if (Defs.IsSurvival)
				{
					if (GlobalGameController.Score > PlayerPrefs.GetInt(Defs.SurvivalScoreSett, 0))
					{
						PlayerPrefs.SetInt(Defs.SurvivalScoreSett, GlobalGameController.Score);
						PlayerPrefs.Save();
					}
					PlayerPrefs.SetInt("IsGameOver", 1);
					LevelCompleteLoader.action = null;
					LevelCompleteLoader.sceneName = "LevelComplete";
					Application.LoadLevel("LevelToCompleteProm");
				}
				else
				{
					LevelCompleteLoader.action = null;
					LevelCompleteLoader.sceneName = "ChooseLevel";
					bool flag4 = PlayerPrefs.GetInt("MultyPlayer") != 1;
					if (!flag4)
					{
						FlurryPluginWrapper.LogEvent("Back to Main Menu");
					}
					Application.LoadLevel((!flag4) ? Defs.MainMenuScene : "LevelToCompleteProm");
				}
			}
			if (GUI.Button(position11, string.Empty, shopFromPauseStyle) && CurHealth > 0f)
			{
				SetInApp();
				inAppOpenedFromPause = true;
			}
			float num25 = 15f;
			Rect position12 = new Rect(21f * num, (float)Screen.height - (21f + (float)soundStyle.normal.background.height) * num, (float)soundStyle.normal.background.width * num, (float)soundStyle.normal.background.height * num);
			Defs.isSoundFX = GUI.Toggle(position12, Defs.isSoundFX, string.Empty, soundStyle);
			PlayerPrefsX.SetBool(PlayerPrefsX.SoundFXSetting, Defs.isSoundFX);
			PlayerPrefs.Save();
			bool isSoundMusic = Defs.isSoundMusic;
			Rect position13 = new Rect(21f * num, (float)Screen.height - (21f + (float)soundStyle.normal.background.height) * 2f * num, (float)soundStyle.normal.background.width * num, (float)soundStyle.normal.background.height * num);
			Defs.isSoundMusic = GUI.Toggle(position13, Defs.isSoundMusic, string.Empty, musicStyle);
			PlayerPrefsX.SetBool(PlayerPrefsX.SoundMusicSetting, Defs.isSoundMusic);
			PlayerPrefs.Save();
			if (isSoundMusic != Defs.isSoundMusic && isSoundMusic != Defs.isSoundMusic)
			{
				if (Defs.isSoundMusic)
				{
					GameObject.FindGameObjectWithTag("BackgroundMusic").GetComponent<BackgroundMusicController>().Play();
				}
				else
				{
					GameObject.FindGameObjectWithTag("BackgroundMusic").GetComponent<BackgroundMusicController>().Stop();
				}
			}
			Rect position14 = new Rect((float)(Screen.width / 2) - (float)sensitPausePlashka.width * 0.5f * Defs.Coef, position12.y + position12.height - (float)sensitPausePlashka.height * Defs.Coef, (float)sensitPausePlashka.width * Defs.Coef, (float)sensitPausePlashka.height * Defs.Coef);
			GUI.DrawTexture(position14, sensitPausePlashka);
			sliderSensStyle.fixedWidth = (float)slow_fast.width * num;
			sliderSensStyle.fixedHeight = (float)slow_fast.height * num;
			thumbSensStyle.fixedWidth = (float)polzunok.width * num;
			thumbSensStyle.fixedHeight = (float)polzunok.height * num;
			float num26 = (float)slow_fast.height * num;
			Rect position15 = new Rect((float)Screen.width * 0.5f - (float)slow_fast.width * 0.5f * num, position14.y + position14.height * 0.62f - num26 * 0.5f, (float)slow_fast.width * num, num26);
			mySens = GUI.HorizontalSlider(position15, PlayerPrefs.GetFloat("SensitivitySett", 12f), 6f, 18f, sliderSensStyle, thumbSensStyle);
			PlayerPrefs.SetFloat("SensitivitySett", mySens);
		}
		GUI.enabled = true;
	}

	public void hit(float dam)
	{
		if (!Defs.IsTraining)
		{
			if (curArmor >= dam)
			{
				curArmor -= dam;
			}
			else
			{
				CurHealth -= dam - curArmor;
				curArmor = 0f;
				CurrentCampaignGame.withoutHits = false;
			}
		}
		if (!damageShown)
		{
			StartCoroutine(FlashWhenHit());
		}
	}

	private void Awake()
	{
		isMulti = PlayerPrefs.GetInt("MultyPlayer") == 1;
		isInet = PlayerPrefs.GetString("TypeConnect").Equals("inet");
		isCompany = PlayerPrefs.GetInt("company", 0) == 1;
		isCOOP = PlayerPrefs.GetInt("COOP", 0) == 1;
	}

	[RPC]
	private void setMySkin(string str)
	{
		Debug.Log("setMySkin");
		byte[] data = Convert.FromBase64String(str);
		Texture2D texture2D = new Texture2D(64, 32);
		texture2D.LoadImage(data);
		texture2D.filterMode = FilterMode.Point;
		texture2D.Apply();
		sendUstanovlenii = texture2D;
	}

	private void sendMySkin()
	{
		Debug.Log("sendMySkin");
		Texture2D texture2D = sendTek as Texture2D;
		byte[] inArray = texture2D.EncodeToPNG();
		string text = Convert.ToBase64String(inArray);
		photonView.RPC("setMySkin", PhotonTargets.AllBuffered, text);
	}

	[RPC]
	private void SendChatMessage(string text)
	{
		if (!(_weaponManager == null) && !(_weaponManager.myPlayer == null))
		{
			if (!isInet)
			{
				_weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().AddMessage(text, Time.time, -1, base.transform.parent.GetComponent<NetworkView>().viewID, 0);
			}
			else
			{
				_weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().AddMessage(text, Time.time, base.transform.parent.GetComponent<PhotonView>().viewID, base.transform.parent.GetComponent<NetworkView>().viewID, myCommand);
			}
			GameObject gameObject = GameObject.FindGameObjectWithTag("ChatViewer");
		}
	}

	public void SendChat(string text)
	{
		text = _weaponManager.gameObject.GetComponent<FilterBadWorld>().FilterString(text);
		if (text != string.Empty)
		{
			if (!isInet)
			{
				base.GetComponent<NetworkView>().RPC("SendChatMessage", RPCMode.All, "< " + _weaponManager.myTable.GetComponent<NetworkStartTable>().NamePlayer + " > " + text);
			}
			else
			{
				photonView.RPC("SendChatMessage", PhotonTargets.All, "< " + _weaponManager.myTable.GetComponent<NetworkStartTable>().NamePlayer + " > " + text);
			}
		}
	}

	public void AddMessage(string text, float time, int ID, NetworkViewID IDLocal, int _command)
	{
		MessageChat item = default(MessageChat);
		item.text = text;
		item.time = time;
		item.ID = ID;
		item.IDLocal = IDLocal;
		item.command = _command;
		messages.Add(item);
		if (messages.Count > 20)
		{
			messages.RemoveAt(0);
		}
	}

	public void WalkAnimation()
	{
		if (_singleOrMultiMine() && (bool)_weaponManager && (bool)_weaponManager.currentWeaponSounds)
		{
			if (!_weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>().IsPlaying("Reload")) // I added this -met
                _weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>().CrossFade("Walk");
		}
	}

	public void IdleAnimation()
	{
		if (_singleOrMultiMine() && (bool)___weaponManager && (bool)___weaponManager.currentWeaponSounds)
		{
            if (!_weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>().IsPlaying("Reload")) // I added this -met
                ___weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>().CrossFade("Idle");
		}
	}

	public void ZoomPress()
	{
		Debug.Log("zoomPress");
		isZooming = !isZooming;
		if (isZooming)
		{
			myCamera.fieldOfView = _weaponManager.currentWeaponSounds.fieldOfViewZomm;
			gunCamera.fieldOfView = 1f;
			inGameGUI.SetScopeForWeapon(((Weapon)_weaponManager.playerWeapons[_weaponManager.CurrentWeaponIndex]).weaponPrefab.tag);
		}
		else
		{
			myCamera.fieldOfView = 75f;
			gunCamera.fieldOfView = 75f;
			inGameGUI.ResetScope();
		}
	}

	public void hideGUI()
	{
		showGUI = false;
	}

	public void setMyTamble(GameObject _myTable)
	{
		myTable = _myTable;
		myCommand = myTable.GetComponent<NetworkStartTable>().myCommand;
		_skin = myTable.GetComponent<NetworkStartTable>().mySkin;
		GameObject gameObject = null;
		GameObject gameObject2 = null;
		GameObject gameObject3 = base.transform.parent.gameObject;
		foreach (Transform item in gameObject3.transform)
		{
			if (item.gameObject.name.Equals("GameObject"))
			{
				WeaponSounds component = item.transform.GetChild(0).gameObject.GetComponent<WeaponSounds>();
				gameObject = component.bonusPrefab;
				if (!component.isMelee)
				{
					gameObject2 = item.transform.GetChild(0).Find("BulletSpawnPoint").transform.GetChild(0).gameObject;
				}
				break;
			}
		}
		GameObject[] array = null;
		array = ((!(gameObject2 != null)) ? new GameObject[3] { gameObject, capesPoint, hatsPoint } : new GameObject[4] { gameObject, gameObject2, capesPoint, hatsPoint });
		if (gameObject3 != null)
		{
			SetTextureRecursivelyFrom(gameObject3, gameObject3.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>()._skin, array);
		}
	}

	public void AddWeapon(GameObject weaponPrefab)
	{
		int score;
		if (_weaponManager.AddWeapon(weaponPrefab, out score))
		{
			ChangeWeapon(_weaponManager.CurrentWeaponIndex, false);
			return;
		}
		if (weaponPrefab != _weaponManager.GetPickPrefab() && weaponPrefab != _weaponManager.GetSwordPrefab() && weaponPrefab != _weaponManager.GetCombatRiflePrefab() && weaponPrefab != _weaponManager.GetGoldenEaglePrefab() && weaponPrefab != _weaponManager.GetMagicBowPrefab() && weaponPrefab != _weaponManager.GetSPASPrefab() && weaponPrefab != _weaponManager.GetAxePrefab() && weaponPrefab != _weaponManager.GetChainsawPrefab() && weaponPrefab != _weaponManager.GetFAMASPrefab() && weaponPrefab != _weaponManager.GetGlockPrefab() && weaponPrefab != _weaponManager.GetScythePrefab() && weaponPrefab != _weaponManager.GetShovelPrefab() && weaponPrefab != _weaponManager.GetHammerPrefab() && weaponPrefab != _weaponManager.GetSword_2_Prefab() && weaponPrefab != _weaponManager.GetStaffPrefab() && weaponPrefab != _weaponManager.GetLaserRiflePrefab() && weaponPrefab != _weaponManager.GetBerettaPrefab() && weaponPrefab != _weaponManager.GetLightSwordPrefab() && weaponPrefab != _weaponManager.GetMacePrefab() && weaponPrefab != _weaponManager.GetCrossbowPrefab() && weaponPrefab != _weaponManager.GetMinigunPrefab() && weaponPrefab != _weaponManager.GetGoldenPickPref() && weaponPrefab != _weaponManager.GetCrystPickPref() && weaponPrefab != _weaponManager.GetIronSwordPrefab() && weaponPrefab != _weaponManager.GetGoldenSwordPrefab() && weaponPrefab != _weaponManager.GetGoldenRedStonePrefab() && weaponPrefab != _weaponManager.GetGoldenSPASPrefab() && weaponPrefab != _weaponManager.GetGoldenGlockPrefab() && weaponPrefab != _weaponManager.GetCrystCrossbowPref() && weaponPrefab != _weaponManager.GetRedMinigunPref() && weaponPrefab != _weaponManager.GetRedLightSaberPrefab() && weaponPrefab != _weaponManager.GetSandFamasPrefab() && weaponPrefab != _weaponManager.GetWhiteBerettaPrefab() && weaponPrefab != _weaponManager.GetBlackEaglePrefab() && weaponPrefab != _weaponManager.GetCrystalAxePrefab() && weaponPrefab != _weaponManager.GetSteelAxePrefab() && weaponPrefab != _weaponManager.GetWoodenBowPrefab() && weaponPrefab != _weaponManager.GetChainsaw2Prefab() && weaponPrefab != _weaponManager.GetSteelCrossbowPrefab() && weaponPrefab != _weaponManager.GetHammer2Prefab() && weaponPrefab != _weaponManager.GetMace2Prefab() && weaponPrefab != _weaponManager.GetSword_22Prefab() && weaponPrefab != _weaponManager.GetStaff2Prefab() && weaponPrefab != _weaponManager.GetCrystGlockPref() && weaponPrefab != _weaponManager.GetCrystalSPASPref() && weaponPrefab != _weaponManager.GetTreePrefab() && weaponPrefab != _weaponManager.GetFireAxePrefab() && weaponPrefab != _weaponManager.Get3plShotgunPrefab() && weaponPrefab != _weaponManager.GetRevolver2Prefab() && weaponPrefab != _weaponManager.GetBarrettPrefab() && weaponPrefab != _weaponManager.GetSVDPrefab())
		{
			GlobalGameController.Score += score;
			if (PlayerPrefsX.GetBool(PlayerPrefsX.SoundFXSetting, true))
			{
				base.gameObject.GetComponent<AudioSource>().PlayOneShot(ChangeWeaponClip);
			}
			return;
		}
		foreach (Weapon playerWeapon in _weaponManager.playerWeapons)
		{
			if (playerWeapon.weaponPrefab == weaponPrefab)
			{
				ChangeWeapon(_weaponManager.playerWeapons.IndexOf(playerWeapon), false);
				break;
			}
		}
	}

	public void minusLiveFromZombi(int _minusLive)
	{
		photonView.RPC("minusLiveFromZombiRPC", PhotonTargets.All, _minusLive);
	}

	[RPC]
	public void minusLiveFromZombiRPC(int live)
	{
		if (photonView.isMine && !isKilled)
		{
			float num = (float)live - curArmor;
			if (num < 0f)
			{
				curArmor -= live;
				num = 0f;
			}
			else
			{
				curArmor = 0f;
			}
			CurHealth -= num;
		}
		StartCoroutine(Flash(base.gameObject.transform.parent.gameObject));
	}

	public static void SetLayerRecursively(GameObject obj, int newLayer)
	{
		if (null == obj)
		{
			return;
		}
		obj.layer = newLayer;
		foreach (Transform item in obj.transform)
		{
			if (!(null == item))
			{
				SetLayerRecursively(item.gameObject, newLayer);
			}
		}
	}

	public void ChangeWeapon(int index, bool shouldSetMaxAmmo = true)
	{
		if (isZooming)
		{
			ZoomPress();
		}
		photonView = PhotonView.Get(this);
		Quaternion rotation = Quaternion.identity;
		if ((bool)_player)
		{
			rotation = _player.transform.rotation;
		}
		if ((bool)_weaponManager.currentWeaponSounds)
		{
			rotation = _weaponManager.currentWeaponSounds.gameObject.transform.rotation;
			if (!isMulti)
			{
				_SetGunFlashActive(false);
				_weaponManager.currentWeaponSounds.gameObject.transform.parent = null;
				UnityEngine.Object.Destroy(_weaponManager.currentWeaponSounds.gameObject);
			}
			else
			{
				_weaponManager.currentWeaponSounds.gameObject.transform.parent = null;
				if (isInet)
				{
					PhotonNetwork.Destroy(_weaponManager.currentWeaponSounds.gameObject);
				}
				else
				{
					Network.Destroy(_weaponManager.currentWeaponSounds.gameObject);
				}
			}
			_weaponManager.currentWeaponSounds = null;
		}
		GameObject gameObject;
		if (!isMulti)
		{
			gameObject = (GameObject)UnityEngine.Object.Instantiate(((Weapon)_weaponManager.playerWeapons[index]).weaponPrefab, Vector3.zero, Quaternion.identity);
			gameObject.transform.parent = base.gameObject.transform;
			gameObject.transform.rotation = rotation;
		}
		else if (isInet)
		{
			gameObject = PhotonNetwork.Instantiate("Weapons/" + ((Weapon)_weaponManager.playerWeapons[index]).weaponPrefab.name, -Vector3.up * 1000f, Quaternion.identity, 0);
			gameObject.transform.position = -1000f * Vector3.up;
		}
		else
		{
			gameObject = (GameObject)Network.Instantiate(((Weapon)_weaponManager.playerWeapons[index]).weaponPrefab, -Vector3.up * 1000f, Quaternion.identity, 0);
			gameObject.transform.position = -1000f * Vector3.up;
		}
		SetLayerRecursively(gameObject, 9);
		_weaponManager.CurrentWeaponIndex = index;
		_weaponManager.currentWeaponSounds = gameObject.GetComponent<WeaponSounds>();
		if (gameObject.transform.parent == null)
		{
			Debug.LogWarning("nw.transform.parent == null");
		}
		else if (_weaponManager.currentWeaponSounds == null)
		{
			Debug.LogWarning("_weaponManager.currentWeaponSounds == null");
		}
		else
		{
			gameObject.transform.position = gameObject.transform.parent.TransformPoint(_weaponManager.currentWeaponSounds.gunPosition);
		}
		PlayerPrefs.SetInt("setSeriya", _weaponManager.currentWeaponSounds.isSerialShooting ? 1 : 0);
		PlayerPrefs.Save();
		JoystickSharp component = _rightJoystick.GetComponent<JoystickSharp>();
		if (component != null && _weaponManager != null && _weaponManager.currentWeaponSounds != null)
		{
			component.setSeriya(_weaponManager.currentWeaponSounds.isSerialShooting);
		}
		if (shouldSetMaxAmmo)
		{
		}
		JoystickSharp component2 = _rightJoystick.GetComponent<JoystickSharp>();
		if (component2 != null)
		{
			if (((Weapon)_weaponManager.playerWeapons[_weaponManager.CurrentWeaponIndex]).currentAmmoInClip > 0 || _weaponManager.currentWeaponSounds.isMelee)
			{
				component2.HasAmmo();
			}
			else
			{
				component2.NoAmmo();
			}
		}
//		_weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>()["Reload"].layer = 1;
//		_weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>()["Shoot"].layer = 1;
		if (!_weaponManager.currentWeaponSounds.isMelee)
		{
			foreach (Transform item in _weaponManager.currentWeaponSounds.gameObject.transform)
			{
				if (item.name.Equals("BulletSpawnPoint"))
				{
					_bulletSpawnPoint = item.gameObject;
					break;
				}
			}
			GunFlash = GameObject.Find("GunFlash").transform;
		}
		SendSpeedModifier();
		if (PlayerPrefsX.GetBool(PlayerPrefsX.SoundFXSetting, true))
		{
			base.gameObject.GetComponent<AudioSource>().PlayOneShot(ChangeWeaponClip);
		}
	}

	private void SendSpeedModifier()
	{
		if (_player != null)
		{
			_player.GetComponent<FirstPersonControlSharp>().SetSpeedModifier(_weaponManager.currentWeaponSounds.speedModifier);
		}
	}

	public bool NeedAmmo()
	{
		int currentWeaponIndex = _weaponManager.CurrentWeaponIndex;
		Weapon weapon = (Weapon)_weaponManager.playerWeapons[currentWeaponIndex];
		return weapon.currentAmmoInBackpack < _weaponManager.currentWeaponSounds.MaxAmmoWithRespectToInApp;
	}

	private void SwitchPause()
	{
		if (CurHealth > 0f)
		{
			SetPause();
		}
	}

	private void ShopPressed()
	{
		if (CurHealth > 0f)
		{
			SetInApp();
			SetPause();
		}
		GUI.enabled = true;
	}

	private void AddButtonHandlers()
	{
		PauseTapReceiver.PauseClicked += SwitchPause;
		ShopTapReceiver.ShopClicked += ShopPressed;
		RanksTapReceiver.RanksClicked += RanksPressed;
	}

	private void RemoveButtonHandelrs()
	{
		PauseTapReceiver.PauseClicked -= SwitchPause;
		ShopTapReceiver.ShopClicked -= ShopPressed;
		RanksTapReceiver.RanksClicked -= RanksPressed;
	}

	private void RanksPressed()
	{
		RemoveButtonHandelrs();
		showRanks = true;
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	[RPC]
	private void setIp(string _ip)
	{
		myIp = _ip;
	}

	private void CheckTimeCondition()
	{
		CampaignLevel campaignLevel = null;
		foreach (LevelBox campaignBox in LevelBox.campaignBoxes)
		{
			if (!campaignBox.name.Equals(CurrentCampaignGame.boXName))
			{
				continue;
			}
			foreach (CampaignLevel level in campaignBox.levels)
			{
				if (level.sceneName.Equals(CurrentCampaignGame.levelSceneName))
				{
					campaignLevel = level;
					break;
				}
			}
			break;
		}
		float timeToComplete = campaignLevel.timeToComplete;
		if (inGameTime >= timeToComplete)
		{
			CurrentCampaignGame.completeInTime = false;
		}
	}

	private void Start()
	{
		photonView = PhotonView.Get(this);
		if (isMulti)
		{
			if (!isInet)
			{
				isMine = base.GetComponent<NetworkView>().isMine;
			}
			else
			{
				isMine = photonView.isMine;
			}
		}
		if (GameObject.FindGameObjectWithTag("TrainingController") != null)
		{
			trainigController = GameObject.FindGameObjectWithTag("TrainingController").GetComponent<TrainingController>();
		}
		expController = GameObject.FindGameObjectWithTag("ExperienceController").GetComponent<ExperienceController>();
		widthPoduct = (float)(healthInApp.normal.background.width * Screen.height) / 768f * (320f / (float)healthInApp.normal.background.height);
		if (isMulti)
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag("NetworkTable");
			if (isInet)
			{
				GameObject[] array2 = array;
				foreach (GameObject gameObject in array2)
				{
					if (gameObject.GetComponent<PhotonView>().owner == base.transform.GetComponent<PhotonView>().owner)
					{
						myTable = gameObject;
						break;
					}
				}
			}
			else
			{
				GameObject[] array3 = array;
				foreach (GameObject gameObject2 in array3)
				{
					if (gameObject2.GetComponent<NetworkView>().owner == base.transform.GetComponent<NetworkView>().owner)
					{
						myTable = gameObject2;
						break;
					}
				}
			}
			if (myTable != null)
			{
				_skin = myTable.GetComponent<NetworkStartTable>().mySkin;
			}
		}
		if (!isMulti)
		{
			CurrentCampaignGame.ResetConditionParameters();
			CurrentCampaignGame._levelStartedAtTime = Time.time;
			ZombieCreator.BossKilled += CheckTimeCondition;
		}
		if (isMulti && isCompany && (bool)photonView && photonView.isMine)
		{
			countKillsCommandBlue = GlobalGameController.countKillsBlue;
			countKillsCommandRed = GlobalGameController.countKillsRed;
		}
		if (!isMulti)
		{
			productIdentifiers = StoreKitEventListener.idsForSingle;
		}
		else
		{
			productIdentifiers = StoreKitEventListener.idsForMulti;
			if (isCOOP && GameObject.FindGameObjectWithTag("ZombiCreator") != null)
			{
				zombiManager = GameObject.FindGameObjectWithTag("ZombiCreator").GetComponent<ZombiManager>();
			}
		}
		maxCountKills = int.Parse(PlayerPrefs.GetString("MaxKill", "10"));
		if (isMulti && isInet && photonView.isMine)
		{
			maxCountKills = int.Parse(PhotonNetwork.room.customProperties["MaxKill"].ToString());
			PlayerPrefs.SetInt("MaxKill", int.Parse(PhotonNetwork.room.customProperties["MaxKill"].ToString()));
		}
		if (!isMulti || isMine)
		{
			_actionsForPurchasedItems.Add("bigammopack", new KeyValuePair<Action, GUIStyle>(ProvideAmmo, puliInApp));
			_actionsForPurchasedItems.Add("Fullhealth", new KeyValuePair<Action, GUIStyle>(ProvideHealth, healthInApp));
			_actionsForPurchasedItems.Add("MinerWeapon", new KeyValuePair<Action, GUIStyle>(provideminerweapon, pulemetInApp));
			_actionsForPurchasedItems.Add(StoreKitEventListener.elixirID, new KeyValuePair<Action, GUIStyle>(delegate
			{
				Defs.NumberOfElixirs++;
			}, elixirInapp));
			_actionsForPurchasedItems.Add("crystalsword", new KeyValuePair<Action, GUIStyle>(providesword, crystalSwordInapp));
			_actionsForPurchasedItems.Add(StoreKitEventListener.combatrifle, new KeyValuePair<Action, GUIStyle>(providecombatrifle, combatRifleStyle));
			_actionsForPurchasedItems.Add(StoreKitEventListener.goldeneagle, new KeyValuePair<Action, GUIStyle>(providegoldeneagle, goldenEagleInappStyle));
			_actionsForPurchasedItems.Add(StoreKitEventListener.magicbow, new KeyValuePair<Action, GUIStyle>(providemagicbow, magicBowInappStyle));
			_actionsForPurchasedItems.Add(StoreKitEventListener.woodenBow, new KeyValuePair<Action, GUIStyle>(provideWoodenBow, magicBowInappStyle));
			_actionsForPurchasedItems.Add(StoreKitEventListener.spas, new KeyValuePair<Action, GUIStyle>(providespas, spasStyle));
			_actionsForPurchasedItems.Add(StoreKitEventListener.axe, new KeyValuePair<Action, GUIStyle>(provideaxe, axeStyle));
			_actionsForPurchasedItems.Add(StoreKitEventListener.steelAxe, new KeyValuePair<Action, GUIStyle>(provideSteelAxe, axeStyle));
			_actionsForPurchasedItems.Add(StoreKitEventListener.crystalAxe, new KeyValuePair<Action, GUIStyle>(provideCrystalAxe, axeStyle));
			_actionsForPurchasedItems.Add(StoreKitEventListener.armor, new KeyValuePair<Action, GUIStyle>(delegate
			{
				curArmor = 3f;
				_armorType = 0;
			}, armorStyle));
			_actionsForPurchasedItems.Add(StoreKitEventListener.armor2, new KeyValuePair<Action, GUIStyle>(delegate
			{
				curArmor = 6f;
				_armorType = 1;
			}, armorStyle));
			_actionsForPurchasedItems.Add(StoreKitEventListener.armor3, new KeyValuePair<Action, GUIStyle>(delegate
			{
				curArmor = MaxArmor;
				_armorType = 2;
			}, armorStyle));
			_actionsForPurchasedItems.Add(StoreKitEventListener.famas, new KeyValuePair<Action, GUIStyle>(provideFAMAS, famasStyle));
			_actionsForPurchasedItems.Add(StoreKitEventListener.glock, new KeyValuePair<Action, GUIStyle>(provideGlock, glockStyle));
			_actionsForPurchasedItems.Add(StoreKitEventListener.chainsaw, new KeyValuePair<Action, GUIStyle>(provideChainsaw, chainsawStyle));
			_actionsForPurchasedItems.Add(StoreKitEventListener.chainsaw2, new KeyValuePair<Action, GUIStyle>(provideChainsaw2, chainsawStyle));
			_actionsForPurchasedItems.Add(StoreKitEventListener.scythe, new KeyValuePair<Action, GUIStyle>(provideScythe, scytheStyle));
			_actionsForPurchasedItems.Add(StoreKitEventListener.shovel, new KeyValuePair<Action, GUIStyle>(provideShovel, shovelStyle));
			_actionsForPurchasedItems.Add(StoreKitEventListener.sword_2, new KeyValuePair<Action, GUIStyle>(provideSword_2, sword_2_Style));
			_actionsForPurchasedItems.Add(StoreKitEventListener.hammer, new KeyValuePair<Action, GUIStyle>(provideHammer, hammerStyle));
			_actionsForPurchasedItems.Add(StoreKitEventListener.hammer2, new KeyValuePair<Action, GUIStyle>(provideHammer2, hammerStyle));
			_actionsForPurchasedItems.Add(StoreKitEventListener.staff, new KeyValuePair<Action, GUIStyle>(provideStaff, staffStyle));
			_actionsForPurchasedItems.Add(StoreKitEventListener.staff2, new KeyValuePair<Action, GUIStyle>(provideStaff2, staffStyle));
			_actionsForPurchasedItems.Add(StoreKitEventListener.laser, new KeyValuePair<Action, GUIStyle>(provideLaser, laserStyle));
			_actionsForPurchasedItems.Add(StoreKitEventListener.lightSword, new KeyValuePair<Action, GUIStyle>(provideLightSword, lightSwordStyle));
			_actionsForPurchasedItems.Add(StoreKitEventListener.beretta, new KeyValuePair<Action, GUIStyle>(provideBeretta, berettaStyle));
			_actionsForPurchasedItems.Add(StoreKitEventListener.mace, new KeyValuePair<Action, GUIStyle>(provideMace, maceStyle));
			_actionsForPurchasedItems.Add(StoreKitEventListener.mace2, new KeyValuePair<Action, GUIStyle>(provideMace2, maceStyle));
			_actionsForPurchasedItems.Add(StoreKitEventListener.crossbow, new KeyValuePair<Action, GUIStyle>(provideCrossbow, crossbowStyle));
			_actionsForPurchasedItems.Add(StoreKitEventListener.minigun, new KeyValuePair<Action, GUIStyle>(provideMinigun, minigunStyle));
			_actionsForPurchasedItems.Add(StoreKitEventListener.crystalPick, new KeyValuePair<Action, GUIStyle>(providecrystalpickweapon, crossbowStyle));
			_actionsForPurchasedItems.Add(StoreKitEventListener.goldenPick, new KeyValuePair<Action, GUIStyle>(providegoldenpickweapon, minigunStyle));
			_actionsForPurchasedItems.Add(StoreKitEventListener.ironSword, new KeyValuePair<Action, GUIStyle>(provideIronSword, maceStyle));
			_actionsForPurchasedItems.Add(StoreKitEventListener.goldenSword, new KeyValuePair<Action, GUIStyle>(provideGoldenSword, crossbowStyle));
			_actionsForPurchasedItems.Add(StoreKitEventListener.goldenRedStone, new KeyValuePair<Action, GUIStyle>(provideGoldenRed_Stone, minigunStyle));
			_actionsForPurchasedItems.Add(StoreKitEventListener.goldenGlock, new KeyValuePair<Action, GUIStyle>(provideGoldenGlock, crossbowStyle));
			_actionsForPurchasedItems.Add(StoreKitEventListener.goldenSPAS, new KeyValuePair<Action, GUIStyle>(provideGoldenSPAS, minigunStyle));
			_actionsForPurchasedItems.Add(StoreKitEventListener.redMinigun, new KeyValuePair<Action, GUIStyle>(provideRedMinigun, crossbowStyle));
			_actionsForPurchasedItems.Add(StoreKitEventListener.crystalCrossbow, new KeyValuePair<Action, GUIStyle>(provideCrystalCrossbow, minigunStyle));
			_actionsForPurchasedItems.Add(StoreKitEventListener.steelCrossbow, new KeyValuePair<Action, GUIStyle>(provideSteelCrossbow, minigunStyle));
			_actionsForPurchasedItems.Add(StoreKitEventListener.redLightSaber, new KeyValuePair<Action, GUIStyle>(provideRedLightSaber, crossbowStyle));
			_actionsForPurchasedItems.Add(StoreKitEventListener.sandFamas, new KeyValuePair<Action, GUIStyle>(provideSandFamas, minigunStyle));
			_actionsForPurchasedItems.Add(StoreKitEventListener.whiteBeretta, new KeyValuePair<Action, GUIStyle>(provideWhiteBeretta, crossbowStyle));
			_actionsForPurchasedItems.Add(StoreKitEventListener.blackEagle, new KeyValuePair<Action, GUIStyle>(provideBlackEagle, minigunStyle));
			_actionsForPurchasedItems.Add(StoreKitEventListener.crystalGlock, new KeyValuePair<Action, GUIStyle>(provideCrystalGlock, crossbowStyle));
			_actionsForPurchasedItems.Add(StoreKitEventListener.crystalSPAS, new KeyValuePair<Action, GUIStyle>(provideCrystalSpas, minigunStyle));
			_actionsForPurchasedItems.Add(StoreKitEventListener.sword_22, new KeyValuePair<Action, GUIStyle>(provideSword_22, sword_2_Style));
			_actionsForPurchasedItems.Add(StoreKitEventListener.tree, new KeyValuePair<Action, GUIStyle>(providetree, crossbowStyle));
			_actionsForPurchasedItems.Add(StoreKitEventListener.fireAxe, new KeyValuePair<Action, GUIStyle>(provideFireAxe, minigunStyle));
			_actionsForPurchasedItems.Add(StoreKitEventListener._3plShotgun, new KeyValuePair<Action, GUIStyle>(provide3plShotgun, crossbowStyle));
			_actionsForPurchasedItems.Add(StoreKitEventListener.revolver2, new KeyValuePair<Action, GUIStyle>(provideRevolver2, minigunStyle));
			_actionsForPurchasedItems.Add(StoreKitEventListener.barrett, new KeyValuePair<Action, GUIStyle>(providebarrett, crossbowStyle));
			_actionsForPurchasedItems.Add(StoreKitEventListener.svd, new KeyValuePair<Action, GUIStyle>(provideSvd, minigunStyle));
			_purchaseActivityIndicator = StoreKitEventListener.purchaseActivityInd;
			if (_purchaseActivityIndicator == null)
			{
				Debug.LogWarning("Start(): _purchaseActivityIndicator is null.");
			}
			else
			{
				_purchaseActivityIndicator.SetActive(false);
			}
		}
		_inAppGameObject = GameObject.FindGameObjectWithTag("InAppGameObject");
		_listener = _inAppGameObject.GetComponent<StoreKitEventListener>();
		if (!isMulti)
		{
			foreach (Transform item in base.transform.parent)
			{
				if (item.gameObject.name.Equals("FPS_Player"))
				{
					item.gameObject.SetActive(false);
					break;
				}
			}
		}
		zoneCreatePlayer = GameObject.FindGameObjectsWithTag((!isCOOP) ? "MultyPlayerCreateZone" : "MultyPlayerCreateZoneCOOP");
		HOTween.Init(true, true, true);
		HOTween.EnableOverwriteManager();
		if (isMulti)
		{
			if (isMine)
			{
				showGUI = true;
			}
			else
			{
				showGUI = false;
			}
		}
		zaglushkaTexture = Resources.Load("zaglushka") as Texture;
		if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
		{
			AmazonIAPManager.purchaseSuccessfulEvent += HandlePurchaseSuccessful;
		}
		else
		{
			GoogleIABManager.purchaseSucceededEvent += purchaseSuccessful;
			GoogleIABManager.consumePurchaseSucceededEvent += consumptionSucceeded;
		}
		if (!isMulti || isMine)
		{
			_player = base.transform.parent.gameObject;
		}
		else
		{
			_player = null;
		}
		_weaponManager = GameObject.FindGameObjectWithTag("WeaponManager").GetComponent<WeaponManager>();
		if (((PlayerPrefs.GetString("TypeConnect").Equals("local") && base.GetComponent<NetworkView>().isMine) || (PlayerPrefs.GetString("TypeConnect").Equals("inet") && photonView.isMine && PlayerPrefs.GetInt("StartAfterDisconnect") == 0)) && PlayerPrefs.GetInt("MultyPlayer") == 1)
		{
			foreach (Weapon playerWeapon in _weaponManager.playerWeapons)
			{
				playerWeapon.currentAmmoInClip = playerWeapon.weaponPrefab.GetComponent<WeaponSounds>().ammoInClip;
				playerWeapon.currentAmmoInBackpack = playerWeapon.weaponPrefab.GetComponent<WeaponSounds>().InitialAmmo;
				/*if (Application.platform == RuntimePlatform.WindowsEditor)
				{
					playerWeapon.currentAmmoInClip = playerWeapon.weaponPrefab.GetComponent<WeaponSounds>().ammoInClip;
					playerWeapon.currentAmmoInBackpack = 1000000;
				}*/
			}
		}
		if (isMulti && !isMine)
		{
			base.gameObject.transform.parent.transform.Find("LeftTouchPad").gameObject.SetActive(false);
			base.gameObject.transform.parent.transform.Find("RightTouchPad").gameObject.SetActive(false);
		}
		if (!isMulti || isMine)
		{
			GameObject original = Resources.Load("Damage") as GameObject;
			damage = (GameObject)UnityEngine.Object.Instantiate(original);
			Color color = damage.GetComponent<GUITexture>().color;
			color.a = 0f;
			damage.GetComponent<GUITexture>().color = color;
		}
		if (!isMulti)
		{
			_gameController = GameObject.FindGameObjectWithTag("GameController");
			_zombieCreator = _gameController.GetComponent<ZombieCreator>();
		}
		_pauser = GameObject.FindGameObjectWithTag("GameController").GetComponent<Pauser>();
		if (_pauser == null)
		{
			Debug.LogWarning("Start(): _pauser is null.");
		}
		_leftJoystick = GameObject.Find("LeftTouchPad");
		_rightJoystick = GameObject.Find("RightTouchPad");
		if (_singleOrMultiMine())
		{
			if (!isMulti)
			{
				ChangeWeapon(_weaponManager.CurrentWeaponIndex, false);
			}
			else
			{
				ChangeWeapon(_weaponManager.playerWeapons.Count - 1, false);
			}
			_weaponManager.myGun = base.gameObject;
			if (_weaponManager.currentWeaponSounds != null)
			{
//				_weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>()["Reload"].layer = 1;
//				_weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>().Stop();
			}
		}
		if (isMulti && isMine)
		{
			string text = _weaponManager.gameObject.GetComponent<FilterBadWorld>().FilterString(PlayerPrefs.GetString("NamePlayer", Defs.defaultPlayerName));
			if (isInet)
			{
				photonView.RPC("SetNickName", PhotonTargets.AllBuffered, text);
			}
			else
			{
				base.GetComponent<NetworkView>().RPC("SetNickName", RPCMode.AllBuffered, text);
			}
		}
		_SetGunFlashActive(false);
		if (!isMulti)
		{
			CurHealth = PlayerPrefs.GetFloat(Defs.CurrentHealthSett, MaxPlayerHealth);
			curArmor = PlayerPrefs.GetFloat(Defs.CurrentArmorSett, MaxArmor);
			_armorType = PlayerPrefs.GetInt(Defs.ArmorType, 0);
		}
		else
		{
			CurHealth = MaxPlayerHealth;
			curArmor = 0f;
		}
		Invoke("SendSpeedModifier", 0.5f);
		GameObject gameObject3 = (GameObject)UnityEngine.Object.Instantiate(renderAllObjectPrefab, Vector3.zero, Quaternion.identity);
		if (_singleOrMultiMine())
		{
			GameObject original2 = Resources.Load("InGameGUI") as GameObject;
			inGameGUI = (UnityEngine.Object.Instantiate(original2, new Vector3(-10000f, -10000f, -1000f), Quaternion.identity) as GameObject).GetComponent<InGameGUI>();
			inGameGUI.health = () => CurHealth;
			inGameGUI.armor = () => curArmor;
			inGameGUI.armorType = () => _armorType;
			inGameGUI.killsToMaxKills = () => "Kills \n" + countKills + "/" + maxCountKills;
			inGameGUI.timeLeft = delegate
			{
				if (zombiManager == null && GameObject.FindGameObjectWithTag("ZombiCreator") != null)
				{
					zombiManager = GameObject.FindGameObjectWithTag("ZombiCreator").GetComponent<ZombiManager>();
				}
				if (zombiManager != null)
				{
					float num = zombiManager.maxTimeGame - zombiManager.timeGame;
					if (num < 0f)
					{
						num = 0f;
					}
					return "Time\n" + Mathf.FloorToInt(num / 60f) + ":" + ((Mathf.FloorToInt(num - (float)(Mathf.FloorToInt(num / 60f) * 60)) >= 10) ? string.Empty : "0") + Mathf.FloorToInt(num - (float)(Mathf.FloorToInt(num / 60f) * 60));
				}
				float num2 = 240f;
				if (num2 < 0f)
				{
					num2 = 0f;
				}
				return "Time\n" + Mathf.FloorToInt(num2 / 60f) + ":" + ((Mathf.FloorToInt(num2 - (float)(Mathf.FloorToInt(num2 / 60f) * 60)) >= 10) ? string.Empty : "0") + Mathf.FloorToInt(num2 - (float)(Mathf.FloorToInt(num2 / 60f) * 60));
			};
			AddButtonHandlers();
			Shop.sharedShop.SetHatsAndCapesEnabled(false);
			Shop.sharedShop.SetGearCatEnabled(PlayerPrefs.GetInt(Defs.TrainingCompleted_4_4_Sett, 0) != 0);
			Shop.sharedShop.gameObject.SetActive(true);
			Shop.sharedShop.buyAction = PurchaseSuccessful;
			Shop.sharedShop.resumeAction = delegate
			{
				SetInApp();
				if (inAppOpenedFromPause)
				{
					inAppOpenedFromPause = false;
				}
				else
				{
					SetPause();
				}
			};
		}
		if (PlayerPrefs.GetInt("StartAfterDisconnect") == 1 && PlayerPrefs.GetInt("MultyPlayer") == 1 && PlayerPrefs.GetString("TypeConnect").Equals("inet") && photonView.isMine)
		{
			countKills = GlobalGameController.Score;
			if (countKills < 0)
			{
				countKills = 0;
			}
			CurHealth = GlobalGameController.healthMyPlayer;
			base.transform.parent.transform.position = GlobalGameController.posMyPlayer;
			base.transform.parent.transform.rotation = GlobalGameController.rotMyPlayer;
			PlayerPrefs.SetInt("StartAfterDisconnect", 0);
		}
		if (isMine) WindowsMouseManager.MouseLocked = true;
	}

	private IEnumerator AddCampaignWeapon()
	{
		yield return new WaitForSeconds(0.1f);
		string[] _arr = Storager.getString(Defs.WeaponsGotInCampaign, false).Split("#"[0]);
		List<string> wInC = new List<string>();
		string[] array = _arr;
		foreach (string s in array)
		{
			wInC.Add(s);
		}
		if (wInC.Contains(WeaponManager.campaignBonusWeapons[Application.loadedLevelName]))
		{
			yield break;
		}
		wInC.Add(WeaponManager.campaignBonusWeapons[Application.loadedLevelName]);
		Storager.setString(Defs.WeaponsGotInCampaign, string.Join("#"[0].ToString(), wInC.ToArray()), false);
		GameObject wp = null;
		UnityEngine.Object[] weaponsInGame = _weaponManager.weaponsInGame;
		for (int j = 0; j < weaponsInGame.Length; j++)
		{
			GameObject w = (GameObject)weaponsInGame[j];
			if (w.name.Equals(WeaponManager.campaignBonusWeapons[Application.loadedLevelName]))
			{
				wp = w;
				break;
			}
		}
		AddWeapon(wp);
		_showArt = true;
		_campaignWeaponTexture = Resources.Load(ResPath.Combine("CampaignWeaponArts", Application.loadedLevelName)) as Texture;
		yield return new WaitForSeconds(3f);
		_showArt = false;
		_campaignWeaponTexture = null;
	}

	[RPC]
	public void SetNickName(string _nickName)
	{
		photonView = PhotonView.Get(this);
		base.transform.parent.gameObject.GetComponent<SkinName>().NickName = _nickName;
		if (!isMine && _label == null)
		{
			GameObject original = Resources.Load("ObjectLabel") as GameObject;
			_label = UnityEngine.Object.Instantiate(original) as GameObject;
			_label.GetComponent<ObjectLabel>().target = base.transform;
			_label.GetComponent<GUIText>().text = _nickName;
		}
	}

	public bool _singleOrMultiMine()
	{
		return !isMulti || isMine;
	}

	private void OnDestroy()
	{
		Debug.Log("OnDestroy player");
		if (_singleOrMultiMine())
		{
			if ((bool)inGameGUI && (bool)inGameGUI.gameObject)
			{
				UnityEngine.Object.Destroy(inGameGUI.gameObject);
			}
			GameObject gameObject = GameObject.FindGameObjectWithTag("ChatViewer");
			if (gameObject != null)
			{
				gameObject.GetComponent<ChatViewrController>().closeChat();
			}
		}
		if (coinsShop.thisScript.enabled)
		{
			coinsShop.ExitFromShop(false);
		}
		coinsPlashka.hidePlashka();
		if (isMulti && !isMine && _label != null)
		{
			UnityEngine.Object.Destroy(_label);
		}
		if ((!isMulti || isMine) && Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
		{
			GoogleIABManager.purchaseSucceededEvent -= purchaseSuccessful;
			GoogleIABManager.consumePurchaseSucceededEvent -= consumptionSucceeded;
		}
		if (_singleOrMultiMine() || (_weaponManager != null && _weaponManager.myPlayer == base.transform.parent.gameObject))
		{
			if (_pauser != null && (bool)_pauser && _pauser.paused)
			{
				Debug.Log("pauser YES");
				_pauser.paused = !_pauser.paused;
				Time.timeScale = 1f;
				AddButtonHandlers();
			}
			GameObject gameObject2 = GameObject.FindGameObjectWithTag("DamageFrame");
			if (gameObject2 != null)
			{
				UnityEngine.Object.Destroy(gameObject2);
			}
			RemoveButtonHandelrs();
			Shop.sharedShop.buyAction = null;
			Shop.sharedShop.resumeAction = null;
			ZombieCreator.BossKilled -= CheckTimeCondition;
		}
	}

	private void _SetGunFlashActive(bool state)
	{
		if (GunFlash != null && !_weaponManager.currentWeaponSounds.isMelee && !isZooming)
		{

		}
	}

	[RPC]
	private void ReloadGun()
	{
		base.transform.GetChild(0).GetComponent<WeaponSounds>().animationObject.GetComponent<Animation>().Play("Reload");
		if (PlayerPrefsX.GetBool(PlayerPrefsX.SoundFXSetting, true))
		{
			base.GetComponent<AudioSource>().PlayOneShot(base.transform.GetChild(0).GetComponent<WeaponSounds>().reload);
		}
	}

	public void ReloadPressed()
	{
		if (_weaponManager.currentWeaponSounds.isMelee)
		{
			return;
		}
		if (isZooming)
		{
			ZoomPress();
		}
		if (_weaponManager.CurrentWeaponIndex < 0 || _weaponManager.CurrentWeaponIndex >= _weaponManager.playerWeapons.Count || ((Weapon)_weaponManager.playerWeapons[_weaponManager.CurrentWeaponIndex]).currentAmmoInBackpack <= 0 || ((Weapon)_weaponManager.playerWeapons[_weaponManager.CurrentWeaponIndex]).currentAmmoInClip == _weaponManager.currentWeaponSounds.ammoInClip)
		{
			return;
		}
		_weaponManager.Reload();
		if (isMulti)
		{
			if (!isInet)
			{
				base.GetComponent<NetworkView>().RPC("ReloadGun", RPCMode.Others);
			}
			else
			{
				photonView.RPC("ReloadGun", PhotonTargets.Others);
			}
		}
		if (PlayerPrefsX.GetBool(PlayerPrefsX.SoundFXSetting, true))
		{
			base.GetComponent<AudioSource>().PlayOneShot(_weaponManager.currentWeaponSounds.reload);
		}
		if (_rightJoystick != null && _rightJoystick.GetComponent<JoystickSharp>() != null)
		{
			_rightJoystick.GetComponent<JoystickSharp>().HasAmmo();
		}
		else
		{
			Debug.Log("_rightJoystick=" + _rightJoystick);
		}
	}

	public void ShotPressed()
	{
		if (Defs.IsTraining && TrainingController.stepTraining == TrainingController.stepTrainingList["TapToShoot"])
		{
			TrainingController.isNextStep = TrainingController.stepTrainingList["TapToShoot"];
		}
		if ((isMulti && isInet && (bool)photonView && !photonView.isMine) || _weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>().IsPlaying("Shoot") || _weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>().IsPlaying("Reload") || _weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>().IsPlaying("Empty"))
		{
			return;
		}
		_weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>().Stop();
		if (_weaponManager.currentWeaponSounds.isMelee)
		{
			_Shot();
		}
		else if (((Weapon)_weaponManager.playerWeapons[_weaponManager.CurrentWeaponIndex]).currentAmmoInClip > 0)
		{
			((Weapon)_weaponManager.playerWeapons[_weaponManager.CurrentWeaponIndex]).currentAmmoInClip--;
			if (((Weapon)_weaponManager.playerWeapons[_weaponManager.CurrentWeaponIndex]).currentAmmoInClip == 0)
			{
				if (((Weapon)_weaponManager.playerWeapons[_weaponManager.CurrentWeaponIndex]).currentAmmoInBackpack > 0)
				{
					Invoke("ReloadPressed", 0.2f);
				}
				else
				{
					JoystickSharp component = _rightJoystick.GetComponent<JoystickSharp>();
					component.NoAmmo();
				}
			}
			_Shot();
			_SetGunFlashActive(true);
			GunFlashLifetime = _weaponManager.currentWeaponSounds.gameObject.GetComponent<FlashFire>().timeFireAction;
		}
		else
		{
			_weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>().Play("Empty");
			if (PlayerPrefsX.GetBool(PlayerPrefsX.SoundFXSetting, true))
			{
				base.GetComponent<AudioSource>().PlayOneShot(_weaponManager.currentWeaponSounds.empty);
			}
		}
	}

	private void _Shot()
	{
		_weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>().Play("Shoot");
		shootS();
		if (PlayerPrefsX.GetBool(PlayerPrefsX.SoundFXSetting, true))
		{
			base.GetComponent<AudioSource>().PlayOneShot(_weaponManager.currentWeaponSounds.shoot);
		}
	}

	public void sendImDeath(string _name)
	{
		if (!isInet)
		{
			base.GetComponent<NetworkView>().RPC("imDeath", RPCMode.All, _name);
		}
		else
		{
			photonView.RPC("imDeath", PhotonTargets.All, _name);
		}
	}

	public void setInString(string nick)
	{
		if (!(_weaponManager == null) && !(_weaponManager.myPlayer == null))
		{
			_weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().killedSpisok[2] = _weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().killedSpisok[1];
			_weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().killedSpisok[1] = _weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().killedSpisok[0];
			_weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().killedSpisok[0] = nick + " joined the game";
			_weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().timerShow[2] = _weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().timerShow[1];
			_weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().timerShow[1] = _weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().timerShow[0];
			_weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().timerShow[0] = 3f;
		}
	}

	public void setOutString(string nick)
	{
		if (!(_weaponManager == null) && !(_weaponManager.myPlayer == null))
		{
			_weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().killedSpisok[2] = _weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().killedSpisok[1];
			_weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().killedSpisok[1] = _weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().killedSpisok[0];
			_weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().killedSpisok[0] = nick + " left the game";
			_weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().timerShow[2] = _weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().timerShow[1];
			_weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().timerShow[1] = _weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().timerShow[0];
			_weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().timerShow[0] = 3f;
		}
	}

	[RPC]
	public void imDeath(string _name)
	{
		if (!(_weaponManager == null) && !(_weaponManager.myPlayer == null))
		{
			_weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().killedSpisok[2] = _weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().killedSpisok[1];
			_weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().killedSpisok[1] = _weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().killedSpisok[0];
			_weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().killedSpisok[0] = _name + " killed himself";
			_weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().timerShow[2] = _weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().timerShow[1];
			_weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().timerShow[1] = _weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().timerShow[0];
			_weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().timerShow[0] = 3f;
		}
	}

	[RPC]
	public void Killed(NetworkViewID idKiller, NetworkViewID id)
	{
		if (_weaponManager == null || _weaponManager.myPlayer == null)
		{
			return;
		}
		string text = string.Empty;
		string empty = string.Empty;
		empty = base.transform.parent.GetComponent<SkinName>().NickName;
		GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			if (!gameObject.GetComponent<NetworkView>().viewID.Equals(idKiller))
			{
				continue;
			}
			text = gameObject.GetComponent<SkinName>().NickName;
			if ((bool)_weaponManager && gameObject == _weaponManager.myPlayer)
			{
				countKills++;
				_weaponManager.myTable.GetComponent<NetworkStartTable>().CountKills = countKills;
				_weaponManager.myTable.GetComponent<NetworkStartTable>().synchState();
				if (countKills >= maxCountKills)
				{
					base.GetComponent<NetworkView>().RPC("pobeda", RPCMode.All, idKiller);
					PlayerPrefs.SetInt("Rating", PlayerPrefs.GetInt("Rating", 0) + 1);
				}
			}
			break;
		}
		if ((bool)_weaponManager && _weaponManager.myPlayer != null)
		{
			_weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().killedSpisok[2] = _weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().killedSpisok[1];
			_weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().killedSpisok[1] = _weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().killedSpisok[0];
			_weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().killedSpisok[0] = text + " kill " + empty;
			_weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().timerShow[2] = _weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().timerShow[1];
			_weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().timerShow[1] = _weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().timerShow[0];
			_weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().timerShow[0] = 4f;
		}
	}

	public void OnPhotonPlayerConnected(PhotonPlayer player)
	{
		if ((bool)photonView && photonView.isMine)
		{
			photonView.RPC("CountKillsCommandSynch", PhotonTargets.Others, countKillsCommandBlue, countKillsCommandRed);
		}
	}

	[RPC]
	private void CountKillsCommandSynch(int _blue, int _red)
	{
		GlobalGameController.countKillsBlue = _blue;
		GlobalGameController.countKillsRed = _red;
	}

	[RPC]
	private void plusCountKillsCommand(int _command)
	{
		if (_command == 1)
		{
			if ((bool)_weaponManager && (bool)_weaponManager.myPlayer)
			{
				_weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().countKillsCommandBlue++;
			}
			else
			{
				GlobalGameController.countKillsBlue++;
			}
		}
		if (_command == 2)
		{
			if ((bool)_weaponManager && (bool)_weaponManager.myPlayer)
			{
				_weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().countKillsCommandRed++;
			}
			else
			{
				GlobalGameController.countKillsRed++;
			}
		}
	}

	public void addMultyKill()
	{
		multiKill++;
		if (multiKill > 1)
		{
			timerShowMultyKill = 3f;
		}
	}

	public void resetMultyKill()
	{
		multiKill = 0;
		timerShowMultyKill = -1f;
	}

	public void ImKill(int idKiller)
	{
		countKills++;
		addMultyKill();
		if (isCompany)
		{
			if (myCommand == 1)
			{
				countKillsCommandBlue++;
				photonView.RPC("plusCountKillsCommand", PhotonTargets.Others, 1);
			}
			if (myCommand == 2)
			{
				countKillsCommandRed++;
				photonView.RPC("plusCountKillsCommand", PhotonTargets.Others, 2);
			}
		}
		_weaponManager.myTable.GetComponent<NetworkStartTable>().CountKills = countKills;
		_weaponManager.myTable.GetComponent<NetworkStartTable>().synchState();
		Debug.Log(countKillsCommandBlue + " " + countKillsCommandRed + " " + maxCountKills);
		if ((!isCompany && countKills >= maxCountKills) || (myCommand == 1 && isCompany && countKillsCommandBlue >= maxCountKills) || (myCommand == 2 && isCompany && countKillsCommandRed >= maxCountKills))
		{
			photonView.RPC("pobedaPhoton", PhotonTargets.All, idKiller, myCommand);
			PlayerPrefs.SetInt("Rating", PlayerPrefs.GetInt("Rating", 0) + 1);
			_weaponManager.myTable.GetComponent<NetworkStartTable>().isIwin = true;
		}
	}

	[RPC]
	public void KilledPhoton(int idKiller)
	{
		if (_weaponManager == null || _weaponManager.myPlayer == null)
		{
			return;
		}
		string text = string.Empty;
		string empty = string.Empty;
		empty = base.transform.parent.GetComponent<SkinName>().NickName;
		GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			if (gameObject.GetComponent<PhotonView>().viewID == idKiller)
			{
				text = gameObject.GetComponent<SkinName>().NickName;
				if ((bool)_weaponManager && gameObject == _weaponManager.myPlayer)
				{
					gameObject.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().ImKill(idKiller);
				}
				break;
			}
		}
		if ((bool)_weaponManager && _weaponManager.myPlayer != null)
		{
			_weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().killedSpisok[2] = _weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().killedSpisok[1];
			_weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().killedSpisok[1] = _weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().killedSpisok[0];
			_weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().killedSpisok[0] = text + " kill " + empty;
			_weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().timerShow[2] = _weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().timerShow[1];
			_weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().timerShow[1] = _weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().timerShow[0];
			_weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().timerShow[0] = 4f;
		}
	}

	[RPC]
	public void pobeda(NetworkViewID idKiller)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			if (idKiller.Equals(gameObject.GetComponent<NetworkView>().viewID))
			{
				nickPobeditel = gameObject.GetComponent<SkinName>().NickName;
			}
		}
		GameObject.FindGameObjectWithTag("NetworkTable").GetComponent<NetworkStartTable>().win(nickPobeditel);
	}

	[RPC]
	public void pobedaPhoton(int idKiller, int _command)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			if (idKiller == gameObject.GetComponent<PhotonView>().viewID)
			{
				nickPobeditel = gameObject.GetComponent<SkinName>().NickName;
			}
		}
		GameObject.FindGameObjectWithTag("NetworkTable").GetComponent<NetworkStartTable>().win(nickPobeditel, _command);
	}

	[RPC]
	public void MinusLiveRPC(NetworkViewID idKiller, float minus, bool _isHeadShot)
	{
		if (_isHeadShot && !isMine)
		{
			UnityEngine.Object.Instantiate(headShotParticle, base.transform.parent.transform.position, base.transform.parent.transform.rotation);
		}
		if (PlayerPrefsX.GetBool(PlayerPrefsX.SoundFXSetting, true))
		{
			base.GetComponent<AudioSource>().PlayOneShot((!_isHeadShot) ? damagePlayerSound : headShotSound);
		}
		if (isMine && !isKilled)
		{
			float num = minus - curArmor;
			if (num < 0f)
			{
				curArmor -= minus;
				num = 0f;
			}
			else
			{
				curArmor = 0f;
			}
			if (CurHealth > 0f)
			{
				CurHealth -= num;
				if (CurHealth <= 0f)
				{
					base.GetComponent<NetworkView>().RPC("Killed", RPCMode.All, idKiller);
				}
			}
		}
		StartCoroutine(Flash(base.transform.parent.gameObject));
	}

	public void MinusLive(int idKiller, float minus, bool _isHeadShot)
	{
		photonView.RPC("MinusLiveRPCPhoton", PhotonTargets.All, idKiller, minus, _isHeadShot);
	}

	public void MinusLive(NetworkViewID idKiller, float minus, bool _isHeadShot)
	{
		base.GetComponent<NetworkView>().RPC("MinusLiveRPC", RPCMode.All, idKiller, minus, _isHeadShot);
	}

	[RPC]
	public void MinusLiveRPCPhoton(int idKiller, float minus, bool _isHeadShot)
	{
		if (_isHeadShot && !isMine)
		{
			UnityEngine.Object.Instantiate(headShotParticle, base.transform.parent.transform.position, base.transform.parent.transform.rotation);
		}
		if (PlayerPrefsX.GetBool(PlayerPrefsX.SoundFXSetting, true))
		{
			base.gameObject.GetComponent<AudioSource>().PlayOneShot((!_isHeadShot) ? damagePlayerSound : headShotSound);
		}
		if (isMine && !isKilled)
		{
			float num = minus - curArmor;
			if (num < 0f)
			{
				curArmor -= minus;
				num = 0f;
			}
			else
			{
				curArmor = 0f;
			}
			if (CurHealth > 0f)
			{
				CurHealth -= num;
				if (CurHealth <= 0f)
				{
					Debug.Log("send KilledPhoton " + idKiller);
					photonView.RPC("KilledPhoton", PhotonTargets.All, idKiller);
				}
			}
		}
		StartCoroutine(Flash(base.transform.parent.gameObject));
	}

	public static void SetTextureRecursivelyFrom(GameObject obj, Texture txt, GameObject[] stopObjs)
	{
		foreach (Transform item in obj.transform)
		{
			bool flag = false;
			foreach (GameObject o in stopObjs)
			{
				if (item.gameObject.Equals(o))
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				continue;
			}
			if ((bool)item.gameObject.GetComponent<Renderer>() && (bool)item.gameObject.GetComponent<Renderer>().material)
			{
				item.gameObject.GetComponent<Renderer>().material.mainTexture = txt;
			}
			flag = false;
			foreach (GameObject o2 in stopObjs)
			{
				if (item.gameObject.Equals(o2))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				SetTextureRecursivelyFrom(item.gameObject, txt, stopObjs);
			}
		}
	}

	private IEnumerator Flash(GameObject _obj)
	{
		_flashing = true;
		GameObject _gunWiapon2 = null;
		GameObject gunFlashTmp2 = null;
		GameObject[] stopObjs2 = null;
		foreach (Transform chaild2 in _obj.transform)
		{
			if (chaild2.gameObject.name.Equals("GameObject"))
			{
				WeaponSounds ws2 = chaild2.transform.GetChild(0).gameObject.GetComponent<WeaponSounds>();
				_gunWiapon2 = ws2.bonusPrefab;
				if (!ws2.isMelee)
				{
					gunFlashTmp2 = chaild2.transform.GetChild(0).Find("BulletSpawnPoint").transform.GetChild(0).gameObject;
				}
				break;
			}
		}
		SetTextureRecursivelyFrom(stopObjs: (!(gunFlashTmp2 != null)) ? new GameObject[3]
		{
			_gunWiapon2,
			_obj.GetComponent<SkinName>().capesPoint,
			_obj.GetComponent<SkinName>().hatsPoint
		} : new GameObject[4]
		{
			_gunWiapon2,
			gunFlashTmp2,
			_obj.GetComponent<SkinName>().capesPoint,
			_obj.GetComponent<SkinName>().hatsPoint
		}, obj: _obj, txt: hitTexture);
		yield return new WaitForSeconds(0.125f);
		if (_obj != null)
		{
			_gunWiapon2 = null;
			gunFlashTmp2 = null;
			stopObjs2 = null;
			foreach (Transform chaild in _obj.transform)
			{
				if (chaild.gameObject.name.Equals("GameObject"))
				{
					WeaponSounds ws = chaild.transform.GetChild(0).gameObject.GetComponent<WeaponSounds>();
					_gunWiapon2 = ws.bonusPrefab;
					if (!ws.isMelee)
					{
						gunFlashTmp2 = chaild.transform.GetChild(0).Find("BulletSpawnPoint").transform.GetChild(0).gameObject;
					}
					break;
				}
			}
			SetTextureRecursivelyFrom(stopObjs: (!(gunFlashTmp2 != null)) ? new GameObject[3]
			{
				_gunWiapon2,
				_obj.GetComponent<SkinName>().capesPoint,
				_obj.GetComponent<SkinName>().hatsPoint
			} : new GameObject[4]
			{
				_gunWiapon2,
				gunFlashTmp2,
				_obj.GetComponent<SkinName>().capesPoint,
				_obj.GetComponent<SkinName>().hatsPoint
			}, obj: _obj, txt: _obj.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>()._skin);
		}
		_flashing = false;
	}

	[RPC]
	private void fireFlash(bool isFlash)
	{
		if (base.transform.childCount != 0)
		{
			if (isFlash)
			{
				base.transform.GetChild(0).GetComponent<FlashFire>().fire();
			}
			base.transform.GetChild(0).GetComponent<WeaponSounds>().animationObject.GetComponent<Animation>().Play("Shoot");
			if (PlayerPrefsX.GetBool(PlayerPrefsX.SoundFXSetting, true))
			{
				base.GetComponent<AudioSource>().PlayOneShot(base.transform.GetChild(0).GetComponent<WeaponSounds>().shoot);
			}
		}
	}

	[RPC]
	public void HoleRPC(bool _isBloodParticle, Vector3 _pos, Quaternion _rot)
	{
		if (_isBloodParticle)
		{
			UnityEngine.Object.Instantiate(bloodParticle, _pos, _rot);
			return;
		}
		UnityEngine.Object.Instantiate(hole, _pos, _rot);
		UnityEngine.Object.Instantiate(wallParticle, _pos, _rot);
	}

	public void shootS()
	{
		if (!_weaponManager.currentWeaponSounds.isMelee)
		{
			Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)Screen.width * 0.5f - _weaponManager.currentWeaponSounds.startZone.x * _weaponManager.currentWeaponSounds.tekKoof * Defs.Coef * 0.5f + (float)UnityEngine.Random.Range(0, Mathf.RoundToInt(_weaponManager.currentWeaponSounds.startZone.x * _weaponManager.currentWeaponSounds.tekKoof * Defs.Coef)), (float)Screen.height * 0.5f - _weaponManager.currentWeaponSounds.startZone.y * _weaponManager.currentWeaponSounds.tekKoof * Defs.Coef * 0.5f + (float)UnityEngine.Random.Range(0, Mathf.RoundToInt(_weaponManager.currentWeaponSounds.startZone.y * _weaponManager.currentWeaponSounds.tekKoof * Defs.Coef)), 0f));
			_weaponManager.currentWeaponSounds.fire();
			RaycastHit hitInfo;
			if (!Physics.Raycast(ray, out hitInfo, 100f, -2053))
			{
				return;
			}
			bool flag;
			if (hitInfo.collider.gameObject.transform.parent != null && !hitInfo.collider.gameObject.transform.parent.CompareTag("Enemy") && !hitInfo.collider.gameObject.transform.parent.CompareTag("Player"))
			{
				UnityEngine.Object.Instantiate(hole, hitInfo.point + hitInfo.normal * 0.001f, Quaternion.FromToRotation(Vector3.up, hitInfo.normal));
				UnityEngine.Object.Instantiate(wallParticle, hitInfo.point + hitInfo.normal * 0.001f, Quaternion.FromToRotation(Vector3.up, hitInfo.normal));
				flag = false;
			}
			else
			{
				UnityEngine.Object.Instantiate(bloodParticle, hitInfo.point + hitInfo.normal * 0.001f, Quaternion.FromToRotation(Vector3.up, hitInfo.normal));
				flag = true;
			}
			if (isMulti)
			{
				if (!isInet)
				{
					base.GetComponent<NetworkView>().RPC("HoleRPC", RPCMode.Others, flag, hitInfo.point + hitInfo.normal * 0.001f, Quaternion.FromToRotation(Vector3.up, hitInfo.normal));
				}
				else
				{
					photonView.RPC("HoleRPC", PhotonTargets.Others, flag, hitInfo.point + hitInfo.normal * 0.001f, Quaternion.FromToRotation(Vector3.up, hitInfo.normal));
				}
			}
			if ((bool)hitInfo.collider.gameObject.transform.parent && hitInfo.collider.gameObject.transform.parent.CompareTag("Enemy"))
			{
				if (!isMulti)
				{
					BotHealth component = hitInfo.collider.gameObject.transform.parent.GetComponent<BotHealth>();
					component.adjustHealth((float)(-_weaponManager.currentWeaponSounds.damage) + UnityEngine.Random.Range(_weaponManager.currentWeaponSounds.damageRange.x, _weaponManager.currentWeaponSounds.damageRange.y), Camera.main.transform);
				}
				else if (isCOOP)
				{
					float health = hitInfo.collider.gameObject.transform.parent.GetComponent<ZombiUpravlenie>().health;
					if (health > 0f)
					{
						health -= (float)_weaponManager.currentWeaponSounds.damage + UnityEngine.Random.Range(_weaponManager.currentWeaponSounds.damageRange.x, _weaponManager.currentWeaponSounds.damageRange.y);
						hitInfo.collider.gameObject.transform.parent.GetComponent<ZombiUpravlenie>().setHealth(health, true);
						GlobalGameController.Score += 5;
						if (health <= 0f)
						{
							GlobalGameController.Score += hitInfo.collider.gameObject.GetComponent<Sounds>().scorePerKill;
						}
						_weaponManager.myTable.GetComponent<NetworkStartTable>().score = GlobalGameController.Score;
						_weaponManager.myTable.GetComponent<NetworkStartTable>().synchState();
					}
				}
			}
			if (isMulti)
			{
				if (isInet)
				{
					photonView.RPC("fireFlash", PhotonTargets.Others, true);
				}
				else
				{
					base.GetComponent<NetworkView>().RPC("fireFlash", RPCMode.Others, true);
				}
			}
			if (!hitInfo.collider.gameObject.transform.parent || !hitInfo.collider.gameObject.transform.parent.CompareTag("Player"))
			{
				return;
			}
			float num = 1f;
			bool flag2 = hitInfo.collider.gameObject.CompareTag("HeadCollider");
			if (flag2)
			{
				num = 2.5f;
			}
			if ((isMulti && !isCOOP && !isCompany) || (isCompany && myCommand != hitInfo.collider.gameObject.transform.parent.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().myCommand))
			{
				if (!isInet)
				{
					hitInfo.collider.gameObject.transform.parent.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().MinusLive(base.transform.parent.gameObject.GetComponent<NetworkView>().viewID, _weaponManager.currentWeaponSounds.multiplayerDamage * num, flag2);
				}
				else
				{
					hitInfo.collider.gameObject.transform.parent.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().MinusLive(base.transform.parent.gameObject.GetComponent<PhotonView>().viewID, _weaponManager.currentWeaponSounds.multiplayerDamage * num, flag2);
				}
			}
			return;
		}
		if (isMulti)
		{
			if (!isInet)
			{
				base.GetComponent<NetworkView>().RPC("fireFlash", RPCMode.Others, false);
			}
			else
			{
				photonView.RPC("fireFlash", PhotonTargets.Others, false);
			}
		}
		if (!_weaponManager.currentWeaponSounds.isMagic)
		{
			Ray ray2 = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f));
			RaycastHit hitInfo2;
			if (Physics.Raycast(ray2, out hitInfo2, 100f, -2053) && ((hitInfo2.collider.gameObject.transform.parent == null && !hitInfo2.collider.gameObject.transform.CompareTag("Player")) || (hitInfo2.collider.gameObject.transform.parent != null && !hitInfo2.collider.gameObject.transform.parent.CompareTag("Enemy") && !hitInfo2.collider.gameObject.transform.parent.CompareTag("Player"))))
			{
				return;
			}
		}
		GameObject[] array = ((!isMulti || isCOOP) ? GameObject.FindGameObjectsWithTag("Enemy") : GameObject.FindGameObjectsWithTag("Player"));
		GameObject gameObject = null;
		float num2 = float.PositiveInfinity;
		GameObject[] array2 = array;
		foreach (GameObject gameObject2 in array2)
		{
			if (!gameObject2.transform.position.Equals(_player.transform.position))
			{
				Vector3 to = gameObject2.transform.position - _player.transform.position;
				float magnitude = to.magnitude;
				if (magnitude < num2 && ((magnitude < _weaponManager.currentWeaponSounds.range && Vector3.Angle(base.gameObject.transform.forward, to) < _weaponManager.currentWeaponSounds.meleeAngle) || magnitude < 1.5f))
				{
					num2 = magnitude;
					gameObject = gameObject2;
				}
			}
		}
		if ((bool)gameObject)
		{
			StartCoroutine(HitByMelee(gameObject));
		}
	}

	private IEnumerator HitByMelee(GameObject enemyToHit)
	{
		yield return new WaitForSeconds(_weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>()["Shoot"].length * _weaponManager.currentWeaponSounds.meleeAttackTimeModifier);
		if (!(enemyToHit != null))
		{
			yield break;
		}
		if (isMulti && !isCOOP)
		{
			if ((!isCompany || (isCompany && myCommand != enemyToHit.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().myCommand)) && isMulti)
			{
				if (!isInet)
				{
					enemyToHit.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().MinusLive(base.transform.parent.gameObject.GetComponent<NetworkView>().viewID, _weaponManager.currentWeaponSounds.multiplayerDamage, false);
				}
				else
				{
					enemyToHit.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().MinusLive(base.transform.parent.gameObject.GetComponent<PhotonView>().viewID, _weaponManager.currentWeaponSounds.multiplayerDamage, false);
				}
			}
		}
		else if (isMulti && isCOOP)
		{
			float liveEnemy2 = enemyToHit.GetComponent<ZombiUpravlenie>().health;
			if (liveEnemy2 > 0f)
			{
				liveEnemy2 -= (float)_weaponManager.currentWeaponSounds.damage + UnityEngine.Random.Range(_weaponManager.currentWeaponSounds.damageRange.x, _weaponManager.currentWeaponSounds.damageRange.y);
				enemyToHit.GetComponent<ZombiUpravlenie>().setHealth(liveEnemy2, true);
				GlobalGameController.Score += 5;
				if (liveEnemy2 <= 0f)
				{
					GlobalGameController.Score += enemyToHit.transform.GetChild(0).gameObject.GetComponent<Sounds>().scorePerKill;
				}
				_weaponManager.myTable.GetComponent<NetworkStartTable>().score = GlobalGameController.Score;
				_weaponManager.myTable.GetComponent<NetworkStartTable>().synchState();
			}
		}
		else if ((bool)enemyToHit && (bool)enemyToHit.GetComponent<BotHealth>() && enemyToHit.GetComponent<BotHealth>().getIsLife())
		{
			enemyToHit.GetComponent<BotHealth>().adjustHealth((float)(-_weaponManager.currentWeaponSounds.damage) + UnityEngine.Random.Range(_weaponManager.currentWeaponSounds.damageRange.x, _weaponManager.currentWeaponSounds.damageRange.y), Camera.main.transform);
		}
	}

	private IEnumerator Fade(float start, float end, float length, GameObject currentObject)
	{
		for (float i = 0f; i < 1f; i += Time.deltaTime / length)
		{
			Color rgba = currentObject.GetComponent<GUITexture>().color;
			rgba.a = Mathf.Lerp(start, end, i);
			currentObject.GetComponent<GUITexture>().color = rgba;
			yield return 0;
			Color rgba_ = currentObject.GetComponent<GUITexture>().color;
			rgba_.a = end;
			currentObject.GetComponent<GUITexture>().color = rgba_;
		}
	}

	[RPC]
	private void imKilled()
	{
		isKilled = true;
	}

	[RPC]
	private void imNotKilled()
	{
		isKilled = false;
	}

	[RPC]
	private void ImKilled()
	{
		if (PlayerPrefsX.GetBool(PlayerPrefsX.SoundFXSetting, true))
		{
			base.gameObject.GetComponent<AudioSource>().PlayOneShot(deadPlayerSound);
		}
	}

	private IEnumerator FlashWhenHit()
	{
		damageShown = true;
		Color rgba = damage.GetComponent<GUITexture>().color;
		rgba.a = 0f;
		damage.GetComponent<GUITexture>().color = rgba;
		float danageTime = 0.15f;
		yield return StartCoroutine(Fade(0f, 1f, danageTime, damage));
		yield return new WaitForSeconds(0.01f);
		yield return StartCoroutine(Fade(1f, 0f, danageTime, damage));
		damageShown = false;
	}

	private IEnumerator FlashWhenDead()
	{
		damageShown = true;
		Color rgba = damage.GetComponent<GUITexture>().color;
		rgba.a = 0f;
		damage.GetComponent<GUITexture>().color = rgba;
		float danageTime = 0.15f;
		yield return StartCoroutine(Fade(0f, 1f, danageTime, damage));
		while (isDeadFrame)
		{
			yield return null;
		}
		yield return StartCoroutine(Fade(1f, 0f, danageTime / 3f, damage));
		damageShown = false;
	}

	private IEnumerator SetCanReceiveSwipes()
	{
		yield return new WaitForSeconds(0.1f);
		canReceiveSwipes = true;
	}

	private void setisDeadFrameFalse()
	{
		isDeadFrame = false;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && !Shop.sharedShop.inShop) SwitchPause();
		float mouseScroll = Input.GetAxisRaw("Mouse ScrollWheel");
		if (mouseScroll >= 0.1f)
		{
            if ((isMulti && isMine) || !isMulti)
            {
                if (Defs.IsTraining && TrainingController.stepTraining == TrainingController.stepTrainingList["SwipeWeapon"])
                {
                    TrainingController.isNextStep = TrainingController.stepTrainingList["SwipeWeapon"];
                }
                _weaponManager.CurrentWeaponIndex++;
                int count = _weaponManager.playerWeapons.Count;
                count = ((count == 0) ? 1 : count);
                _weaponManager.CurrentWeaponIndex %= count;
                ChangeWeapon(_weaponManager.CurrentWeaponIndex, false);
            }
            canReceiveSwipes = false;
            StartCoroutine(SetCanReceiveSwipes());
        }
		else if (mouseScroll <= -0.1f)
		{
            if ((isMulti && isMine) || !isMulti)
            {
                _weaponManager.CurrentWeaponIndex--;
                if (_weaponManager.CurrentWeaponIndex < 0)
                {
                    _weaponManager.CurrentWeaponIndex = _weaponManager.playerWeapons.Count - 1;
                }
                _weaponManager.CurrentWeaponIndex %= _weaponManager.playerWeapons.Count;
                ChangeWeapon(_weaponManager.CurrentWeaponIndex, false);
            }
            canReceiveSwipes = false;
            StartCoroutine(SetCanReceiveSwipes());
        }
		inGameTime += Time.deltaTime;
		if (isCompany && myCommand == 0 && myTable != null)
		{
			myCommand = myTable.GetComponent<NetworkStartTable>().myCommand;
		}
		if (isMulti && isMine && _weaponManager.myPlayer != null)
		{
			GlobalGameController.posMyPlayer = _weaponManager.myPlayer.transform.position;
			GlobalGameController.rotMyPlayer = _weaponManager.myPlayer.transform.rotation;
			GlobalGameController.healthMyPlayer = CurHealth;
		}
		slideScroll();
		if (timerShow[0] > 0f)
		{
			timerShow[0] -= Time.deltaTime;
		}
		if (timerShow[1] > 0f)
		{
			timerShow[1] -= Time.deltaTime;
		}
		if (timerShow[2] > 0f)
		{
			timerShow[2] -= Time.deltaTime;
		}
		if (_pauser == null)
		{
			Debug.LogWarning("Update(): _pauser is null.");
		}
		Func<bool> pauserIsPaused = () => _pauser != null && _pauser.paused;
		if (!pauserIsPaused() && canReceiveSwipes && !isInappWinOpen)
		{
			Rect rect = new Rect((float)Screen.width - 264f * (float)Screen.width / 1024f, (float)Screen.height - 94f * (float)Screen.width / 1024f - 95f * (float)Screen.width / 1024f, 264f * (float)Screen.width / 1024f, 95f * (float)Screen.width / 1024f);
			if (!showChat && (!Defs.IsTraining || TrainingController.stepTraining >= TrainingController.stepTrainingList["SwipeWeapon"]))
			{
				Touch[] touches = Input.touches;
				for (int i = 0; i < touches.Length; i++)
				{
					Touch touch = touches[i];
					if (!rect.Contains(touch.position))
					{
						continue;
					}
					if (touch.phase == TouchPhase.Began)
					{
						leftFingerPos = Vector2.zero;
						leftFingerLastPos = Vector2.zero;
						leftFingerMovedBy = Vector2.zero;
						slideMagnitudeX = 0f;
						slideMagnitudeY = 0f;
						leftFingerPos = touch.position;
					}
					else if (touch.phase == TouchPhase.Moved)
					{
						leftFingerMovedBy = touch.position - leftFingerPos;
						leftFingerLastPos = leftFingerPos;
						leftFingerPos = touch.position;
						slideMagnitudeX = leftFingerMovedBy.x / (float)Screen.width;
						float num = 0.02f;
						if (slideMagnitudeX > num)
						{
							if ((isMulti && isMine) || !isMulti)
							{
								if (Defs.IsTraining && TrainingController.stepTraining == TrainingController.stepTrainingList["SwipeWeapon"])
								{
									TrainingController.isNextStep = TrainingController.stepTrainingList["SwipeWeapon"];
								}
								_weaponManager.CurrentWeaponIndex++;
								int count = _weaponManager.playerWeapons.Count;
								count = ((count == 0) ? 1 : count);
								_weaponManager.CurrentWeaponIndex %= count;
								ChangeWeapon(_weaponManager.CurrentWeaponIndex, false);
							}
							canReceiveSwipes = false;
							StartCoroutine(SetCanReceiveSwipes());
							leftFingerLastPos = leftFingerPos;
							leftFingerPos = touch.position;
							slideMagnitudeX = 0f;
						}
						else if (slideMagnitudeX < 0f - num)
						{
							if ((isMulti && isMine) || !isMulti)
							{
								_weaponManager.CurrentWeaponIndex--;
								if (_weaponManager.CurrentWeaponIndex < 0)
								{
									_weaponManager.CurrentWeaponIndex = _weaponManager.playerWeapons.Count - 1;
								}
								_weaponManager.CurrentWeaponIndex %= _weaponManager.playerWeapons.Count;
								ChangeWeapon(_weaponManager.CurrentWeaponIndex, false);
							}
							canReceiveSwipes = false;
							StartCoroutine(SetCanReceiveSwipes());
							leftFingerLastPos = leftFingerPos;
							leftFingerPos = touch.position;
							slideMagnitudeX = 0f;
						}
						slideMagnitudeY = leftFingerMovedBy.y / (float)Screen.height;
					}
					else if (touch.phase == TouchPhase.Stationary)
					{
						leftFingerLastPos = leftFingerPos;
						leftFingerPos = touch.position;
						slideMagnitudeX = 0f;
						slideMagnitudeY = 0f;
					}
					else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
					{
						slideMagnitudeX = 0f;
						slideMagnitudeY = 0f;
					}
				}
			}
		}
		if (GunFlashLifetime > 0f)
		{
			GunFlashLifetime -= Time.deltaTime;
		}
		if (GunFlashLifetime <= 0f)
		{
			_SetGunFlashActive(false);
		}
		if (!(CurHealth <= 0f) || isKilled)
		{
			return;
		}
		if (isZooming)
		{
			ZoomPress();
			// How does it even work
		}
		if (isMulti)
		{
			resetMultyKill();
			if (!isInet)
			{
				base.GetComponent<NetworkView>().RPC("ImKilled", RPCMode.Others);
			}
			else
			{
				photonView.RPC("ImKilled", PhotonTargets.Others);
			}
			if (PlayerPrefsX.GetBool(PlayerPrefsX.SoundFXSetting, true))
			{
				base.gameObject.GetComponent<AudioSource>().PlayOneShot(deadPlayerSound);
			}
			if (isCOOP)
			{
				_weaponManager.myTable.GetComponent<NetworkStartTable>().score -= 1000f;
				if (_weaponManager.myTable.GetComponent<NetworkStartTable>().score < 0f)
				{
					_weaponManager.myTable.GetComponent<NetworkStartTable>().score = 0f;
				}
				GlobalGameController.Score = Mathf.RoundToInt(_weaponManager.myTable.GetComponent<NetworkStartTable>().score);
				_weaponManager.myTable.GetComponent<NetworkStartTable>().synchState();
			}
			isKilled = true;
			isDeadFrame = true;
			AutoFade.fadeKilled(0.5f, 1.5f, 0.5f, Color.white);
			Invoke("setisDeadFrameFalse", 1f);
			StartCoroutine(FlashWhenDead());
			_leftJoystick.SetActive(false);
			_rightJoystick.SetActive(false);
			HOTween.To(base.transform.parent.transform, 2f, new TweenParms().Prop("localPosition", new Vector3(base.transform.parent.transform.localPosition.x, 100f, base.transform.parent.transform.localPosition.z)).Ease(EaseType.EaseInCubic).OnComplete((TweenDelegate.TweenCallback)delegate
			{
				base.transform.parent.transform.localScale = new Vector3(1f, 1f, 1f);
				base.transform.rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
				isDeadFrame = false;
				Invoke("SetNoKilled", 0.5f);
				_weaponManager.myPlayer.GetComponent<SkinName>().camPlayer.transform.parent = _weaponManager.myPlayer.transform;
				if (!pauserIsPaused())
				{
					_leftJoystick.SetActive(true);
					_rightJoystick.SetActive(true);
				}
				if (_rightJoystick != null && _rightJoystick.GetComponent<JoystickSharp>() != null)
				{
					_rightJoystick.GetComponent<JoystickSharp>().HasAmmo();
				}
				else
				{
					Debug.Log("_rightJoystick=" + _rightJoystick);
				}
				CurHealth = MaxHealth;
				curArmor = 0f;
				zoneCreatePlayer = GameObject.FindGameObjectsWithTag(isCOOP ? "MultyPlayerCreateZoneCOOP" : ((!isCompany) ? "MultyPlayerCreateZone" : ("MultyPlayerCreateZoneCommand" + myCommand)));
				GameObject gameObject = zoneCreatePlayer[UnityEngine.Random.Range(0, zoneCreatePlayer.Length)];
				BoxCollider component = gameObject.GetComponent<BoxCollider>();
				Vector2 vector = new Vector2(component.size.x * gameObject.transform.localScale.x, component.size.z * gameObject.transform.localScale.z);
				Rect rect2 = new Rect(gameObject.transform.position.x - vector.x / 2f, gameObject.transform.position.z - vector.y / 2f, vector.x, vector.y);
				Vector3 position = new Vector3(rect2.x + UnityEngine.Random.Range(0f, rect2.width), gameObject.transform.position.y + 2f, rect2.y + UnityEngine.Random.Range(0f, rect2.height));
				Quaternion rotation = gameObject.transform.rotation;
				base.transform.parent.transform.position = position;
				base.transform.parent.transform.rotation = rotation;
				Invoke("ChangePositionAfterRespawn", 0.01f);
				foreach (Weapon playerWeapon in _weaponManager.playerWeapons)
				{
					if (playerWeapon.weaponPrefab.name.Equals("Weapon1") || playerWeapon.weaponPrefab.name.Equals("Weapon2"))
					{
						playerWeapon.currentAmmoInClip = playerWeapon.weaponPrefab.GetComponent<WeaponSounds>().ammoInClip;
						playerWeapon.currentAmmoInBackpack = playerWeapon.weaponPrefab.GetComponent<WeaponSounds>().InitialAmmo;
						/*if (Application.platform == RuntimePlatform.WindowsEditor)
						{
							playerWeapon.currentAmmoInClip = playerWeapon.weaponPrefab.GetComponent<WeaponSounds>().ammoInClip;
							playerWeapon.currentAmmoInBackpack = 1000000;
						}*/
					}
				}
			}));
			return;
		}
		if (Defs.IsSurvival)
		{
			if (GlobalGameController.Score > PlayerPrefs.GetInt(Defs.SurvivalScoreSett, 0))
			{
				PlayerPrefs.SetInt(Defs.SurvivalScoreSett, GlobalGameController.Score);
				PlayerPrefs.Save();
			}
		}
		else if (GlobalGameController.Score > PlayerPrefs.GetInt(Defs.BestScoreSett, 0))
		{
			PlayerPrefs.SetInt(Defs.BestScoreSett, GlobalGameController.Score);
			PlayerPrefs.Save();
		}
		PlayerPrefs.SetInt("IsGameOver", 1);
		LevelCompleteLoader.action = null;
		LevelCompleteLoader.sceneName = "LevelComplete";
		Application.LoadLevel("LevelToCompleteProm");
	}

	private void SetNoKilled()
	{
		isKilled = false;
	}

	private void ChangePositionAfterRespawn()
	{
		if ((bool)base.transform.parent)
		{
			base.transform.parent.transform.position += Vector3.forward * 0.01f;
		}
	}

	public static Rect SuccessMessageRect()
	{
		return new Rect((float)(Screen.width / 2) - (float)Screen.height * 0.5f, (float)Screen.height * 0.5f - (float)Screen.height * 0.0525f, Screen.height, (float)Screen.height * 0.105f);
	}

	public void showUnlockGUI()
	{
	}

	private void SetPause()
	{
		if (_pauser == null)
		{
			Debug.LogWarning("SetPause(): _pauser is null.");
			return;
		}
		_pauser.paused = !_pauser.paused;
		if (_pauser.paused)
		{
			if (!isMulti)
			{
				Time.timeScale = 0f;
			}
		}
		else
		{
			Time.timeScale = 1f;
		}
		if (_pauser.paused)
		{
			RemoveButtonHandelrs();
		}
		else
		{
			AddButtonHandlers();
        }
		WindowsMouseManager.MouseLocked = !_pauser.paused;
    }

	private void SetInApp()
	{
		isInappWinOpen = !isInappWinOpen;
		if (isInappWinOpen)
		{
			Shop.sharedShop.loadShopCategories();
			if (StoreKitEventListener.restoreInProcess)
			{
				_purchaseActivityIndicator.SetActive(true);
			}
			if (!isMulti)
			{
				Time.timeScale = 0f;
			}
			return;
		}
		Shop.sharedShop.unloadShopCategories();
		if (_purchaseActivityIndicator == null)
		{
			Debug.LogWarning("SetInApp(): _purchaseActivityIndicator is null.");
		}
		else
		{
			_purchaseActivityIndicator.SetActive(false);
		}
		if (_pauser == null)
		{
			Debug.LogWarning("SetInApp(): _pauser is null.");
		}
		else if (!_pauser.paused)
		{
			Time.timeScale = 1f;
		}
	}

	private void ProvideAmmo()
	{
		_listener.ProvideContent();
		_weaponManager.SetMaxAmmoFrAllWeapons();
		if (_rightJoystick != null && _rightJoystick.GetComponent<JoystickSharp>() != null)
		{
			_rightJoystick.GetComponent<JoystickSharp>().HasAmmo();
		}
		else
		{
			Debug.Log("_rightJoystick=" + _rightJoystick);
		}
	}

	private void ProvideHealth()
	{
		CurHealth = MaxHealth;
		CurrentCampaignGame.withoutHits = true;
	}

	public static void SaveMinerWeaponInPrefabs()
	{
		Storager.setInt(Defs.MinerWeaponSett, 1, true);
		PlayerPrefs.Save();
	}

	public static void SaveSwordInPrefs()
	{
		Storager.setInt(Defs.SwordSett, 1, true);
		PlayerPrefs.Save();
	}

	public static void SaveBarrettInPrefabs()
	{
		Storager.setInt(Defs.BarrettSN, 1, true);
		PlayerPrefs.Save();
	}

	public static void SaveSvdInPrefs()
	{
		Storager.setInt(Defs.SVDSN, 1, true);
		PlayerPrefs.Save();
	}

	public static void SaveCombatRifleInPrefs()
	{
		Storager.setInt(Defs.CombatRifleSett, 1, true);
		PlayerPrefs.Save();
	}

	public static void SaveStaffPrefs()
	{
		Storager.setInt(Defs.StaffSN, 1, true);
		PlayerPrefs.Save();
	}

	public static void SaveGoldenEagleInPrefs()
	{
		Storager.setInt(Defs.GoldenEagleSett, 1, true);
		PlayerPrefs.Save();
	}

	public static void SaveMagicBowInPrefs()
	{
		Storager.setInt(Defs.MagicBowSett, 1, true);
		PlayerPrefs.Save();
	}

	public static void SaveTreeInPrefs()
	{
		Storager.setInt(Defs.TreeSN, 1, true);
		PlayerPrefs.Save();
	}

	public static void SaveFireAxeInPrefs()
	{
		Storager.setInt(Defs.FireAxeSN, 1, true);
		PlayerPrefs.Save();
	}

	public static void Save3plShotInnPrefs()
	{
		Storager.setInt(Defs._3PLShotgunSN, 1, true);
		PlayerPrefs.Save();
	}

	public static void SaveRevolver2InPrefs()
	{
		Storager.setInt(Defs.Revolver2SN, 1, true);
		PlayerPrefs.Save();
	}

	public static void SaveChainsawInPrefs()
	{
		Storager.setInt(Defs.ChainsawS, 1, true);
		PlayerPrefs.Save();
	}

	public static void SaveFAMASPrefs()
	{
		if (!Defs.IsTraining)
		{
			Storager.setInt(Defs.FAMASS, 1, true);
			PlayerPrefs.Save();
		}
	}

	public static void SaveGlockInPrefs()
	{
		Storager.setInt(Defs.GlockSett, 1, true);
		PlayerPrefs.Save();
	}

	public static void SaveCrystalGlockPrefs()
	{
		Storager.setInt(Defs.CrystalGlockSN, 1, true);
		PlayerPrefs.Save();
	}

	public static void SaveCrystalSPASInPrefs()
	{
		Storager.setInt(Defs.CrystalSPASSN, 1, true);
		PlayerPrefs.Save();
	}

	public static void SaveRedMinigunPrefs()
	{
		Storager.setInt(Defs.RedMinigunSN, 1, true);
		PlayerPrefs.Save();
	}

	public static void SaveCrystalCrossbowInPrefs()
	{
		Storager.setInt(Defs.CrystalCrossbowSN, 1, true);
		PlayerPrefs.Save();
	}

	public static void SaveRedLightSaberInPrefs()
	{
		Storager.setInt(Defs.RedLightSaberSN, 1, true);
		PlayerPrefs.Save();
	}

	public static void SaveSandFamasInPrefs()
	{
		if (!Defs.IsTraining)
		{
			Storager.setInt(Defs.SandFamasSN, 1, true);
			PlayerPrefs.Save();
		}
	}

	public static void SaveWhiteBerettaInPrefs()
	{
		Storager.setInt(Defs.WhiteBerettaSN, 1, true);
		PlayerPrefs.Save();
	}

	public static void SaveBlackEagleInPrefs()
	{
		Storager.setInt(Defs.BlackEagleSN, 1, true);
		PlayerPrefs.Save();
	}

	public static void SaveScytheInPrefs()
	{
		Storager.setInt(Defs.ScytheSN, 1, true);
		PlayerPrefs.Save();
	}

	public static void SaveShovelPrefs()
	{
		Storager.setInt(Defs.ShovelSN, 1, true);
		PlayerPrefs.Save();
	}

	public static void SaveLaserRiflePrefs()
	{
		Storager.setInt(Defs.LaserRifleSN, 1, true);
		PlayerPrefs.Save();
	}

	public static void SaveGoldenPickPrefs()
	{
		Storager.setInt(Defs.GoldenPickSN, 1, true);
		PlayerPrefs.Save();
	}

	public static void SaveCrystalPickPrefs()
	{
		Storager.setInt(Defs.CrystakPickSN, 1, true);
		PlayerPrefs.Save();
	}

	public static void SaveIronSwordInPrefs()
	{
		Storager.setInt(Defs.IronSwordSN, 1, true);
		PlayerPrefs.Save();
	}

	public static void SaveGoldenSwordInPrefs()
	{
		Storager.setInt(Defs.GoldenSwordSN, 1, true);
		PlayerPrefs.Save();
	}

	public static void SaveGoldenRed_Stone()
	{
		Storager.setInt(Defs.GoldenRed_StoneSN, 1, true);
		PlayerPrefs.Save();
	}

	public static void SaveGoldenSPASSN()
	{
		Storager.setInt(Defs.GoldenSPASSN, 1, true);
		PlayerPrefs.Save();
	}

	public static void SaveGoldenGlockInPrefs()
	{
		Storager.setInt(Defs.GoldenGlockSN, 1, true);
		PlayerPrefs.Save();
	}

	public static void SaveMaceInPrefs()
	{
		Storager.setInt(Defs.MaceSN, 1, true);
		PlayerPrefs.Save();
	}

	public static void SaveCrossbowInPrefs()
	{
		Storager.setInt(Defs.CrossbowSN, 1, true);
		PlayerPrefs.Save();
	}

	public static void SaveMinigunInPrefs()
	{
		Storager.setInt(Defs.MinigunSN, 1, true);
		PlayerPrefs.Save();
	}

	public static void SaveSword_2_InPrefs()
	{
		Storager.setInt(Defs.Sword_2_SN, 1, true);
		PlayerPrefs.Save();
	}

	public static void SaveHammerPrefs()
	{
		Storager.setInt(Defs.HammerSN, 1, true);
		PlayerPrefs.Save();
	}

	public static void SaveSPASInPrefs()
	{
		Storager.setInt(Defs.SPASSett, 1, true);
		PlayerPrefs.Save();
	}

	public static void SaveMGoldenAxeInPrefs()
	{
		Storager.setInt(Defs.GoldenAxeSett, 1, true);
		PlayerPrefs.Save();
	}

	public static void SaveLightSwordInPrefs()
	{
		Storager.setInt(Defs.LightSwordSN, 1, true);
		PlayerPrefs.Save();
	}

	public static void SaveBerettaInPrefs()
	{
		Storager.setInt(Defs.BerettaSN, 1, true);
		PlayerPrefs.Save();
	}

	public static void SaveCrystalAxeInPrefs()
	{
		Storager.setInt(Defs.CrystalAxeSN, 1, true);
		PlayerPrefs.Save();
	}

	public static void SaveSteelAxeInPrefs()
	{
		Storager.setInt(Defs.SteelAxeSN, 1, true);
		PlayerPrefs.Save();
	}

	public static void SaveWoodenBowInPrefs()
	{
		Storager.setInt(Defs.WoodenBowSN, 1, true);
		PlayerPrefs.Save();
	}

	public static void SaveChainsaw2InPrefs()
	{
		Storager.setInt(Defs.Chainsaw2SN, 1, true);
		PlayerPrefs.Save();
	}

	public static void SaveSteelCrossbowInPrefs()
	{
		Storager.setInt(Defs.SteelCrossbowSN, 1, true);
		PlayerPrefs.Save();
	}

	public static void SaveHammer2InPrefs()
	{
		Storager.setInt(Defs.Hammer2SN, 1, true);
		PlayerPrefs.Save();
	}

	public static void SaveMace2InPrefs()
	{
		Storager.setInt(Defs.Mace2SN, 1, true);
		PlayerPrefs.Save();
	}

	public static void SaveSword_22InPrefs()
	{
		Storager.setInt(Defs.Sword_22SN, 1, true);
		PlayerPrefs.Save();
	}

	public static void SaveStaff2InPrefs()
	{
		Storager.setInt(Defs.Staff2SN, 1, true);
		PlayerPrefs.Save();
	}

	private void providetree()
	{
		GameObject treePrefab = _weaponManager.GetTreePrefab();
		SaveTreeInPrefs();
		AddWeapon(treePrefab);
	}

	private void provideFireAxe()
	{
		GameObject fireAxePrefab = _weaponManager.GetFireAxePrefab();
		SaveFireAxeInPrefs();
		AddWeapon(fireAxePrefab);
	}

	private void provide3plShotgun()
	{
		GameObject weaponPrefab = _weaponManager.Get3plShotgunPrefab();
		Save3plShotInnPrefs();
		AddWeapon(weaponPrefab);
	}

	private void provideRevolver2()
	{
		GameObject revolver2Prefab = _weaponManager.GetRevolver2Prefab();
		SaveRevolver2InPrefs();
		AddWeapon(revolver2Prefab);
	}

	private void provideminerweapon()
	{
		GameObject pickPrefab = _weaponManager.GetPickPrefab();
		SaveMinerWeaponInPrefabs();
		AddWeapon(pickPrefab);
	}

	private void providesword()
	{
		GameObject swordPrefab = _weaponManager.GetSwordPrefab();
		SaveSwordInPrefs();
		AddWeapon(swordPrefab);
	}

	private void providebarrett()
	{
		GameObject barrettPrefab = _weaponManager.GetBarrettPrefab();
		SaveBarrettInPrefabs();
		AddWeapon(barrettPrefab);
	}

	private void provideSvd()
	{
		GameObject sVDPrefab = _weaponManager.GetSVDPrefab();
		SaveSvdInPrefs();
		AddWeapon(sVDPrefab);
	}

	private void providegoldenpickweapon()
	{
		GameObject goldenPickPref = _weaponManager.GetGoldenPickPref();
		SaveGoldenPickPrefs();
		AddWeapon(goldenPickPref);
	}

	private void provideRedMinigun()
	{
		GameObject redMinigunPref = _weaponManager.GetRedMinigunPref();
		SaveRedMinigunPrefs();
		AddWeapon(redMinigunPref);
	}

	private void provideCrystalCrossbow()
	{
		GameObject crystCrossbowPref = _weaponManager.GetCrystCrossbowPref();
		SaveCrystalCrossbowInPrefs();
		AddWeapon(crystCrossbowPref);
	}

	private void provideRedLightSaber()
	{
		GameObject redLightSaberPrefab = _weaponManager.GetRedLightSaberPrefab();
		SaveRedLightSaberInPrefs();
		AddWeapon(redLightSaberPrefab);
	}

	private void provideSandFamas()
	{
		GameObject sandFamasPrefab = _weaponManager.GetSandFamasPrefab();
		SaveSandFamasInPrefs();
		AddWeapon(sandFamasPrefab);
	}

	private void provideWhiteBeretta()
	{
		GameObject whiteBerettaPrefab = _weaponManager.GetWhiteBerettaPrefab();
		SaveWhiteBerettaInPrefs();
		AddWeapon(whiteBerettaPrefab);
	}

	private void provideBlackEagle()
	{
		GameObject blackEaglePrefab = _weaponManager.GetBlackEaglePrefab();
		SaveBlackEagleInPrefs();
		AddWeapon(blackEaglePrefab);
	}

	private void providecrystalpickweapon()
	{
		GameObject crystPickPref = _weaponManager.GetCrystPickPref();
		SaveCrystalPickPrefs();
		AddWeapon(crystPickPref);
	}

	private void provideSword_2()
	{
		GameObject sword_2_Prefab = _weaponManager.GetSword_2_Prefab();
		SaveSword_2_InPrefs();
		AddWeapon(sword_2_Prefab);
	}

	private void provideHammer()
	{
		GameObject hammerPrefab = _weaponManager.GetHammerPrefab();
		SaveHammerPrefs();
		AddWeapon(hammerPrefab);
	}

	private void provideLaser()
	{
		GameObject laserRiflePrefab = _weaponManager.GetLaserRiflePrefab();
		SaveLaserRiflePrefs();
		AddWeapon(laserRiflePrefab);
	}

	private void provideLightSword()
	{
		GameObject lightSwordPrefab = _weaponManager.GetLightSwordPrefab();
		SaveLightSwordInPrefs();
		AddWeapon(lightSwordPrefab);
	}

	private void provideBeretta()
	{
		GameObject berettaPrefab = _weaponManager.GetBerettaPrefab();
		SaveBerettaInPrefs();
		AddWeapon(berettaPrefab);
	}

	private void provideIronSword()
	{
		GameObject ironSwordPrefab = _weaponManager.GetIronSwordPrefab();
		SaveIronSwordInPrefs();
		AddWeapon(ironSwordPrefab);
	}

	private void provideGoldenSword()
	{
		GameObject goldenSwordPrefab = _weaponManager.GetGoldenSwordPrefab();
		SaveGoldenSwordInPrefs();
		AddWeapon(goldenSwordPrefab);
	}

	private void provideGoldenRed_Stone()
	{
		GameObject goldenRedStonePrefab = _weaponManager.GetGoldenRedStonePrefab();
		SaveGoldenRed_Stone();
		AddWeapon(goldenRedStonePrefab);
	}

	private void provideGoldenSPAS()
	{
		GameObject goldenSPASPrefab = _weaponManager.GetGoldenSPASPrefab();
		SaveGoldenSPASSN();
		AddWeapon(goldenSPASPrefab);
	}

	private void provideGoldenGlock()
	{
		GameObject goldenGlockPrefab = _weaponManager.GetGoldenGlockPrefab();
		SaveGoldenGlockInPrefs();
		AddWeapon(goldenGlockPrefab);
	}

	private void provideCrystalAxe()
	{
		GameObject crystalAxePrefab = _weaponManager.GetCrystalAxePrefab();
		SaveCrystalAxeInPrefs();
		AddWeapon(crystalAxePrefab);
	}

	private void provideSteelAxe()
	{
		GameObject steelAxePrefab = _weaponManager.GetSteelAxePrefab();
		SaveSteelAxeInPrefs();
		AddWeapon(steelAxePrefab);
	}

	private void provideWoodenBow()
	{
		GameObject woodenBowPrefab = _weaponManager.GetWoodenBowPrefab();
		SaveWoodenBowInPrefs();
		AddWeapon(woodenBowPrefab);
	}

	private void provideChainsaw2()
	{
		GameObject chainsaw2Prefab = _weaponManager.GetChainsaw2Prefab();
		SaveChainsaw2InPrefs();
		AddWeapon(chainsaw2Prefab);
	}

	private void provideSteelCrossbow()
	{
		GameObject steelCrossbowPrefab = _weaponManager.GetSteelCrossbowPrefab();
		SaveSteelCrossbowInPrefs();
		AddWeapon(steelCrossbowPrefab);
	}

	private void provideHammer2()
	{
		GameObject hammer2Prefab = _weaponManager.GetHammer2Prefab();
		SaveHammer2InPrefs();
		AddWeapon(hammer2Prefab);
	}

	private void provideMace2()
	{
		GameObject mace2Prefab = _weaponManager.GetMace2Prefab();
		SaveMace2InPrefs();
		AddWeapon(mace2Prefab);
	}

	private void provideSword_22()
	{
		GameObject sword_22Prefab = _weaponManager.GetSword_22Prefab();
		SaveSword_22InPrefs();
		AddWeapon(sword_22Prefab);
	}

	private void provideStaff2()
	{
		GameObject staff2Prefab = _weaponManager.GetStaff2Prefab();
		SaveStaff2InPrefs();
		AddWeapon(staff2Prefab);
	}

	private void provideMace()
	{
		GameObject macePrefab = _weaponManager.GetMacePrefab();
		SaveMaceInPrefs();
		AddWeapon(macePrefab);
	}

	private void provideCrossbow()
	{
		GameObject crossbowPrefab = _weaponManager.GetCrossbowPrefab();
		SaveCrossbowInPrefs();
		AddWeapon(crossbowPrefab);
	}

	private void provideMinigun()
	{
		GameObject minigunPrefab = _weaponManager.GetMinigunPrefab();
		SaveMinigunInPrefs();
		AddWeapon(minigunPrefab);
	}

	private void providecombatrifle()
	{
		GameObject combatRiflePrefab = _weaponManager.GetCombatRiflePrefab();
		SaveCombatRifleInPrefs();
		AddWeapon(combatRiflePrefab);
	}

	private void provideStaff()
	{
		GameObject staffPrefab = _weaponManager.GetStaffPrefab();
		SaveStaffPrefs();
		AddWeapon(staffPrefab);
	}

	private void providegoldeneagle()
	{
		GameObject goldenEaglePrefab = _weaponManager.GetGoldenEaglePrefab();
		SaveGoldenEagleInPrefs();
		AddWeapon(goldenEaglePrefab);
	}

	private void providemagicbow()
	{
		GameObject magicBowPrefab = _weaponManager.GetMagicBowPrefab();
		SaveMagicBowInPrefs();
		AddWeapon(magicBowPrefab);
	}

	private void provideChainsaw()
	{
		GameObject chainsawPrefab = _weaponManager.GetChainsawPrefab();
		SaveChainsawInPrefs();
		AddWeapon(chainsawPrefab);
	}

	private void provideFAMAS()
	{
		GameObject fAMASPrefab = _weaponManager.GetFAMASPrefab();
		SaveFAMASPrefs();
		AddWeapon(fAMASPrefab);
	}

	private void provideGlock()
	{
		GameObject glockPrefab = _weaponManager.GetGlockPrefab();
		SaveGlockInPrefs();
		AddWeapon(glockPrefab);
	}

	private void provideScythe()
	{
		GameObject scythePrefab = _weaponManager.GetScythePrefab();
		SaveScytheInPrefs();
		AddWeapon(scythePrefab);
	}

	private void provideShovel()
	{
		GameObject shovelPrefab = _weaponManager.GetShovelPrefab();
		SaveShovelPrefs();
		AddWeapon(shovelPrefab);
	}

	private void providespas()
	{
		GameObject sPASPrefab = _weaponManager.GetSPASPrefab();
		SaveSPASInPrefs();
		AddWeapon(sPASPrefab);
	}

	private void provideaxe()
	{
		GameObject axePrefab = _weaponManager.GetAxePrefab();
		SaveMGoldenAxeInPrefs();
		AddWeapon(axePrefab);
	}

	private void provideCrystalGlock()
	{
		GameObject crystGlockPref = _weaponManager.GetCrystGlockPref();
		SaveCrystalGlockPrefs();
		AddWeapon(crystGlockPref);
	}

	private void provideCrystalSpas()
	{
		GameObject crystalSPASPref = _weaponManager.GetCrystalSPASPref();
		SaveCrystalSPASInPrefs();
		AddWeapon(crystalSPASPref);
	}

	public void PurchaseSuccessful(string id)
	{
		if (id == null)
		{
			throw new ArgumentNullException("id");
		}
		if ((id.Equals("bigammopack") || id.Equals("Fullhealth")) && Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
		{
			GoogleIAB.consumeProduct(id);
		}
		if (_actionsForPurchasedItems.ContainsKey(id))
		{
			_actionsForPurchasedItems[id].Key();
		}
		else
		{
			Debug.LogWarning("Cannot find action for key \"" + id + "\".");
		}
		string eventName = ((!InAppData.inappReadableNames.ContainsKey(id)) ? id : InAppData.inappReadableNames[id]);
		FlurryAndroid.logEvent(eventName, false);
		productPurchased = true;
		_timeWhenPurchShown = Time.realtimeSinceStartup;
	}

	private void purchaseSuccessful(GooglePurchase purchase)
	{
		try
		{
			if (purchase == null)
			{
				throw new ArgumentNullException("purchase");
			}
			PurchaseSuccessful(purchase.productId);
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
	}

	private void HandlePurchaseSuccessful(AmazonReceipt receipt)
	{
		PurchaseSuccessful(receipt.sku);
	}

	private void consumptionSucceeded(GooglePurchase purchase)
	{
	}

	private IEnumerator _ResetProductPurchased()
	{
		yield return new WaitForSeconds(1f);
		productPurchased = false;
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
			scrollPosition.x += scrollStartTouch.x - position.x;
			scrollStartTouch = position;
		}
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended && scrollEnabled)
		{
			scrollEnabled = false;
		}
	}
}