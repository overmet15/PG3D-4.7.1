using System;
using System.Collections;
using UnityEngine;

internal sealed class BotHealth : MonoBehaviour
{
	private static SkinsManagerPixlGun _skinsManager;

	public string myName = "Bot";

	private bool IsLife = true;

	public Texture hitTexture;

	private BotAI ai;

	private Player_move_c healthDown;

	private bool _flashing;

	private GameObject _modelChild;

	private Sounds _soundClips;

	private Texture _skin;

	private void Awake()
	{
		if (PlayerPrefs.GetInt("COOP") == 1)
		{
			base.enabled = false;
		}
	}

	private void Start()
	{
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			if (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				_modelChild = transform.gameObject;
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
		_soundClips = _modelChild.GetComponent<Sounds>();
		ai = GetComponent<BotAI>();
		healthDown = GameObject.FindGameObjectWithTag("PlayerGun").GetComponent<Player_move_c>();
		if (base.gameObject.name.IndexOf("Boss") == -1)
		{
			_skin = SetSkinForObj(_modelChild);
			return;
		}
		Renderer componentInChildren = _modelChild.GetComponentInChildren<Renderer>();
		_skin = componentInChildren.material.mainTexture;
	}

	public static Texture SetSkinForObj(GameObject go)
	{
		if (!_skinsManager)
		{
			_skinsManager = GameObject.FindGameObjectWithTag("SkinsManager").GetComponent<SkinsManagerPixlGun>();
		}
		Texture texture = null;
		string text = SkinNameForObj(go.name);
		if (!(texture = _skinsManager.skins[text] as Texture))
		{
			Debug.Log("No skin: " + text);
		}
		SetTextureRecursivelyFrom(go, texture);
		return texture;
	}

	public static string SkinNameForObj(string objName)
	{
		return Defs.IsSurvival ? objName : ((PlayerPrefs.GetInt(Defs.TrainingCompleted_4_4_Sett, 0) != 0) ? (objName + "_Level" + CurrentCampaignGame.currentLevel) : (objName + "_Level3"));
	}

	public static void SetTextureRecursivelyFrom(GameObject obj, Texture txt)
	{
		foreach (Transform item in obj.transform)
		{
			if ((bool)item.gameObject.GetComponent<Renderer>() && (bool)item.gameObject.GetComponent<Renderer>().material)
			{
				item.gameObject.GetComponent<Renderer>().material.mainTexture = txt;
			}
			SetTextureRecursivelyFrom(item.gameObject, txt);
		}
	}

	private IEnumerator Flash()
	{
		_flashing = true;
		SetTextureRecursivelyFrom(_modelChild, hitTexture);
		yield return new WaitForSeconds(0.125f);
		SetTextureRecursivelyFrom(_modelChild, _skin);
		_flashing = false;
	}

	private void _CreateBonusWeapon()
	{
		if (!LevelBox.weaponsFromBosses.ContainsKey(Application.loadedLevelName) || !base.gameObject.name.Contains("Boss"))
		{
			return;
		}
		string value = LevelBox.weaponsFromBosses[Application.loadedLevelName];
		WeaponManager component = GameObject.FindGameObjectWithTag("WeaponManager").GetComponent<WeaponManager>();
		GameObject wp = null;
		UnityEngine.Object[] weaponsInGame = component.weaponsInGame;
		for (int i = 0; i < weaponsInGame.Length; i++)
		{
			GameObject gameObject = (GameObject)weaponsInGame[i];
			if (gameObject.name.Equals(value))
			{
				wp = gameObject;
				break;
			}
		}
		GameObject gameObject2 = BonusCreator._CreateBonus(wp, base.gameObject.transform.position + new Vector3(0f, 0.25f, 0f));
		gameObject2.AddComponent<GotToNextLevel>();
	}

	public void adjustHealth(float _health, Transform target)
	{
		if (_health < 0f && !_flashing)
		{
			StartCoroutine(Flash());
		}
		_soundClips.health += _health;
		if (_soundClips.health < 0f)
		{
			_soundClips.health = 0f;
		}
		if (Debug.isDebugBuild)
		{
			_CreateBonusWeapon();
			IsLife = false;
		}
		else if (_soundClips.health == 0f)
		{
			_CreateBonusWeapon();
			IsLife = false;
		}
		else
		{
			GlobalGameController.Score += 5;
		}
		if (PlayerPrefsX.GetBool(PlayerPrefsX.SoundFXSetting, true))
		{
			base.GetComponent<AudioSource>().PlayOneShot(_soundClips.hurt);
		}
		ai.SetTarget(target, true);
	}

	public bool getIsLife()
	{
		return IsLife;
	}
}
