using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Boo.Lang;
using UnityEngine;

[Serializable]
public class GameOverControl : MonoBehaviour
{
	[Serializable]
	[CompilerGenerated]
	internal sealed class _0024Start_002462 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		[CompilerGenerated]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal Transform _0024gameOverText_002463;

			internal Rigidbody[] _0024rigidbodies_002464;

			internal Rigidbody _0024rb_002465;

			internal float _0024i_002466;

			internal Rigidbody _0024rb_002467;

			internal int _0024_002412_002468;

			internal Rigidbody[] _0024_002413_002469;

			internal int _0024_002414_002470;

			internal int _0024_002416_002471;

			internal Rigidbody[] _0024_002417_002472;

			internal int _0024_002418_002473;

			internal float _0024_002440_002474;

			internal Vector3 _0024_002441_002475;

			internal float _0024_002442_002476;

			internal Vector3 _0024_002443_002477;

			internal GameOverControl _0024self__002478;

			public _0024(GameOverControl self_)
			{
				_0024self__002478 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					RenderSettings.fog = true;
					RenderSettings.fogColor = Camera.main.backgroundColor;
					RenderSettings.fogMode = FogMode.Linear;
					RenderSettings.fogEndDistance = 20f;
					goto case 5;
				case 5:
				{
					_0024gameOverText_002463 = FlyingText.GetObjects("GAME<br>OVER").transform;
					float num = (_0024_002440_002474 = -6.5f);
					Vector3 vector = (_0024_002441_002475 = _0024gameOverText_002463.position);
					float num2 = (_0024_002441_002475.z = _0024_002440_002474);
					Vector3 vector3 = (_0024gameOverText_002463.position = _0024_002441_002475);
					_0024rigidbodies_002464 = _0024gameOverText_002463.GetComponentsInChildren<Rigidbody>();
					_0024_002412_002468 = 0;
					_0024_002413_002469 = _0024rigidbodies_002464;
					for (_0024_002414_002470 = _0024_002413_002469.Length; _0024_002412_002468 < _0024_002414_002470; _0024_002412_002468++)
					{
						_0024_002413_002469[_0024_002412_002468].useGravity = false;
					}
					_0024i_002466 = 0f;
					goto IL_01ad;
				}
				case 2:
					_0024i_002466 += Time.deltaTime;
					goto IL_01ad;
				case 3:
					UnityEngine.Object.Instantiate(_0024self__002478.explosion, new Vector3(0f, 1f, -6.3f), Quaternion.identity);
					_0024_002416_002471 = 0;
					_0024_002417_002472 = _0024rigidbodies_002464;
					for (_0024_002418_002473 = _0024_002417_002472.Length; _0024_002416_002471 < _0024_002418_002473; _0024_002416_002471++)
					{
						_0024_002417_002472[_0024_002416_002471].useGravity = true;
						_0024_002417_002472[_0024_002416_002471].AddExplosionForce(220f, new Vector3(0f, 1f, -6.5f), 10f, 9f);
					}
					result = (Yield(4, new WaitForSeconds(5f)) ? 1 : 0);
					break;
				case 4:
					UnityEngine.Object.Destroy(_0024gameOverText_002463.gameObject);
					result = (Yield(5, new WaitForSeconds(1f)) ? 1 : 0);
					break;
				case 1:
					{
						result = 0;
						break;
					}
					IL_01ad:
					if (_0024i_002466 < 1f)
					{
						float num3 = (_0024_002442_002476 = Mathf.Lerp(5f, -0.05f, _0024i_002466));
						Vector3 vector4 = (_0024_002443_002477 = _0024gameOverText_002463.position);
						float num4 = (_0024_002443_002477.y = _0024_002442_002476);
						Vector3 vector6 = (_0024gameOverText_002463.position = _0024_002443_002477);
						result = (YieldDefault(2) ? 1 : 0);
					}
					else
					{
						_0024self__002478.StartCoroutine(_0024self__002478.CameraShake(Camera.main));
						result = (Yield(3, new WaitForSeconds(1.75f)) ? 1 : 0);
					}
					break;
				}
				return (byte)result != 0;
			}
		}

		internal GameOverControl _0024self__002479;

		public _0024Start_002462(GameOverControl self_)
		{
			_0024self__002479 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024self__002479);
		}
	}

	[Serializable]
	[CompilerGenerated]
	internal sealed class _0024CameraShake_002480 : GenericGenerator<object>
	{
		[Serializable]
		[CompilerGenerated]
		internal sealed class _0024 : GenericGeneratorEnumerator<object>, IEnumerator
		{
			internal float _0024originalPosition_002481;

			internal int _0024shakeCounter_002482;

			internal float _0024shakeDistance_002483;

			internal float _0024timer_002484;

			internal float _0024_002444_002485;

			internal Vector3 _0024_002445_002486;

			internal float _0024_002446_002487;

			internal Vector3 _0024_002447_002488;

			internal Camera _0024cam_002489;

			internal GameOverControl _0024self__002490;

			public _0024(Camera cam, GameOverControl self_)
			{
				_0024cam_002489 = cam;
				_0024self__002490 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					_0024originalPosition_002481 = _0024cam_002489.transform.localPosition.y;
					_0024shakeCounter_002482 = _0024self__002490.numberOfShakes;
					_0024shakeDistance_002483 = _0024self__002490.startingShakeDistance;
					_0024timer_002484 = 0f;
					goto case 2;
				case 2:
				{
					if (_0024shakeCounter_002482 > 0)
					{
						float num = (_0024_002444_002485 = _0024originalPosition_002481 + Mathf.Sin(_0024timer_002484) * _0024shakeDistance_002483);
						Vector3 vector = (_0024_002445_002486 = _0024cam_002489.transform.localPosition);
						float num2 = (_0024_002445_002486.y = _0024_002444_002485);
						Vector3 vector3 = (_0024cam_002489.transform.localPosition = _0024_002445_002486);
						_0024timer_002484 += Time.deltaTime * _0024self__002490.shakeSpeed;
						if (!(_0024timer_002484 <= (float)Math.PI * 2f))
						{
							_0024timer_002484 = 0f;
							_0024shakeDistance_002483 *= _0024self__002490.decreasePercentage;
							_0024shakeCounter_002482--;
						}
						result = (YieldDefault(2) ? 1 : 0);
						break;
					}
					float num3 = (_0024_002446_002487 = _0024originalPosition_002481);
					Vector3 vector4 = (_0024_002447_002488 = _0024cam_002489.transform.localPosition);
					float num4 = (_0024_002447_002488.y = _0024_002446_002487);
					Vector3 vector6 = (_0024cam_002489.transform.localPosition = _0024_002447_002488);
					YieldDefault(1);
					goto case 1;
				}
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal Camera _0024cam_002491;

		internal GameOverControl _0024self__002492;

		public _0024CameraShake_002480(Camera cam, GameOverControl self_)
		{
			_0024cam_002491 = cam;
			_0024self__002492 = self_;
		}

		public override IEnumerator<object> GetEnumerator()
		{
			return new _0024(_0024cam_002491, _0024self__002492);
		}
	}

	public GameObject explosion;

	public float startingShakeDistance;

	public float decreasePercentage;

	public float shakeSpeed;

	public int numberOfShakes;

	public GameOverControl()
	{
		startingShakeDistance = 0.4f;
		decreasePercentage = 0.5f;
		shakeSpeed = 40f;
		numberOfShakes = 3;
	}

	public virtual IEnumerator Start()
	{
		return new _0024Start_002462(this).GetEnumerator();
	}

	public virtual IEnumerator CameraShake(Camera cam)
	{
		return new _0024CameraShake_002480(cam, this).GetEnumerator();
	}

	public virtual void Main()
	{
	}
}
