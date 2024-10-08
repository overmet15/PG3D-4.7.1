using UnityEngine;

public class BestScoresStat : MonoBehaviour
{
	private void Start()
	{
		GetComponent<UILabel>().text = string.Empty + PlayerPrefs.GetInt(Defs.SurvivalScoreSett, 0);
	}
}
