using System.Text.Json.Serialization;

namespace DragonSpark.Identity.DeviantArt.Api;

sealed class AccessTokenResponse : ApiResponse
{
	[JsonPropertyName("access_token")]
	public string Token { get; set; } = default!;

	[JsonPropertyName("expires_in")]
	public int ExpirationInSeconds { get; set; } = default!;
}