using System.Web.Http;
using Microsoft.Web.WebPages.OAuth;

namespace DragonSpark.Server.Configuration
{
	public class RegisterGoogleClient : RegisterClientBase
	{
		public RegisterGoogleClient()
		{
			DisplayName = "Google";
		}

		public override void Configure( HttpConfiguration configuration )
		{
			OAuthWebSecurity.RegisterGoogleClient( DisplayName, ExtraData );
		}
	}
}