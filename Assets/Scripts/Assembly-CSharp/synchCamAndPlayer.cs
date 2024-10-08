using UnityEngine;

public class synchCamAndPlayer : MonoBehaviour
{
	public GameObject[] synchScript;

	private void Start()
	{
	}

	public void setSynh(bool _isActive)
	{
		GameObject[] array = synchScript;
		foreach (GameObject gameObject in array)
		{
			gameObject.SetActive(_isActive);
		}
	}

	private void Update()
	{
	}
}
