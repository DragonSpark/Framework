using DragonSpark.Extensions;
using Microsoft.AspNet.SignalR;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace DragonSpark.Server.ClientHosting
{
	public abstract class ClientHubBase : Hub
	{
		readonly string exceptionReportingPolicyName;

		protected ClientHubBase( string exceptionReportingPolicyName = "Client Exception Reporting" )
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
}