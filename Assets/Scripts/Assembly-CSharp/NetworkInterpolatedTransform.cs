using UnityEngine;

public class NetworkInterpolatedTransform : MonoBehaviour
{
	private Vector3 correctPlayerPos = Vector3.zero;

	private Quaternion correctPlayerRot = Quaternion.identity;

	private void Awake()
	{
		if (PlayerPrefs.GetInt("MultyPlayer") != 1 || PlayerPrefs.GetString("TypeConnect").Equals("inet"))
		{
			base.enabled = false;
		}
	}

	private void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		if (stream.isWriting)
		{
			Vector3 value = base.transform.localPosition;
			Quaternion value2 = base.transform.localRotation;
			stream.Serialize(ref value);
			stream.Serialize(ref value2);
		}
		else
		{
			Vector3 value3 = Vector3.zero;
			Quaternion value4 = Quaternion.identity;
			stream.Serialize(ref value3);
			stream.Serialize(ref value4);
			correctPlayerPos = value3;
			correctPlayerRot = value4;
		}
	}

	private void Update()
	{
		if (PlayerPrefs.GetString("TypeConnect").Equals("local") && !base.GetComponent<NetworkView>().isMine)
		{
			base.transform.position = Vector3.Lerp(base.transform.position, correctPlayerPos, Time.deltaTime * 5f);
			base.transform.rotation = Quaternion.Lerp(base.transform.rotation, correctPlayerRot, Time.deltaTime * 5f);
		}
	}
}
