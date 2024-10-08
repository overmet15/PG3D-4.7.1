using System;
using UnityEngine;

[Serializable]
public class LightFade : MonoBehaviour
{
	public float lightIntensity;

	public float fadeSpeed;

	public LightFade()
	{
		lightIntensity = 2f;
		fadeSpeed = 1f;
	}

	public virtual void Update()
	{
		lightIntensity = Mathf.Max(lightIntensity - Time.deltaTime * fadeSpeed, 0f);
		GetComponent<Light>().intensity = lightIntensity;
	}

	public virtual void Main()
	{
	}
}
