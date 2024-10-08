using UnityEngine;

[ExecuteInEditMode]
public class ActivityIndicator : MonoBehaviour
{
	public Texture2D texture;

	public float angle;

	public Vector2 size = new Vector2(128f, 128f);

	private Vector2 pos = new Vector2(0f, 0f);

	private Rect rect;

	private Vector2 pivot;

	private float rotSpeed = 180f;

	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		size *= (float)Screen.height / 768f;
		UpdateSettings();
	}

	private void Start()
	{
		base.gameObject.SetActive(false);
	}

	private void UpdateSettings()
	{
		pos = new Vector2((float)Screen.width / 2f, (float)Screen.height / 2f);
		rect = new Rect(pos.x - size.x * 0.5f, pos.y - size.y * 0.5f, size.x, size.y);
		pivot = new Vector2(rect.xMin + rect.width * 0.5f, rect.yMin + rect.height * 0.5f);
	}

	private void OnGUI()
	{
		angle = rotSpeed * Time.realtimeSinceStartup;
		angle = (int)angle % 360;
		GUI.depth = -3;
		Matrix4x4 matrix = GUI.matrix;
		GUIUtility.RotateAroundPivot(angle, pivot);
		GUI.DrawTexture(rect, texture);
		GUI.matrix = matrix;
	}

	private void OnDestroy()
	{
	}
}
