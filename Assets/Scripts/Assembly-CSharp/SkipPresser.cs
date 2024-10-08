using System;
using System.Collections;
using UnityEngine;

public class SkipPresser : MonoBehaviour
{
	public GameObject windowAnchor;

	public static event Action SkipPressed;

	private void Start()
	{
		base.gameObject.SetActive(Defs.IsTraining);
	}

	private void OnClick()
	{
		base.gameObject.SetActive(false);
		windowAnchor.SetActive(true);
		if (SkipPresser.SkipPressed != null)
		{
			SkipPresser.SkipPressed();
		}
		GameObject gameObject = GameObject.FindGameObjectWithTag("PlayerGun");
		if ((bool)gameObject && gameObject != null)
		{
			Transform child = gameObject.transform.GetChild(0);
			if ((bool)child && child != null)
			{
				child.gameObject.SetActive(false);
			}
		}
		TrainingController.SkipTraining();
	}

	private IEnumerator FireSkipPressed()
	{
		yield return new WaitForSeconds(0.05f);
		if (SkipPresser.SkipPressed != null)
		{
			SkipPresser.SkipPressed();
		}
	}
}
