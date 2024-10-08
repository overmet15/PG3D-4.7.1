using System;
using UnityEngine;

internal sealed class ZombiManagerSynch : MonoBehaviour
{
	private ThirdPersonCamera cameraScript;

	private ThirdPersonController controllerScript;

	private Vector3 correctPlayerPos = Vector3.zero;

	private Quaternion correctPlayerRot = Quaternion.identity;

	private void Awake()
	{
		try
		{
			if (PlayerPrefs.GetInt("MultyPlayer") != 1 || PlayerPrefs.GetString("TypeConnect").Equals("local"))
			{
				base.enabled = false;
			}
		}
		catch (Exception exception)
		{
			Debug.LogError("Cooperative mode failure.");
			Debug.LogException(exception);
			throw;
		}
	}

	private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.SendNext(base.transform.position);
		}
		else
		{
			correctPlayerPos = (Vector3)stream.ReceiveNext();
		}
	}
}
