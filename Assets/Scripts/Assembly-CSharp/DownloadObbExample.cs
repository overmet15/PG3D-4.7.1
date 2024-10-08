using System.Collections;
using UnityEngine;

public sealed class DownloadObbExample : MonoBehaviour
{
	private string expPath;

	private string logtxt;

	private bool alreadyLogged;

	private string nextScene = "SceneMenu";

	private bool downloadStarted;

	public Texture2D background;

	public GUISkin mySkin;

	private void log(string t)
	{
		logtxt = logtxt + t + "\n";
		MonoBehaviour.print("MYLOG " + t);
	}

	private void OnGUI()
	{
		GUI.skin = mySkin;
		GUI.DrawTexture(new Rect(0f, 0f, background.width, background.height), background);
		if (!GooglePlayDownloader.RunningOnAndroid())
		{
			GUI.Label(new Rect(10f, 10f, Screen.width - 10, 20f), "Use GooglePlayDownloader only on Android device!");
			return;
		}
		expPath = GooglePlayDownloader.GetExpansionFilePath();
		if (expPath == null)
		{
			GUI.Label(new Rect(10f, 10f, Screen.width - 10, 20f), "External storage is not available!");
			return;
		}
		string mainOBBPath = GooglePlayDownloader.GetMainOBBPath(expPath);
		if (!alreadyLogged)
		{
			alreadyLogged = true;
			log("expPath = " + expPath);
			log("Main = " + mainOBBPath);
			log("Main = " + mainOBBPath.Substring(expPath.Length));
			if (mainOBBPath != null)
			{
				StartCoroutine(loadLevel());
			}
		}
		if (mainOBBPath == null)
		{
			GUI.Label(new Rect(Screen.width - 600, Screen.height - 230, 430f, 60f), "The game needs to download 200MB of game content. It's recommanded to use WIFI connexion.");
			if (GUI.Button(new Rect(Screen.width - 500, Screen.height - 170, 250f, 60f), "Start Download !"))
			{
				GooglePlayDownloader.FetchOBB();
				StartCoroutine(loadLevel());
			}
		}
	}

	protected IEnumerator loadLevel()
	{
		string mainPath;
		do
		{
			yield return new WaitForSeconds(0.5f);
			mainPath = GooglePlayDownloader.GetMainOBBPath(expPath);
			log("waiting mainPath " + mainPath);
		}
		while (mainPath == null);
		if (!downloadStarted)
		{
			downloadStarted = true;
			string uri = "file://" + mainPath;
			log("downloading " + uri);
			WWW www = WWW.LoadFromCacheOrDownload(uri, 0);
			yield return www;
			if (www.error != null)
			{
				log("wwww error " + www.error);
			}
			else
			{
				Application.LoadLevel(nextScene);
			}
		}
	}
}
