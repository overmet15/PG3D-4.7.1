using Photon;
using UnityEngine;

public class ThirdPersonNetwork : Photon.MonoBehaviour
{
	private ThirdPersonCamera cameraScript;

	private ThirdPersonController controllerScript;

	private bool iskilled;

	private bool oldIsKilled;

	public GameObject playerDeadPrefab;

	public Vector3 correctPlayerPos;

	public Quaternion correctPlayerRot = Quaternion.identity;

	private void Awake()
	{
		if (PlayerPrefs.GetInt("MultyPlayer") != 1 || PlayerPrefs.GetString("TypeConnect").Equals("local"))
		{
			base.enabled = false;
		}
		correctPlayerPos = new Vector3(0f, -10000f, 0f);
	}

	private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			iskilled = GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().isKilled;
			stream.SendNext(base.transform.position);
			stream.SendNext(base.transform.rotation);
			stream.SendNext(iskilled);
		}
		else
		{
			correctPlayerPos = (Vector3)stream.ReceiveNext();
			correctPlayerRot = (Quaternion)stream.ReceiveNext();
			oldIsKilled = iskilled;
			iskilled = (bool)stream.ReceiveNext();
			GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().isKilled = iskilled;
		}
	}

	private void Update()
	{
		if (base.photonView.isMine)
		{
			return;
		}
		if (iskilled)
		{
			if (!oldIsKilled)
			{
				oldIsKilled = iskilled;
				Object.Instantiate(playerDeadPrefab, base.transform.position, base.transform.rotation);
			}
			base.transform.position = new Vector3(0f, -1000f, 0f);
		}
		else if (!oldIsKilled)
		{
			base.transform.position = Vector3.Lerp(base.transform.position, correctPlayerPos, Time.deltaTime * 5f);
			base.transform.rotation = Quaternion.Lerp(base.transform.rotation, correctPlayerRot, Time.deltaTime * 5f);
		}
		else
		{
			base.transform.position = correctPlayerPos;
			base.transform.rotation = correctPlayerRot;
		}
	}
}
