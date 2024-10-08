using System;
using System.Collections.Generic;
using UnityEngine;

internal sealed class NotificationController : MonoBehaviour
{
	private readonly IList<int> _notificationIds = new List<int>();

	private void Start()
	{
		if (!Load.LoadBool("bilZapuskKey"))
		{
			Save.SaveBool("bilZapuskKey", true);
		}
		else
		{
			appStart();
		}
	}

	private void Update()
	{
	}

	private void appStop()
	{
		Save.SaveString("appStopTime", DateTime.Now.ToString());
		for (int i = 1; i < 7; i++)
		{
			int num = i * 172800 - 3600;
			if (Debug.isDebugBuild)
			{
				num = 5;
			}
			string empty = string.Empty;
			int item = EtceteraAndroid.scheduleNotification(num, "Challenge", "You are challenged to fight!", "Are you ready?", empty);
			_notificationIds.Add(item);
			if (Debug.isDebugBuild)
			{
				break;
			}
		}
	}

	private void appStart()
	{
		string text = Load.LoadString("appStopTime");
		if (text == null || text.Equals(string.Empty))
		{
			return;
		}
		Debug.Log("-" + text + "-");
		Debug.Log("appStartTime=" + DateTime.Now.Subtract(DateTime.Parse(text)).TotalSeconds);
		foreach (int notificationId in _notificationIds)
		{
			EtceteraAndroid.cancelNotification(notificationId);
		}
		Save.SaveString("appStopTime", string.Empty);
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (pauseStatus)
		{
			appStop();
		}
		else
		{
			appStart();
		}
	}
}
