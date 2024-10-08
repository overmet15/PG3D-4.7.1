using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WindowsMouseManager : MonoBehaviour
{
	public static WindowsMouseManager Instance;

	public Vector2 MouseInputs;

	public bool MouseLocked {get; private set;}

	void Awake()
	{
		Instance = this;
		DontDestroyOnLoad(gameObject);
	}

	void Start()
	{
		MouseLocked = true;
		Cursor.lockState = CursorLockMode.None;
	}

    void Update()
	{
		if (Input.GetKeyDown(KeyCode.F1)) ToggleMouse();
		if (MouseLocked)
		{
			MouseInputs.x = Input.GetAxis("Mouse X");
			MouseInputs.y = Input.GetAxis("Mouse Y"); 
		} 
		else MouseInputs = Vector2.zero;
    }
	public void ToggleMouse()
	{
		MouseLocked = !MouseLocked;
		if (MouseLocked) Cursor.lockState = CursorLockMode.Locked;
		else Cursor.lockState = CursorLockMode.None;
	}
	public void SetMouseLock(bool setTo)
	{
		if (MouseLocked != setTo)
        {
            MouseLocked = setTo;
            if (setTo) Cursor.lockState = CursorLockMode.Locked;
            else Cursor.lockState = CursorLockMode.None;
        }
    }
}
