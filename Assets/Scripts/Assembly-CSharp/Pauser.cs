using System;
using UnityEngine;

public class Pauser : MonoBehaviour
{
	private Action OnPlayerAddedAction;

	public bool pausedVar;

	private GameObject _leftJoystick;

	private GameObject _rightJoystick;

	public bool paused
	{
		get
		{
			return pausedVar;
		}
		set
		{
			pausedVar = value;
			if (!(_leftJoystick == null) && !(_rightJoystick == null))
			{
				if (pausedVar)
				{
					_leftJoystick.SendMessage("Disable");
					_rightJoystick.SendMessage("Disable");
				}
				else
				{
					_leftJoystick.active = true;
					_rightJoystick.active = true;
				}
			}
		}
	}

	private void Start()
	{
		OnPlayerAddedAction = delegate
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag("Joystick");
			_leftJoystick = array[0];
			_rightJoystick = array[1];
		};
		Initializer.PlayerAddedEvent += OnPlayerAddedAction;
	}

	private void OnDestroy()
	{
		Initializer.PlayerAddedEvent -= OnPlayerAddedAction;
	}
}
