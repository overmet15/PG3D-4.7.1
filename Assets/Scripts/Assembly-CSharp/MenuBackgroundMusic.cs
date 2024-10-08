using System;
using UnityEngine;

public class MenuBackgroundMusic : MonoBehaviour
{
	public static bool keepPlaying = false;

	private static string[] scenetsToPlayMusicOn = new string[7]
	{
		Defs.MainMenuScene,
		"ConnectScene",
		"SettingScene",
		"SkinEditor",
		"ChooseLevel",
		"CampaignChooseBox",
		"ProfileShop"
	};

	private void Start()
	{
		Defs.isSoundMusic = PlayerPrefsX.GetBool(PlayerPrefsX.SoundMusicSetting, true);
		Defs.isSoundFX = PlayerPrefsX.GetBool(PlayerPrefsX.SoundFXSetting, true);
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	public void Play()
	{
		base.GetComponent<AudioSource>().Play();
	}

	public void Stop()
	{
		base.GetComponent<AudioSource>().Stop();
	}

	private void OnLevelWasLoaded(int idx)
	{
		if (Array.IndexOf(scenetsToPlayMusicOn, Application.loadedLevelName) >= 0 || keepPlaying)
		{
			if (!base.GetComponent<AudioSource>().isPlaying && PlayerPrefsX.GetBool(PlayerPrefsX.SoundMusicSetting, true))
			{
				base.GetComponent<AudioSource>().Play();
			}
		}
		else
		{
			base.GetComponent<AudioSource>().Stop();
		}
		keepPlaying = false;
	}
}
