using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Button Sound")]
public class UIButtonSound : MonoBehaviour
{
	public enum Trigger
	{
		OnClick = 0,
		OnMouseOver = 1,
		OnMouseOut = 2,
		OnPress = 3,
		OnRelease = 4
	}

	public GameObject chatViewer;

	public AudioClip audioClip;

	public Trigger trigger;

	public float volume = 1f;

	public float pitch = 1f;

	private void OnHover(bool isOver)
	{
		if (base.enabled && ((isOver && trigger == Trigger.OnMouseOver) || (!isOver && trigger == Trigger.OnMouseOut)) && PlayerPrefsX.GetBool(PlayerPrefsX.SoundFXSetting, true))
		{
			NGUITools.PlaySound(audioClip, volume, pitch);
		}
	}

	private void OnPress(bool isPressed)
	{
		if (base.enabled && ((isPressed && trigger == Trigger.OnPress) || (!isPressed && trigger == Trigger.OnRelease)) && PlayerPrefsX.GetBool(PlayerPrefsX.SoundFXSetting, true))
		{
			NGUITools.PlaySound(audioClip, volume, pitch);
		}
	}

	private void OnClick()
	{
		if (base.enabled && trigger == Trigger.OnClick)
		{
			if (chatViewer != null)
			{
				chatViewer.GetComponent<ChatViewrController>().clickButton(base.gameObject.name);
			}
			if (PlayerPrefsX.GetBool(PlayerPrefsX.SoundFXSetting, true))
			{
				NGUITools.PlaySound(audioClip, volume, pitch);
			}
		}
	}
}
