using UnityEngine;

public class CountKillsCommandBlue : MonoBehaviour
{
	private UILabel _label;

	private bool isAmBlueCommandLabel;

	public WeaponManager _weaponManager;

	private void Start()
	{
		base.gameObject.SetActive(PlayerPrefs.GetInt("MultyPlayer", 0) == 1 && PlayerPrefs.GetInt("company", 0) == 1);
		if (PlayerPrefs.GetInt("MultyPlayer", 0) == 1 && PlayerPrefs.GetInt("company", 0) == 1)
		{
			isAmBlueCommandLabel = base.gameObject.name.Equals("CountKillsBlueLabel");
			_weaponManager = GameObject.FindGameObjectWithTag("WeaponManager").GetComponent<WeaponManager>();
			UIRoot uIRoot = NGUITools.FindInParents<UIRoot>(base.gameObject);
			float num = (float)uIRoot.manualHeight * ((float)Screen.width / (float)Screen.height);
			InGameGUI component = GameObject.FindGameObjectWithTag("InGameGUI").GetComponent<InGameGUI>();
			GameObject gameObject = component.hearts[component.hearts.Length - 1];
			float num2 = gameObject.transform.localPosition.x + gameObject.transform.localScale.x / 2f;
			float num3 = num - 131f - 128f - 72f - 72f;
			float num4 = (num3 - num2) / 3f;
			float num5 = ((isAmBlueCommandLabel != (_weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().myCommand == 1)) ? 2.1f : 0.9f);
			base.transform.localPosition = new Vector3(0f - (num - (num2 + num4 * num5)), base.transform.localPosition.y, base.transform.localPosition.z);
			_label = GetComponent<UILabel>();
		}
	}

	private void Update()
	{
		base.transform.localScale = new Vector3(22f, 22f, 1f);
		if ((bool)_weaponManager && (bool)_weaponManager.myPlayer && PhotonNetwork.room != null)
		{
			if (isAmBlueCommandLabel)
			{
				_label.text = "Blue\n" + _weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().countKillsCommandBlue + "/" + int.Parse(PhotonNetwork.room.customProperties["MaxKill"].ToString());
			}
			else
			{
				_label.text = "Red\n" + _weaponManager.myPlayer.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>().countKillsCommandRed + "/" + int.Parse(PhotonNetwork.room.customProperties["MaxKill"].ToString());
			}
		}
	}
}
