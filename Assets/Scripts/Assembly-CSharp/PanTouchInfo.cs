using UnityEngine;

public class PanTouchInfo
{
	private Vector2 _fingerPos = Vector2.zero;

	private Vector2 fingerLastPos = Vector2.zero;

	private Vector2 fingerMovedBy = Vector2.zero;

	private float slideMagnitudeX;

	private float slideMagnitudeY;

	private float _startTime;

	private Vector2 _initialTouchPos = Vector2.zero;

	public Vector2 InitialTouchPos
	{
		get
		{
			return _initialTouchPos;
		}
		set
		{
			_initialTouchPos = value;
		}
	}

	public float StartTime
	{
		get
		{
			return _startTime;
		}
		set
		{
			_startTime = value;
		}
	}

	public Vector2 FingerLastPos
	{
		get
		{
			return fingerLastPos;
		}
		set
		{
			fingerLastPos = value;
		}
	}

	public Vector2 FingerMovedBy
	{
		get
		{
			return fingerMovedBy;
		}
		set
		{
			fingerMovedBy = value;
		}
	}

	public Vector2 FingerPos
	{
		get
		{
			return _fingerPos;
		}
		set
		{
			_fingerPos = value;
		}
	}

	public float SlideMagnitudeX
	{
		get
		{
			return slideMagnitudeX;
		}
		set
		{
			slideMagnitudeX = value;
		}
	}

	public float SlideMagnitudeY
	{
		get
		{
			return slideMagnitudeY;
		}
		set
		{
			slideMagnitudeY = value;
		}
	}
}
