using UnityEngine;

public class GUIHelper : MonoBehaviour
{
	public static float Int = 1.25f;

	public static GUIHelper instance;

	public GUIStyle loadingStyle;

	public static void DrawLoading()
	{
		float num = 113f * Defs.Coef;
		float num2 = (float)Screen.height * 0.031f;
		Rect position = new Rect((float)Screen.width - num - Defs.BottomOffs * Defs.Coef, (float)Screen.height - num2, num, num2);
		instance.loadingStyle.fontSize = Mathf.RoundToInt(17f * Defs.Coef);
		GUI.Box(position, "Loading", instance.loadingStyle);
	}

	private void Start()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		instance = this;
	}
}
