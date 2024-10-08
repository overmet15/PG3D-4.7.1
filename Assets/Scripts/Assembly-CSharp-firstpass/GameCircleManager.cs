using UnityEngine;

public class GameCircleManager : MonoBehaviour
{
	private void Awake()
	{
		base.gameObject.name = GetType().ToString();
		Object.DontDestroyOnLoad(this);
	}
}
