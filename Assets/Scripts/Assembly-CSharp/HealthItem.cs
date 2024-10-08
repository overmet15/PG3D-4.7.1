using UnityEngine;

public class HealthItem : MonoBehaviour
{
	public Player_move_c test;

	public GameObject player;

	private bool isKilled;

	public AudioClip HealthItemUp;

	private PhotonView photonView;

	private bool isMulti;

	private void Start()
	{
		photonView = PhotonView.Get(this);
		if (PlayerPrefs.GetInt("MultyPlayer") == 1)
		{
			isMulti = true;
			if (GameObject.FindGameObjectWithTag("WeaponManager").GetComponent<WeaponManager>() != null && GameObject.FindGameObjectWithTag("WeaponManager").GetComponent<WeaponManager>().myGun != null)
			{
				test = GameObject.FindGameObjectWithTag("WeaponManager").GetComponent<WeaponManager>().myGun.GetComponent<Player_move_c>();
			}
			if (GameObject.FindGameObjectWithTag("WeaponManager").GetComponent<WeaponManager>() != null)
			{
				player = GameObject.FindGameObjectWithTag("WeaponManager").GetComponent<WeaponManager>().myPlayer;
			}
		}
		else
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("PlayerGun");
			if ((bool)gameObject)
			{
				test = gameObject.GetComponent<Player_move_c>();
			}
			player = GameObject.FindGameObjectWithTag("WeaponManager").GetComponent<WeaponManager>().myPlayer;
		}
	}

	[RPC]
	private void delBonus(NetworkViewID idPlayer)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			if (idPlayer.Equals(gameObject.GetComponent<NetworkView>().viewID) && gameObject != null && PlayerPrefsX.GetBool(PlayerPrefsX.SoundFXSetting, true))
			{
				gameObject.transform.Find("GameObject").GetComponent<AudioSource>().PlayOneShot(GetComponent<HealthItem>().HealthItemUp);
			}
		}
		Object.Destroy(base.gameObject, 0.3f);
	}

	[RPC]
	private void delBonusPhoton(int idPlayer)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			if (idPlayer == gameObject.GetComponent<PhotonView>().viewID && gameObject != null && PlayerPrefsX.GetBool(PlayerPrefsX.SoundFXSetting, true))
			{
				gameObject.transform.Find("GameObject").GetComponent<AudioSource>().PlayOneShot(base.gameObject.GetComponent<HealthItem>().HealthItemUp);
			}
		}
		Object.Destroy(base.gameObject, 0.3f);
	}

	private void Update()
	{
		if (isKilled)
		{
			return;
		}
		if (test == null || player == null)
		{
			if (isMulti)
			{
				if (GameObject.FindGameObjectWithTag("WeaponManager").GetComponent<WeaponManager>() != null && GameObject.FindGameObjectWithTag("WeaponManager").GetComponent<WeaponManager>().myGun != null)
				{
					test = GameObject.FindGameObjectWithTag("WeaponManager").GetComponent<WeaponManager>().myGun.GetComponent<Player_move_c>();
				}
				if (GameObject.FindGameObjectWithTag("WeaponManager").GetComponent<WeaponManager>() != null)
				{
					player = GameObject.FindGameObjectWithTag("WeaponManager").GetComponent<WeaponManager>().myPlayer;
				}
			}
			else
			{
				GameObject gameObject = GameObject.FindGameObjectWithTag("PlayerGun");
				if ((bool)gameObject)
				{
					test = gameObject.GetComponent<Player_move_c>();
				}
				player = GameObject.FindGameObjectWithTag("Player");
			}
		}
		if (test == null || player == null || !(Vector3.Distance(base.transform.position, player.transform.position) < 2f))
		{
			return;
		}
		test.CurHealth += 1f;
		if (PlayerPrefsX.GetBool(PlayerPrefsX.SoundFXSetting, true))
		{
			test.gameObject.GetComponent<AudioSource>().PlayOneShot(HealthItemUp);
		}
		isKilled = true;
		if (isMulti)
		{
			if (PlayerPrefs.GetString("TypeConnect").Equals("local"))
			{
				base.GetComponent<NetworkView>().RPC("delBonus", RPCMode.All, player.GetComponent<NetworkView>().viewID);
			}
			else
			{
				photonView.RPC("delBonusPhoton", PhotonTargets.All, player.GetComponent<PhotonView>().viewID);
			}
		}
		else
		{
			Object.Destroy(base.gameObject);
		}
		if (test.CurHealth > test.MaxHealth)
		{
			test.CurHealth = test.MaxHealth;
			GlobalGameController.Score += 100;
		}
	}
}
