using UnityEngine;

public class TapInfo
{
	private Collider _collider;

	public Collider TappedCollider
	{
		get
		{
			return _collider;
		}
		set
		{
			_collider = value;
		}
	}
}
