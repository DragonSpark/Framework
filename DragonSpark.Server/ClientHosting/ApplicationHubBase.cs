using AttributeRouting.Web.Http;
using DragonSpark.Extensions;
using Microsoft.AspNet.SignalR;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.IdentityModel.Services;
using System.Web.Http;

namespace DragonSpark.Server.ClientHosting
{
	public abstract class ApplicationHubBase : Hub
	{
		readonly string exceptionReportingPolicyName;

		protected ApplicationHubBase( string exceptionReportingPolicyName = "Client Exception Reporting" )
		{
			this.exceptionReportingPolicyName = exceptionReportingPolicyName;
		}

		public void ReportException( ClientExceptionDetail clientExceptionToReport )
		{
			var address = Context.Request.Environment[ "server.RemoteIpAddress" ].To<string>();
			var name = Context.Request.Headers[ "Host" ];
			var exception = new ClientException( clientExceptionToReport, address, name );
			ExceptionPolicy.HandleException( exception, exceptionReportingPolicyName );
		}
	}

	public class SessionControllerBase : ApiController
	{
		readonly ClientApplicationConfiguration configuration;

		public SessionControllerBase( ClientApplicationConfiguration configuration )
		{
			this.configuration = configuration;
		}
		
		[POST( "{action}" ), System.Web.Http.Authorize, HttpPost]
		public void SignOut()
		{
			FederatedAuthentication.SessionAuthenticationModule.SignOut();
		}

		[GET( "Configuration" ), HttpGet]
		public ClientApplicationConfiguration GetConfiguration()
		{
			return configuration;
		}
	}
}