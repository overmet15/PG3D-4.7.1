using System;
using System.Collections;
using UnityEngine;

public class LevelCompleteLoader : MonoBehaviour
{
	public static Action action;

	public static string sceneName = string.Empty;

	public Texture fon;

	private void Start()
	{
		if (!sceneName.Equals("LevelComplete"))
		{
			fon = Resources.Load("main_loading") as Texture;
		}
		StartCoroutine(loadNext());
	}

	private IEnumerator loadNext()
	{
		yield return new WaitForSeconds(0.25f);
		Application.LoadLevel(sceneName);
	}

	private void OnGUI()
	{
		Rect position = new Rect((float)Screen.width / 2f - 1366f * Defs.Coef / 2f, 0f, 1366f * Defs.Coef, 768f * Defs.Coef);
		GUI.DrawTexture(position, fon);
	}
}
