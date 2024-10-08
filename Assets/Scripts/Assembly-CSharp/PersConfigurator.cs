using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersConfigurator : MonoBehaviour
{
	public GameObject body;

	public GameObject gun;

	private GameObject weapon;

	private GameObject _label;

	private GameObject shadow;

	private IEnumerator Start()
	{
		yield return null;
		ObjectLabel.currentCamera = Camera.main;
		GameObject ol = Resources.Load("ObjectLabel") as GameObject;
		_label = Object.Instantiate(ol) as GameObject;
		_label.GetComponent<ObjectLabel>().target = base.transform;
		_label.GetComponent<ObjectLabel>().isMenu = true;
		string nameFilter = GameObject.FindGameObjectWithTag("WeaponManager").GetComponent<FilterBadWorld>().FilterString(PlayerPrefs.GetString("NamePlayer", Defs.defaultPlayerName));
		_label.GetComponent<GUIText>().text = nameFilter;
		_label.GetComponent<GUIText>().pixelOffset = new Vector2(-85f * (float)Screen.height / 768f, 0f * (float)Screen.height / 768f);
		PlayerPrefs.SetInt("MultyPlayer", 1);
		WeaponManager weaponManager = GameObject.FindGameObjectWithTag("WeaponManager").GetComponent<WeaponManager>();
		weaponManager.Reset();
		PlayerPrefs.SetInt("MultyPlayer", 0);
		int maxCost = 0;
		GameObject pref = null;
		List<Weapon> boughtWeapons = new List<Weapon>();
		foreach (Weapon pw2 in weaponManager.playerWeapons)
        {
            if (WeaponManager.tagToStoreIDMapping == null || pw2 == null || pw2.weaponPrefab == null)
            {
                StartCoroutine(Start());
                yield break;
            }
            //else if (WeaponManager.tagToStoreIDMapping.ContainsKey(pw2.weaponPrefab.tag))
			//{
				boughtWeapons.Add(pw2);
			//}
		}
		if (boughtWeapons.Count == 0)
		{
			foreach (Weapon pw in weaponManager.playerWeapons)
			{
				if (pw.weaponPrefab.tag.Equals(WeaponManager._initialWeaponName))
				{
					pref = pw.weaponPrefab;
					break;
				}
			}
		}
		else
		{
			pref = boughtWeapons[Random.Range(0, boughtWeapons.Count)].weaponPrefab;
		}
		GameObject w = Object.Instantiate(pref) as GameObject;
		w.transform.parent = body.transform;
		weapon = w;
		weapon.transform.localPosition = Vector3.zero;
		weapon.transform.localRotation = Quaternion.identity;
		weapon.GetComponent<WeaponSounds>().animationObject.GetComponent<Animation>().Play("Profile");
		gun = w.GetComponent<WeaponSounds>().bonusPrefab;
		Texture hitTexture = SkinsManager.currentMultiplayerSkin();
		if (hitTexture != null)
		{
			hitTexture.filterMode = FilterMode.Point;
			Player_move_c.SetTextureRecursivelyFrom(base.gameObject, hitTexture, new GameObject[1] { gun });
		}
		yield return new WaitForEndOfFrame();
		_AddCapeAndHat();
	}

	private void _AddCapeAndHat()
	{
		Transform parent = GameObject.Find("Cape").transform;
		Transform parent2 = GameObject.Find("Hat").transform;
		string @string = Storager.getString(Defs.CapeEquppedSN, false);
		if (!@string.Equals(Defs.CapeNoneEqupped))
		{
			GameObject gameObject = Resources.Load(ResPath.Combine(Defs.CapesDir, @string)) as GameObject;
			if (gameObject != null)
			{
				GameObject gameObject2 = Object.Instantiate(gameObject, Vector3.zero, Quaternion.identity) as GameObject;
				gameObject2.transform.parent = parent;
				gameObject2.transform.localPosition = new Vector3(0f, -0.8f, 0f);
				gameObject2.transform.localRotation = Quaternion.identity;
				gameObject2.GetComponent<Animation>().Play("Profile");
			}
			else
			{
				Debug.LogWarning("capePrefab == null");
			}
		}
		string string2 = Storager.getString(Defs.HatEquppedSN, false);
		if (!string2.Equals(Defs.HatNoneEqupped))
		{
			GameObject gameObject3 = Resources.Load(ResPath.Combine(Defs.HatsDir, string2)) as GameObject;
			if (gameObject3 != null)
			{
				GameObject gameObject4 = Object.Instantiate(gameObject3, Vector3.zero, Quaternion.identity) as GameObject;
				gameObject4.transform.parent = parent2;
				gameObject4.transform.localPosition = Vector3.zero;
				gameObject4.transform.localRotation = Quaternion.identity;
			}
			else
			{
				Debug.LogWarning("hatPrefab == null");
			}
		}
	}

	private void Update()
	{
		Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f));
		Touch[] touches = Input.touches;
		foreach (Touch touch in touches)
		{
			RaycastHit hitInfo;
			if (touch.phase == TouchPhase.Began && Physics.Raycast(ray, out hitInfo, 1000f, -5) && hitInfo.collider.gameObject.name.Equals("MainMenu_Pers"))
			{
				PlayerPrefs.SetInt(Defs.ProfileEnteredFromMenu, 1);
				ConnectGUI.GoToProfile();
				break;
			}
		}
	}
}
