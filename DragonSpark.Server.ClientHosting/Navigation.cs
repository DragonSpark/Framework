using System.Collections.Generic;

namespace DragonSpark.Server.ClientHosting
{
	public class Navigation
	{
		public RouteInfo Shell { get; set; }
		
		public RouteInfo NotFound { get; set; }

		public IEnumerable<RouteInfo> Routes { get; set; }
	}
}