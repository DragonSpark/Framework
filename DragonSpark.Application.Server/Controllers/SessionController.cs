using AttributeRouting;
using AttributeRouting.Web.Http;
using DragonSpark.Server.ClientHosting;
using System.IdentityModel.Services;
using System.Web.Http;

namespace DragonSpark.Application.Server.Controllers
{
	[RoutePrefix( "{controller}" )]
	public class SessionController : ApiController
	{
		readonly ClientApplicationConfiguration configuration;

		public SessionController( ClientApplicationConfiguration configuration )
		{
			this.configuration = configuration;
		}

		[POST( "{action}" ), Authorize]
		public void SignOut()
		{
			FederatedAuthentication.SessionAuthenticationModule.SignOut();
		}

		[GET( "Configuration" )]
		public ClientApplicationConfiguration GetConfiguration()
		{
			return configuration;
		}
	}
}