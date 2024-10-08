using System;
using UnityEngine;

internal sealed class ZombiSynchPhoton : MonoBehaviour
{
	private ThirdPersonCamera cameraScript;

	private ThirdPersonController controllerScript;

	private PhotonView photonView;

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

	private void Start()
	{
		try
		{
			photonView = PhotonView.Get(this);
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
			stream.SendNext(base.transform.rotation);
		}
		else
		{
			correctPlayerPos = (Vector3)stream.ReceiveNext();
			correctPlayerRot = (Quaternion)stream.ReceiveNext();
		}
	}

	private void Update()
	{
		try
		{
			if (!photonView.isMine)
			{
				base.transform.position = Vector3.Lerp(base.transform.position, correctPlayerPos, Time.deltaTime * 5f);
				base.transform.rotation = Quaternion.Lerp(base.transform.rotation, correctPlayerRot, Time.deltaTime * 5f);
			}
		}
		catch (Exception exception)
		{
			Debug.LogError("Cooperative mode failure.");
			Debug.LogException(exception);
			throw;
		}
	}
}
