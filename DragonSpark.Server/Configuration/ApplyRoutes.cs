using DragonSpark.Extensions;
using System;
using System.Collections.ObjectModel;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Windows.Markup;

namespace DragonSpark.Server.Configuration
{
	[ContentProperty( "Routes" )]
	public class ApplyRoutes : IHttpApplicationConfigurator
	{
		public void Configure( HttpConfiguration configuration )
		{
			RouteTable.Routes.With( x =>
			{
				Ignores.Apply( y => x.Ignore( y.Url, y.Constraints ) );
				Routes.Apply( y => x.MapRoute( y.RouteName ?? Guid.NewGuid().ToString(), y.Template, y.Defaults, y.Constraints ) );
				ApiRoutes.Apply( y => x.MapHttpRoute( y.RouteName ?? Guid.NewGuid().ToString(), y.Template, y.Defaults, y.Constraints ) );
			} );
		}

		public Collection<IgnoreRoute> Ignores
		{
			get { return ignores; }
		}	readonly Collection<IgnoreRoute> ignores = new Collection<IgnoreRoute>();

		public Collection<Route> Routes
		{
			get { return routes; }
		}	readonly Collection<Route> routes = new Collection<Route>();

		public Collection<Route> ApiRoutes
		{
			get { return apiRoutes; }
		}	readonly Collection<Route> apiRoutes = new Collection<Route>();
	}
}