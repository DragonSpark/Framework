using Newtonsoft.Json;

namespace DragonSpark.Server.Security
{
	public sealed class AuthenticationClaim
	{
		[JsonProperty("typ")]
		public string Type { get; set; } = default!;

		[JsonProperty("val")]
		public string Value { get; set; } = default!;
	}
}