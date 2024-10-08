using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Boo.Lang;
using Boo.Lang.Runtime;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class InputDemo : MonoBehaviour
{
	[Serializable]
	[CompilerGenerated]
	internal sealed class _0024ExplodeText_002493 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		[CompilerGenerated]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal GameObject[] _0024letters_002494;

			internal GameObject _0024letter_002495;

			internal GameObject _0024letter_002496;

			internal int _0024_002420_002497;

			internal GameObject[] _0024_002421_002498;

			internal int _0024_002422_002499;

			internal int _0024_002424_0024100;

			internal GameObject[] _0024_002425_0024101;

			internal int _0024_002426_0024102;

			internal InputDemo _0024self__0024103;

			public _0024(InputDemo self_)
			{
				_0024self__0024103 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					_0024self__0024103.acceptInput = false;
					_0024self__0024103.CancelInvoke("BlinkCursor");
					UnityEngine.Object.Destroy(_0024self__0024103.textObject);
					FlyingText.addRigidbodies = true;
					_0024letters_002494 = FlyingText.GetObjectsArray(_0024self__0024103.enteredText, new Vector3(-7f, 6f, 0f), Quaternion.identity);
					_0024_002420_002497 = 0;
					_0024_002421_002498 = _0024letters_002494;
					for (_0024_002422_002499 = _0024_002421_002498.Length; _0024_002420_002497 < _0024_002422_002499; _0024_002420_002497++)
					{
						_0024_002421_002498[_0024_002420_002497].GetComponent<Rigidbody>().useGravity = false;
						_0024_002421_002498[_0024_002420_002497].GetComponent<Rigidbody>().AddTorque(new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)) * 10f);
						_0024_002421_002498[_0024_002420_002497].GetComponent<Rigidbody>().AddExplosionForce(390f, new Vector3(0f, 1f, 11f), 15f);
					}
					result = (Yield(2, new WaitForSeconds(5f)) ? 1 : 0);
					break;
				case 2:
					_0024_002424_0024100 = 0;
					_0024_002425_0024101 = _0024letters_002494;
					for (_0024_002426_0024102 = _0024_002425_0024101.Length; _0024_002424_0024100 < _0024_002426_0024102; _0024_002424_0024100++)
					{
						UnityEngine.Object.Destroy(_0024_002425_0024101[_0024_002424_0024100]);
					}
					_0024self__0024103.InitializeText();
					YieldDefault(1);
					goto case 1;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal InputDemo _0024self__0024104;

		public _0024ExplodeText_002493(InputDemo self_)
		{
			_0024self__0024104 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024self__0024104);
		}
	}

	private GameObject textObject;

	private string enteredText;

	private char cursorChar;

	private bool acceptInput;

	public InputDemo()
	{
		cursorChar = "-"[0];
	}

	public virtual void Start()
	{
		InitializeText();
	}

	public virtual void InitializeText()
	{
		FlyingText.addRigidbodies = false;
		enteredText = string.Empty;
		acceptInput = true;
		textObject = FlyingText.GetObject("-", new Vector3(-7f, 6f, 0f), Quaternion.identity);
		InvokeRepeating("BlinkCursor", 0.5f, 0.5f);
	}

	public virtual void OnGUI()
	{
		if (acceptInput)
		{
			GUI.Label(new Rect(10f, 10f, 500f, 30f), "Type some text! Hit return when done.");
		}
	}

	public virtual void Update()
	{
		if (!acceptInput)
		{
			return;
		}
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(Input.inputString);
		while (enumerator.MoveNext())
		{
			char c = RuntimeServices.UnboxChar(enumerator.Current);
			if (c == "\b"[0])
			{
				if (enteredText.Length > 0)
				{
					enteredText = enteredText.Substring(0, enteredText.Length - 1);
				}
			}
			else if (c == "\n"[0] || c == "\r"[0])
			{
				if (enteredText.Length > 0)
				{
					StartCoroutine(ExplodeText());
				}
			}
			else if (c != "<"[0] && c != ">"[0])
			{
				enteredText += c;
				UnityRuntimeServices.Update(enumerator, c);
			}
			FlyingText.UpdateObject(textObject, enteredText + cursorChar);
		}
	}

	public virtual IEnumerator ExplodeText()
	{
		return new _0024ExplodeText_002493(this).GetEnumerator();
	}

	public virtual void BlinkCursor()
	{
		if (cursorChar == "-"[0])
		{
			cursorChar = " "[0];
		}
		else
		{
			cursorChar = "-"[0];
		}
		FlyingText.UpdateObject(textObject, enteredText + cursorChar);
	}

	public virtual void Main()
	{
	}
}
