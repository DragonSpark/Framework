using Newtonsoft.Json;

namespace DragonSpark.Web.Client
{
	public class ShellInfo : RouteInfo
	{
		[JsonProperty( "transitionName" )]
		public string TransitionName { get; set; }
	}

	public class RouteInfo
	{
		[JsonProperty( "route" )]
		public string Route { get; set; }

		[JsonProperty( "title" )]
		public string Title { get; set; }

		[JsonProperty( "moduleId" )]
		public string Path { get; set; }

		[JsonProperty( "nav" )]
		public bool IsVisible { get; set; }

		[JsonProperty( "type" )]
		public string Type { get; set; }

		[JsonProperty( "hash" )]
		public string Tag { get; set; }

		[JsonProperty( "children" )]
		public RouteInfo[] Children { get; set; }
	}
}