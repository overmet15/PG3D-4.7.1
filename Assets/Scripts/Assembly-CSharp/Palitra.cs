using UnityEngine;

public class Palitra : MonoBehaviour
{
	public bool showEnabled;

	private static float koefMashtab = (float)Screen.height / 768f;

	private Redactor redactorController;

	private Texture2D texNewColor;

	private Texture2D texOldColor;

	public Color oldColor = new Color(1f, 1f, 1f, 1f);

	public Color newColor = new Color(1f, 1f, 1f, 1f);

	public Texture2D palitra;

	public Texture2D framePalitra;

	public Texture2D fon;

	public Texture2D title;

	public Texture2D frameNewColor;

	public Texture2D frameOldColor;

	public Texture2D plaskaNiz;

	private Rect rectPalitra;

	private Rect rectNewColor;

	private Rect rectOldColor;

	public GUIStyle styleButBack;

	public GUIStyle styleButSet;

	private void Start()
	{
		redactorController = GetComponent<Redactor>();
		rectPalitra = new Rect(((float)Screen.width - (float)palitra.width * koefMashtab) * 0.5f, 120f * koefMashtab, (float)palitra.width * koefMashtab, (float)palitra.height * koefMashtab);
		rectNewColor = new Rect((float)Screen.width - 70f * koefMashtab - 146f * koefMashtab, 287f * koefMashtab, 146f * koefMashtab, 145f * koefMashtab);
		rectOldColor = new Rect(70f * koefMashtab, 287f * koefMashtab, 146f * koefMashtab, 145f * koefMashtab);
		texNewColor = new Texture2D(1, 1);
		texNewColor.filterMode = FilterMode.Point;
		texNewColor.SetPixel(0, 0, newColor);
		texNewColor.Apply();
		texOldColor = new Texture2D(1, 1);
		texOldColor.filterMode = FilterMode.Point;
		updateOldColorTexture(oldColor);
	}

	private void Update()
	{
		if (showEnabled && Input.touchCount > 0)
		{
			Vector2 vector = new Vector2(Input.touches[0].position.x, Input.touches[0].position.y);
			if (rectPalitra.Contains(new Vector2(vector.x, (float)Screen.height - vector.y)))
			{
				int x = (int)((float)palitra.width * (vector.x - rectPalitra.x) / rectPalitra.width);
				int y = (int)((float)palitra.height * (vector.y - ((float)Screen.height - rectPalitra.y - rectPalitra.height)) / rectPalitra.height);
				newColor = palitra.GetPixel(x, y);
				texNewColor.SetPixel(0, 0, newColor);
				texNewColor.Apply();
			}
		}
	}

	private void OnGUI()
	{
		if (showEnabled)
		{
			GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), fon, ScaleMode.ScaleAndCrop);
			GUI.DrawTexture(new Rect((float)Screen.width * 0.5f - (float)title.width * 0.5f * koefMashtab, 30f * koefMashtab, (float)title.width * koefMashtab, (float)title.height * koefMashtab), title);
			GUI.DrawTexture(rectPalitra, palitra);
			GUI.DrawTexture(new Rect(rectPalitra.x - 9f * koefMashtab, rectPalitra.y - 9f * koefMashtab, (float)framePalitra.width * koefMashtab, (float)framePalitra.height * koefMashtab), framePalitra);
			GUI.DrawTexture(rectOldColor, texOldColor);
			GUI.DrawTexture(new Rect(rectOldColor.x - 11f * koefMashtab, rectOldColor.y - 59f * koefMashtab, (float)frameOldColor.width * koefMashtab, (float)frameOldColor.height * koefMashtab), frameOldColor);
			GUI.DrawTexture(rectNewColor, texNewColor);
			GUI.DrawTexture(new Rect(rectNewColor.x - 11f * koefMashtab, rectNewColor.y - 59f * koefMashtab, (float)frameNewColor.width * koefMashtab, (float)frameNewColor.height * koefMashtab), frameNewColor);
			GUI.DrawTexture(new Rect(0f, (float)Screen.height - (float)plaskaNiz.height * koefMashtab, Screen.width, (float)plaskaNiz.height * koefMashtab), plaskaNiz);
			if (GUI.Button(new Rect(55f * koefMashtab, (float)Screen.height - (9f + (float)styleButBack.normal.background.height) * koefMashtab, (float)styleButBack.normal.background.width * koefMashtab, (float)styleButBack.normal.background.height * koefMashtab), string.Empty, styleButBack))
			{
				updateOldColorTexture(oldColor);
				redactorController.showEnabled = true;
				showEnabled = false;
			}
			if (GUI.Button(new Rect((float)Screen.width - 55f * koefMashtab - (float)styleButSet.normal.background.width * koefMashtab, (float)Screen.height - (9f + (float)styleButSet.normal.background.height) * koefMashtab, (float)styleButSet.normal.background.width * koefMashtab, (float)styleButSet.normal.background.height * koefMashtab), string.Empty, styleButSet))
			{
				updateOldColorTexture(newColor);
				redactorController.updateColorForPaint(newColor);
				redactorController.showEnabled = true;
				showEnabled = false;
			}
		}
	}

	public void updateOldColorTexture(Color setColor)
	{
		oldColor = setColor;
		texOldColor.SetPixel(0, 0, setColor);
		texOldColor.Apply();
		newColor = setColor;
		texNewColor.SetPixel(0, 0, setColor);
		texNewColor.Apply();
	}
}
