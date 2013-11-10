using System;
using DragonSpark.Server.ClientHosting;
using Microsoft.AspNet.SignalR.Hubs;

namespace DragonSpark.Application.Server.Controllers
{
	[HubName( "application" )]
	public class ApplicationHub : ApplicationHubBase
	{
		public const string ClientExceptionReporting = "Client Exception Reporting";

		public void Throw()
		{
			throw new InvalidOperationException( "This is an exception thrown on the server-side." );
		}
	}
}