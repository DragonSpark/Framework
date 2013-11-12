using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DragonSpark.Io;
using DragonSpark.Objects;
using DragonSpark.Server.Configuration;

namespace DragonSpark.Server.ClientHosting
{
	public class EnableClientResources : IHttpApplicationConfigurator, IRouteHandler
	{
		[DefaultPropertyValue( "Client/{assemblyName}/{*filePath}" )]
		public string RouteTemplate { get; set; }

		/*[DefaultPropertyValue( VirtualPathProvider.Directory )]
		public string VirtualPath { get; set; }*/

		[IoCDefault]
		public IPathResolver PathResolver { get; set; }

		[DefaultPropertyValue( ".." )]
		public string FrameworkPath { get; set; } // TODO: Figure this out.  It's lowwwwwwww rent.

		public void Configure( HttpConfiguration configuration )
		{
			BundleTable.VirtualPathProvider = new VirtualPathProvider( HostingEnvironment.VirtualPathProvider );
			RouteTable.Routes.MapRoute( "Client", RouteTemplate ).RouteHandler = this;
			BundleTable.Bundles.Add( new ClientResourcesBundle() );
			BundleTable.Bundles.Add( new PackagedClientResourcesBundle() );
		}

		IHttpHandler IRouteHandler.GetHttpHandler(RequestContext requestContext)
		{
			var result = new ClientResourceHttpHandler( PathResolver, requestContext.RouteData, FrameworkPath );
			return result;
		}
	}
}