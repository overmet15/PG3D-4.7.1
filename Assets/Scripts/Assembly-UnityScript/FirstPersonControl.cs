using System;
using UnityEngine;

[Serializable]
[RequireComponent(typeof(CharacterController))]
public class FirstPersonControl : MonoBehaviour
{
	public Joystick moveTouchPad;

	public Joystick rotateTouchPad;

	public Transform cameraPivot;

	public float forwardSpeed;

	public float backwardSpeed;

	public float sidestepSpeed;

	public float jumpSpeed;

	public float inAirMultiplier;

	public Vector2 rotationSpeed;

	public float tiltPositiveYAxis;

	public float tiltNegativeYAxis;

	public float tiltXAxisMinimum;

	public string myIp;

	public GameObject playerGameObject;

	public int typeAnim;

	private Transform thisTransform;

	public GameObject camPlayer;

	private CharacterController character;

	private Vector3 cameraVelocity;

	private Vector3 velocity;

	private bool canJump;

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

	public FirstPersonControl()
	{
		forwardSpeed = 4f;
		backwardSpeed = 1f;
		sidestepSpeed = 1f;
		jumpSpeed = 4.5f;
		inAirMultiplier = 0.25f;
		rotationSpeed = new Vector2(2f, 1f);
		tiltPositiveYAxis = 0.6f;
		tiltNegativeYAxis = 0.4f;
		tiltXAxisMinimum = 0.1f;
		canJump = true;
	}

	public virtual void setIsMine()
	{
		isMine = true;
	}

	public virtual void SetSpeedModifier(float speedMod)
	{
		if (!(startForwardSpeed <= 0f))
		{
			forwardSpeed = startForwardSpeed * speedMod;
		}
		if (!(startBackwardSpeed <= 0f))
		{
			backwardSpeed = startBackwardSpeed * speedMod;
		}
		if (!(startSidestepSpeed <= 0f))
		{
			sidestepSpeed = startSidestepSpeed * speedMod;
		}
	}

	public virtual void Start()
	{
		Screen.lockCursor = true;
		startForwardSpeed = forwardSpeed;
		startBackwardSpeed = backwardSpeed;
		startSidestepSpeed = sidestepSpeed;
		thisTransform = (Transform)GetComponent(typeof(Transform));
		character = (CharacterController)GetComponent(typeof(CharacterController));
		_playerGun = GameObject.FindGameObjectWithTag("PlayerGun");
		GameObject gameObject = GameObject.Find("PlayerSpawn");
		if ((bool)gameObject)
		{
			thisTransform.position = gameObject.transform.position;
		}
	}

	public virtual void OnEndGame()
	{
		moveTouchPad.Disable();
		if ((bool)rotateTouchPad)
		{
			rotateTouchPad.Disable();
		}
		enabled = false;
	}

	public virtual void Jumping()
	{
		if (jump)
		{
		}
	}

	[RPC]
	public virtual void setIp(string _ip)
	{
		myIp = _ip;
		Debug.Log("firstPesonControl=" + Network.player.ipAddress + " myIp=" + _ip);
	}

	public virtual void popal(NetworkViewID id)
	{
		if (PlayerPrefs.GetInt("MultyPlayer") == 1)
		{
			GetComponent<NetworkView>().RPC("minusLive", RPCMode.All, id);
		}
	}

	public virtual Vector2 updateKeyboardControls()
	{
		int num = default(int);
		int num2 = default(int);
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

	public virtual void updateMouseControls()
	{
	}

	public virtual void Jump()
	{
		jump = true;
		canJump = false;
	}

	public virtual void Update()
	{
		Vector3 motion = thisTransform.TransformDirection(new Vector3(moveTouchPad.position.x, 0f, moveTouchPad.position.y));
		motion.y = 0f;
		motion.Normalize();
		Vector2 vector = new Vector2(Mathf.Abs(moveTouchPad.position.x), Mathf.Abs(moveTouchPad.position.y));
		if (!(vector.y <= vector.x))
		{
			if (!(moveTouchPad.position.y <= 0f))
			{
				motion *= forwardSpeed * vector.y;
			}
			else
			{
				motion *= backwardSpeed * vector.y;
			}
		}
		else
		{
			motion *= sidestepSpeed * vector.x;
		}
		if (character.isGrounded)
		{
			canJump = true;
			jump = false;
			Joystick joystick = rotateTouchPad;
			if (canJump && joystick.jumpPressed)
			{
				joystick.jumpPressed = false;
				Jump();
			}
			if (jump)
			{
				velocity = character.velocity;
				velocity.y = jumpSpeed;
			}
		}
		else
		{
			velocity.y += Physics.gravity.y * Time.deltaTime;
			motion.x *= inAirMultiplier;
			motion.z *= inAirMultiplier;
			if (rotateTouchPad.jumpPressed)
			{
				rotateTouchPad.jumpPressed = false;
			}
		}
		motion += velocity;
		motion += Physics.gravity;
		motion *= Time.deltaTime;
		timeUpdateAnim -= Time.deltaTime;
		if (!(timeUpdateAnim >= 0f) && character.isGrounded)
		{
			timeUpdateAnim = 0.5f;
			if (!(new Vector2(motion.x, motion.z).magnitude <= 0f))
			{
				if (_playerGun != null)
				{
					_playerGun.SendMessage("WalkAnimation");
				}
			}
			else if (_playerGun != null)
			{
				_playerGun.SendMessage("IdleAnimation");
			}
		}
		if (!character.enabled)
		{
			return;
		}
		character.Move(motion);
		if (character.isGrounded)
		{
			velocity = Vector3.zero;
		}
		if (!character.isGrounded)
		{
			return;
		}
		Vector2 vector2 = Vector2.zero;
		if ((bool)rotateTouchPad)
		{
			vector2 = rotateTouchPad.position;
		}
		else
		{
			Vector3 acceleration = Input.acceleration;
			float num = Mathf.Abs(acceleration.x);
			if (!(acceleration.z >= 0f) && !(acceleration.x >= 0f))
			{
				if (!(num < tiltPositiveYAxis))
				{
					vector2.y = (num - tiltPositiveYAxis) / (1f - tiltPositiveYAxis);
				}
				else if (!(num > tiltNegativeYAxis))
				{
					vector2.y = (0f - (tiltNegativeYAxis - num)) / tiltNegativeYAxis;
				}
			}
			if (!(Mathf.Abs(acceleration.y) < tiltXAxisMinimum))
			{
				vector2.x = (0f - (acceleration.y - tiltXAxisMinimum)) / (1f - tiltXAxisMinimum);
			}
		}
		if (!(vector2.magnitude <= 1f))
		{
		}
		float @float = PlayerPrefs.GetFloat("SensitivitySett", 12f);
		vector2 *= Time.deltaTime * @float;
		thisTransform.Rotate(0f, vector2.x, 0f, Space.World);
		cameraPivot.Rotate(0f - vector2.y, 0f, 0f);
	}

	public virtual void Main()
	{
	}
}
