using System.Collections;
using UnityEngine;

internal sealed class InAppInstancer : MonoBehaviour
{
	public GameObject inAppGameObjectPrefab;

	public GameObject amazonGameCircleManager;

	public GameObject amazonIapManagerPrefab;

	private bool _amazonGamecircleManagerInitialized;

	private bool _amazonIapManagerInitialized;

	private string _leaderboardId = string.Empty;

	private void Start()
	{
		if (GameObject.FindGameObjectWithTag("InAppGameObject") == null)
		{
			Instantiate(inAppGameObjectPrefab, Vector3.zero, Quaternion.identity);
		}
		if (amazonIapManagerPrefab == null)
		{
			Debug.LogWarning("amazonIapManager == null");
		}
		else if (!_amazonIapManagerInitialized)
		{
			Object.Instantiate(amazonIapManagerPrefab, Vector3.zero, Quaternion.identity);
			_amazonIapManagerInitialized = true;
		}
		if (Application.platform == RuntimePlatform.Android && Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
		{
			if (amazonGameCircleManager == null)
			{
				Debug.LogWarning("amazonGamecircleManager == null");
			}
			else if (!_amazonGamecircleManagerInitialized)
			{
				StartCoroutine(InitializeAmazonGamecircleManager());
				_amazonGamecircleManagerInitialized = true;
			}
		}
	}

	private IEnumerator InitializeAmazonGamecircleManager()
	{
		Object.DontDestroyOnLoad(amazonGameCircleManager);
		yield return null;
		_leaderboardId = ((!Debug.isDebugBuild) ? "best_players" : "test_leaderboard");
	}

	private void HandleAmazonGamecircleServiceReady()
	{
		Debug.Log("Amazon GameCircle service is initialized. Trying to submit.");
	}

	private void HandleAmazonGamecircleServiceNotReady(string message)
	{
		Debug.LogError("Amazon GameCircle service is not ready:\n" + message);
	}

	private void HandleSubmitScoreSucceeded(string leaderbordId)
	{
		if (Debug.isDebugBuild)
		{
			Debug.Log("Submit score succeeded for leaderboard " + leaderbordId);
		}
	}

	private void HandleSubmitScoreFailed(string leaderbordId, string error)
	{
		string message = string.Format("Submit score failed for leaderboard {0}:\n{1}", leaderbordId, error);
		Debug.LogError(message);
	}
}
