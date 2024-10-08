using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelArt : MonoBehaviour
{
	public GUIStyle startButton;

	public static bool endOfBox;

	private bool _shoBut;

	private float _alpha;

	private float interval = 1.5f;

	private int curTexture;

	private int _artsOnScreen = 4;

	private bool _firstLaunch = true;

	private bool _skip;

	private int _numOfArts = 4;

	private Texture fon;

	private List<Texture> _textures = new List<Texture>();

	private bool _showButton;

	private void Start()
	{
		if (Resources.Load(_NameForNumber(5)) as Texture != null)
		{
			_numOfArts *= 2;
		}
		StartCoroutine("ShowArts");
		fon = Resources.Load("Arts_background_" + CurrentCampaignGame.boXName) as Texture;
		if (endOfBox)
		{
			string[] array = Load.LoadStringArray(Defs.ArtBoxS);
			if (array == null)
			{
				array = new string[0];
			}
			string[] array2 = array;
			foreach (string text in array2)
			{
				if (text.Equals(CurrentCampaignGame.boXName))
				{
					_firstLaunch = false;
					break;
				}
			}
		}
		else
		{
			string[] array3 = Load.LoadStringArray(Defs.ArtLevsS);
			if (array3 == null)
			{
				array3 = new string[0];
			}
			string[] array4 = array3;
			foreach (string text2 in array4)
			{
				if (text2.Equals(CurrentCampaignGame.levelSceneName))
				{
					_firstLaunch = false;
					break;
				}
			}
		}
		_showButton = !_firstLaunch;
	}

	private void GoToLevel()
	{
		if (endOfBox)
		{
			string[] array = Load.LoadStringArray(Defs.ArtBoxS);
			if (array == null)
			{
				array = new string[0];
			}
			if (Array.IndexOf(array, CurrentCampaignGame.boXName) == -1)
			{
				List<string> list = new List<string>();
				string[] array2 = array;
				foreach (string item in array2)
				{
					list.Add(item);
				}
				list.Add(CurrentCampaignGame.boXName);
				Save.SaveStringArray(Defs.ArtBoxS, list.ToArray());
			}
		}
		else
		{
			string[] array3 = Load.LoadStringArray(Defs.ArtLevsS);
			if (array3 == null)
			{
				array3 = new string[0];
			}
			if (!endOfBox && Array.IndexOf(array3, CurrentCampaignGame.levelSceneName) == -1)
			{
				List<string> list2 = new List<string>();
				string[] array4 = array3;
				foreach (string item2 in array4)
				{
					list2.Add(item2);
				}
				list2.Add(CurrentCampaignGame.levelSceneName);
				Save.SaveStringArray(Defs.ArtLevsS, list2.ToArray());
			}
		}
		Application.LoadLevel((!endOfBox) ? "CampaignLoading" : "ChooseLevel");
	}

	private string _NameForNumber(int num)
	{
		return ResPath.Combine("Arts", ResPath.Combine((!endOfBox) ? CurrentCampaignGame.levelSceneName : CurrentCampaignGame.boXName, num.ToString()));
	}

	private IEnumerator ShowArts()
	{
		string p = string.Empty;
		Texture newT = null;
		do
		{
			newT = null;
			curTexture++;
			p = _NameForNumber(curTexture);
			try
			{
				newT = Resources.Load(p) as Texture;
			}
			catch
			{
			}
			if (newT != null)
			{
				if (_textures.Count == _artsOnScreen)
				{
					_textures.Clear();
				}
				_textures.Add(newT);
				Resources.UnloadUnusedAssets();
				_alpha = 0f;
				float prevTime = Time.time;
				float startTime = Time.time;
				do
				{
					yield return new WaitForEndOfFrame();
					_alpha += (Time.time - prevTime) / interval;
					prevTime = Time.time;
				}
				while (Time.time - startTime < interval && !_skip);
				_skip = false;
				_alpha = 1f;
				continue;
			}
			GoToLevel();
			yield break;
		}
		while (newT != null && curTexture % _artsOnScreen != 0);
		yield return new WaitForSeconds(interval);
		_showButton = true;
	}

	private void OnGUI()
	{
		float num = Screen.height;
		float num2 = num * 1.7786459f;
		Rect position = new Rect((float)Screen.width / 2f - num2 / 2f, 0f, num2, num);
		GUI.DrawTexture(position, fon);
		for (int i = 0; i < _textures.Count; i++)
		{
			Rect position2;
			switch (i % _artsOnScreen)
			{
			case 0:
				position2 = new Rect(position.x, position.y, (float)_textures[i].width * Defs.Coef, (float)_textures[i].height * Defs.Coef);
				break;
			case 1:
				position2 = new Rect(position.x + position.width - (float)_textures[i].width * Defs.Coef, position.y, (float)_textures[i].width * Defs.Coef, (float)_textures[i].height * Defs.Coef);
				break;
			case 2:
				position2 = new Rect(position.x, position.y + position.height - (float)_textures[i].height * Defs.Coef, (float)_textures[i].width * Defs.Coef, (float)_textures[i].height * Defs.Coef);
				break;
			default:
				position2 = new Rect(position.x + position.width - (float)_textures[i].width * Defs.Coef, position.y + position.height - (float)_textures[i].height * Defs.Coef, (float)_textures[i].width * Defs.Coef, (float)_textures[i].height * Defs.Coef);
				break;
			}
			Color color = GUI.color;
			if (i == _textures.Count - 1)
			{
				GUI.color = new Color(1f, 1f, 1f, _alpha);
			}
			GUI.DrawTexture(position2, _textures[i]);
			if (i == _textures.Count - 1)
			{
				GUI.color = color;
			}
		}
		if (!_showButton)
		{
			return;
		}
		Rect position3 = new Rect((float)Screen.width - ((float)startButton.normal.background.width + 21f) * Defs.Coef, (float)Screen.height - ((float)startButton.normal.background.height + 21f) * Defs.Coef, (float)startButton.normal.background.width * Defs.Coef, (float)startButton.normal.background.height * Defs.Coef);
		GUI.depth = -3;
		if (GUI.Button(position3, string.Empty, startButton))
		{
			_showButton = !_firstLaunch;
			if (_numOfArts > _artsOnScreen && curTexture <= _artsOnScreen)
			{
				StopCoroutine("ShowArts");
				_alpha = 1f;
				curTexture = _artsOnScreen;
				_textures.Clear();
				StartCoroutine("ShowArts");
			}
			else
			{
				GoToLevel();
			}
		}
	}
}
