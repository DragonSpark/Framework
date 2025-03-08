using Newtonsoft.Json;

namespace DragonSpark.Server.Security;

public sealed class AuthenticationInformation
{
	[JsonProperty("access_token", NullValueHandling = NullValueHandling.Ignore)]
	public string AccessToken { get; set; } = null!;

	[JsonProperty("provider_name", NullValueHandling = NullValueHandling.Ignore)]
	public string ProviderName { get; set; } = null!;

	[JsonProperty("user_id", NullValueHandling = NullValueHandling.Ignore)]
	public string UserId { get; set; } = null!;

	[JsonProperty("user_claims", NullValueHandling = NullValueHandling.Ignore)]
	public AuthenticationClaim[] UserClaims { get; set; } = null!;

	[JsonProperty("access_token_secret", NullValueHandling = NullValueHandling.Ignore)]
	public string AccessTokenSecret { get; set; } = null!;

	[JsonProperty("authentication_token", NullValueHandling = NullValueHandling.Ignore)]
	public string AuthenticationToken { get; set; } = null!;

	[JsonProperty("expires_on", NullValueHandling = NullValueHandling.Ignore)]
	public string ExpiresOn { get; set; } = null!;

	[JsonProperty("id_token", NullValueHandling = NullValueHandling.Ignore)]
	public string IdToken { get; set; } = null!;

	[JsonProperty("refresh_token", NullValueHandling = NullValueHandling.Ignore)]
	public string RefreshToken { get; set; } = null!;
}