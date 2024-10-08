using UnityEngine;

public class KillsLabel : MonoBehaviour
{
	private UILabel _label;

	private InGameGUI _inGameGUI;

	private void Start()
	{
		base.gameObject.SetActive(PlayerPrefs.GetInt("MultyPlayer", 0) == 1 && PlayerPrefs.GetInt("COOP", 0) == 0 && PlayerPrefs.GetInt("company", 0) == 0);
		_label = GetComponent<UILabel>();
		_inGameGUI = GameObject.FindObjectOfType<InGameGUI>();
	}

	private void Update()
	{
		if ((bool)_inGameGUI && (bool)_label)
		{
			base.transform.localScale = new Vector3(22f, 22f, 1f);
			if (_inGameGUI != null)
			{
				_label.text = _inGameGUI.killsToMaxKills();
			}
		}
	}
}
