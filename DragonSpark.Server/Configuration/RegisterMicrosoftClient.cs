using System.Web.Http;
using Microsoft.Web.WebPages.OAuth;

namespace DragonSpark.Server.Configuration
{
	public class RegisterMicrosoftClient : RegisterClientBase
	{
		public RegisterMicrosoftClient()
		{
			DisplayName = "Microsoft";
		}

		public string ApplicationId { get; set; }

		public string ApplicationSecret { get; set; }

		public override void Configure( HttpConfiguration configuration )
		{
			OAuthWebSecurity.RegisterMicrosoftClient( ApplicationId, ApplicationSecret, DisplayName, ExtraData );
		}
	}
}