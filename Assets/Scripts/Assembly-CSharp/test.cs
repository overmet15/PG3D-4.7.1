using UnityEngine;

public class test : MonoBehaviour
{
	private string _tName = "ttt";

	private bool _drawTexture;

	private bool _browseIsShown;

	private Texture2D t;

	private WebViewObject _wvo;

	private float bottomPnaelHeight = Screen.height / 8;

	private void Start()
	{
	}

	private void OnGUI()
	{
		if (!_browseIsShown)
		{
			if (GUI.Button(new Rect(0f, 0f, Screen.width / 3, Screen.height), "Save"))
			{
				Texture2D texture2D = (Texture2D)Resources.Load("txt");
				SkinsManager.SaveTextureWithName(texture2D, _tName);
			}
			if (GUI.Button(new Rect(Screen.width / 3, 0f, Screen.width / 3, Screen.height), "Load"))
			{
				t = SkinsManager.TextureForName(_tName);
				_drawTexture = true;
			}
			if (_drawTexture)
			{
				GUI.DrawTexture(new Rect(Screen.width / 3, 0f, Screen.width / 3, Screen.height), t);
			}
			if (GUI.Button(new Rect(Screen.width * 2 / 3, 0f, Screen.width / 3, Screen.height), "Browser"))
			{
				_wvo = WebViewStarter.StartBrowser("http://minecraft.net/login");
				_wvo.SetMargins(0, 0, 0, (int)bottomPnaelHeight);
				_browseIsShown = true;
			}
		}
		else if (GUI.Button(new Rect(0f, (float)Screen.height - bottomPnaelHeight, Screen.width / 2, bottomPnaelHeight), "Back"))
		{
			_browseIsShown = false;
			Object.Destroy(_wvo.gameObject);
			_wvo = null;
		}
	}
}
