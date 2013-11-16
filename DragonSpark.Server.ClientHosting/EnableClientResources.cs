using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Windows.Markup;
using DragonSpark.Extensions;
using DragonSpark.Objects;
using DragonSpark.Server.Configuration;
using Activator = DragonSpark.Runtime.Activator;

namespace DragonSpark.Server.ClientHosting
{
	[ContentProperty( "Includes" )]
	public class EnableClientResources : IHttpApplicationConfigurator, IRouteHandler
	{
		readonly ClientResourceHttpHandler handler = new ClientResourceHttpHandler();

		[DefaultPropertyValue( VirtualPathProvider.Qualifier + "{*url}" )]
		public string RouteTemplate { get; set; }

		[IoCDefault]
		public IClientResourceLocator Locator { get; set; }

		public void Configure( HttpConfiguration configuration )
		{
			BundleTable.VirtualPathProvider = Activator.Create<VirtualPathProvider>( HostingEnvironment.VirtualPathProvider );
			RouteTable.Routes.MapRoute( "Client", RouteTemplate ).RouteHandler = this;

			var bundle = new ScriptBundle( VirtualPathProvider.Application );

			var resources = Locator.LocateAll().Select( x => x.LocalPath ).Concat( Includes.Select( x => x.Location.LocalPath ) ).Select( x => string.Concat( "~/", VirtualPathProvider.Qualifier, x.Substring( 1 ) ) ).ToArray();
				
			resources.Apply( x => bundle.Include( x ) );
				
			BundleTable.Bundles.Add( bundle );
		}

		public Collection<ClientResource> Includes
		{
			get { return includes; }
		}	readonly Collection<ClientResource> includes = new Collection<ClientResource>();
		
		IHttpHandler IRouteHandler.GetHttpHandler( RequestContext requestContext )
		{
			return handler;
		}
	}
}