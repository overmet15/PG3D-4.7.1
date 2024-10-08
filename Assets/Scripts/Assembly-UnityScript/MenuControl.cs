using System;
using UnityEngine;

[Serializable]
public class MenuControl : MonoBehaviour
{
	public GameObject highlight;

	public float rotateSpeed;

	public string[] itemText;

	public Vector3[] itemPositions;

	[NonSerialized]
	public static bool clicked;

	public MenuControl()
	{
		rotateSpeed = 150f;
		itemText = new string[3] { "<color=#13b62a>Start", "<color=#235fb9>Options", "<color=#cb3125>Quit" };
		itemPositions = new Vector3[3]
		{
			new Vector3(0f, 4f, 0f),
			new Vector3(0f, 0f, 0f),
			new Vector3(0f, -4f, 0f)
		};
	}

	public virtual void Start()
	{
		for (int i = 0; i < itemText.Length; i++)
		{
			GameObject @object = FlyingText.GetObject(itemText[i]);
			@object.transform.position = itemPositions[i];
			GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
			gameObject.transform.localScale = new Vector3(7f, 1.8f, 0.1f);
			gameObject.transform.position = itemPositions[i];
			gameObject.GetComponent<Renderer>().enabled = false;
			MenuObject menuObject = (MenuObject)gameObject.AddComponent(typeof(MenuObject));
			menuObject.highlight = highlight;
			menuObject.rotateSpeed = rotateSpeed;
			menuObject.menuObject = @object.transform;
		}
	}

	public virtual void Main()
	{
	}
}
