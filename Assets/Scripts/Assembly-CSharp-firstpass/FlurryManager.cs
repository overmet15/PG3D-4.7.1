using System;
using Prime31;

public class FlurryManager : AbstractManager
{
	public static event Action<string> spaceDidDismissEvent;

	public static event Action<string> spaceWillLeaveApplicationEvent;

	public static event Action<string> spaceDidFailToRenderEvent;

	public static event Action<string> spaceDidFailToReceiveAdEvent;

	public static event Action<string> spaceDidReceiveAdEvent;

	public static event Action<string, float> onCurrencyValueUpdatedEvent;

	public static event Action<P31Error> onCurrencyValueFailedToUpdateEvent;

	public static event Action<string> videoDidFinishEvent;

	static FlurryManager()
	{
		AbstractManager.initialize(typeof(FlurryManager));
	}

	private void spaceDidDismiss(string space)
	{
		if (FlurryManager.spaceDidDismissEvent != null)
		{
			FlurryManager.spaceDidDismissEvent(space);
		}
	}

	private void spaceWillLeaveApplication(string space)
	{
		if (FlurryManager.spaceWillLeaveApplicationEvent != null)
		{
			FlurryManager.spaceWillLeaveApplicationEvent(space);
		}
	}

	private void spaceDidFailToRender(string space)
	{
		if (FlurryManager.spaceDidFailToRenderEvent != null)
		{
			FlurryManager.spaceDidFailToRenderEvent(space);
		}
	}

	private void spaceDidFailToReceiveAd(string space)
	{
		if (FlurryManager.spaceDidFailToReceiveAdEvent != null)
		{
			FlurryManager.spaceDidFailToReceiveAdEvent(space);
		}
	}

	private void spaceDidReceiveAd(string space)
	{
		if (FlurryManager.spaceDidReceiveAdEvent != null)
		{
			FlurryManager.spaceDidReceiveAdEvent(space);
		}
	}

	private void onCurrencyValueFailedToUpdate(string json)
	{
		FlurryManager.onCurrencyValueFailedToUpdateEvent.fire(P31Error.errorFromJson(json));
	}

	private void onCurrencyValueUpdated(string response)
	{
		if (FlurryManager.onCurrencyValueUpdatedEvent != null)
		{
			string[] array = response.Split(',');
			if (array.Length == 2)
			{
				FlurryManager.onCurrencyValueUpdatedEvent(array[0], float.Parse(array[1]));
			}
		}
	}

	private void videoDidFinish(string space)
	{
		if (FlurryManager.videoDidFinishEvent != null)
		{
			FlurryManager.videoDidFinishEvent(space);
		}
	}
}
