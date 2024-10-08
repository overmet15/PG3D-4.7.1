using System;
using System.Text;
using UnityEngine;

public sealed class Controller : MonoBehaviour
{
	public static string[] SkinMaker_arrVremTitle = new string[16]
	{
		"Harlem Boy", "Slender-Man", "Star Capitan", "Bear", "Boy", "Girl", "Vampire", "Zombie", "Knight", "Alien",
		"Man in black", "Super hero", "Robot", "Fiance", "Bridge", "Ninja"
	};

	private static float koefMashtab = (float)Screen.height / 768f;

	public bool showEnabled;

	private SpisokSkinov spisokSkinovContoller;

	private ViborChastiTela viborChastiTelaController;

	public ArrayListWrapper arrNameSkin;

	public ArrayListWrapper arrTitleSkin;

	public GameObject objPeople;

	public PreviewController previewControl;

	public Texture2D fon;

	public Texture2D title;

	public Texture2D plashkaNiz;

	public Texture2D plashkaInfo;

	public Texture2D palshkaAbout;

	public GUIStyle butCreateNew;

	public GUIStyle butPresets;

	public GUIStyle butUpload;

	public GUIStyle butAbout;

	public GUIStyle butBack;

	public GUIStyle butInfo;

	public GUIStyle textInfo;

	public GUIStyle backBut;

	public GUIStyle textBrowserInfo;

	public GUIStyle stButHome;

	private Rect rectCreateNew;

	private Rect rectPresets;

	private Rect rectUpload;

	private Rect rectAbout;

	private bool _browseIsShown;

	private WebViewObject _wvo;

	private float bottomPnaelHeight = Screen.height / 8;

	private bool infoActive;

	private bool aboutActive;

	private Rect rectInfo;

	private Rect rectScroll;

	private Vector2 scrollPosition;

	private bool exceptionCatched;

	public GUISkin optionsSkin;

	public static string SkinMaker_folderName = "Skins";

	public static string SkinMaker_baseName = "Skin_";

	public static string SkinMaker_MultUser_baseName = "User_Multi_Skin_";

	private string txtAbout = "\"Presets\" section.\nHere you can choose favorite skin from the presets collection and edit it. Also you can save it to the gallery.\n\n\"Create New\" section.\nHere you can make your own skin and save it to the \"Presets\" section.\n\n\"Upload\" section.\nHere you can upload skin to your Mojang profile.\n\n     Why does this app need access to your pictures (If you use iOS 6):\nIt needs access in order to save new skins in your photo album. This is required by the device's operating system and it's the only way to save skins on your device. App will not delete, read or use your private photos. Why does this app need access to your location.\n(If you use iOS 5):\nThis is required for creating an Album for your new skins. Albums are often associated with locations when you are taking pictures and this is an automatic setting in the operating system for creating of albums. If you do not give to Pixel Gun 3D access to location,\n     the skins will be saved to Camera Roll. If you declined giving access  to  your photos ,you will not be able to save new skins to the separate album. To give to this app access to Photos, go to SETTINGS on your device. Select PRIVACY and then PHOTOS. Turn the switch next to Pixel Gun 3D - ON.";

	private string txtInfo = "To upload skin:\n\n - Log in Majong account (app doesn't have access to your data about username and password)\n\n- Click \"HOME\" button on the lower panel\n\n- Click on the \"Choose file\", then on \"Choose Existing\" and select the skin from folder \"Pixel Gun 3D Skins\" in the gallery.\n\n- Click \"Upload\" button.";

	public static int IndexBaseForUserMultiSkins
	{
		get
		{
			return 100000;
		}
	}

	public static string arrNameSkin_sett
	{
		get
		{
			return (PlayerPrefs.GetInt(Defs.SkinEditorMode, 0) != 0) ? "Mult_arrNameSkin" : "arrNameSkin";
		}
	}

	public static string arrTitleSkin_sett
	{
		get
		{
			return (PlayerPrefs.GetInt(Defs.SkinEditorMode, 0) != 0) ? "Mult_arrTitleSkin" : "arrTitleSkin";
		}
	}

	public static string nomSkinForSave_sett
	{
		get
		{
			return (PlayerPrefs.GetInt(Defs.SkinEditorMode, 0) != 0) ? "Mult_nomSkinForSave" : "nomSkinForSave";
		}
	}

	private static string InitializeAboutText()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("\"Presets\" section.");
		stringBuilder.AppendLine("Here you can choose favorite skin from the presets collection and edit it. Also you can save it to the gallery.\n");
		stringBuilder.AppendLine("\"Create New\" section.");
		stringBuilder.AppendLine("Here you can make your own skin and save it to the \"Presets\" section.\n");
		stringBuilder.AppendLine("\"Upload\" section.");
		stringBuilder.AppendLine("Here you can upload skin to your Mojang profile.\n");
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			stringBuilder.AppendLine("     Why does this app need access to your pictures (If you use iOS 6):\nIt needs access in order to save new skins in your photo album. This is required by the device's operating system and it's the only way to save skins on your device. App will not delete, read or use your private photos. Why does this app need access to your location.\n(If you use iOS 5):\nThis is required for creating an Album for your new skins. Albums are often associated with locations when you are taking pictures and this is an automatic setting in the operating system for creating of albums. If you do not give to Pixel Gun 3D access to location,\n     the skins will be saved to Camera Roll. If you declined giving access  to  your photos ,you will not be able to save new skins to the separate album. To give to this app access to Photos, go to SETTINGS on your device. Select PRIVACY and then PHOTOS. Turn the switch next to Pixel Gun 3D - ON.");
		}
		return stringBuilder.ToString();
	}

	private void WriteSkinsToGallery()
	{
        Texture2D[] array = Resources.LoadAll<Texture2D>(SkinMaker_folderName);
		for (int i = 0; i < array.Length; i++)
		{
			string nm = SkinMaker_baseName + i;
			SkinsManager.SaveTextureToGallery(array[i], nm);
		}
	}

	private void PrepareSkins(string folderName, string baseName, string arrNameSkin_sett, string arrTitleSkin_sett, string CreateSpisokSkinov_sett, string nomSkinForSave_sett, string[] arrVremTitle)
	{
		if (PlayerPrefs.GetInt(Defs.SkinEditorMode, 0) == 0)
		{
			Debug.Log("folderName: " + folderName + "\nbaseName: " + baseName + "\narrNameSkin_sett: " + arrNameSkin_sett + "\narrTitleSkin_sett: " + arrTitleSkin_sett + "\nCreateSpisokSkinov_sett " + CreateSpisokSkinov_sett + " = " + Load.LoadBool(CreateSpisokSkinov_sett) + "\nnomSkinForSave_sett: " + nomSkinForSave_sett + "\narrVremTitle:" + arrVremTitle);
		}
		if (PlayerPrefs.GetInt(Defs.SkinEditorMode, 0) == 1)
		{
			object[] array = Resources.LoadAll(folderName);
			PlayerPrefs.SetInt(Defs.NumOfMultSkinsSett, array.Length);
		}
		if (!Load.LoadBool(CreateSpisokSkinov_sett))
		{
			Texture2D[] array2 = Resources.LoadAll<Texture2D>(folderName);
			if (PlayerPrefs.GetInt(Defs.SkinEditorMode, 0) == 0)
			{
				Debug.Log("arrTextur.length: " + array2.Length + "\narrTextur: " + array2);
			}
			if (PlayerPrefs.GetInt(Defs.SkinEditorMode, 0) == 1)
			{
				NumericComparer comparer = new NumericComparer();
				Array.Sort(array2, comparer);
			}
			for (int i = 0; i < array2.Length; i++)
			{
				string text = baseName + i;
				arrTitleSkin.Add(arrVremTitle[i]);
				arrNameSkin.Add(text);
				try
                {
                    SkinsManager.SaveTextureWithName(array2[i], text, false);
                }
				catch (Exception ex)
				{
					Debug.Log(ex);
				}
			}
			string[] variable = arrNameSkin.ToArray(typeof(string)) as string[];
			Debug.Log("arrStringNameSkin");
			Save.SaveStringArray(arrNameSkin_sett, variable);
			Debug.Log("arrVremTitle");
			Save.SaveStringArray(arrTitleSkin_sett, arrVremTitle);
			spisokSkinovContoller.arrNameSkin = arrNameSkin;
			spisokSkinovContoller.arrTitleSkin = arrTitleSkin;
			Save.SaveInt(nomSkinForSave_sett, arrTitleSkin.Count);
			Save.SaveBool(CreateSpisokSkinov_sett, true);
			return;
		}
		string[] array3 = Load.LoadStringArray(arrNameSkin_sett);
		if (array3.Length > 100)
		{
			Debug.LogWarning("1");
		}
		if (arrNameSkin.Count > 100)
		{
			Debug.LogWarning("2");
		}
		if (array3 != null)
		{
			string[] array4 = array3;
			foreach (string text2 in array4)
			{
				if (!arrNameSkin.Contains(text2))
				{
					Debug.Log("ADDED: " + text2);
					arrNameSkin.Add(text2);
				}
				else
				{
					Debug.LogWarning("DUPLICATE: " + text2);
				}
			}
		}
		spisokSkinovContoller.arrNameSkin = arrNameSkin;
		string[] array5 = Load.LoadStringArray(arrTitleSkin_sett);
		if (array5 != null)
		{
			string[] array6 = array5;
			foreach (string item in array6)
			{
				arrTitleSkin.Add(item);
			}
		}
		spisokSkinovContoller.arrTitleSkin = arrTitleSkin;
		if (PlayerPrefs.GetInt(Defs.SkinEditorMode, 0) == 0)
		{
			Debug.Log("arrTitleSkin.length: " + arrTitleSkin.Count + "\narrTitleSkin: " + arrTitleSkin);
			Debug.Log("arrNameSkin.length: " + arrNameSkin.Count + "\narrNameSkin: " + arrNameSkin);
		}
	}

	private void Awake()
	{
		objPeople = GameObject.Find("PreviewObject");
		previewControl = objPeople.GetComponent<PreviewController>();
		objPeople.active = false;
		spisokSkinovContoller = GetComponent<SpisokSkinov>();
		viborChastiTelaController = GetComponent<ViborChastiTela>();
		optionsSkin.verticalScrollbar.fixedWidth = 35f * koefMashtab;
		optionsSkin.verticalScrollbarThumb.fixedWidth = 35f * koefMashtab;
		textInfo.fontSize = Mathf.RoundToInt(30f * koefMashtab);
		rectInfo = new Rect((float)Screen.width * 0.5f - (float)plashkaInfo.width * 0.5f * koefMashtab, 100f * koefMashtab, (float)plashkaInfo.width * koefMashtab, (float)plashkaInfo.height * koefMashtab);
		rectAbout = new Rect((float)Screen.width * 0.5f - (float)butAbout.normal.background.width * 0.5f * koefMashtab, (float)Screen.height - (float)butAbout.normal.background.height * koefMashtab - 45f * koefMashtab, (float)butAbout.normal.background.width * koefMashtab, (float)butAbout.normal.background.height * koefMashtab);
		rectUpload = new Rect((float)Screen.width * 0.5f - (float)butUpload.normal.background.width * 0.5f * koefMashtab, rectAbout.y - (float)butUpload.normal.background.height * koefMashtab - 25f * koefMashtab, (float)butUpload.normal.background.width * koefMashtab, (float)butUpload.normal.background.height * koefMashtab);
		rectPresets = new Rect(rectUpload.x, rectUpload.y - (rectAbout.y - rectUpload.y), rectUpload.width, rectUpload.height);
		rectCreateNew = new Rect(rectUpload.x, rectPresets.y - (rectAbout.y - rectUpload.y), rectUpload.width, rectUpload.height);
		rectScroll = new Rect(rectInfo.x + 50f * koefMashtab, rectInfo.y + 120f * koefMashtab, rectInfo.width - 100f * koefMashtab, rectInfo.height - 180f * koefMashtab);
		spisokSkinovContoller.showEnabled = false;
		arrNameSkin = new ArrayListWrapper();
		arrTitleSkin = new ArrayListWrapper();
		string text = "Multiplayer Skins";
		string folderName = ((PlayerPrefs.GetInt(Defs.SkinEditorMode, 0) != 0) ? text : SkinMaker_folderName);
		string baseName = ((PlayerPrefs.GetInt(Defs.SkinEditorMode, 0) != 0) ? Defs.SkinBaseName : SkinMaker_baseName);
		string createSpisokSkinov_sett = ((PlayerPrefs.GetInt(Defs.SkinEditorMode, 0) != 0) ? "Mult_CreateSpisokSkinov_UPDATE2" : "CreateSpisokSkinov");
		if (PlayerPrefs.GetInt(Defs.SkinEditorMode, 0) == 0)
		{
			Debug.Log(" previewControl  arrNameSkin_sett " + arrNameSkin_sett);
		}
		previewControl.arrNameSkin_sett = arrNameSkin_sett;
		string[] array = new string[28]
		{
			"Pixlgunner", "Zombie", "Skeleton", "Mummy", "Creeper Guy", "End Man", "Knight", "Police Officer", "SWAT Trooper", "Maniac",
			"Bad Guy", "Chief", "Space Engineer", "Nano Soldier", "Steel Man", "Captain Skin", "Hawk Skin", "Green Guy Skin", "Thunder God Skin", "Gordon Skin",
			"Anime Girl", "EMO Girl", "Nurse", "Magic Girl", "Brave Girl", "Glam Doll", "Kitty", "Famos Boy"
		};
		string[] arrVremTitle = ((PlayerPrefs.GetInt(Defs.SkinEditorMode, 0) != 0) ? array : SkinMaker_arrVremTitle);
		PrepareSkins(folderName, baseName, arrNameSkin_sett, arrTitleSkin_sett, createSpisokSkinov_sett, nomSkinForSave_sett, arrVremTitle);
		if (PlayerPrefs.GetInt(Defs.SkinEditorMode, 0) == 1)
		{
			GoToSpisokSkinov();
			return;
		}
		string @string = PlayerPrefs.GetString(Defs.ShouldReoeatActionSett, string.Empty);
		if (@string.Equals(Defs.GoToSkinsMakerAction) || @string.Equals(Defs.GoToPresetsAction))
		{
			PlayerPrefs.SetString(Defs.ShouldReoeatActionSett, string.Empty);
			PlayerPrefs.Save();
		}
	}

	public void GoToSpisokSkinov()
	{
		objPeople.active = true;
		if (PlayerPrefs.GetInt(Defs.SkinEditorMode, 0) == 0)
		{
			Debug.Log("before update spisok  arrTitleSkin.length: " + arrTitleSkin.Count + "\narrTitleSkin: " + arrTitleSkin);
			Debug.Log("before update spisok  arrNameSkin.length: " + arrNameSkin.Count + "\narrNameSkin: " + arrNameSkin);
		}
		previewControl.updateSpisok();
		previewControl.CurrentTextureIndex = arrNameSkin.Count - 1;
		previewControl.ShowSkin(previewControl.CurrentTextureIndex);
		spisokSkinovContoller.showEnabled = true;
		showEnabled = false;
	}

	public void GoToCreateNew()
	{
		objPeople.active = true;
		showEnabled = false;
		previewControl.CurrentTextureIndex = -1;
		viborChastiTelaController.cutSkin(-1);
		viborChastiTelaController.showEnabled = true;
	}

	private void OnGUI()
	{
		if (!showEnabled)
		{
			return;
		}
		GUI.skin = optionsSkin;
		if (aboutActive)
		{
			GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), fon, ScaleMode.ScaleAndCrop);
			GUI.DrawTexture(rectInfo, palshkaAbout);
			string text = InitializeAboutText();
			float num = 5.6f;
			scrollPosition = GUI.BeginScrollView(rectScroll, scrollPosition, new Rect(rectScroll.x, rectScroll.y, rectScroll.width - 40f * koefMashtab, rectScroll.height * num), false, true);
			GUI.Label(new Rect(rectScroll.x, rectScroll.y, rectScroll.width - 40f * koefMashtab, rectScroll.height * num), text, textInfo);
			GUI.EndScrollView();
			GUI.DrawTexture(new Rect(0f, (float)Screen.height - (float)plashkaNiz.height * koefMashtab, Screen.width, (float)plashkaNiz.height * koefMashtab), plashkaNiz);
			if (GUI.Button(new Rect(55f * koefMashtab, (float)Screen.height - (9f + (float)butBack.normal.background.height) * koefMashtab, (float)butBack.normal.background.width * koefMashtab, (float)butBack.normal.background.height * koefMashtab), string.Empty, butBack))
			{
				aboutActive = false;
			}
		}
		else if (!_browseIsShown)
		{
			GUI.DrawTexture(new Rect((float)Screen.width * 0.5f - (float)title.width * 0.5f * koefMashtab, 45f * koefMashtab, (float)title.width * koefMashtab, (float)title.height * koefMashtab), title);
			float num2 = (float)Screen.height / 768f;
			if (GUI.RepeatButton(new Rect(21f * num2, (float)Screen.height - (21f + (float)backBut.normal.background.height) * num2, (float)backBut.active.background.width * num2, (float)backBut.active.background.height * num2), string.Empty, backBut))
			{
				GUIHelper.DrawLoading();
				FlurryPluginWrapper.LogEvent("Back to Main Menu");
				Application.LoadLevel(Defs.MainMenuScene);
			}
			if (GUI.Button(rectCreateNew, string.Empty, butCreateNew))
			{
				GoToCreateNew();
			}
			if (GUI.Button(rectPresets, string.Empty, butPresets))
			{
				GoToSpisokSkinov();
			}
			if ((Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android) && GUI.Button(rectUpload, string.Empty, butUpload))
			{
				if (Application.platform == RuntimePlatform.IPhonePlayer)
				{
					_wvo = WebViewStarter.StartBrowser("http://minecraft.net/login");
					_wvo.SetMargins(0, 0, 0, Mathf.RoundToInt((float)plashkaNiz.height * koefMashtab));
				}
				else if (Application.platform == RuntimePlatform.Android)
				{
					try
					{
						using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
						{
							using (AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
							{
								using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("com.aunityplugin.WebViewActivity"))
								{
									androidJavaClass2.CallStatic("launchWebViewActivity", androidJavaObject, "https://minecraft.net/login::https://minecraft.net/profile");
								}
							}
						}
					}
					catch (Exception exception)
					{
						Debug.LogError("WebViewOne failure.");
						Debug.LogException(exception);
						throw;
					}
				}
				_browseIsShown = true;
				if (!Storager.hasKey(Defs.SkinsWrittenToGallery))
				{
					Storager.setInt(Defs.SkinsWrittenToGallery, 0, false);
				}
				if (Storager.getInt(Defs.SkinsWrittenToGallery, false) == 0)
				{
					WriteSkinsToGallery();
					Storager.setInt(Defs.SkinsWrittenToGallery, 1, false);
				}
			}
			if (GUI.Button(rectAbout, string.Empty, butAbout))
			{
				scrollPosition = new Vector2(0f, 0f);
				aboutActive = true;
			}
		}
		else if (infoActive)
		{
			GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), fon, ScaleMode.ScaleAndCrop);
			GUI.DrawTexture(rectInfo, plashkaInfo);
			GUI.Label(rectScroll, txtInfo, textBrowserInfo);
			GUI.DrawTexture(new Rect(0f, (float)Screen.height - (float)plashkaNiz.height * koefMashtab, Screen.width, (float)plashkaNiz.height * koefMashtab), plashkaNiz);
			if (GUI.Button(new Rect(55f * koefMashtab, (float)Screen.height - (9f + (float)butBack.normal.background.height) * koefMashtab, (float)butBack.normal.background.width * koefMashtab, (float)butBack.normal.background.height * koefMashtab), string.Empty, butBack))
			{
				_wvo.SetVisibility(true);
				infoActive = false;
			}
		}
		else
		{
			GUI.DrawTexture(new Rect(0f, (float)Screen.height - (float)plashkaNiz.height * koefMashtab, Screen.width, (float)plashkaNiz.height * koefMashtab), plashkaNiz);
			if (GUI.Button(new Rect(55f * koefMashtab, (float)Screen.height - (9f + (float)butBack.normal.background.height) * koefMashtab, (float)butBack.normal.background.width * koefMashtab, (float)butBack.normal.background.height * koefMashtab), string.Empty, butBack))
			{
				_browseIsShown = false;
				UnityEngine.Object.Destroy(_wvo.gameObject);
				_wvo = null;
			}
			if (GUI.Button(new Rect((float)Screen.width * 0.5f - (float)stButHome.normal.background.width * koefMashtab * 0.5f, (float)Screen.height - (9f + (float)stButHome.normal.background.height) * koefMashtab, (float)stButHome.normal.background.width * koefMashtab, (float)stButHome.normal.background.height * koefMashtab), string.Empty, stButHome))
			{
				_wvo.goHome();
			}
			if (GUI.Button(new Rect((float)Screen.width - 55f * koefMashtab - (float)butInfo.normal.background.width * koefMashtab, (float)Screen.height - (9f + (float)butInfo.normal.background.height) * koefMashtab, (float)butInfo.normal.background.width * koefMashtab, (float)butInfo.normal.background.height * koefMashtab), string.Empty, butInfo))
			{
				_wvo.SetVisibility(false);
				infoActive = true;
			}
		}
	}
}
