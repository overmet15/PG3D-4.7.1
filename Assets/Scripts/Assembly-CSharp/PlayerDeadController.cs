using UnityEngine;

public class PlayerDeadController : MonoBehaviour
{
	private void Start()
	{
		Invoke("RemoveMyObject", 4.8f);
	}

	private void Update()
	{
	}

	private void RemoveMyObject()
	{
		Object.Destroy(base.gameObject);
	}
}
