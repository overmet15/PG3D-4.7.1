using UnityEngine;

public class ScoresLabel : MonoBehaviour
{
	private UILabel _label;

	private void Start()
	{
		base.gameObject.SetActive(Defs.IsSurvival || PlayerPrefs.GetInt("COOP", 0) == 1);
		if (PlayerPrefs.GetInt("MultyPlayer", 0) == 0)
		{
			UIRoot uIRoot = NGUITools.FindInParents<UIRoot>(base.gameObject);
			float num = (float)uIRoot.manualHeight * ((float)Screen.width / (float)Screen.height);
			InGameGUI component = GameObject.FindObjectOfType<InGameGUI>();
			GameObject gameObject = component.hearts[component.hearts.Length - 1];
			float num2 = gameObject.transform.localPosition.x + gameObject.transform.localScale.x / 2f;
			float num3 = num - 131f - 128f;
			float num4 = (num3 - num2) / 3f;
			base.transform.localPosition = new Vector3(0f - (num - (num2 + num4)), base.transform.localPosition.y, base.transform.localPosition.z);
		}
		_label = GetComponent<UILabel>();
	}

	private void Update()
	{
		base.transform.localScale = new Vector3(22f, 22f, 1f);
		_label.text = "Score\n" + GlobalGameController.Score;
	}
}
