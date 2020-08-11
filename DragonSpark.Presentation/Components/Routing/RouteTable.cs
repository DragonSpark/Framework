using DragonSpark.Model.Sequences;
using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Components.Routing
{
	/// <summary>
	/// ATTRIBUTION: https://github.com/ShaunCurtis/CEC.Routing/tree/master/CEC.Routing/Routing
	/// </summary>
	class RouteTable
	{
		readonly uint _length;

		public RouteTable(Array<RouteEntry> routes) : this(routes, routes.Length) {}

		public RouteTable(Array<RouteEntry> routes, uint length)
		{
			_length = length;
			Routes  = routes;
		}

		public RouteEntry[] Routes { get; }

		internal RouteData? Route(RouteContext routeContext)
		{
			for (var i = 0; i < _length; i++)
			{
				var result = Routes[i].Get(routeContext);
				if (result != null)
				{
					return result;
				}
			}

			return null;
		}
	}
}