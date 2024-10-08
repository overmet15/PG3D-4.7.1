using System.Collections.Generic;

namespace Fuckhead.PixlGun3D
{
	internal sealed class AndroidFacebookFacade : FacebookFacade
	{
		public override bool CanUserUseFacebookComposer()
		{
			return false;
		}

		public override List<object> GetSessionPermissions()
		{
			return FacebookAndroid.getSessionPermissions();
		}

		public override void Init()
		{
			FacebookAndroid.init();
		}

		public override bool IsSessionValid()
		{
			return FacebookAndroid.isSessionValid();
		}

		public override void LoginWithReadPermissions(string[] permissions)
		{
			FacebookAndroid.loginWithReadPermissions(permissions);
		}

		public override void ReauthorizeWithPublishPermissions(string[] permissions, FacebookSessionDefaultAudience defaultAudience)
		{
			FacebookAndroid.reauthorizeWithPublishPermissions(permissions, defaultAudience);
		}

		public override void SetSessionLoginBehavior(FacebookSessionLoginBehavior loginBehavior)
		{
			FacebookAndroid.setSessionLoginBehavior(loginBehavior);
		}
	}
}
