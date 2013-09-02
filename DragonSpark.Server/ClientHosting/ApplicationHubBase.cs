using DragonSpark.Extensions;
using Microsoft.AspNet.SignalR;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace DragonSpark.Server.ClientHosting
{
	public abstract class ApplicationHubBase : Hub
	{
		readonly ClientApplicationConfiguration configuration;
		readonly string exceptionReportingPolicyName;

		protected ApplicationHubBase( ClientApplicationConfiguration configuration, string exceptionReportingPolicyName = "Client Exception Reporting" )
		{
			this.configuration = configuration;
			this.exceptionReportingPolicyName = exceptionReportingPolicyName;
		}

		public void ReportException( ClientExceptionDetail clientExceptionToReport )
		{
			var address = Context.Request.Environment[ "server.RemoteIpAddress" ].To<string>();
			var name = Context.Request.Headers[ "Host" ];
			var exception = new ClientException( clientExceptionToReport, address, name );
			ExceptionPolicy.HandleException( exception, exceptionReportingPolicyName );
		}

		public ClientApplicationConfiguration GetConfiguration()
		{
			return configuration;
		}
	}
}