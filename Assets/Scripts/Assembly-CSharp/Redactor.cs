using System.Collections;
using UnityEngine;

public class Redactor : MonoBehaviour
{
	public bool showEnabled;

	private bool touchEnabled;

	private static float koefMashtab = (float)Screen.height / 768f;

	public int typeInstrumenta;

	public Texture2D redactTexture;

	private Texture2D texColor;

	public Color colorForPaint = new Color(1f, 1f, 1f, 1f);

	private Color colorEraser = new Color(1f, 1f, 1f, 1f);

	private Palitra palitraController;

	private ViborStoroniForRedact storoniForRedactController;

	private ArrayList arrHistory;

	private int tekTexHistory;

	private bool saveToHistory;

	public Texture2D fon;

	public Texture2D title;

	public Texture2D plashkaNiz;

	public GUIStyle butUndo;

	public GUIStyle butRedo;

	public GUIStyle butBack;

	public GUIStyle butPalitra;

	public GUIStyle butPencil;

	public GUIStyle butBrush;

	public GUIStyle butEraser;

	public GUIStyle butFill;

	private bool otklUndo = true;

	private bool otklRedo = true;

	private bool activPencil = true;

	private bool activBrush;

	private bool activEraser;

	private bool activFill;

	private Rect rectTexture;

	private Rect rectButBrush;

	private Rect rectButPencil;

	private Rect rectButEraser;

	private Rect rectButFill;

	private void Start()
	{
		arrHistory = new ArrayList();
		tekTexHistory = -1;
		palitraController = GetComponent<Palitra>();
		storoniForRedactController = GetComponent<ViborStoroniForRedact>();
		texColor = new Texture2D(1, 1);
		texColor.filterMode = FilterMode.Point;
		updateColorForPaint(colorForPaint);
		rectButBrush = new Rect((float)Screen.width * 0.5f - (float)butBrush.normal.background.width * koefMashtab, (float)Screen.height - (float)butBrush.normal.background.height * koefMashtab, (float)butBrush.normal.background.width * koefMashtab, (float)butBrush.normal.background.height * koefMashtab);
		rectButPencil = new Rect(rectButBrush.x - (float)butPencil.normal.background.width * koefMashtab, rectButBrush.y, rectButBrush.width, rectButBrush.height);
		rectButEraser = new Rect(rectButBrush.x + (float)butBrush.normal.background.width * koefMashtab, rectButBrush.y, rectButBrush.width, rectButBrush.height);
		rectButFill = new Rect(rectButEraser.x + (float)butEraser.normal.background.width * koefMashtab, rectButBrush.y, rectButBrush.width, rectButBrush.height);
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
		if (redactTexture != null)
		{
			float num;
			float num2;
			if (redactTexture.width > redactTexture.height)
			{
				num = 500f;
				num2 = 500f * (float)redactTexture.height / (float)redactTexture.width;
			}
			else
			{
				num2 = 500f;
				num = 500f * (float)redactTexture.width / (float)redactTexture.height;
			}
			rectTexture = new Rect((float)Screen.width * 0.5f - num * 0.5f * koefMashtab, (float)Screen.height * 0.5f - num2 * 0.5f * koefMashtab, num * koefMashtab, num2 * koefMashtab);
			GUI.DrawTexture(rectTexture, redactTexture);
		}
		if (GUI.Toggle(new Rect(55f * koefMashtab, (float)Screen.height * 0.5f - (float)butUndo.normal.background.height * 0.5f * koefMashtab, (float)butUndo.normal.background.width * koefMashtab, (float)butUndo.normal.background.height * koefMashtab), otklUndo, string.Empty, butUndo) && !otklUndo)
		{
			tekTexHistory--;
			redactTexture = createCopyTexture((Texture2D)arrHistory[tekTexHistory]);
			Debug.Log("tekTexHistory=" + tekTexHistory + " all=" + arrHistory.Count);
		}
		if (GUI.Toggle(new Rect((float)Screen.width - 55f * koefMashtab - (float)butRedo.normal.background.width * koefMashtab, (float)Screen.height * 0.5f - (float)butRedo.normal.background.height * 0.5f * koefMashtab, (float)butRedo.normal.background.width * koefMashtab, (float)butRedo.normal.background.height * koefMashtab), otklRedo, string.Empty, butRedo) && !otklRedo)
		{
			tekTexHistory++;
			redactTexture = createCopyTexture((Texture2D)arrHistory[tekTexHistory]);
			Debug.Log("tekTexHistory=" + tekTexHistory + " all=" + arrHistory.Count);
		}
		if (GUI.Button(new Rect(55f * koefMashtab, (float)Screen.height - (9f + (float)butBack.normal.background.height) * koefMashtab, (float)butBack.normal.background.width * koefMashtab, (float)butBack.normal.background.height * koefMashtab), string.Empty, butBack))
		{
			showEnabled = false;
			storoniForRedactController.arrStoronForRedact[storoniForRedactController.shooseNomStoroni] = redactTexture;
			arrHistory.Clear();
			storoniForRedactController.showEnabled = true;
		}
		Rect position = new Rect((float)Screen.width - 55f * koefMashtab - (float)butPalitra.normal.background.width * koefMashtab, (float)Screen.height - (9f + (float)butPalitra.normal.background.height) * koefMashtab, (float)butPalitra.normal.background.width * koefMashtab, (float)butPalitra.normal.background.height * koefMashtab);
		GUI.DrawTexture(position, texColor);
		if (GUI.Button(position, string.Empty, butPalitra))
		{
			showEnabled = false;
			palitraController.showEnabled = true;
			palitraController.updateOldColorTexture(colorForPaint);
		}
		if (GUI.Toggle(rectButBrush, activBrush, string.Empty, butBrush) && !activBrush)
		{
			activBrush = true;
			activPencil = false;
			activEraser = false;
			activFill = false;
			typeInstrumenta = 1;
		}
		if (GUI.Toggle(rectButPencil, activPencil, string.Empty, butPencil) && !activPencil)
		{
			activBrush = false;
			activPencil = true;
			activEraser = false;
			activFill = false;
			typeInstrumenta = 0;
		}
		if (GUI.Toggle(rectButEraser, activEraser, string.Empty, butEraser) && !activEraser)
		{
			activBrush = false;
			activPencil = false;
			activEraser = true;
			activFill = false;
			typeInstrumenta = 2;
		}
		if (GUI.Toggle(rectButFill, activFill, string.Empty, butFill) && !activFill)
		{
			activBrush = false;
			activPencil = false;
			activEraser = false;
			activFill = true;
			typeInstrumenta = 3;
		}
	}

	private void Update()
	{
		if (!showEnabled && touchEnabled)
		{
			touchEnabled = false;
		}
		if (showEnabled && !touchEnabled && Input.touchCount == 0)
		{
			touchEnabled = true;
		}
		if (!showEnabled || !touchEnabled)
		{
			return;
		}
		if (Input.touchCount > 0)
		{
			Vector2 point = new Vector2(Input.touches[0].position.x, Input.touches[0].position.y);
			if (rectTexture.Contains(point))
			{
				int num = (int)((float)redactTexture.width * (point.x - rectTexture.x) / rectTexture.width);
				int num2 = (int)((float)redactTexture.height * (point.y - rectTexture.y) / rectTexture.height);
				if (typeInstrumenta == 0)
				{
					redactTexture.SetPixel(num, num2, colorForPaint);
					redactTexture.Apply();
				}
				if (typeInstrumenta == 1)
				{
					redactTexture.SetPixel(num, num2, colorForPaint);
					if (num > 0)
					{
						redactTexture.SetPixel(num - 1, num2, colorForPaint);
					}
					if (num < redactTexture.width - 1)
					{
						redactTexture.SetPixel(num + 1, num2, colorForPaint);
					}
					if (num2 > 0)
					{
						redactTexture.SetPixel(num, num2 - 1, colorForPaint);
					}
					if (num2 < redactTexture.height - 1)
					{
						redactTexture.SetPixel(num, num2 + 1, colorForPaint);
					}
					redactTexture.Apply();
				}
				if (typeInstrumenta == 2)
				{
					redactTexture.SetPixel(num, num2, colorEraser);
					redactTexture.Apply();
				}
				if (typeInstrumenta == 3)
				{
					for (int i = 0; i < redactTexture.width; i++)
					{
						for (int j = 0; j < redactTexture.height; j++)
						{
							redactTexture.SetPixel(i, j, colorForPaint);
						}
					}
					redactTexture.Apply();
				}
				saveToHistory = true;
			}
			if (Input.touches[0].phase == TouchPhase.Ended && saveToHistory)
			{
				ViborChastiTela.skinIzm = true;
				ViborStoroniForRedact.izmTexture = true;
				if (tekTexHistory < arrHistory.Count)
				{
					arrHistory.RemoveRange(tekTexHistory + 1, arrHistory.Count - (tekTexHistory + 1));
				}
				saveToHistory = false;
				saveTextureToHistory(redactTexture);
			}
		}
		if (tekTexHistory > 0)
		{
			otklUndo = false;
		}
		else
		{
			otklUndo = true;
		}
		if (tekTexHistory < arrHistory.Count - 1 && arrHistory.Count > 0)
		{
			otklRedo = false;
		}
		else
		{
			otklRedo = true;
		}
	}

	public void updateColorForPaint(Color newColor)
	{
		colorForPaint = newColor;
		texColor.SetPixel(0, 0, colorForPaint);
		texColor.Apply();
	}

	public void saveTextureToHistory(Texture2D textureForSave)
	{
		arrHistory.Add(createCopyTexture(textureForSave));
		tekTexHistory = arrHistory.Count - 1;
		Debug.Log("tekTexHistory=" + tekTexHistory);
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
