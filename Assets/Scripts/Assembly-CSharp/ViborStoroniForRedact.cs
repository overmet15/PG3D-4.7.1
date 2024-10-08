using System.Collections;
using UnityEngine;

public class ViborStoroniForRedact : MonoBehaviour
{
	public bool showEnabled;

	private static float koefMashtab = (float)Screen.height / 768f;

	private ViborChastiTela chastiTelaController;

	private Redactor redactorController;

	private Controller mainController;

	public ArrayList arrStoronForRedact;

	private int typeChastiTela;

	public int shooseNomStoroni;

	private Rect rectCenter;

	private Rect rectLeft;

	private Rect rectVerx;

	private Rect rectRight;

	private Rect rectNiz;

	private Rect rectBack;

	public GUIStyle styleButShoosePath;

	public Texture2D fon;

	public Texture2D plashkaNiz;

	public Texture2D plashkaExitBezSave;

	public Texture2D title;

	public Texture2D nadpisNaPlashke;

	public GUIStyle butBack;

	public GUIStyle butDlgCancel;

	public GUIStyle butDlgOk;

	private bool saveIzmActive;

	private bool touchEnabled = true;

	public static bool izmTexture = false;

	private Rect rectExitBezSave;

	private void Start()
	{
		typeChastiTela = 0;
		arrStoronForRedact = new ArrayList();
		chastiTelaController = GetComponent<ViborChastiTela>();
		redactorController = GetComponent<Redactor>();
		mainController = GetComponent<Controller>();
		rectExitBezSave = new Rect((float)Screen.width * 0.5f - (float)plashkaExitBezSave.width * 0.5f * koefMashtab, (float)Screen.height * 0.5f - (float)plashkaExitBezSave.height * 0.5f * koefMashtab, (float)plashkaExitBezSave.width * koefMashtab, (float)plashkaExitBezSave.height * koefMashtab);
	}

	private void Update()
	{
		if (showEnabled)
		{
			if (!showEnabled && touchEnabled)
			{
				touchEnabled = false;
			}
			if (showEnabled && !touchEnabled && Input.touchCount == 0)
			{
				touchEnabled = true;
			}
		}
	}

	private void OnGUI()
	{
		if (!showEnabled)
		{
			return;
		}
		GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), fon, ScaleMode.ScaleAndCrop);
		GUI.DrawTexture(new Rect((float)Screen.width * 0.5f - (float)title.width * 0.5f * koefMashtab, 30f * koefMashtab, (float)title.width * koefMashtab, (float)title.height * koefMashtab), title);
		GUI.DrawTexture(new Rect(0f, (float)Screen.height - (float)plashkaNiz.height * koefMashtab, Screen.width, (float)plashkaNiz.height * koefMashtab), plashkaNiz);
		GUI.DrawTexture(new Rect((float)Screen.width * 0.5f - (float)nadpisNaPlashke.width * 0.5f * koefMashtab, (float)Screen.height - (19f + (float)nadpisNaPlashke.height) * koefMashtab, (float)nadpisNaPlashke.width * koefMashtab, (float)nadpisNaPlashke.height * koefMashtab), nadpisNaPlashke);
		if (GUI.Button(new Rect(55f * koefMashtab, (float)Screen.height - (9f + (float)butBack.normal.background.height) * koefMashtab, (float)butBack.normal.background.width * koefMashtab, (float)butBack.normal.background.height * koefMashtab), string.Empty, butBack) && !saveIzmActive)
		{
			if (izmTexture)
			{
				saveIzmActive = true;
			}
			else
			{
				close();
			}
		}
		if (typeChastiTela == 0)
		{
			rectCenter = new Rect((float)Screen.width * 0.5f - 155f * koefMashtab, (float)Screen.height * 0.5f - 77.5f * koefMashtab, 155f * koefMashtab, 155f * koefMashtab);
			rectLeft = new Rect(rectCenter.x - 155f * koefMashtab, rectCenter.y, 155f * koefMashtab, 155f * koefMashtab);
			rectVerx = new Rect(rectCenter.x, rectCenter.y - 155f * koefMashtab, 155f * koefMashtab, 155f * koefMashtab);
			rectRight = new Rect(rectCenter.x + 155f * koefMashtab, rectCenter.y, 155f * koefMashtab, 155f * koefMashtab);
			rectNiz = new Rect(rectCenter.x, rectCenter.y + 155f * koefMashtab, 155f * koefMashtab, 155f * koefMashtab);
			rectBack = new Rect(rectCenter.x + 310f * koefMashtab, rectCenter.y, 155f * koefMashtab, 155f * koefMashtab);
		}
		if (typeChastiTela == 1 || typeChastiTela == 3)
		{
			rectCenter = new Rect((float)Screen.width * 0.5f - 93f * koefMashtab, (float)Screen.height * 0.5f - 139.5f * koefMashtab, 93f * koefMashtab, 279f * koefMashtab);
			rectLeft = new Rect(rectCenter.x - 93f * koefMashtab, rectCenter.y, 93f * koefMashtab, 279f * koefMashtab);
			rectVerx = new Rect(rectCenter.x, rectCenter.y - 93f * koefMashtab, 93f * koefMashtab, 93f * koefMashtab);
			rectRight = new Rect(rectCenter.x + 93f * koefMashtab, rectCenter.y, 93f * koefMashtab, 279f * koefMashtab);
			rectNiz = new Rect(rectCenter.x, rectCenter.y + 279f * koefMashtab, 93f * koefMashtab, 93f * koefMashtab);
			rectBack = new Rect(rectCenter.x + 186f * koefMashtab, rectCenter.y, 93f * koefMashtab, 279f * koefMashtab);
		}
		if (typeChastiTela == 2)
		{
			rectCenter = new Rect((float)Screen.width * 0.5f - 186f * koefMashtab, (float)Screen.height * 0.5f - 139.5f * koefMashtab, 186f * koefMashtab, 279f * koefMashtab);
			rectLeft = new Rect(rectCenter.x - 93f * koefMashtab, rectCenter.y, 93f * koefMashtab, 279f * koefMashtab);
			rectVerx = new Rect(rectCenter.x, rectCenter.y - 93f * koefMashtab, 186f * koefMashtab, 93f * koefMashtab);
			rectRight = new Rect(rectCenter.x + 186f * koefMashtab, rectCenter.y, 93f * koefMashtab, 279f * koefMashtab);
			rectNiz = new Rect(rectCenter.x, rectCenter.y + 279f * koefMashtab, 186f * koefMashtab, 93f * koefMashtab);
			rectBack = new Rect(rectCenter.x + 279f * koefMashtab, rectCenter.y, 186f * koefMashtab, 279f * koefMashtab);
		}
		GUI.DrawTexture(rectVerx, (Texture2D)arrStoronForRedact[0]);
		if (!saveIzmActive && GUI.Button(rectVerx, string.Empty, styleButShoosePath) && touchEnabled)
		{
			shooseNomStoroni = 0;
			redactorController.redactTexture = (Texture2D)arrStoronForRedact[0];
			showEnabled = false;
			redactorController.saveTextureToHistory(redactorController.redactTexture);
			redactorController.showEnabled = true;
		}
		GUI.DrawTexture(rectNiz, (Texture2D)arrStoronForRedact[1]);
		if (!saveIzmActive && GUI.Button(rectNiz, string.Empty, styleButShoosePath) && touchEnabled)
		{
			shooseNomStoroni = 1;
			redactorController.redactTexture = (Texture2D)arrStoronForRedact[1];
			showEnabled = false;
			redactorController.saveTextureToHistory(redactorController.redactTexture);
			redactorController.showEnabled = true;
		}
		GUI.DrawTexture(rectLeft, (Texture2D)arrStoronForRedact[2]);
		if (!saveIzmActive && GUI.Button(rectLeft, string.Empty, styleButShoosePath) && touchEnabled)
		{
			shooseNomStoroni = 2;
			redactorController.redactTexture = (Texture2D)arrStoronForRedact[2];
			showEnabled = false;
			redactorController.saveTextureToHistory(redactorController.redactTexture);
			redactorController.showEnabled = true;
		}
		GUI.DrawTexture(rectCenter, (Texture2D)arrStoronForRedact[3]);
		if (!saveIzmActive && GUI.Button(rectCenter, string.Empty, styleButShoosePath) && touchEnabled)
		{
			shooseNomStoroni = 3;
			redactorController.redactTexture = (Texture2D)arrStoronForRedact[3];
			showEnabled = false;
			redactorController.saveTextureToHistory(redactorController.redactTexture);
			redactorController.showEnabled = true;
		}
		GUI.DrawTexture(rectRight, (Texture2D)arrStoronForRedact[4]);
		if (!saveIzmActive && GUI.Button(rectRight, string.Empty, styleButShoosePath) && touchEnabled)
		{
			shooseNomStoroni = 4;
			redactorController.redactTexture = (Texture2D)arrStoronForRedact[4];
			showEnabled = false;
			redactorController.saveTextureToHistory(redactorController.redactTexture);
			redactorController.showEnabled = true;
		}
		GUI.DrawTexture(rectBack, (Texture2D)arrStoronForRedact[5]);
		if (!saveIzmActive && GUI.Button(rectBack, string.Empty, styleButShoosePath) && touchEnabled)
		{
			shooseNomStoroni = 5;
			redactorController.redactTexture = (Texture2D)arrStoronForRedact[5];
			showEnabled = false;
			redactorController.saveTextureToHistory(redactorController.redactTexture);
			redactorController.showEnabled = true;
		}
		if (saveIzmActive)
		{
			GUI.DrawTexture(rectExitBezSave, plashkaExitBezSave);
			if (GUI.Button(new Rect(rectExitBezSave.x + 55f * koefMashtab, rectExitBezSave.y + rectExitBezSave.height - 125f * koefMashtab, (float)butDlgCancel.normal.background.width * koefMashtab, (float)butDlgCancel.normal.background.height * koefMashtab), string.Empty, butDlgCancel))
			{
				saveIzmActive = false;
				izmTexture = false;
				close();
			}
			if (GUI.Button(new Rect(rectExitBezSave.x + rectExitBezSave.width - 55f * koefMashtab - (float)butDlgOk.normal.background.width * koefMashtab, rectExitBezSave.y + rectExitBezSave.height - 125f * koefMashtab, (float)butDlgOk.normal.background.width * koefMashtab, (float)butDlgOk.normal.background.height * koefMashtab), string.Empty, butDlgOk))
			{
				saveIzmActive = false;
				izmTexture = false;
				closeAndSave();
			}
		}
	}

	public void shooseChastTela(int nom)
	{
		typeChastiTela = nom;
		arrStoronForRedact.Clear();
		switch (nom)
		{
		case 0:
		{
			for (int l = 0; l < 6; l++)
			{
				arrStoronForRedact.Add(createCopyTexture((Texture2D)chastiTelaController.arrChastiTela[l]));
			}
			break;
		}
		case 1:
		{
			for (int j = 6; j < 12; j++)
			{
				arrStoronForRedact.Add(createCopyTexture((Texture2D)chastiTelaController.arrChastiTela[j]));
			}
			break;
		}
		case 2:
		{
			for (int k = 12; k < 18; k++)
			{
				arrStoronForRedact.Add(createCopyTexture((Texture2D)chastiTelaController.arrChastiTela[k]));
			}
			break;
		}
		case 3:
		{
			for (int i = 18; i < 24; i++)
			{
				arrStoronForRedact.Add(createCopyTexture((Texture2D)chastiTelaController.arrChastiTela[i]));
			}
			break;
		}
		}
	}

	private void vernutTextureToArrPoNomChasti()
	{
		if (typeChastiTela == 0)
		{
			for (int i = 0; i < 6; i++)
			{
				chastiTelaController.arrChastiTela[i] = arrStoronForRedact[i];
			}
		}
		else if (typeChastiTela == 1)
		{
			for (int j = 6; j < 12; j++)
			{
				chastiTelaController.arrChastiTela[j] = arrStoronForRedact[j - 6];
			}
		}
		else if (typeChastiTela == 2)
		{
			for (int k = 12; k < 18; k++)
			{
				chastiTelaController.arrChastiTela[k] = arrStoronForRedact[k - 12];
			}
		}
		else if (typeChastiTela == 3)
		{
			for (int l = 18; l < 24; l++)
			{
				chastiTelaController.arrChastiTela[l] = arrStoronForRedact[l - 18];
			}
		}
	}

	public void close()
	{
		showEnabled = false;
		mainController.objPeople.active = true;
		chastiTelaController.showEnabled = true;
	}

	public void closeAndSave()
	{
		showEnabled = false;
		mainController.objPeople.active = true;
		vernutTextureToArrPoNomChasti();
		PreviewController.SetTextureRecursivelyFrom(mainController.objPeople, chastiTelaController.sobratSkinIzArr(), mainController.objPeople.GetComponent<PreviewController>()._CurrentStopObjs());
		chastiTelaController.showEnabled = true;
	}

	private Rect myRect(float leftOtstup, float topOtstup, float width, float height)
	{
		return new Rect(leftOtstup * koefMashtab, topOtstup * koefMashtab, width * koefMashtab, height * koefMashtab);
	}

	private Texture2D createCopyTexture(Texture2D tekTexture)
	{
		Texture2D texture2D = new Texture2D(tekTexture.width, tekTexture.height);
		texture2D.SetPixels(tekTexture.GetPixels());
		texture2D.filterMode = FilterMode.Point;
		texture2D.Apply();
		return texture2D;
	}
}
