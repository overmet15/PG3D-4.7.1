using UnityEngine;

public class Shoot : MonoBehaviour
{
	public float Range = 1000f;

	public Transform _transform;

	public GameObject bullet;

	private GameObject _bulletSpawnPoint;

	public int lives = 100;

	private void Start()
	{
		_bulletSpawnPoint = GameObject.Find("BulletSpawnPoint");
	}

	[RPC]
	private void Popal(NetworkViewID Popal, NetworkMessageInfo info)
	{
		Debug.Log(string.Concat(Popal, " ", base.gameObject.transform.GetComponent<NetworkView>().viewID, " ", info.sender));
	}

	public void shootS()
	{
		Debug.Log("Shot!!" + base.transform.position);
		Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f));
		RaycastHit hitInfo;
		if (!Physics.Raycast(ray, out hitInfo, 100f, -5))
		{
			return;
		}
		Debug.Log("Hit!");
		if (hitInfo.collider.gameObject.transform.CompareTag("Enemy"))
		{
			Debug.Log("popal " + hitInfo.collider.gameObject.transform.GetComponent<NetworkView>().viewID);
			if (PlayerPrefs.GetInt("MultyPlayer") == 1)
			{
				base.GetComponent<NetworkView>().RPC("Popal", RPCMode.All, hitInfo.collider.gameObject.transform.GetComponent<NetworkView>().viewID);
			}
		}
	}

	private void OnGUI()
	{
	}
}
