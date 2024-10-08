using UnityEngine;

public class SetParentWeapon : MonoBehaviour
{
	private void Start()
	{
		if (PlayerPrefs.GetInt("MultyPlayer") != 1)
		{
			return;
		}
		bool flag = PlayerPrefs.GetString("TypeConnect").Equals("inet");
		PhotonView photonView = PhotonView.Get(this);
		bool flag2 = (flag ? photonView.isMine : base.GetComponent<NetworkView>().isMine);
		int num = -1;
		NetworkPlayer owner = base.GetComponent<NetworkView>().owner;
		if (flag && (bool)photonView)
		{
			num = photonView.owner.ID;
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			if ((!flag || gameObject.GetComponent<PhotonView>().owner.ID != num) && (flag || !gameObject.GetComponent<NetworkView>().owner.Equals(owner)))
			{
				continue;
			}
			GameObject playerGameObject = gameObject.GetComponent<SkinName>().playerGameObject;
			Player_move_c component = playerGameObject.GetComponent<Player_move_c>();
			GameObject gameObject2 = null;
			base.transform.position = Vector3.zero;
			if (!base.transform.GetComponent<WeaponSounds>().isMelee)
			{
				foreach (Transform item in base.transform)
				{
					if (item.gameObject.name.Equals("BulletSpawnPoint"))
					{
						gameObject2 = item.GetChild(0).gameObject;
						if (!flag2)
						{
							gameObject2.SetActive(false);
						}
						break;
					}
				}
			}
			foreach (Transform item2 in playerGameObject.transform)
			{
				item2.parent = null;
				item2.position += -Vector3.up * 1000f;
			}
			base.transform.parent = playerGameObject.transform;
			if (base.transform.Find("BulletSpawnPoint") != null)
			{
				component._bulletSpawnPoint = base.transform.Find("BulletSpawnPoint").gameObject;
			}
			base.transform.localPosition = new Vector3(0f, -1.7f, 0f);
			base.transform.rotation = playerGameObject.transform.rotation;
			GameObject gameObject3 = null;
			gameObject3 = base.transform.GetComponent<WeaponSounds>().bonusPrefab;
			GameObject[] array3 = null;
			Player_move_c.SetTextureRecursivelyFrom(stopObjs: (base.transform.GetComponent<WeaponSounds>().isMelee || !(gameObject2 != null)) ? new GameObject[3] { gameObject3, component.capesPoint, component.hatsPoint } : new GameObject[4] { gameObject3, gameObject2, component.capesPoint, component.hatsPoint }, obj: playerGameObject.transform.parent.gameObject, txt: component._skin);
		}
	}

	private void Update()
	{
	}
}
