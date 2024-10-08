using UnityEngine;

public class SkinName : MonoBehaviour
{
	public GameObject playerGameObject;

	public Player_move_c playerMoveC;

	public string skinName;

	public GameObject hatsPoint;

	public GameObject capesPoint;

	public string NickName;

	public GameObject camPlayer;

	public GameObject headObj;

	public GameObject bodyLayer;

	public CharacterController character;

	public PhotonView photonView;

	public int typeAnim;

	public WeaponManager _weaponManager;

	public bool isInet;

	public bool isMine;

	public bool isMulti;

	public AudioClip walkAudio;

	public AudioClip jumpAudio;

	public AudioClip jumpDownAudio;

	public bool isPlayDownSound;

	public GameObject FPSplayerObject;

	public void sendAnimJump()
	{
		if (isMulti && !isInet && isMine)
		{
			base.GetComponent<NetworkView>().RPC("SetAnim", RPCMode.All, 2);
		}
		if (isMulti && isInet && isMine)
		{
			photonView.RPC("SetAnim", PhotonTargets.All, 2);
		}
	}

	[RPC]
	private void SetAnim(int _typeAnim)
	{
		string text;
		switch (_typeAnim)
		{
		case 0:
			text = "Idle";
			if (PlayerPrefsX.GetBool(PlayerPrefsX.SoundFXSetting, true))
			{
				base.GetComponent<AudioSource>().Stop();
			}
			break;
		case 1:
			text = "Walk";
			base.GetComponent<AudioSource>().loop = true;
			base.GetComponent<AudioSource>().clip = walkAudio;
			if (PlayerPrefsX.GetBool(PlayerPrefsX.SoundFXSetting, true))
			{
				base.GetComponent<AudioSource>().Play();
			}
			break;
		case 2:
			text = "Jump";
			if (PlayerPrefsX.GetBool(PlayerPrefsX.SoundFXSetting, true))
			{
				base.GetComponent<AudioSource>().Stop();
			}
			break;
		default:
			text = "Dead";
			if (PlayerPrefsX.GetBool(PlayerPrefsX.SoundFXSetting, true))
			{
				base.GetComponent<AudioSource>().Stop();
			}
			break;
		}
		FPSplayerObject.GetComponent<Animation>().Play(text);
		if (capesPoint.transform.childCount > 0)
		{
			capesPoint.transform.GetChild(0).GetComponent<Animation>().Play(text);
		}
	}

	[RPC]
	private void setCapeRPC(string _currentCape)
	{
		GameObject original = Resources.Load("Capes/" + _currentCape) as GameObject;
		GameObject gameObject = Object.Instantiate(original) as GameObject;
		gameObject.transform.parent = capesPoint.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
	}

	[RPC]
	private void setHatRPC(string _currentHat)
	{
		GameObject original = Resources.Load("Hats/" + _currentHat) as GameObject;
		GameObject gameObject = Object.Instantiate(original) as GameObject;
		gameObject.transform.parent = hatsPoint.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
	}

	private void Start()
	{
		_weaponManager = GameObject.FindGameObjectWithTag("WeaponManager").GetComponent<WeaponManager>();
		playerMoveC = playerGameObject.GetComponent<Player_move_c>();
		character = base.transform.GetComponent<CharacterController>();
		isMulti = PlayerPrefs.GetInt("MultyPlayer") == 1;
		photonView = PhotonView.Get(this);
		isInet = PlayerPrefs.GetString("TypeConnect").Equals("inet");
		if (!isInet)
		{
			isMine = base.GetComponent<NetworkView>().isMine;
		}
		else
		{
			isMine = photonView.isMine;
		}
		if (((PlayerPrefs.GetString("TypeConnect").Equals("local") && !base.GetComponent<NetworkView>().isMine) || (PlayerPrefs.GetString("TypeConnect").Equals("inet") && !photonView.isMine)) && PlayerPrefs.GetInt("MultyPlayer") == 1)
		{
			camPlayer.active = false;
			character.enabled = false;
		}
		else
		{
			base.transform.Find("FPS_Player").gameObject.SetActive(false);
		}
		if (PlayerPrefs.GetInt("MultyPlayer") != 1 || (PlayerPrefs.GetString("TypeConnect").Equals("local") && base.GetComponent<NetworkView>().isMine) || (PlayerPrefs.GetString("TypeConnect").Equals("inet") && photonView.isMine))
		{
			base.gameObject.layer = 11;
			bodyLayer.layer = 11;
			headObj.layer = 11;
		}
		if (!isMulti || !isMine)
		{
			return;
		}
		string @string = Storager.getString(Defs.CapeEquppedSN, false);
		string string2 = Storager.getString(Defs.HatEquppedSN, false);
		if (!@string.Equals(Defs.CapeNoneEqupped))
		{
			if (isInet)
			{
				photonView.RPC("setCapeRPC", PhotonTargets.OthersBuffered, @string);
			}
			else
			{
				base.GetComponent<NetworkView>().RPC("setCapeRPC", RPCMode.OthersBuffered, @string);
			}
		}
		if (!string2.Equals(Defs.HatNoneEqupped))
		{
			if (isInet)
			{
				photonView.RPC("setHatRPC", PhotonTargets.OthersBuffered, string2);
			}
			else
			{
				base.GetComponent<NetworkView>().RPC("setHatRPC", RPCMode.OthersBuffered, string2);
			}
		}
	}

	private void Update()
	{
		if ((!isMulti || !isMine) && isMulti)
		{
			return;
		}
		if (playerMoveC.isKilled)
		{
			isPlayDownSound = false;
		}
		int num = 0;
		if (character.velocity.y > 0.01f || character.velocity.y < -0.01f)
		{
			num = 2;
		}
		else if ((character.velocity.x != 0f || character.velocity.z != 0f) && character.isGrounded)
		{
			num = 1;
		}
		if (character.velocity.y < -2.5f && !character.isGrounded)
		{
			isPlayDownSound = true;
		}
		if (isPlayDownSound && character.isGrounded)
		{
			if (PlayerPrefsX.GetBool(PlayerPrefsX.SoundFXSetting, true))
			{
				NGUITools.PlaySound(jumpDownAudio);
			}
			isPlayDownSound = false;
		}
		if (num == typeAnim)
		{
			return;
		}
		typeAnim = num;
		if (isMulti && !isInet && isMine)
		{
			base.GetComponent<NetworkView>().RPC("SetAnim", RPCMode.All, typeAnim);
		}
		if (isMulti && isInet && isMine && typeAnim != 2)
		{
			photonView.RPC("SetAnim", PhotonTargets.All, typeAnim);
		}
		if (isMulti)
		{
			return;
		}
		if (typeAnim != 1)
		{
			if (PlayerPrefsX.GetBool(PlayerPrefsX.SoundFXSetting, true))
			{
				base.GetComponent<AudioSource>().Stop();
			}
			return;
		}
		base.GetComponent<AudioSource>().loop = true;
		base.GetComponent<AudioSource>().clip = walkAudio;
		if (PlayerPrefsX.GetBool(PlayerPrefsX.SoundFXSetting, true))
		{
			base.GetComponent<AudioSource>().Play();
		}
	}

	private void OnControllerColliderHit(ControllerColliderHit col)
	{
		if (PlayerPrefs.GetInt("MultyPlayer") != 1 && col.collider.gameObject.name.Equals("DeadCollider"))
		{
			isPlayDownSound = false;
			if (playerGameObject.GetComponent<Player_move_c>().CurHealth > 0f)
			{
				playerGameObject.GetComponent<Player_move_c>().CurHealth = 0f;
				playerGameObject.GetComponent<Player_move_c>().curArmor = 0f;
			}
		}
		else
		{
			if (PlayerPrefs.GetInt("MultyPlayer") != 1 || ((!PlayerPrefs.GetString("TypeConnect").Equals("local") || !base.GetComponent<NetworkView>().isMine) && (!PlayerPrefs.GetString("TypeConnect").Equals("inet") || !photonView || !photonView.isMine)) || !col.collider.gameObject.name.Equals("DeadCollider"))
			{
				return;
			}
			isPlayDownSound = false;
			if (playerGameObject.GetComponent<Player_move_c>().CurHealth > 0f)
			{
				playerGameObject.GetComponent<Player_move_c>().curArmor = 0f;
				playerGameObject.GetComponent<Player_move_c>().CurHealth = 0f;
				if (playerGameObject.GetComponent<Player_move_c>().countKills > 0)
				{
					playerGameObject.GetComponent<Player_move_c>().countKills--;
				}
				_weaponManager.myTable.GetComponent<NetworkStartTable>().CountKills = playerGameObject.GetComponent<Player_move_c>().countKills;
				_weaponManager.myTable.GetComponent<NetworkStartTable>().synchState();
				playerGameObject.GetComponent<Player_move_c>().sendImDeath(NickName);
			}
		}
	}
}
