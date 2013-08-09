using AttributeRouting;
using AttributeRouting.Web.Http;
using DragonSpark.Web.Client;
using System;
using System.IdentityModel.Services;

namespace DragonSpark.Application.Controllers
{
    [RoutePrefix( "{controller}" )]
	public class ClientController : ClientControllerBase
	{
	    public const string ClientExceptionReporting = "Client Exception Reporting";

	    public ClientController( ClientApplicationConfiguration configuration, string exceptionReportingPolicyName = ClientExceptionReporting ) : base( configuration, exceptionReportingPolicyName )
	    {}

	    [POST( "{action}" )]
		public void SignOut()
		{
			FederatedAuthentication.SessionAuthenticationModule.SignOut();
		}

		[POST( "{action}" )]
		public void Throw()
		{
			throw new InvalidOperationException( "This is an exception thrown on the server-side." );
		}



		/*public override ClientApplicationConfiguration GetConfiguration()
		{
			return base.GetConfiguration();
		}*/

		/*public override void ReportException( ClientExceptionDetail clientExceptionToReport )
		{
			base.ReportException( clientExceptionToReport );
		}*/
	}
}
