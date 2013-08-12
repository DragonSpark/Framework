using System.Web.Http;
using Microsoft.Web.WebPages.OAuth;

namespace DragonSpark.Server.Configuration
{
	public class RegisterFacebookClient : RegisterClientBase
	{
		public RegisterFacebookClient()
		{
			DisplayName = "Facebook";
		}

		public string ApplicationId { get; set; }

		public string ApplicationSecret { get; set; }

		public override void Configure( HttpConfiguration configuration )
		{
			OAuthWebSecurity.RegisterFacebookClient( ApplicationId, ApplicationSecret, DisplayName, ExtraData );
		}
	}
}