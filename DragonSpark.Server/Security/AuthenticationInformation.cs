using Newtonsoft.Json;

namespace DragonSpark.Server.Security
{
	public sealed class AuthenticationInformation
	{
		[JsonProperty("access_token", NullValueHandling = NullValueHandling.Ignore)]
		public string AccessToken { get; set; } = default!;

		[JsonProperty("provider_name", NullValueHandling = NullValueHandling.Ignore)]
		public string ProviderName { get; set; } = default!;

		[JsonProperty("user_id", NullValueHandling = NullValueHandling.Ignore)]
		public string UserId { get; set; } = default!;

		[JsonProperty("user_claims", NullValueHandling = NullValueHandling.Ignore)]
		public AuthenticationClaim[] UserClaims { get; set; } = default!;

		[JsonProperty("access_token_secret", NullValueHandling = NullValueHandling.Ignore)]
		public string AccessTokenSecret { get; set; } = default!;

		[JsonProperty("authentication_token", NullValueHandling = NullValueHandling.Ignore)]
		public string AuthenticationToken { get; set; } = default!;

		[JsonProperty("expires_on", NullValueHandling = NullValueHandling.Ignore)]
		public string ExpiresOn { get; set; } = default!;

		[JsonProperty("id_token", NullValueHandling = NullValueHandling.Ignore)]
		public string IdToken { get; set; } = default!;

		[JsonProperty("refresh_token", NullValueHandling = NullValueHandling.Ignore)]
		public string RefreshToken { get; set; } = default!;
	}
}