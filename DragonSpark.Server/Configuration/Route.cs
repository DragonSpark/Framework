using System.Collections.Generic;
using System.Web.Routing;
using System.Windows.Markup;

namespace DragonSpark.Server.Configuration
{
	[ContentProperty( "Defaults" )]
	public class Route
	{
		public string RouteName { get; set; }

		public string Template { get; set; }

		public IDictionary<string, object> Defaults { get; set; }

		public IDictionary<string, object> Constraints { get; set; }

		public IRouteHandler Handler { get; set; }
	}
}