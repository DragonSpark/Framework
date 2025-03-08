using System.Text.Json.Serialization;

namespace DragonSpark.Identity.DeviantArt.Api;

sealed class AccessTokenResponse : ApiResponse
{
	[JsonPropertyName("access_token")]
	public string Token { get; set; } = null!;

	[JsonPropertyName("expires_in")]
	public int ExpirationInSeconds { get; set; }
}