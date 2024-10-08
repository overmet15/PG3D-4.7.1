using UnityEngine;

public class FlashFire : MonoBehaviour
{
	public GameObject gunFlashObj;

	public float timeFireAction = 0.1f;

	private float activeTime;

	private void Start()
	{
		gunFlashObj.SetActive(false);
	}

	private void Update()
	{
		if (activeTime > 0f)
		{
			activeTime -= Time.deltaTime;
			if (activeTime <= 0f)
			{
				gunFlashObj.SetActive(false);
			}
		}
	}

	public void fire()
	{
		gunFlashObj.SetActive(true);
		activeTime = timeFireAction;
	}
}
