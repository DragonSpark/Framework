using DragonSpark.Application.Communication.Configuration;
using System;
using System.IdentityModel.Services;
using System.Web;

namespace DragonSpark.Application.Communication.Security
{
    public class AuthenticationModule : WSFederationAuthenticationModule
    {
        protected override void InitializeModule( HttpApplication context )
        {
            base.InitializeModule( context );

            // FederatedAuthentication.SessionAuthenticationModule.IsSessionMode = true;
        }

		/*public override bool CanReadSignInResponse( HttpRequest request, bool onPage )
		{
			var sessionAuthenticationModule = FederatedAuthentication.SessionAuthenticationModule;
			var @equals = string.Equals( request.HttpMethod, "POST", StringComparison.OrdinalIgnoreCase );
			var containsSessionTokenCookie = sessionAuthenticationModule.ContainsSessionTokenCookie( request.Cookies );
			var isSignInResponse = IsSignInResponse( request );
			if (@equals)
			{
				if ((onPage || !containsSessionTokenCookie) && isSignInResponse)
                {
                    return true;
                }
			}

			return base.CanReadSignInResponse( request, onPage );
		}*/

        protected override void OnSignedIn( EventArgs args )
        {
            base.OnSignedIn( args );

            var locator = HttpContext.Current.ApplicationInstance.GetModule<IServiceLocatorModule>().ServiceLocator;
            locator.GetInstance<IIdentitySynchronizer>().Apply( HttpContext.Current.User );
        }
    }
}