using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Boo.Lang;
using UnityEngine;

[Serializable]
public class MenuObject : MonoBehaviour
{
	[Serializable]
	[CompilerGenerated]
	internal sealed class _0024OnMouseDown_0024105 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		[CompilerGenerated]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal Vector3 _0024startPos_0024106;

			internal Vector3 _0024endPos_0024107;

			internal Color _0024endColor_0024108;

			internal float _0024i_0024109;

			internal MenuObject _0024self__0024110;

			public _0024(MenuObject self_)
			{
				_0024self__0024110 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					if (MenuControl.clicked)
					{
						goto case 1;
					}
					MenuControl.clicked = true;
					_0024self__0024110.highlight.SetActive(false);
					_0024self__0024110.menuObject.eulerAngles = Vector3.zero;
					_0024startPos_0024106 = _0024self__0024110.transform.position;
					_0024endPos_0024107 = new Vector3(0f, -2f, -16f);
					_0024endColor_0024108 = Camera.main.backgroundColor;
					_0024i_0024109 = 0f;
					goto IL_011c;
				case 2:
					_0024i_0024109 += Time.deltaTime * 2f;
					goto IL_011c;
				case 3:
					_0024self__0024110.menuObject.transform.position = _0024startPos_0024106;
					_0024self__0024110.menuObject.GetComponent<Renderer>().material.color = Color.white;
					MenuControl.clicked = false;
					YieldDefault(1);
					goto case 1;
				case 1:
					{
						result = 0;
						break;
					}
					IL_011c:
					if (_0024i_0024109 <= 1f)
					{
						_0024self__0024110.menuObject.transform.position = Vector3.Lerp(_0024startPos_0024106, _0024endPos_0024107, _0024i_0024109);
						_0024self__0024110.menuObject.GetComponent<Renderer>().material.color = Color.Lerp(Color.white, _0024endColor_0024108, _0024i_0024109);
						result = (YieldDefault(2) ? 1 : 0);
					}
					else
					{
						result = (Yield(3, new WaitForSeconds(1.5f)) ? 1 : 0);
					}
					break;
				}
				return (byte)result != 0;
			}
		}

		internal MenuObject _0024self__0024111;

		public _0024OnMouseDown_0024105(MenuObject self_)
		{
			_0024self__0024111 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024self__0024111);
		}
	}

	public GameObject highlight;

	public float rotateSpeed;

	public Transform menuObject;

	public virtual void OnMouseOver()
	{
		if (!MenuControl.clicked)
		{
			menuObject.Rotate(Vector3.right * Time.deltaTime * rotateSpeed);
			highlight.SetActive(true);
			float y = transform.position.y;
			Vector3 position = highlight.transform.position;
			float num = (position.y = y);
			Vector3 vector2 = (highlight.transform.position = position);
		}
	}

	public virtual void OnMouseExit()
	{
		if (!MenuControl.clicked)
		{
			highlight.SetActive(false);
			menuObject.eulerAngles = Vector3.zero;
		}
	}

	public virtual IEnumerator OnMouseDown()
	{
		return new _0024OnMouseDown_0024105(this).GetEnumerator();
	}

	public virtual void Main()
	{
	}
}
