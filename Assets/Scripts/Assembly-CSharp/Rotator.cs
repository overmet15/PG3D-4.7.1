using UnityEngine;

public class Rotator : MonoBehaviour
{
	public GameObject playerGun;

	private void Update()
	{
		playerGun.transform.rotation = base.transform.rotation;
	}
}
