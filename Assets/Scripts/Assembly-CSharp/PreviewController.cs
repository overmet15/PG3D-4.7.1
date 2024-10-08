using System;
using System.Collections;
using System.Collections.Generic;
using Holoville.HOTween;
using UnityEngine;

public sealed class PreviewController : MonoBehaviour
{
	public delegate void EditModeEntered();

	public delegate void PartSelected(int partNumber);

	public string arrNameSkin_sett = "arrNameSkin";

	public EditModeEntered editModeEnteredDelegate;

	public PartSelected _partEslected;

	private ArrayList arrNameSkin = new ArrayList();

	public int CurrentTextureIndex;

	public static Vector3 RotationInMuktProfile = new Vector3(0f, 38.28f, 0f);

	public ICollection<GoogleSkuInfo> _products = new GoogleSkuInfo[0];

	public GameObject _purchaseActivityIndicator;

	public GameObject ModelPrefab;

	private Dictionary<int, PanTouchInfo> _panTouches = new Dictionary<int, PanTouchInfo>();

	private Dictionary<int, TapInfo> _tapTouches = new Dictionary<int, TapInfo>();

	private Vector3 rememberedScale;

	private float _scaleModif = 1.25f;

	private float[] bodyYs = new float[2] { 0f, -0.3f };

	private Vector3 _bodyOffset;

	private static string _bodyName = "Body";

	private Vector3 rememberedBodyOffs;

	private float _timeOfLastTapOnChar;

	public bool isEditingMode;

	public bool Locked;

	private GameObject _controller;

	private SpisokSkinov _spisokSkinov;

	private ViborChastiTela _viborChastiTela;

	private Transform _cape;

	private Transform _hat;

	private bool IsEditingMode
	{
		get
		{
			return !_spisokSkinov.showEnabled;
		}
	}

	public Dictionary<int, PanTouchInfo> PanTouches
	{
		get
		{
			return _panTouches;
		}
	}

	public Dictionary<int, TapInfo> TapTouches
	{
		get
		{
			return _tapTouches;
		}
	}

	public static void SetTextureRecursivelyFrom(GameObject obj, Texture txt, GameObject[] stopObjs = null)
	{
		if (stopObjs == null)
		{
			stopObjs = new GameObject[0];
		}
		foreach (Transform item in obj.transform)
		{
			bool flag = false;
			GameObject[] array = stopObjs;
			foreach (GameObject gameObject in array)
			{
				if (item.gameObject == gameObject)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				continue;
			}
			if ((bool)item.gameObject.GetComponent<Renderer>() && (bool)item.gameObject.GetComponent<Renderer>().material)
			{
				if (item.gameObject.GetComponent<Renderer>().materials.Length > 1)
				{
					Material[] materials = item.gameObject.GetComponent<Renderer>().materials;
					foreach (Material material in materials)
					{
						material.mainTexture = txt;
					}
				}
				else
				{
					item.gameObject.GetComponent<Renderer>().material.mainTexture = txt;
				}
			}
			Texture2D texture2D = (Texture2D)txt;
			texture2D.filterMode = FilterMode.Point;
			flag = false;
			GameObject[] array2 = stopObjs;
			foreach (GameObject gameObject2 in array2)
			{
				if (item.gameObject == gameObject2)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				SetTextureRecursivelyFrom(item.gameObject, texture2D, stopObjs);
			}
		}
	}

	public void ResetState()
	{
		foreach (TapInfo value in TapTouches.Values)
		{
			Unhighlight(value.TappedCollider.gameObject);
		}
		TapTouches.Clear();
		PanTouches.Clear();
		base.transform.rotation = ((PlayerPrefs.GetInt(Defs.SkinEditorMode, 0) != 1) ? Quaternion.identity : Quaternion.Euler(RotationInMuktProfile));
	}

	public void ShowSkin(int idx)
	{
		CurrentTextureIndex = idx;
		SetTextureWithIndex(base.gameObject, CurrentTextureIndex);
		ResetState();
	}

	public void move(int dir)
	{
		int count = arrNameSkin.Count;
		int num = -1;
		if (dir == 1 * num)
		{
			CurrentTextureIndex++;
			if (CurrentTextureIndex >= count)
			{
				CurrentTextureIndex = 0;
			}
		}
		else
		{
			CurrentTextureIndex--;
			if (CurrentTextureIndex < 0)
			{
				CurrentTextureIndex = count - 1;
			}
		}
		SetTextureWithIndex(base.gameObject, CurrentTextureIndex);
		StartCoroutine(clearAssets());
		ResetState();
		if (PlayerPrefs.GetInt(Defs.SkinEditorMode, 0) == 1)
		{
			base.transform.rotation = Quaternion.Euler(RotationInMuktProfile);
		}
		Locked = false;
	}

	private IEnumerator clearAssets()
	{
		Font f = coinsPlashka.thisScript.stLabelCoins.font;
		yield return Resources.UnloadUnusedAssets();
	}

	public GameObject[] _CurrentStopObjs()
	{
		if (PlayerPrefs.GetInt(Defs.SkinEditorMode, 0) == 0)
		{
			return new GameObject[0];
		}
		if (_hat != null && _cape != null)
		{
			return new GameObject[2] { _hat.gameObject, _cape.gameObject };
		}
		if (_hat != null)
		{
			return new GameObject[1] { _hat.gameObject };
		}
		if (_cape != null)
		{
			return new GameObject[1] { _cape.gameObject };
		}
		return new GameObject[0];
	}

	private IEnumerator Start()
	{
		_purchaseActivityIndicator = StoreKitEventListener.purchaseActivityInd;
		if (_purchaseActivityIndicator == null)
		{
			Debug.LogWarning("_purchaseActivityIndicator == null");
		}
		else
		{
			_purchaseActivityIndicator.SetActive(false);
		}
		if (PlayerPrefs.GetInt(Defs.SkinEditorMode, 0) == 1)
		{
			base.transform.rotation = Quaternion.Euler(RotationInMuktProfile);
			CurrentTextureIndex = SpisokSkinov.EquippedSkinIndexToPreviewControllerIndex();
		}
		if (PlayerPrefs.GetInt(Defs.SkinEditorMode, 0) == 1)
		{
			GameObject sel = GameObject.FindGameObjectWithTag("InAppGameObject");
			StoreKitEventListener skel = sel.GetComponent<StoreKitEventListener>();
			_products = skel._skinProducts;
		}
		_controller = GameObject.Find("Controller");
		_spisokSkinov = _controller.GetComponent<SpisokSkinov>();
		_viborChastiTela = _controller.GetComponent<ViborChastiTela>();
		if (PlayerPrefs.GetInt(Defs.SkinEditorMode, 0) == 0)
		{
			Debug.Log("preview controller Start()");
		}
		updateSpisok();
		if (CurrentTextureIndex >= 0 && arrNameSkin.Count > 0)
		{
			SetTextureRecursivelyFrom(base.gameObject, SkinsManager.TextureForName((string)arrNameSkin[CurrentTextureIndex]), _CurrentStopObjs());
		}
		HOTween.Init(true, true, true);
		HOTween.EnableOverwriteManager();
		_bodyOffset = GameObject.Find(_bodyName).transform.InverseTransformPoint(new Vector3(0f, bodyYs[1], 0f));
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		_AddCapeAndHat();
	}

	private void _AddCapeAndHat()
	{
		if (PlayerPrefs.GetInt(Defs.SkinEditorMode, 0) != 0)
		{
			_cape = GameObject.Find("Cape").transform;
			_hat = GameObject.Find("Hat").transform;
			string @string = Storager.getString(Defs.CapeEquppedSN, false);
			if (!@string.Equals(Defs.CapeNoneEqupped))
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load(ResPath.Combine(Defs.CapesDir, @string)) as GameObject, Vector3.zero, Quaternion.identity) as GameObject;
				gameObject.transform.parent = _cape;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localRotation = Quaternion.identity;
			}
			string string2 = Storager.getString(Defs.HatEquppedSN, false);
			if (!string2.Equals(Defs.HatNoneEqupped))
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate(Resources.Load(ResPath.Combine(Defs.HatsDir, string2)) as GameObject, Vector3.zero, Quaternion.identity) as GameObject;
				gameObject2.transform.parent = _hat;
				gameObject2.transform.localPosition = Vector3.zero;
				gameObject2.transform.localRotation = Quaternion.identity;
			}
		}
	}

	public void updateSpisok()
	{
		Debug.Log("updatespisok  arrNameSkin.Count: " + arrNameSkin.Count + "\narrNameSkin: " + arrNameSkin);
		if (arrNameSkin != null && arrNameSkin.Count > 0)
		{
			arrNameSkin.Clear();
		}
		string[] array = Load.LoadStringArray(arrNameSkin_sett);
		Debug.Log("updatespisok 2 arrNameSkin.Count: " + arrNameSkin.Count + "\narrNameSkin: " + arrNameSkin);
		if (arrNameSkin != null && array != null)
		{
			string[] array2 = array;
			foreach (string text in array2)
			{
				if (!arrNameSkin.Contains(text))
				{
					Debug.Log("ADDED: " + text);
					arrNameSkin.Add(text);
				}
				else
				{
					Debug.LogWarning("DUPLICATE: " + text);
				}
			}
		}
		Debug.Log("updatespisok 3 arrNameSkin.Count: " + arrNameSkin.Count + "\narrNameSkin: " + arrNameSkin);
	}

	public void Highlight(GameObject go)
	{
		rememberedScale = go.transform.localScale;
		rememberedBodyOffs = Vector3.zero;
		if (go.name.Equals(_bodyName))
		{
			rememberedBodyOffs = go.transform.TransformPoint(_bodyOffset);
		}
		go.transform.localScale *= _scaleModif;
		go.transform.position += rememberedBodyOffs;
	}

	public void Unhighlight(GameObject go)
	{
		go.transform.localScale = rememberedScale;
		if (go.name.Equals(_bodyName))
		{
			go.transform.position -= rememberedBodyOffs;
		}
	}

	public bool TouchOnModel(Touch touch)
	{
		return Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(touch.position.x, touch.position.y, 0f)));
	}

	private Collider CheckTap(Touch touch)
	{
		Collider result = null;
		Ray ray = Camera.main.ScreenPointToRay(new Vector3(touch.position.x, touch.position.y, 0f));
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, 100f, -5))
		{
			result = hitInfo.collider;
		}
		return result;
	}

	public void ColliderSelected(Collider collider)
	{
		int partNumber = 0;
		switch (collider.gameObject.name)
		{
		case "Body":
			partNumber = 2;
			break;
		case "Foot_left":
			partNumber = 1;
			break;
		case "Foot_right":
			partNumber = 1;
			break;
		case "Arm_left":
			partNumber = 3;
			break;
		case "Arm_right":
			partNumber = 3;
			break;
		}
		if (_partEslected != null)
		{
			_partEslected(partNumber);
		}
	}

	private void SetTextureWithIndex(GameObject tmpMan, int ind)
	{
		Transform transform = null;
		Transform transform2 = null;
		if (PlayerPrefs.GetInt(Defs.SkinEditorMode, 0) == 1)
		{
			if ((bool)_hat && _hat != null && _hat.childCount > 0)
			{
				transform = _hat.GetChild(0);
				transform.parent = null;
			}
			if ((bool)_cape && _cape != null && _cape.childCount > 0)
			{
				transform2 = _cape.GetChild(0);
				transform2.parent = null;
			}
		}
		Texture txt = SkinsManager.TextureForName((string)arrNameSkin[ind]);
		SetTextureRecursivelyFrom(tmpMan, txt, _CurrentStopObjs());
		if (PlayerPrefs.GetInt(Defs.SkinEditorMode, 0) == 1)
		{
			if ((bool)_hat && _hat != null && transform != null)
			{
				transform.parent = _hat;
			}
			if ((bool)_cape && _cape != null && transform2 != null)
			{
				transform2.parent = _cape;
			}
		}
	}

	private void Update()
	{
		_purchaseActivityIndicator.SetActive(StoreKitEventListener.purchaseInProcess);
		if (Locked)
		{
			return;
		}
		float num = 25f;
		float num2 = 35f;
		Rect rect = new Rect(num, num2, (float)Screen.width - num * 2f, (float)Screen.height - num2 * 2f);
		Touch[] touches = Input.touches;
		for (int i = 0; i < touches.Length; i++)
		{
			Touch touch = touches[i];
			if (touch.phase == TouchPhase.Began)
			{
				if (!rect.Contains(touch.position))
				{
					continue;
				}
				if (TouchOnModel(touch) && IsEditingMode)
				{
					if (TapTouches.Count == 0 && Time.realtimeSinceStartup - _timeOfLastTapOnChar > 1.5f)
					{
						TapInfo tapInfo = new TapInfo();
						tapInfo.TappedCollider = CheckTap(touch);
						if (TapTouches.ContainsKey(touch.fingerId))
						{
							TapTouches.Remove(touch.fingerId);
						}
						TapTouches.Add(touch.fingerId, tapInfo);
						Highlight(tapInfo.TappedCollider.gameObject);
					}
					continue;
				}
				PanTouchInfo panTouchInfo = new PanTouchInfo();
				panTouchInfo.FingerPos = Vector2.zero;
				panTouchInfo.FingerLastPos = Vector2.zero;
				panTouchInfo.FingerMovedBy = Vector2.zero;
				float slideMagnitudeX = (panTouchInfo.SlideMagnitudeY = 0f);
				panTouchInfo.SlideMagnitudeX = slideMagnitudeX;
				panTouchInfo.StartTime = Time.realtimeSinceStartup;
				if (PanTouches.ContainsKey(touch.fingerId))
				{
					PanTouches.Remove(touch.fingerId);
				}
				PanTouches.Add(touch.fingerId, panTouchInfo);
				panTouchInfo.FingerPos = touch.position;
				panTouchInfo.InitialTouchPos = panTouchInfo.FingerPos;
			}
			else if (touch.phase == TouchPhase.Moved)
			{
				if (TapTouches.ContainsKey(touch.fingerId))
				{
					if (!(touch.deltaPosition.magnitude > 10f))
					{
						continue;
					}
					PanTouchInfo panTouchInfo2 = new PanTouchInfo();
					panTouchInfo2.FingerPos = Vector2.zero;
					panTouchInfo2.FingerLastPos = Vector2.zero;
					panTouchInfo2.FingerMovedBy = Vector2.zero;
					float slideMagnitudeX = (panTouchInfo2.SlideMagnitudeY = 0f);
					panTouchInfo2.SlideMagnitudeX = slideMagnitudeX;
					panTouchInfo2.StartTime = Time.realtimeSinceStartup;
					if (PanTouches.ContainsKey(touch.fingerId))
					{
						PanTouches.Remove(touch.fingerId);
					}
					PanTouches.Add(touch.fingerId, panTouchInfo2);
					panTouchInfo2.FingerPos = touch.position - touch.deltaPosition;
					panTouchInfo2.InitialTouchPos = panTouchInfo2.FingerPos;
					Unhighlight(TapTouches[touch.fingerId].TappedCollider.gameObject);
					TapTouches.Remove(touch.fingerId);
				}
				if (PanTouches.ContainsKey(touch.fingerId))
				{
					PanTouches[touch.fingerId].FingerMovedBy = touch.position - PanTouches[touch.fingerId].FingerPos;
					PanTouches[touch.fingerId].FingerLastPos = PanTouches[touch.fingerId].FingerPos;
					PanTouches[touch.fingerId].FingerPos = touch.position;
					PanTouches[touch.fingerId].SlideMagnitudeX = PanTouches[touch.fingerId].FingerMovedBy.x;
					PanTouches[touch.fingerId].SlideMagnitudeY = PanTouches[touch.fingerId].FingerMovedBy.y;
					if (IsEditingMode || PlayerPrefs.GetInt(Defs.SkinEditorMode, 0) == 1)
					{
						float num5 = 0.5f;
						base.gameObject.transform.Rotate(0f, (0f - num5) * PanTouches[touch.fingerId].SlideMagnitudeX, 0f, (PlayerPrefs.GetInt(Defs.SkinEditorMode, 0) == 1) ? Space.Self : Space.World);
					}
				}
			}
			else if (touch.phase == TouchPhase.Stationary)
			{
				if (!TapTouches.ContainsKey(touch.fingerId) && PanTouches.ContainsKey(touch.fingerId))
				{
					PanTouches[touch.fingerId].FingerLastPos = PanTouches[touch.fingerId].FingerPos;
					PanTouches[touch.fingerId].FingerPos = touch.position;
					PanTouches[touch.fingerId].SlideMagnitudeX = 0f;
					PanTouches[touch.fingerId].SlideMagnitudeY = 0f;
				}
			}
			else
			{
				if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
				{
					continue;
				}
				if (TapTouches.ContainsKey(touch.fingerId))
				{
					if (touch.phase == TouchPhase.Ended)
					{
						ColliderSelected(TapTouches[touch.fingerId].TappedCollider);
					}
					Unhighlight(TapTouches[touch.fingerId].TappedCollider.gameObject);
					TapTouches.Remove(touch.fingerId);
				}
				else
				{
					if (!PanTouches.ContainsKey(touch.fingerId))
					{
						continue;
					}
					if (!IsEditingMode && touch.phase == TouchPhase.Ended)
					{
						if (TouchOnModel(touch) && (touch.position - PanTouches[touch.fingerId].InitialTouchPos).magnitude < 15f && Time.realtimeSinceStartup - PanTouches[touch.fingerId].StartTime < 0.45f)
						{
							if (editModeEnteredDelegate != null && PlayerPrefs.GetInt(Defs.SkinEditorMode, 0) != 1)
							{
								editModeEnteredDelegate();
							}
							if (PlayerPrefs.GetInt(Defs.SkinEditorMode, 0) != 1)
							{
								ResetState();
							}
							break;
						}
						if (Mathf.Abs((PanTouches[touch.fingerId].InitialTouchPos - touch.position).x) > (float)(Screen.width / 10) && !(Time.realtimeSinceStartup - PanTouches[touch.fingerId].StartTime < 2.5f))
						{
						}
					}
					PanTouches.Remove(touch.fingerId);
				}
			}
		}
	}

	public void TestDelegate(int pn)
	{
		Debug.Log(pn);
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	public void PurchaseSuccessful(string id)
	{
		if (Array.IndexOf(StoreKitEventListener.skinIDs, id) >= 0)
		{
			Storager.setInt(InAppData.inAppData[CurrentTextureIndex].Value, 1, true);
		}
		Locked = false;
		string eventName = ((!InAppData.inappReadableNames.ContainsKey(id)) ? id : InAppData.inappReadableNames[id]);
		FlurryAndroid.logEvent(eventName, false);
	}

	private void purchaseCancelled(string err)
	{
		Locked = false;
	}

	private void purchaseFailed(string err)
	{
		Locked = false;
	}
}
