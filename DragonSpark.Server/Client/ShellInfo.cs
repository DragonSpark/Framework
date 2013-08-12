using Newtonsoft.Json;

namespace DragonSpark.Server.Client
{
	public class ShellInfo : RouteInfo
	{
		[JsonProperty( "transitionName" )]
		public string TransitionName { get; set; }
	}
}