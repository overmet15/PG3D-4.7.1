﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Stole this from https://github.com/Zweronz/Star-Warfare/blob/main/Assets/KeyboardListener.cs !!!
public class KeyboardListener : MonoBehaviour
{
	// Dont feel like redoing whole ass system for one fix - met
	public static KeyboardListener current;

	public TouchScreenKeyboard keyboard;

	public Action<string> onFinish;

	public string pcStringDisplay;
	public string pcString;

	private float holdingBackFor = 0;
    private float timeBeforeDeletingKey;

	private float timeForBlinkAnim = 1; //To update at start
	private bool blinkAnimToggle;

	private static Dictionary<KeyCode, string> keyMap = new Dictionary<KeyCode, string>
	{
		{KeyCode.A, "a"},
		{KeyCode.B, "b"},
		{KeyCode.C, "c"},
		{KeyCode.D, "d"},
		{KeyCode.E, "e"},
		{KeyCode.F, "f"},
		{KeyCode.G, "g"},
		{KeyCode.H, "h"},
		{KeyCode.I, "i"},
		{KeyCode.J, "j"},
		{KeyCode.K, "k"},
		{KeyCode.L, "l"},
		{KeyCode.M, "m"},
		{KeyCode.N, "n"},
		{KeyCode.O, "o"},
		{KeyCode.P, "p"},
		{KeyCode.Q, "q"},
		{KeyCode.R, "r"},
		{KeyCode.S, "s"},
		{KeyCode.T, "t"},
		{KeyCode.U, "u"},
		{KeyCode.V, "v"},
		{KeyCode.W, "w"},
		{KeyCode.X, "x"},
		{KeyCode.Y, "y"},
		{KeyCode.Z, "z"},
		{KeyCode.Space, " "},
		{KeyCode.Alpha0, "0"},
		{KeyCode.Alpha1, "1"},
		{KeyCode.Alpha2, "2"},
		{KeyCode.Alpha3, "3"},
		{KeyCode.Alpha4, "4"},
		{KeyCode.Alpha5, "5"},
		{KeyCode.Alpha6, "6"},
		{KeyCode.Alpha7, "7"},
		{KeyCode.Alpha8, "8"},
		{KeyCode.Alpha9, "9"},
		{KeyCode.Comma, ","},
		{KeyCode.Period, "."},
		{KeyCode.Slash, "/"},
		{KeyCode.Semicolon, ";"},
		{KeyCode.Quote, "'"},
		{KeyCode.LeftBracket, "["},
		{KeyCode.RightBracket, "]"},
		{KeyCode.Backslash, "\\"},
		{KeyCode.Minus, "-"},
		{KeyCode.Equals, "="},
		{KeyCode.BackQuote, "`"},
	};

	private static Dictionary<KeyCode, string> upperKeyMap = new Dictionary<KeyCode, string>
	{
		{KeyCode.Alpha0, "!"},
		{KeyCode.Alpha1, "@"},
		{KeyCode.Alpha2, "#"},
		{KeyCode.Alpha3, "$"},
		{KeyCode.Alpha4, "%"},
		{KeyCode.Alpha5, "^"},
		{KeyCode.Alpha6, "&"},
		{KeyCode.Alpha7, "*"},
		{KeyCode.Alpha8, "("},
		{KeyCode.Alpha9, ")"},
		{KeyCode.Comma, "<"},
		{KeyCode.Period, ">"},
		{KeyCode.Slash, "?"},
		{KeyCode.Semicolon, ":"},
		{KeyCode.Quote, "\""},
		{KeyCode.LeftBracket, "{"},
		{KeyCode.RightBracket, "}"},
		{KeyCode.Backslash, "\\"},
		{KeyCode.Minus, "_"},
		{KeyCode.Equals, "+"},
		{KeyCode.BackQuote, "~"},
	};

	public static KeyboardListener GetOrCreate(Action<string> onFinish)
    {
        if (current == null) current = new GameObject("Keyboard Listener").AddComponent<KeyboardListener>();

		current.onFinish = onFinish;

        return current;
    }
	void Update()
	{
		if (Application.isMobilePlatform)
		{
			
		}
		else
		{
			if (Input.GetKeyDown(KeyCode.Return))
			{
				onFinish(pcString);
				pcString = string.Empty;
				//Destroy(gameObject);
                //current = null;

                return;
			}

			ListenForKeys();
		}
	}

    // I can improve this later or something I really don't feel like it - Zweronz
	// OOF - Met
    void OnGUI()
    {
		if (!Application.isMobilePlatform)
		{
			GUIStyle displayStyle = new GUIStyle(GUI.skin.label) 
			{ 
				alignment = TextAnchor.LowerLeft, 
				fontSize = 48,
			};

			// The text
        	GUI.Label(new Rect(0, 0, Screen.width-50, Screen.height-50), pcStringDisplay, displayStyle);
		}
    }

    private void ListenForKeys()
    {
        bool isHoldingBack = Input.GetKey(KeyCode.Backspace);
		if (isHoldingBack)
		{
			BackspaceLogic();
			return;
        }

        bool upper = Input.GetKey(KeyCode.LeftShift);

		foreach (KeyCode key in keyMap.Keys)
		{
			if (Input.GetKeyDown(key))
			{
				if (upper)
				{
					pcString += upperKeyMap.ContainsKey(key) ? upperKeyMap[key] : keyMap[key].ToUpper();
				}
				else
				{
					pcString += keyMap[key];
				}
				// Automaticaly reset
				timeForBlinkAnim = 1;
            }
		}

        holdingBackFor = timeBeforeDeletingKey = 0; // Reseting manualy cuz caused weird behaviour

		timeForBlinkAnim += Time.unscaledDeltaTime;

		if (timeForBlinkAnim >= 0.5)
		{
			string toAdd = blinkAnimToggle == false ? " " : "|";

			pcStringDisplay = pcString + toAdd;

			timeForBlinkAnim = 0;
			blinkAnimToggle = !blinkAnimToggle;
        }
    }

	void BackspaceLogic() // fully made by me rn - met
    {
		// is holding and leight is not 0
        if (pcString.Length > 0)
        {
            // if holding for first time or hodling for ? seconds and timeBeforeDeletingKey = 0
            if (holdingBackFor == 0 || (holdingBackFor > 0.5f && timeBeforeDeletingKey <= 0))
            {
                pcString = pcString.Substring(0, pcString.Length - 1);

                timeBeforeDeletingKey = 0.02f;
                pcStringDisplay = pcString + " ";
            }

			//Update timers
            holdingBackFor += Time.unscaledDeltaTime;
            timeBeforeDeletingKey -= Time.unscaledDeltaTime;
        }
    }
}
