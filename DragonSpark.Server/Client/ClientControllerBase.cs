using AttributeRouting.Web.Http;
using DragonSpark.Extensions;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Web;
using System.Web.Http;

namespace DragonSpark.Server.Client
{
	public abstract class ClientControllerBase : ApiController
	{
		readonly ClientApplicationConfiguration configuration;
		readonly string exceptionReportingPolicyName;

		protected ClientControllerBase( ClientApplicationConfiguration configuration, string exceptionReportingPolicyName = "Client Exception Reporting" )
		{
			this.configuration = configuration;
			this.exceptionReportingPolicyName = exceptionReportingPolicyName;
		}

		public ClientApplicationConfiguration ClientApplicationConfiguration
		{
			get { return configuration; }
		}

		[HttpPost, POST( "Report" )]
		public void ReportException( [FromBody] ClientExceptionDetail clientExceptionToReport )
		{
			var context = Request.Properties.TryGet( "MS_HttpContext" );
			var address = context.AsTo<HttpContextBase, string>( x => x.Request.UserHostAddress );
			var name = context.AsTo<HttpContextBase, string>( x => x.Request.UserHostName );
			var exception = new ClientException( clientExceptionToReport, address, name );
			ExceptionPolicy.HandleException( exception, exceptionReportingPolicyName );
		}

		[GET("Configuration")]
		public ClientApplicationConfiguration GetConfiguration()
		{
			return configuration;
		}
	}
}