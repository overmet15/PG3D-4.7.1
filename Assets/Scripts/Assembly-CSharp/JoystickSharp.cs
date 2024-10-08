using System.Collections;
using UnityEngine;

public class JoystickSharp : MonoBehaviour
{
	private class Boundary
	{
		public Vector2 min = Vector2.zero;

		public Vector2 max = Vector2.zero;
	}

	private static JoystickSharp[] joysticks;

	private static bool enumeratedJoysticks;

	private static float tapTimeDelta = 0.3f;

	public Texture2D obodok;

	public bool touchPad;

	public Rect touchZone;

	public Vector2 deadZone = Vector2.zero;

	public bool normalize;

	public Vector2 position;

	public int tapCount;

	public bool halfScreenZone;

	private bool _moveReceived;

	private int lastFingerId = -1;

	private float tapTimeWindow;

	public Vector2 fingerDownPos;

	private float fingerDownTime;

	private float firstDeltaTime = 0.5f;

	private Rect defaultRect;

	private Boundary guiBoundary;

	private Vector2 guiTouchOffset;

	private Vector2 guiCenter;

	public bool jumpPressed;

	public Texture fireTexture;

	public Texture reloadTexture;

	public Texture reloadTextureNoAmmo;

	private Rect fireZone;

	private Rect zoomZone;

	public GameObject _playerGun;

	private Rect reloadZone;

	private Rect joystickZone;

	private bool blink;

	private bool NormalReloadMode = true;

	private bool isSerialShooting;

	public Vector3 pos = new Vector3(0f, 0f, 0f);

	private WeaponManager _weaponManager;

	private Rect guiPixelInset;

	private Rect jumpTexturePixelInset;

	private Texture gui;

	private Texture jumpTexture;

	public Texture zoomTexture;

	private float guiCoeff = (float)Screen.height / 640f;

	private bool touchBeginsOnFireZone;

	private bool _isFirstFrame = true;

	public void NoAmmo()
	{
		if (NormalReloadMode)
		{
			NormalReloadMode = false;
			StartCoroutine("BlinkReload");
		}
	}

	public void HasAmmo()
	{
		if (!NormalReloadMode)
		{
			NormalReloadMode = true;
			StopCoroutine("BlinkReload");
			blink = false;
		}
	}

	private IEnumerator BlinkReload()
	{
		while (true)
		{
			yield return new WaitForSeconds(0.5f);
			blink = !blink;
		}
	}

	private void Awake()
	{
		_weaponManager = GameObject.FindGameObjectWithTag("WeaponManager").GetComponent<WeaponManager>();
	}

	private void Start()
	{
		WindowsMouseManager.Instance.SetMouseLock(true);
		if (!touchPad)
		{
			guiPixelInset = new Rect(0f, 0f, 128f, 128f);
			gui = Resources.Load("Move") as Texture;
		}
		else
		{
			guiPixelInset = new Rect(-200f, 0f, 200f, 125f);
			gui = Resources.Load("Jump") as Texture;
		}
		if (touchPad)
		{
			base.transform.position = new Vector3(1f, base.transform.position.y, base.transform.position.z);
		}
		guiPixelInset = new Rect(guiPixelInset.x * (float)Screen.height / 640f, guiPixelInset.y * (float)Screen.height / 640f, guiPixelInset.width * (float)Screen.height / 640f, guiPixelInset.height * (float)Screen.height / 640f);
		defaultRect = guiPixelInset;
		defaultRect.x += pos.x * (float)Screen.width;
		defaultRect.y += pos.y * (float)Screen.height;
		float num = 1.2f;
		if (halfScreenZone)
		{
			defaultRect.y = 0f;
			defaultRect.x = (float)Screen.width / 2f;
			defaultRect.width = (float)Screen.width / 2f;
			defaultRect.height = (float)Screen.height * 0.6f;
			jumpTexture = gui;
			float num2 = (num - 1f) * 0.5f;
			jumpTexture = gui;
			jumpTexturePixelInset = new Rect((float)Screen.width - (float)jumpTexture.width * (num2 + 1f) * (float)Screen.height / 640f, (float)(jumpTexture.height * Screen.height / 640) * num2 / 2f, jumpTexture.width * Screen.height / 640, jumpTexture.height * Screen.height / 640);
			guiPixelInset = jumpTexturePixelInset;
			float num3 = fireTexture.width * Screen.height / 640;
			fireZone = new Rect((float)Screen.width - (float)Screen.height * 0.4f, (float)Screen.height * 0.15f - num3 / 2f, num3, num3);
			zoomZone = new Rect((float)Screen.width - (float)Screen.height * 0.4f, (float)Screen.height * 0.55f - num3 / 2f, num3, num3);
			if ((bool)reloadTexture)
			{
				reloadZone = new Rect((float)Screen.width - (float)reloadTexture.width * 1.1f * (float)Screen.height / 640f, (float)Screen.height * 0.4f, fireZone.width * 0.65f, fireZone.height * 0.65f);
			}
		}
		else if ((bool)reloadTexture)
		{
			reloadZone = new Rect((float)Screen.width - (float)reloadTexture.width * 1.1f * (float)Screen.height / 640f, (float)Screen.height * 0.4f, fireZone.width * 0.65f, fireZone.height * 0.65f);
		}
		pos.x = 0f;
		pos.y = 0f;
		if (touchPad)
		{
			touchZone = defaultRect;
		}
		else
		{
			joystickZone = new Rect(0f, 0f, (float)Screen.width / 2f, (float)Screen.height / 2f);
			defaultRect = guiPixelInset;
			defaultRect.x = (float)Screen.height * 0.1f;
			defaultRect.y = (float)Screen.height * 0.1f;
			guiTouchOffset.x = defaultRect.width * 0.5f;
			guiTouchOffset.y = defaultRect.height * 0.5f;
			guiCenter.x = defaultRect.x + guiTouchOffset.x;
			guiCenter.y = defaultRect.y + guiTouchOffset.y;
			guiBoundary = new Boundary();
			guiBoundary.min = new Vector2(defaultRect.x - guiTouchOffset.x, defaultRect.y - guiTouchOffset.y);
			guiBoundary.max = new Vector2(defaultRect.x + guiTouchOffset.x, defaultRect.y + guiTouchOffset.y);
		}
		StartCoroutine(_SetIsFirstFrame());
	}

	private IEnumerator _SetIsFirstFrame()
	{
		yield return new WaitForSeconds(0.1f);
		_isFirstFrame = false;
	}

	public void Disable()
	{
		base.gameObject.active = false;
		enumeratedJoysticks = false;
	}

	private void Enable()
	{
		base.gameObject.active = true;
	}

	private void ResetJoystick()
	{
		if ((!halfScreenZone || !touchPad || !touchPad) && (bool)gui)
		{
			guiPixelInset = defaultRect;
		}
		lastFingerId = -1;
		if (touchPad)
		{
			_moveReceived = false;
		}
		position = Vector2.zero;
		fingerDownPos = Vector2.zero;
	}

	private bool IsFingerDown()
	{
		return lastFingerId != -1;
	}

	private void LatchedFinger(int fingerId)
	{
		if (lastFingerId == fingerId)
		{
			ResetJoystick();
		}
	}

	private void Update()
	{
		//PC Stuff
		if (touchPad) // Avoid doubling of inputs and stuff
        {
            if (Input.GetMouseButton(0) && WindowsMouseManager.Instance.MouseLocked && (!Defs.IsTraining || TrainingController.stepTraining >= TrainingController.stepTrainingList["TapToShoot"]))
            {
                _playerGun.GetComponent<Player_move_c>().ShotPressed();
            }
            if (Input.GetKeyDown(KeyCode.R) && (!Defs.IsTraining || TrainingController.stepTraining >= TrainingController.stepTrainingList["TapToShoot"]))
            {
                _playerGun.GetComponent<Player_move_c>().ReloadPressed();
            }
            if (Input.GetMouseButtonDown(1) && _weaponManager.currentWeaponSounds != null && _weaponManager.currentWeaponSounds.isZooming)
            {
                _playerGun.GetComponent<Player_move_c>().ZoomPress();
            }
            if (Input.GetKeyDown(KeyCode.Space)) jumpPressed = true;
        }

		if (!enumeratedJoysticks)
		{
			joysticks = Object.FindObjectsOfType(typeof(JoystickSharp)) as JoystickSharp[];
			enumeratedJoysticks = true;
		}
		int touchCount = Input.touchCount;
		if (tapTimeWindow > 0f)
		{
			tapTimeWindow -= Time.deltaTime;
		}
		else
		{
			tapCount = 0;
		}
		if (touchCount == 0)
		{
			ResetJoystick();
		}
		else if (!_isFirstFrame)
		{
			for (int i = 0; i < touchCount; i++)
			{
				Touch touch = Input.GetTouch(i);
				Vector2 vector = touch.position - guiTouchOffset;
				bool flag = false;
				if (touchPad)
				{
					if (touchZone.Contains(touch.position))
					{
						flag = true;
					}
				}
				else if (guiPixelInset.Contains(touch.position))
				{
					flag = true;
				}
				isSerialShooting = PlayerPrefs.GetInt("setSeriya") == 1;
				bool flag2 = flag && (lastFingerId == -1 || lastFingerId != touch.fingerId);
				if (flag2)
				{
					touchBeginsOnFireZone = fireZone.Contains(touch.position);
				}
				if (isSerialShooting && touchPad && flag)
				{
					if ((bool)fireTexture && touchZone.Contains(touch.position) && touchBeginsOnFireZone && !blink)
					{
						if (!Defs.IsTraining || TrainingController.stepTraining >= TrainingController.stepTrainingList["TapToShoot"])
						{
							_playerGun.GetComponent<Player_move_c>().ShotPressed();
						}
					}
					else
					{
						touchBeginsOnFireZone = false;
					}
				}
				if (flag2)
				{
					if (touchPad)
					{
						_moveReceived = false;
					}
					lastFingerId = touch.fingerId;
					if (tapTimeWindow > 0f)
					{
						tapCount++;
					}
					else
					{
						tapCount = 1;
						tapTimeWindow = tapTimeDelta;
					}
					JoystickSharp[] array = joysticks;
					foreach (JoystickSharp joystickSharp in array)
					{
						if (joystickSharp != this)
						{
							joystickSharp.LatchedFinger(touch.fingerId);
						}
					}
					if ((bool)fireTexture && fireZone.Contains(touch.position) && !isSerialShooting)
					{
						Debug.Log("FireZone contains");
						if (!Defs.IsTraining || TrainingController.stepTraining >= TrainingController.stepTrainingList["TapToShoot"])
						{
							_playerGun.GetComponent<Player_move_c>().ShotPressed();
						}
						continue;
					}
					if ((bool)jumpTexture && jumpTexturePixelInset.Contains(touch.position) && (!Defs.IsTraining || TrainingController.stepTraining >= TrainingController.stepTrainingList["GetTheCoin"]))
					{
						jumpPressed = true;
					}
					if (touchPad && reloadZone.Contains(touch.position) && (!Defs.IsTraining || TrainingController.stepTraining >= TrainingController.stepTrainingList["TapToShoot"]))
					{
						_playerGun.GetComponent<Player_move_c>().ReloadPressed();
					}
					if (touchPad && _weaponManager.currentWeaponSounds != null && _weaponManager.currentWeaponSounds.isZooming && zoomZone.Contains(touch.position))
					{
						_playerGun.GetComponent<Player_move_c>().ZoomPress();
					}
				}
				if (lastFingerId == touch.fingerId && ((touchPad && _moveReceived && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)) || !touchPad))
				{
					if (touch.tapCount > tapCount)
					{
						tapCount = touch.tapCount;
					}
					if (touchPad)
					{
						float num = 25f;
						position.x = Mathf.Clamp((touch.position.x - fingerDownPos.x) * 1f / 1f, 0f - num, num);
						position.y = Mathf.Clamp((touch.position.y - fingerDownPos.y) * 1f / 1f, 0f - num, num);
						fingerDownPos = touch.position;
					}
					else
					{
						guiPixelInset.x = Mathf.Clamp(vector.x, guiBoundary.min.x, guiBoundary.max.x);
						guiPixelInset.y = Mathf.Clamp(vector.y, guiBoundary.min.y, guiBoundary.max.y);
					}
				}
				if (lastFingerId == touch.fingerId && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
				{
					ResetJoystick();
				}
				if (lastFingerId == touch.fingerId && touchPad && !_moveReceived && touch.phase == TouchPhase.Moved)
				{
					_moveReceived = true;
					fingerDownPos = touch.position;
				}
			}
		}
		if (!touchPad)
		{
			position.x = (guiPixelInset.x + guiTouchOffset.x - guiCenter.x) / guiTouchOffset.x;
			position.y = (guiPixelInset.y + guiTouchOffset.y - guiCenter.y) / guiTouchOffset.y;
		}
		float num2 = Mathf.Abs(position.x);
		float num3 = Mathf.Abs(position.y);
		if (num2 < deadZone.x)
		{
			position.x = 0f;
		}
		else if (normalize)
		{
			position.x = Mathf.Sign(position.x) * (num2 - deadZone.x) / (1f - deadZone.x);
		}
		if (num3 < deadZone.y)
		{
			position.y = 0f;
		}
		else if (normalize)
		{
			position.y = Mathf.Sign(position.y) * (num3 - deadZone.y) / (1f - deadZone.y);
		}
	}

	private void OnGUI()
	{
		#if UNITY_ANDROID
		Color color = GUI.color;
		GUI.color = new Color(color.r, color.g, color.b, 38f);
		if ((!Defs.IsTraining || !touchPad || TrainingController.stepTraining >= TrainingController.stepTrainingList["GetTheCoin"]) && (!Defs.IsTraining || touchPad || TrainingController.stepTraining >= TrainingController.stepTrainingList["TapToMove"]))
		{
			if (!touchPad && (bool)obodok)
			{
				GUI.DrawTexture(new Rect(defaultRect.x, (float)Screen.height - defaultRect.y - defaultRect.height, defaultRect.width, defaultRect.height), obodok);
			}
			if ((bool)gui)
			{
				GUI.DrawTexture(new Rect(guiPixelInset.x, (float)Screen.height - guiPixelInset.height - guiPixelInset.y, guiPixelInset.width, guiPixelInset.height), gui);
			}
			GUI.color = color;
		}
		if (!Defs.IsTraining || ((!touchPad || TrainingController.stepTraining >= TrainingController.stepTrainingList["TapToShoot"]) && (touchPad || TrainingController.stepTraining >= TrainingController.stepTrainingList["TapToShoot"])))
		{
			if (zoomTexture != null && _weaponManager.currentWeaponSounds != null && _weaponManager.currentWeaponSounds.isZooming)
			{
				GUI.DrawTexture(new Rect(zoomZone.x, (float)Screen.height - zoomZone.height - zoomZone.y, zoomZone.width, zoomZone.height), zoomTexture);
			}
			if ((bool)fireTexture)
			{
				GUI.DrawTexture(new Rect(fireZone.x, (float)Screen.height - fireZone.height - fireZone.y, fireZone.width, fireZone.height), fireTexture);
			}
			bool flag = reloadTexture;
			if (_weaponManager.currentWeaponSounds != null && _weaponManager.currentWeaponSounds.isMelee)
			{
				flag = false;
			}
			if (flag)
			{
				GUI.DrawTexture(new Rect(reloadZone.x, (float)Screen.height - reloadZone.height - reloadZone.y, reloadZone.height, reloadZone.height), NormalReloadMode ? reloadTexture : ((!blink) ? reloadTexture : reloadTextureNoAmmo));
			}
		}
		#endif
	}

	public void setSeriya(bool isSeriya)
	{
		isSerialShooting = isSeriya;
	}
}
