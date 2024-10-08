using UnityEngine;

public class ElixirSprite : MonoBehaviour
{
	private UISprite spr;

	private void Start()
	{
		bool flag = PlayerPrefs.GetInt("MultyPlayer", 0) == 0;
		base.gameObject.SetActive(flag);
		if (flag)
		{
			spr = GetComponent<UISprite>();
			spr.enabled = false;
		}
	}

	private void Update()
	{
	}
}
