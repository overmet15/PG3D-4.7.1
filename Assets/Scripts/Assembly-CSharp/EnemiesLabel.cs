using UnityEngine;

public class EnemiesLabel : MonoBehaviour
{
	private UILabel _label;

	private ZombieCreator _zombieCreator;

	private void Start()
	{
		bool flag = PlayerPrefs.GetInt("MultyPlayer", 0) == 0;
		base.gameObject.SetActive(flag);
		if (flag)
		{
			UIRoot uIRoot = NGUITools.FindInParents<UIRoot>(base.gameObject);
			float num = (float)uIRoot.manualHeight * ((float)Screen.width / (float)Screen.height);
			InGameGUI component = GameObject.FindObjectOfType<InGameGUI>();
			GameObject gameObject = component.hearts[component.hearts.Length - 1];
			float num2 = gameObject.transform.localPosition.x + gameObject.transform.localScale.x / 2f;
			float num3 = num - 131f - 128f;
			float num4 = (num3 - num2) / 3f;
			float num5 = (num3 - num2) / 2f;
			if (PlayerPrefs.GetInt("MultyPlayer", 0) == 0 && Defs.IsSurvival)
			{
				base.transform.localPosition = new Vector3(0f - (num - (num2 + num4 * 2f)), base.transform.localPosition.y, base.transform.localPosition.z);
			}
			else if (PlayerPrefs.GetInt("MultyPlayer", 0) == 0 && !Defs.IsSurvival)
			{
				base.transform.localPosition = new Vector3(0f - (num - (num2 + num5)), base.transform.localPosition.y, base.transform.localPosition.z);
			}
			_label = GetComponent<UILabel>();
			_zombieCreator = FindObjectOfType<ZombieCreator>();
		}
	}

	private void Update()
	{
		base.transform.localScale = new Vector3(22f, 22f, 1f);
		_label.text = "Enemies\n" + (ZombieCreator.NumOfEnemisesToKill - _zombieCreator.NumOfDeadZombies);
	}
}
