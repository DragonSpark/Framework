using System.Web.Http;
using Microsoft.Web.WebPages.OAuth;

namespace DragonSpark.Server.Configuration
{
	public class RegisterYahooClient : RegisterClientBase
	{
		public RegisterYahooClient()
		{
			DisplayName = "Yahoo";
		}

		public override void Configure( HttpConfiguration configuration )
		{
			OAuthWebSecurity.RegisterYahooClient( DisplayName, ExtraData );
		}
	}
}