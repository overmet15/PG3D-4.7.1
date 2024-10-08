using UnityEngine;

public class Bullet : MonoBehaviour
{
	private float LifeTime = 1f;

	private float RespawnTime;

	public float bulletSpeed = 200f;

	public float lifeS = 500f;

	public Vector3 startPos;

	public Vector3 endPos;

	private void Start()
	{
		if (PlayerPrefs.GetInt("MultyPlayer") != 1)
		{
			Invoke("RemoveSelf", LifeTime);
		}
		startPos = base.transform.position;
	}

	private void RemoveSelf()
	{
		Object.Destroy(base.gameObject);
	}

	public float GetDistance(Vector3 vectorA, Vector3 vectorB)
	{
		return Mathf.Sqrt((vectorA.x - vectorB.x) * (vectorA.x - vectorB.x) + (vectorA.y - vectorB.y) * (vectorA.y - vectorB.y) + (vectorA.z - vectorB.z) * (vectorA.z - vectorB.z));
	}

	private void Update()
	{
		PhotonView photonView = null;
		if (base.GetComponent<NetworkView>() == null)
		{
			return;
		}
		if (PlayerPrefs.GetInt("MultyPlayer") != 1 || (PlayerPrefs.GetInt("MultyPlayer") == 1 && ((PlayerPrefs.GetString("TypeConnect").Equals("local") && base.GetComponent<NetworkView>().isMine) || PlayerPrefs.GetString("TypeConnect").Equals("inet"))))
		{
			base.transform.position += base.transform.forward * bulletSpeed * Time.deltaTime;
		}
		if (PlayerPrefs.GetInt("MultyPlayer") == 1 && ((PlayerPrefs.GetString("TypeConnect").Equals("local") && base.GetComponent<NetworkView>().isMine) || PlayerPrefs.GetString("TypeConnect").Equals("inet")) && GetDistance(startPos, base.transform.position) >= lifeS)
		{
			if (PlayerPrefs.GetString("TypeConnect").Equals("local"))
			{
				Network.Destroy(base.gameObject);
			}
			else
			{
				Object.Destroy(base.gameObject);
			}
		}
	}
}
