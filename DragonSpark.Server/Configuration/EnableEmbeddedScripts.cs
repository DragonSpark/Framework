using DragonSpark.Objects;
using DragonSpark.Server.Client;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using VirtualPathProvider = DragonSpark.Server.Client.VirtualPathProvider;

namespace DragonSpark.Server.Configuration
{
	public class EnableEmbeddedScripts : IHttpApplicationConfigurator, IRouteHandler
	{
		[DefaultPropertyValue( "Resources/Embedded/{assemblyName}/{*filePath}" )]
		public string RouteTemplate { get; set; }

		[DefaultPropertyValue( VirtualPathProvider.Path )]
		public string VirtualPath { get; set; }

		public void Configure( HttpConfiguration configuration )
		{
			BundleTable.VirtualPathProvider = new VirtualPathProvider( HostingEnvironment.VirtualPathProvider );
			RouteTable.Routes.MapRoute( "EmbeddedScripts", RouteTemplate ).RouteHandler = this;
			BundleTable.Bundles.Add( new EmbeddedScriptsBundle( VirtualPath ) );
		}

		IHttpHandler IRouteHandler.GetHttpHandler(RequestContext requestContext)
		{
			return new EmbeddedScriptHttpHandler(requestContext.RouteData);
		}
	}
}