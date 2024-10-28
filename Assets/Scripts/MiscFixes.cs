using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiscFixes : MonoBehaviour {

	public static Shader autoFadeShader;

    public Shader _autoFadeShader;
    // Use this for initialization
    void Awake () 
	{
		autoFadeShader = _autoFadeShader;
	}
}
