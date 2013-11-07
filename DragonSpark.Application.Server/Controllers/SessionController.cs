using DragonSpark.Server.ClientHosting;
using System.Web.Http;

namespace DragonSpark.Application.Server.Controllers
{
	[RoutePrefix( "Session" )]
	public class SessionController : SessionControllerBase
	{
		public SessionController( ClientApplicationConfiguration configuration ) : base( configuration )
		{}

		[Route( "{action}" ), System.Web.Http.Authorize]
		public override void SignOut()
		{
			base.SignOut();
		}

		[Route( "Configuration" )]
		public override ClientApplicationConfiguration GetConfiguration()
		{
			return base.GetConfiguration();
		}
	}
}