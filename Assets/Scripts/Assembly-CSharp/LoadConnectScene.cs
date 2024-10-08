using UnityEngine;

public sealed class LoadConnectScene : MonoBehaviour
{
	public static string sceneToLoad = string.Empty;

	public static Texture textureToShow = null;

	public static float interval = _defaultInterval;

	private static readonly float _defaultInterval = 2.5f;

	private Texture loading;

	private GameObject aInd;

	private void Start()
	{
		loading = textureToShow;
		if (loading == null)
		{
			loading = Resources.Load("main_loading") as Texture;
		}
		Invoke("_loadConnectScene", interval);
		interval = _defaultInterval;
		aInd = StoreKitEventListener.purchaseActivityInd;
		if (aInd == null)
		{
			Debug.LogWarning("aInd == null");
		}
		else
		{
			aInd.SetActive(true);
		}
	}

	private void OnGUI()
	{
		aInd.SetActive(true);
		Rect position = new Rect(((float)Screen.width - 2048f * (float)Screen.height / 1154f) / 2f, 0f, 2048f * (float)Screen.height / 1154f, Screen.height);
		GUI.DrawTexture(position, loading, ScaleMode.StretchToFill);
	}

	private void _loadConnectScene()
	{
		if (sceneToLoad.Equals("ConnectScene"))
		{
			Application.LoadLevel(sceneToLoad);
		}
		else
		{
			Application.LoadLevelAsync(sceneToLoad);
		}
	}

	private void OnDestroy()
	{
		if (aInd != null)
		{
			aInd.SetActive(false);
		}
	}
}
