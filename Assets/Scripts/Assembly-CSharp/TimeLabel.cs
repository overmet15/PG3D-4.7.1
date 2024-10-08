using UnityEngine;

public class TimeLabel : MonoBehaviour
{
	private UILabel _label;

	private InGameGUI _inGameGUI;

	private void Start()
	{
		base.gameObject.SetActive(PlayerPrefs.GetInt("COOP", 0) == 1);
		_label = GetComponent<UILabel>();
		_inGameGUI = GameObject.FindObjectOfType<InGameGUI>();
	}

	private void Update()
	{
		if ((bool)_inGameGUI && (bool)_label)
		{
			base.transform.localScale = new Vector3(22f, 22f, 1f);
			_label.text = _inGameGUI.timeLeft();
		}
	}
}
