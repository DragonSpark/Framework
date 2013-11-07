using System.IdentityModel.Services;
using DragonSpark.Server.ClientHosting;
using Microsoft.AspNet.SignalR.Hubs;
using System;

namespace DragonSpark.Application.Server.Controllers
{
    [HubName("application")]
	public class ClientHub : ApplicationHubBase
	{
	    public const string ClientExceptionReporting = "Client Exception Reporting";

	    public ClientHub( string exceptionReportingPolicyName = ClientExceptionReporting ) : base( exceptionReportingPolicyName )
	    {}

	    public void SignOut()
		{
			FederatedAuthentication.SessionAuthenticationModule.SignOut();
		}

		public void Throw()
		{
			throw new InvalidOperationException( "This is an exception thrown on the server-side." );
		}
	}
}
