using System.Collections;
using UnityEngine;

public sealed class AppsMenu : MonoBehaviour
{
	public Texture fon;

	public Texture pixlgun3d;

	public Texture man;

	public Texture androidFon;

	public GUIStyle shooter;

	public GUIStyle skinsmaker;

	private string expPath = string.Empty;

	private string logtxt;

	private bool downloadStarted;

	private bool ApplicationBinarySplitted
	{
		get
		{
			//return Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite || Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GooglePro;
			return false;
		}
	}

	private void Update()
	{
		if (Application.platform == RuntimePlatform.Android && Input.GetKey(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

	private void Start()
	{
		if (!Application.isEditor)
		{
            //expPath = GooglePlayDownloader.GetExpansionFilePath();
        }
        Application.LoadLevel("Loading");
    }

	private void LoadLoading()
	{
		GlobalGameController.currentLevel = -1;
		Application.LoadLevel("Loading");
	}

	private void log(string t)
	{
		logtxt = logtxt + t + "\n";
		MonoBehaviour.print("MYLOG " + t);
	}

	private void OnGUI()
	{
		Rect position = new Rect(((float)Screen.width - 2048f * (float)Screen.height / 1154f) / 2f, 0f, 2048f * (float)Screen.height / 1154f, Screen.height);
		GUI.DrawTexture(position, androidFon, ScaleMode.StretchToFill);
		/*if (!Application.isEditor)
		{
			if (!GooglePlayDownloader.RunningOnAndroid())
			{
				GUI.Label(new Rect(10f, 10f, Screen.width - 10, 20f), "Use GooglePlayDownloader only on Android device!");
			}
			else if (string.IsNullOrEmpty(expPath))
			{
				GUI.Label(new Rect(10f, 10f, Screen.width - 10, 20f), "External storage is not available!");
			}
		}*/
	}
}
