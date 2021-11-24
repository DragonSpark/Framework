using System.Text.Json.Serialization;

namespace DragonSpark.Identity.DeviantArt.Api;

sealed class UserResult
{
	[JsonPropertyName("userid")]
	public string UserId { get; set; } = default!;
}