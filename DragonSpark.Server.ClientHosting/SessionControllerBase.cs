using System.IdentityModel.Services;
using System.Web.Http;

namespace DragonSpark.Server.ClientHosting
{
	public class SessionControllerBase : ApiController
	{
		readonly ClientApplicationConfiguration configuration;

		public SessionControllerBase( ClientApplicationConfiguration configuration )
		{
			this.configuration = configuration;
		}
		
		public virtual void SignOut()
		{
			FederatedAuthentication.SessionAuthenticationModule.SignOut();
		}

		public virtual ClientApplicationConfiguration GetConfiguration()
		{
			return configuration;
		}
	}
}