using System.Collections.Generic;
using System.Windows.Markup;

namespace DragonSpark.Web.Configuration
{
	[ContentProperty( "Defaults" )]
	public class Route
	{
		public string RouteName { get; set; }

		public string Template { get; set; }

		public IDictionary<string, object> Defaults { get; set; }

		public IDictionary<string, object> Constraints { get; set; }
	}
}