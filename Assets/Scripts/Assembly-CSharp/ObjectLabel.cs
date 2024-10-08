using System;
using UnityEngine;

[RequireComponent(typeof(GUIText))]
public class ObjectLabel : MonoBehaviour
{
	public static Camera currentCamera;

	public Transform target;

	public Vector3 offset = Vector3.up;

	public bool clampToScreen;

	public float clampBorderSize = 0.05f;

	public bool useMainCamera = true;

	public Camera cameraToUse;

	public Camera cam;

	public Vector3 posLabel;

	public bool isMenu;

	public bool isShadow;

	private Transform thisTransform;

	private Transform camTransform;

	private ExperienceController expController;

	private bool isSetColor;

	private int rank = 1;

	private float koofScreen = (float)Screen.height / 768f;

	private void Start()
	{
		expController = GameObject.FindObjectOfType<ExperienceController>();
		float num = 36f * Defs.Coef;
		thisTransform = base.transform;
		cam = currentCamera;
		camTransform = cam.transform;
		base.transform.GetComponent<GUITexture>().pixelInset = new Rect(-90f * koofScreen, 0f * koofScreen, 36f * koofScreen, 36f * koofScreen);
		base.transform.GetComponent<GUIText>().pixelOffset = new Vector2(-45f * koofScreen, 0f);
	}

	private void Update()
	{
		if (target == null || cam == null)
		{
			Debug.Log("target=null");
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		try
		{
			cam = currentCamera;
			camTransform = cam.transform;
			if (!isMenu)
			{
				if (!isSetColor)
				{
					if (target.GetComponent<Player_move_c>().myCommand == 1)
					{
						base.gameObject.GetComponent<GUIText>().color = Color.blue;
						isSetColor = true;
					}
					if (target.GetComponent<Player_move_c>().myCommand == 2)
					{
						base.gameObject.GetComponent<GUIText>().color = Color.red;
						isSetColor = true;
					}
				}
				base.transform.GetComponent<GUITexture>().texture = expController.marks[target.GetComponent<Player_move_c>().myTable.GetComponent<NetworkStartTable>().myRanks];
			}
			else
			{
				base.transform.GetComponent<GUITexture>().pixelInset = new Rect(-130f * koofScreen, -6f * koofScreen, 36f * koofScreen, 36f * koofScreen);
				base.transform.GetComponent<GUIText>().pixelOffset = new Vector2(-85f * koofScreen, 0f);
				base.transform.GetComponent<GUIText>().fontSize = Mathf.RoundToInt(22f * Defs.Coef);
				offset = new Vector3(0f, 2.22f, 0f);
				base.transform.GetComponent<GUITexture>().texture = expController.marks[expController.currentLevel];
			}
			if (clampToScreen)
			{
				Vector3 vector = camTransform.InverseTransformPoint(target.position);
				vector.z = Mathf.Max(vector.z, 1f);
				thisTransform.position = cam.WorldToViewportPoint(camTransform.TransformPoint(vector + offset));
				thisTransform.position = new Vector3(Mathf.Clamp(thisTransform.position.x, clampBorderSize, 1f - clampBorderSize), Mathf.Clamp(thisTransform.position.y, clampBorderSize, 1f - clampBorderSize), thisTransform.position.z);
			}
			else
			{
				posLabel = cam.WorldToViewportPoint(target.position + offset);
				if (posLabel.z >= 0f)
				{
					thisTransform.position = posLabel;
				}
				else
				{
					thisTransform.position = new Vector3(-1000f, -1000f, -1000f);
				}
			}
			if (!isMenu && target.transform.parent.transform.position.y < -1000f)
			{
				thisTransform.position = new Vector3(-1000f, -1000f, -1000f);
			}
		}
		catch (Exception ex)
		{
			Debug.Log("Exception in ObjectLabel: " + ex);
		}
	}
}
