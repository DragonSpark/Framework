using System;
using System.ServiceModel.Activation;
using DragonSpark.Application;
using DragonSpark.Application.Communication;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;

namespace DragonSpark.Features
{
	[AspNetCompatibilityRequirements( RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed ), ExceptionShielding, WcfBehaviorBaseWcfErrorBehavior, WcfBehaviorBaseWcfSilverlightFaultBehavior]
	public class FeaturesApplicationService : ApplicationServiceBase, IFeaturesApplicationService
	{
		public FeaturesApplicationService( ApplicationDetails details, string exceptionReportingPolicyName = "Exception Reporting" ) : base( details, exceptionReportingPolicyName )
		{}

		public void ThrowException()
		{
			throw new InvalidOperationException( "This is an exception thrown from the Service." );
		}
	}
}
