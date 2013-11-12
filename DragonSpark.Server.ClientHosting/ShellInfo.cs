using Newtonsoft.Json;

namespace DragonSpark.Server.ClientHosting
{
	public class ShellInfo : RouteInfo
	{
		[JsonProperty( "transitionName" )]
		public string TransitionName { get; set; }
	}
}