using UnityEngine;

internal sealed class FirstPersonControlSharp : MonoBehaviour
{
	public JoystickSharp moveTouchPad;

	public JoystickSharp rotateTouchPad;

	public Transform cameraPivot;

	public float forwardSpeed = 4f;

	public float backwardSpeed = 1f;

	public float sidestepSpeed = 1f;

	public float jumpSpeed = 4.5f;

	public float inAirMultiplier = 0.25f;

	public Vector2 rotationSpeed = new Vector2(2f, 1f);

	public float tiltPositiveYAxis = 0.6f;

	public float tiltNegativeYAxis = 0.4f;

	public float tiltXAxisMinimum = 0.1f;

	public string myIp;

	public GameObject playerGameObject;

	public int typeAnim;

	private Transform thisTransform;

	public GameObject camPlayer;

	private CharacterController character;

	private Vector3 cameraVelocity;

	private Vector3 velocity;

	private bool canJump = true;

	public bool isMine;

	private Rect fireZone;

	private Rect jumpZone;

	private bool _jumping;

	private bool jump;

	private GameObject _playerGun;

	private float startForwardSpeed;

	private float startBackwardSpeed;

	private float startSidestepSpeed;

	private float timeUpdateAnim;

	public AudioClip jumpClip;

	private Player_move_c _moveC;

	private bool _invert;

	private Vector3 oldPos = Vector3.zero;

	private void setIsMine()
	{
		isMine = true;
	}

	public void SetSpeedModifier(float speedMod)
	{
		if (startForwardSpeed > 0f)
		{
			forwardSpeed = startForwardSpeed * speedMod;
		}
		if (startBackwardSpeed > 0f)
		{
			backwardSpeed = startBackwardSpeed * speedMod;
		}
		if (startSidestepSpeed > 0f)
		{
			sidestepSpeed = startSidestepSpeed * speedMod;
		}
	}

	private void Start()
	{
        WindowsMouseManager.Instance.SetMouseLock(true);
        _invert = PlayerPrefs.GetInt(Defs.InvertCamSN, 0) == 1;
		startForwardSpeed = forwardSpeed;
		startBackwardSpeed = backwardSpeed;
		startSidestepSpeed = sidestepSpeed;
		thisTransform = GetComponent<Transform>();
		character = GetComponent<CharacterController>();
		_playerGun = playerGameObject;
		GameObject gameObject = GameObject.Find("PlayerSpawn");
		if ((bool)gameObject)
		{
			thisTransform.position = gameObject.transform.position;
		}
	}

	private void OnEndGame()
    {
        moveTouchPad.Disable();
		if ((bool)rotateTouchPad)
		{
			rotateTouchPad.Disable();
		}
		base.enabled = false;
    }

	private void Jumping()
	{
		if (jump)
		{
		}
	}

	[RPC]
	private void setIp(string _ip)
	{
		myIp = _ip;
		Debug.Log("firstPesonControl=" + Network.player.ipAddress + " myIp=" + _ip);
	}

	private void popal(NetworkViewID id)
	{
		if (PlayerPrefs.GetInt("MultyPlayer") == 1)
		{
			base.GetComponent<NetworkView>().RPC("minusLive", RPCMode.All, id);
		}
	}

	private Vector2 updateKeyboardControls()
	{
		int num = 0;
		int num2 = 0;
		if (Input.GetKey("w"))
		{
			num = 1;
		}
		if (Input.GetKey("s"))
		{
			num = -1;
		}
		if (Input.GetKey("a"))
		{
			num2 = -1;
		}
		if (Input.GetKey("d"))
		{
			num2 = 1;
		}
		return new Vector2(num2, num);
	}

	private void updateMouseControls()
	{
	}

	private void Jump()
	{
		jump = true;
		canJump = false;
		if (PlayerPrefs.GetInt("MultyPlayer") == 1)
		{
			base.transform.GetComponent<SkinName>().sendAnimJump();
		}
		if (PlayerPrefsX.GetBool(PlayerPrefsX.SoundFXSetting, true))
		{
			NGUITools.PlaySound(jumpClip);
		}
	}

	private void Update()
	{
		oldPos = base.transform.position;
		#if UNITY_ANDROID
		Vector3 vector = base.transform.TransformDirection(new Vector3(moveTouchPad.position.x * (float)((!character.isGrounded) ? 1 : 1), 0f, moveTouchPad.position.y));
		#else
		Vector3 vector = base.transform.TransformDirection(new Vector3(Input.GetAxis("Horizontal") * (float)((!character.isGrounded) ? 1 : 1), 0f, Input.GetAxis("Vertical")));
		#endif
		if (Defs.IsTraining && TrainingController.stepTraining < TrainingController.stepTrainingList["TapToMove"])
		{
			vector = Vector3.zero;
		}
		if (Defs.IsTraining && TrainingController.stepTraining == TrainingController.stepTrainingList["TapToMove"] && vector != Vector3.zero)
		{
			TrainingController.isNextStep = TrainingController.stepTrainingList["TapToMove"];
		}
		vector.y = 0f;
		vector.Normalize();
		#if UNITY_ANDROID
		Vector2 vector2 = new Vector2(Mathf.Abs(moveTouchPad.position.x), Mathf.Abs(moveTouchPad.position.y));

		if (vector2.y > vector2.x)
		{
		if (moveTouchPad.position.y > 0f)
		{
		vector *= forwardSpeed * vector2.y;
		}
		else
		{
		vector *= backwardSpeed * vector2.y;
		}
		}
		else
		{
		vector *= sidestepSpeed * vector2.x * (float)((!character.isGrounded) ? 1 : 1);
		}
		#else
		// PC Logic here
		if (vector.y > vector.x)
		{			
			if (moveTouchPad.position.y > 0f)
			{
				vector *= forwardSpeed;
			}
			else
			{
				vector *= backwardSpeed;
			}
		}
		else
		{
			vector *= sidestepSpeed * (float)((!character.isGrounded) ? 1 : 1);
		}
		#endif

		if (character.isGrounded)
		{
			canJump = true;
			jump = false;
			JoystickSharp joystickSharp = rotateTouchPad;
			if (canJump && joystickSharp.jumpPressed)
			{
				joystickSharp.jumpPressed = false;
				Jump();
			}
			if (jump)
			{
				velocity = Vector3.zero;
				velocity.y = jumpSpeed;
			}
		}
		else
		{
			velocity.y += Physics.gravity.y * Time.deltaTime;
			if (rotateTouchPad.jumpPressed)
			{
				rotateTouchPad.jumpPressed = false;
			}
		}
		vector += velocity;
		vector += Physics.gravity;
		vector *= Time.deltaTime;
		timeUpdateAnim -= Time.deltaTime;
		if (timeUpdateAnim < 0f && character.isGrounded)
		{
			timeUpdateAnim = 0.5f;
			if (new Vector2(vector.x, vector.z).magnitude > 0f)
			{
				if (_playerGun != null)
				{
					_playerGun.GetComponent<Player_move_c>().WalkAnimation();
				}
			}
			else if (_playerGun != null)
			{
				_playerGun.GetComponent<Player_move_c>().IdleAnimation();
			}
		}
		if (!character.enabled)
		{
			return;
		}
		character.Move(vector);
		if (character.isGrounded)
		{
			velocity = Vector3.zero;
		}
		Vector2 vector3 = Vector2.zero;

        float num2 = 1f;
        if (_moveC != null) num2 *= ((!_moveC.isZooming) ? 1f : 0.5f);
        float @float = PlayerPrefs.GetFloat("SensitivitySett", 12f);
        if (_moveC == null && _playerGun != null) _moveC = _playerGun.GetComponent<Player_move_c>();
#if UNITY_ANDROID
		if ((bool)rotateTouchPad)
		{
			vector3 = rotateTouchPad.position;
			if (Defs.IsTraining && TrainingController.stepTraining == TrainingController.stepTrainingList["SwipeToRotate"] && !vector3.Equals(Vector2.zero))
			{
				TrainingController.isNextStep = TrainingController.stepTrainingList["SwipeToRotate"];
			}
		}
		else
		{
			Vector3 acceleration = Input.acceleration;
			float num = Mathf.Abs(acceleration.x);
			if (acceleration.z < 0f && acceleration.x < 0f)
			{
				if (num >= tiltPositiveYAxis)
				{
					vector3.y = (num - tiltPositiveYAxis) / (1f - tiltPositiveYAxis);
				}
				else if (num <= tiltNegativeYAxis)
				{
					vector3.y = (0f - (tiltNegativeYAxis - num)) / tiltNegativeYAxis;
				}
			}
			if (Mathf.Abs(acceleration.y) >= tiltXAxisMinimum)
			{
				vector3.x = (0f - (acceleration.y - tiltXAxisMinimum)) / (1f - tiltXAxisMinimum);
			}
		}
		if (vector3.magnitude > 1f)
		{
		}
#else

        vector3 = WindowsMouseManager.Instance.MouseInputs;
		if (Defs.IsTraining && TrainingController.stepTraining == TrainingController.stepTrainingList["SwipeToRotate"] && !vector3.Equals(Vector2.zero))
			TrainingController.isNextStep = TrainingController.stepTrainingList["SwipeToRotate"];
		//vector3 *= 2.5f;
		@float *= 15f;
#endif
        vector3 *= Time.deltaTime * @float * num2;
        thisTransform.Rotate(0f, vector3.x, 0f, Space.World);
		cameraPivot.Rotate(((!_invert) ? 1f : (-1f)) * (0f - vector3.y), 0f, 0f);
	}
	void OnDestroy()
	{
        WindowsMouseManager.Instance.SetMouseLock(false);
    }
}
