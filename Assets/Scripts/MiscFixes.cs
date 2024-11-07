using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MiscFixes : MonoBehaviour {

	public static Shader autoFadeShader;

    public Shader _autoFadeShader;
	// Use this for initialization

	string a = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<Preferences>\r\n  <Preference>\r\n    <Key>Coins</Key>\r\n    <Value>10</Value>\r\n  </Preference>\r\n  <Preference>\r\n    <Key>com.Fuckhead.PixlGun3D_Zg+C6JzNzFU0qZSMkxFJFskkfZk=</Key>\r\n    <Value>ahyP1hmHFG9pQi1Flb6Dmvxxvl621s1nhn+w920P+eV8UWtDHGM1yw6zaXL9ZuSaBEHMzjpdEzp/abBngc/gCg==</Value>\r\n  </Preference>\r\n</Preferences>";
    void Awake () 
	{
		autoFadeShader = _autoFadeShader;

		if (!File.Exists(Application.persistentDataPath + "/com.P3D.Pixlgun.Settings.xml"))
		{
			File.CreateText(Application.persistentDataPath + "/com.P3D.Pixlgun.Settings.xml");
			File.WriteAllText(Application.persistentDataPath + "/com.P3D.Pixlgun.Settings.xml", a);
		}
	}
}
