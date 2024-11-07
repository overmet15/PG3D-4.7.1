using UnityEngine;

public class ChatViewrController : MonoBehaviour
{
	public GameObject PlayerObject;

	private TouchScreenKeyboard mKeyboard;

	public GameObject labelChat;

	private string mText = string.Empty;

	public int maxChars = 40;

	public WeaponManager _weaponManager;

	public GameObject holdButton;

	public GameObject holdButtonOn;

	public bool isHold;

	public AudioClip sendChatClip;

	public void clickButton(string nameButton)
	{
		Debug.Log(nameButton);
		if (nameButton.Equals("CloseChatButton"))
		{
			closeChat();
		}
		if (nameButton.Equals("HoldChatButton"))
		{
			Debug.Log("HoldChatButton");
			isHold = true;
			holdButton.SetActive(false);
			holdButtonOn.SetActive(true);
		}
		if (nameButton.Equals("HoldChatButtonOn"))
		{
			Debug.Log("HoldChatButtonOn");
			isHold = false;
			holdButton.SetActive(true);
			holdButtonOn.SetActive(false);
		}
		if (nameButton.Equals("KeyboardButton"))
		{
			Debug.Log("KeyboardButton");
			mKeyboard.active = true;
		}
	}

	public void postChat(string _text)
	{
		Debug.Log("post " + _text);
		if (PlayerPrefsX.GetBool(PlayerPrefsX.SoundFXSetting, true))
		{
			NGUITools.PlaySound(sendChatClip);
		}
		PlayerObject.GetComponent<Player_move_c>().SendChat(_text);
	}

	public void closeChat()
	{
		mKeyboard.active = false;
		mKeyboard = null;
		PlayerObject.GetComponent<Player_move_c>().showChat = false;
		Object.Destroy(base.gameObject);
	}

	private void OnDestroy()
	{
		if (mKeyboard != null)
		{
			mKeyboard.active = false;
			mKeyboard = null;
		}
	}

	private void Start()
	{
		_weaponManager = GameObject.FindGameObjectWithTag("WeaponManager").GetComponent<WeaponManager>();
		mKeyboard = TouchScreenKeyboard.Open(string.Empty, TouchScreenKeyboardType.Default, false, false);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape)) closeChat();
		labelChat.GetComponent<UILabel>().text = string.Empty;
		if (PlayerObject == null)
		{
			return;
		}
		for (int num = PlayerObject.GetComponent<Player_move_c>().messages.Count - 1; num >= 0; num--)
		{
			string text = "[00FF26]";
			if ((PlayerPrefs.GetString("TypeConnect").Equals("local") && PlayerObject.GetComponent<Player_move_c>().messages[num].IDLocal == _weaponManager.myPlayer.GetComponent<NetworkView>().viewID) || (PlayerPrefs.GetString("TypeConnect").Equals("inet") && PlayerObject.GetComponent<Player_move_c>().messages[num].ID == _weaponManager.myPlayer.GetComponent<PhotonView>().viewID))
			{
				text = "[00FF26]";
			}
			else
			{
				if (PlayerObject.GetComponent<Player_move_c>().messages[num].command == 0)
				{
					text = "[FFFF26]";
				}
				if (PlayerObject.GetComponent<Player_move_c>().messages[num].command == 1)
				{
					text = "[0000FF]";
				}
				if (PlayerObject.GetComponent<Player_move_c>().messages[num].command == 2)
				{
					text = "[FF0000]";
				}
			}
			UILabel component = labelChat.GetComponent<UILabel>();
			component.text = component.text + text + PlayerObject.GetComponent<Player_move_c>().messages[num].text + "\n";
		}
		if (mKeyboard == null)
		{
			return;
		}
		string text2 = mKeyboard.text;
		if (mText != text2)
		{
			mText = string.Empty;
			foreach (char c in text2)
			{
				if (c != 0)
				{
					mText += c;
				}
			}
			if (maxChars > 0 && mKeyboard.text.Length > maxChars)
			{
				mKeyboard.text = mKeyboard.text.Substring(0, maxChars);
			}
			if (mText != text2)
			{
				mKeyboard.text = mText;
			}
		}
		if (mKeyboard.done && !mKeyboard.wasCanceled)
		{
			Debug.Log("pressDone " + mText);
			if (isHold)
			{
				mKeyboard.active = true;
			}
			else
			{
				closeChat();
			}
			if (!mText.Equals(string.Empty))
			{
				postChat(mText);
			}
		}
		else if (mKeyboard.wasCanceled)
		{
			Debug.Log("close");
			if (!isHold)
			{
				closeChat();
			}
		}
	}
}
