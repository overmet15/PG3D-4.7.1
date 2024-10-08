using UnityEngine;

public class BackgroundMusicController : MonoBehaviour
{
	private void Awake()
	{
	}

	private void Start()
	{
		if (PlayerPrefsX.GetBool(PlayerPrefsX.SoundMusicSetting, true))
		{
			Play();
		}
	}

	private void Update()
	{
	}

	public void Play()
	{
		base.GetComponent<AudioSource>().Play();
	}

	public void Stop()
	{
		base.GetComponent<AudioSource>().Stop();
	}
}
