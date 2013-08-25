﻿using DragonSpark.Io;
using DragonSpark.Objects;
using DragonSpark.Server.ClientHosting;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using VirtualPathProvider = DragonSpark.Server.ClientHosting.VirtualPathProvider;

namespace DragonSpark.Server.Configuration
{
	public class EnableEmbeddedScripts : IHttpApplicationConfigurator, IRouteHandler
	{
		[DefaultPropertyValue( "Client/{assemblyName}/{*filePath}" )]
		public string RouteTemplate { get; set; }

		[DefaultPropertyValue( VirtualPathProvider.Path )]
		public string VirtualPath { get; set; }

		[IoCDefault]
		public IPathResolver PathResolver { get; set; }

		public void Configure( HttpConfiguration configuration )
		{
			BundleTable.VirtualPathProvider = new VirtualPathProvider( HostingEnvironment.VirtualPathProvider );
			RouteTable.Routes.MapRoute( "Client", RouteTemplate ).RouteHandler = this;
			BundleTable.Bundles.Add( new EmbeddedScriptsBundle( VirtualPath ) );
		}

		IHttpHandler IRouteHandler.GetHttpHandler(RequestContext requestContext)
		{
			var result = new EmbeddedScriptHttpHandler( PathResolver, requestContext.RouteData );
			return result;
		}
	}
}