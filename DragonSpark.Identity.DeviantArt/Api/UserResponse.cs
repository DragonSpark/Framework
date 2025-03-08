using System.Text.Json.Serialization;

namespace DragonSpark.Identity.DeviantArt.Api;

sealed class UserResponse
{
	[JsonPropertyName("user")]
	public UserResult Result { get; set; } = null!;
}