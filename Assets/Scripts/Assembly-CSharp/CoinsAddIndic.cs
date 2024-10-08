using System.Collections;
using UnityEngine;

public class CoinsAddIndic : MonoBehaviour
{
	private UISprite ind;

	private bool blinking;

	public AudioClip coinsAdded;

	private void Start()
	{
		ind = GetComponent<UISprite>();
	}

	private void OnEnable()
	{
		CoinsMessage.CoinsLabelDisappeared += IndicateCoinsAdd;
	}

	private void OnDisable()
	{
		CoinsMessage.CoinsLabelDisappeared -= IndicateCoinsAdd;
	}

	private void IndicateCoinsAdd()
	{
		if (!blinking)
		{
			StartCoroutine(blink());
		}
		StartCoroutine(PlaySound());
	}

	private IEnumerator blink()
	{
		if (ind == null)
		{
			Debug.LogWarning("Indicator sprite is null.");
			yield return null;
		}
		blinking = true;
		try
		{
			for (int i = 0; i < 15; i++)
			{
				ind.spriteName = "coin_frame_ON";
				yield return null;
				yield return new WaitForSeconds(0.1f);
				ind.spriteName = "coin_frame";
				yield return new WaitForSeconds(0.1f);
			}
		}
		finally
		{
			blinking = false;
		}
	}

	private IEnumerator PlaySound()
	{
		yield return new WaitForSeconds((!Defs.IsSurvival) ? 2f : 0.11f);
		if (!Application.loadedLevelName.Equals("LevelComplete") && PlayerPrefsX.GetBool(PlayerPrefsX.SoundFXSetting, true))
		{
			NGUITools.PlaySound(coinsAdded);
		}
	}
}
