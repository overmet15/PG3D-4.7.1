using UnityEngine;

namespace Rilisoft.PixlGun3D
{
	internal static class ServiceLocator
	{
		private static readonly FacebookFacade _facebookFacade;

		private static readonly TwitterFacade _twitterFacade;

		public static FacebookFacade FacebookFacade
		{
			get
			{
				return _facebookFacade;
			}
		}

		public static TwitterFacade TwitterFacade
		{
			get
			{
				return _twitterFacade;
			}
		}

		static ServiceLocator()
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				_facebookFacade = new IosFacebookFacade();
				_twitterFacade = new IosTwitterFacade();
			}
			else if (Application.platform == RuntimePlatform.Android)
			{
				_facebookFacade = new AndroidFacebookFacade();
				_twitterFacade = new AndroidTwitterFacade();
			}
			else
			{
				_facebookFacade = new DummyFacebookFacade();
				_twitterFacade = new DummyTwitterFacade();
			}
		}
	}
}
