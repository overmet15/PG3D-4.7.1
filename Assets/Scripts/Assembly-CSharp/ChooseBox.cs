using System;
using System.Collections.Generic;
using UnityEngine;

internal sealed class ChooseBox : MonoBehaviour
{
	private Vector2 pressPoint;

	private Vector2 startPoint;

	private Vector2 pointMap;

	private bool isVozvratMap;

	private Vector2 sizeMap = new Vector2(823f, 736f);

	private bool isMoveMap;

	private bool isSetMap;

	private int selectMapIndex;

	private List<Texture> boxPreviews = new List<Texture>();

	private List<Texture> closedBoxPreviews = new List<Texture>();

	public Texture fon;

	public GUIStyle backButton;

	public GUIStyle startButton;

	private GUIStyle _disabledLabelStyle;

	private void LoadBoxPreviews()
	{
		for (int i = 0; i < LevelBox.campaignBoxes.Count; i++)
		{
			Texture item = Resources.Load(ResPath.Combine("Boxes", LevelBox.campaignBoxes[i].PreviewNAme)) as Texture;
			boxPreviews.Add(item);
			Texture item2 = Resources.Load(ResPath.Combine("Boxes", LevelBox.campaignBoxes[i].PreviewNAme + "_closed")) as Texture;
			closedBoxPreviews.Add(item2);
		}
	}

	private void UnloadBoxPreviews()
	{
		boxPreviews.Clear();
		Resources.UnloadUnusedAssets();
	}

	private void Start()
	{
		_disabledLabelStyle = new GUIStyle
		{
			alignment = TextAnchor.MiddleCenter,
			font = (Font)Resources.Load("Ponderosa"),
			fontSize = (int)(24f * Defs.Coef),
			normal = new GUIStyleState
			{
				textColor = Color.white
			}
		};
		pointMap = new Vector2((float)Screen.width / 2f, (float)Screen.height / 2f);
		LoadBoxPreviews();
		GameObject original = Resources.Load("DiffGUI") as GameObject;
		UnityEngine.Object.Instantiate(original);
	}

	private void OnDestroy()
	{
		UnloadBoxPreviews();
	}

	private int CalculateStarsLeftToOpenTheBox(int boxIndex)
	{
		if (boxIndex >= LevelBox.campaignBoxes.Count)
		{
			throw new ArgumentOutOfRangeException("boxIndex");
		}
		int num = 0;
		for (int i = 0; i < boxIndex; i++)
		{
			LevelBox levelBox = LevelBox.campaignBoxes[i];
			Dictionary<string, int> value;
			if (!CampaignProgress.boxesLevelsAndStars.TryGetValue(levelBox.name, out value))
			{
				continue;
			}
			foreach (CampaignLevel level in levelBox.levels)
			{
				int value2 = 0;
				if (value.TryGetValue(level.sceneName, out value2))
				{
					num += value2;
				}
			}
		}
		int starsToOpen = LevelBox.campaignBoxes[boxIndex].starsToOpen;
		return starsToOpen - num;
	}

	private void OnGUI()
	{
		float num = Screen.height;
		float num2 = num * 1.7786459f;
		Rect position = new Rect((float)Screen.width / 2f - num2 / 2f, 0f, num2, num);
		GUI.DrawTexture(position, fon);
		selectMap();
		Rect position2 = new Rect((float)Screen.width / 2f - (float)startButton.normal.background.width * Defs.Coef / 2f, (float)Screen.height - ((float)startButton.normal.background.height + 21f) * Defs.Coef, (float)startButton.normal.background.width * Defs.Coef, (float)startButton.normal.background.height * Defs.Coef);
		Rect position3 = new Rect(21f * Defs.Coef, (float)Screen.height - ((float)backButton.normal.background.height + 21f) * Defs.Coef, (float)backButton.normal.background.width * Defs.Coef, (float)backButton.normal.background.height * Defs.Coef);
		if ((selectMapIndex == 0 || CalculateStarsLeftToOpenTheBox(selectMapIndex) <= 0) && GUI.RepeatButton(position2, string.Empty, startButton))
		{
			GUIHelper.DrawLoading();
			int index = selectMapIndex;
			CurrentCampaignGame.boXName = LevelBox.campaignBoxes[index].name;
			Application.LoadLevel("ChooseLevel");
		}
		if (GUI.RepeatButton(position3, string.Empty, backButton))
		{
			GUIHelper.DrawLoading();
			FlurryPluginWrapper.LogEvent("Back to Main Menu");
			Application.LoadLevel(Defs.MainMenuScene);
		}
	}

	private void selectMap()
	{
		float coef = Defs.Coef;
#if !UNITY_ANDROID
		float axis = Input.GetAxis("Mouse ScrollWheel");
		if (axis > 0f)
		{
			selectMapIndex--;
		}
		else if (axis < 0f)
		{
			selectMapIndex++;
		}
		if (selectMapIndex < 0)
		{
			selectMapIndex = LevelBox.campaignBoxes.Count - 1;
		}
		if (selectMapIndex == LevelBox.campaignBoxes.Count)
		{
			selectMapIndex = 0;
		}
#endif
		float num = -0.04f;
		if (!isVozvratMap && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
		{
			pressPoint = Input.GetTouch(0).position;
			startPoint = pointMap;
			isMoveMap = true;
		}
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved && isMoveMap)
		{
			pointMap = new Vector2(startPoint.x + Input.GetTouch(0).position.x - pressPoint.x, pointMap.y);
		}
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended && isMoveMap)
		{
			isMoveMap = false;
			if (pointMap.x < (float)Screen.width * 0.3f)
			{
				selectMapIndex++;
				pointMap.x += sizeMap.x * (0.5f + num) * coef;
			}
			if (pointMap.x > (float)Screen.width * 0.7f)
			{
				selectMapIndex--;
				pointMap.x -= sizeMap.x * (0.5f + num) * coef;
			}
			if (selectMapIndex < 0)
			{
				selectMapIndex = LevelBox.campaignBoxes.Count - 1;
			}
			if (selectMapIndex == LevelBox.campaignBoxes.Count)
			{
				selectMapIndex = 0;
			}
			pointMap = new Vector2((float)Screen.width * 0.5f, pointMap.y);
		}
		if (!isMoveMap && (pointMap.x > (float)Screen.width * 0.5f + 1f || pointMap.x < (float)Screen.width * 0.5f - 1f))
		{
			isVozvratMap = true;
			float num2 = Time.deltaTime * (float)Screen.width;
			if (Mathf.Abs((float)Screen.width * 0.5f - pointMap.x) > num2)
			{
				if (pointMap.x > (float)Screen.width * 0.5f)
				{
					pointMap.x -= num2;
				}
				else
				{
					pointMap.x += num2;
				}
			}
			else
			{
				pointMap.x = (float)Screen.width * 0.5f;
				isVozvratMap = false;
			}
		}
		else
		{
			isVozvratMap = false;
		}
		int num3 = selectMapIndex - 1;
		int num4 = selectMapIndex + 1;
		if (num3 < 0)
		{
			num3 = LevelBox.campaignBoxes.Count - 1;
		}
		if (num4 == LevelBox.campaignBoxes.Count)
		{
			num4 = 0;
		}
		Func<int, string> func = (int count) => string.Format("You need {0} more star{1}\nto open this world", count, (count != 1) ? "s" : string.Empty);
		bool flag = CalculateStarsLeftToOpenTheBox(num3) <= 0;
		Texture image = ((!flag) ? (closedBoxPreviews[num3] ?? boxPreviews[num3]) : boxPreviews[num3]);
		Rect position = new Rect(pointMap.x - sizeMap.x * (1.5f + num) * coef, pointMap.y - sizeMap.y * 0.5f * coef, sizeMap.x * coef, sizeMap.y * coef);
		GUI.DrawTexture(position, image);
		if (!flag && num3 < LevelBox.campaignBoxes.Count - 1)
		{
			string text = func(CalculateStarsLeftToOpenTheBox(num3));
			position.center = new Vector2(position.center.x - 10f * coef, position.center.y + 30f * coef);
			GUI.Label(position, text, _disabledLabelStyle);
		}
		bool flag2 = CalculateStarsLeftToOpenTheBox(selectMapIndex) <= 0;
		Texture image2 = ((!flag2) ? (closedBoxPreviews[selectMapIndex] ?? boxPreviews[selectMapIndex]) : boxPreviews[selectMapIndex]);
		Rect position2 = new Rect(pointMap.x - sizeMap.x * 0.5f * coef, pointMap.y - sizeMap.y * 0.5f * coef, sizeMap.x * coef, sizeMap.y * coef);
		GUI.DrawTexture(position2, image2);
		if (!flag2 && selectMapIndex < LevelBox.campaignBoxes.Count - 1)
		{
			string text2 = func(CalculateStarsLeftToOpenTheBox(selectMapIndex));
			position2.center = new Vector2(position2.center.x - 10f * coef, position2.center.y + 30f * coef);
			GUI.Label(position2, text2, _disabledLabelStyle);
		}
		bool flag3 = CalculateStarsLeftToOpenTheBox(num4) <= 0;
		Texture image3 = ((!flag3) ? (closedBoxPreviews[num4] ?? boxPreviews[num4]) : boxPreviews[num4]);
		Rect position3 = new Rect(pointMap.x + sizeMap.x * (0.5f + num) * coef, pointMap.y - sizeMap.y * 0.5f * coef, sizeMap.x * coef, sizeMap.y * coef);
		GUI.DrawTexture(position3, image3);
		if (!flag3 && num4 < LevelBox.campaignBoxes.Count - 1)
		{
			string text3 = func(CalculateStarsLeftToOpenTheBox(num4));
			position3.center = new Vector2(position3.center.x - 10f * coef, position3.center.y + 30f * coef);
			GUI.Label(position3, text3, _disabledLabelStyle);
		}
	}
}
