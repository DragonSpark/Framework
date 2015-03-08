using System;
using System.Web;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace DragonSpark.Application
{
	public abstract class ApplicationServiceBase : IApplicationService
	{
		readonly ApplicationDetails details;

		readonly string exceptionReportingPolicyName;

	
		protected ApplicationServiceBase( ApplicationDetails details, string exceptionReportingPolicyName = "Exception Reporting" )
		{
			this.details = details;
			this.exceptionReportingPolicyName = exceptionReportingPolicyName;
		}

		public virtual void ReportException( ClientExceptionDetail clientExceptionToReport )
		{
			var exception = new ClientException( clientExceptionToReport );
			ExceptionPolicy.HandleException( exception, exceptionReportingPolicyName );
		}

		public virtual ApplicationDetails RetrieveApplicationDetails()
		{
			return details;
		}
	}
}