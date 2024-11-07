using System;
using UnityEngine;

namespace Rilisoft.PixlGun3D
{
	internal sealed class ButtonHandler : MonoBehaviour
	{
		public event EventHandler Clicked;

		private void OnClick()
		{
			EventHandler clicked = this.Clicked;
			if (clicked != null)
			{
				clicked(this, EventArgs.Empty);
			}
		}
	}
}
