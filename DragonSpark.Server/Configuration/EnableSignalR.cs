using DragonSpark.Extensions;
using DragonSpark.Logging;
using DragonSpark.Objects;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Newtonsoft.Json;
using Owin;
using System;
using System.Web;
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
				var serializer = new JsonSerializer { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
				y.Register( typeof(JsonSerializer), () => serializer );
				var pipeline = y.GetService( typeof(IHubPipeline) ).To<IHubPipeline>();
				pipeline.AddModule( new ApplicationModule( Instance.ExceptionPolicyName ) );

				var configuration = new HubConfiguration { Resolver = y, EnableDetailedErrors = true };
				
				app.MapSignalR( configuration );
			} ) );
		}
	}

	public static class SystemWebExtensions
    {
		public static HttpModuleCollection GetModules( this HttpContextBase target )
		{
			var result = target.Items[Server.ApplicationModule.ApplicationModulesKey].As<HttpModuleCollection>();
			return result;
		}

        public static HttpContextBase GetHttpContext(this IRequest request)
        {
            object value;
            if (request.Environment.TryGetValue(typeof(HttpContextBase).FullName, out value))
            {
                return (HttpContextBase)value;
            }

            return null;
        }
    }


	public class ApplicationModule : HubPipelineModule
	{
		readonly string policyName;

		public ApplicationModule( string policyName )
		{
			this.policyName = policyName;
		}

		protected override void OnIncomingError(Exception ex, IHubIncomingInvokerContext context)
		{
			Exception error;
			ExceptionPolicy.HandleException( ex, policyName, out error );

			context.Hub.Clients.Caller.ExceptionHandler( error );
		}

		/*protected override void OnAfterDisconnect( IHub hub )
		{
			base.OnAfterDisconnect( hub );

			hub.As<Hub>( x =>
			{
				x.Context.Request.GetHttpContext().DisposeOnPipelineCompleted()
			} );

			ServerContext.Dispose();
		}*/

		/*protected override bool OnBeforeConnect( IHub hub )
		{
			return base.OnBeforeConnect( hub );
		}

		protected override void OnAfterConnect( IHub hub )
		{
			base.OnAfterConnect( hub );
		}

		protected override bool OnBeforeDisconnect( IHub hub )
		{
			var current = ServerContext.Current;
			return base.OnBeforeDisconnect( hub );
		}

		public override Func<IHub, Task> BuildDisconnect( Func<IHub, Task> disconnect )
		{
			var current = ServerContext.Current;
			return base.BuildDisconnect( disconnect );
		}*/
	}

}