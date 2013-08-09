
using System;
using System.IdentityModel.Services;

namespace DragonSpark.Web.Security
{
	public class SessionAuthenticationModule : System.IdentityModel.Services.SessionAuthenticationModule
	{
		protected override void OnSessionSecurityTokenReceived( SessionSecurityTokenReceivedEventArgs args )
		{
			base.OnSessionSecurityTokenReceived( args );

			var now = DateTime.UtcNow;
			var validFrom = args.SessionToken.ValidFrom;
			var validTo = args.SessionToken.ValidTo;

			var tokenLifetime = ( validTo - validFrom ).TotalMinutes;

			if ( now > validFrom.AddMinutes( tokenLifetime / 2 ) )
			{
				args.SessionToken = CreateSessionSecurityToken( args.SessionToken.ClaimsPrincipal, args.SessionToken.Context, now, now.AddMinutes( tokenLifetime ), args.SessionToken.IsPersistent );
				args.ReissueCookie = true;
			}
		}
	}
}