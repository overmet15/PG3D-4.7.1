using UnityEngine;

public class CampaignLevel
{
	public string sceneName;

	public float timeToComplete = 300f;

	private Vector3 _localPosition = Vector3.forward;

	public Vector3 LocalPosition
	{
		get
		{
			return _localPosition;
		}
		set
		{
			_localPosition = value;
		}
	}
}
