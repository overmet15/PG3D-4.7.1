using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WindowsMouseManager : MonoBehaviour
{
	public static Vector2 MouseInputs;

	public static bool MouseLocked
	{
		get
		{
            return Cursor.lockState == CursorLockMode.Locked;
        }
		set
		{
			Cursor.lockState = value ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !value;
        }
	}
	void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}

	void Start()
	{
		MouseLocked = true;
		Cursor.lockState = CursorLockMode.None;
	}

    void Update()
	{
		if (Input.GetKeyDown(KeyCode.F1)) MouseLocked = !MouseLocked;
		if (MouseLocked)
		{
			MouseInputs.x = Input.GetAxis("Mouse X");
			MouseInputs.y = Input.GetAxis("Mouse Y"); 
		} 
		else MouseInputs = Vector2.zero;
    }
}
