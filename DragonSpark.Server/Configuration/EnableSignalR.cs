using DragonSpark.Extensions;
using DragonSpark.Logging;
using DragonSpark.Objects;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Owin;
using System;
using System.Web.Http;
using IDependencyResolver = Microsoft.AspNet.SignalR.IDependencyResolver;

namespace DragonSpark.Server.Configuration
{
	public class EnableSignalR : IHttpApplicationConfigurator
	{
		HttpConfiguration HttpConfiguration { get; set; }

		public void Configure( HttpConfiguration configuration )
		{
			HttpConfiguration = configuration;
			Instance = this;
		}

		[DefaultPropertyValue( EnterpriseLibraryExceptionHandler.DefaultExceptionPolicy )]
		public string ExceptionPolicyName { get; set; }

		static EnableSignalR Instance { get; set; }

		public static void Configuration( IAppBuilder app )
		{
			var config = Instance.Transform( x => x.HttpConfiguration );
			config.NotNull( x => x.DependencyResolver.As<IDependencyResolver>( y =>
			{
				y.GetService( typeof(IHubPipeline) ).To<IHubPipeline>().AddModule( new HubExceptionModule( Instance.ExceptionPolicyName ) );

				var configuration = new HubConfiguration { Resolver = y, EnableDetailedErrors = true };
				app.MapSignalR( configuration );
			} ) );
        }
	}

	public class HubExceptionModule : HubPipelineModule
	{
		readonly string policyName;

		public HubExceptionModule( string policyName )
		{
			this.policyName = policyName;
		}

		protected override void OnIncomingError(Exception ex, IHubIncomingInvokerContext context)
		{
			Exception error;
			ExceptionPolicy.HandleException( ex, policyName, out error );

			context.Hub.Clients.Caller.ExceptionHandler( error );
		}
	}

}