using System.Text.Json.Serialization;

namespace DragonSpark.Identity.Mixcloud.Api;

sealed class ErrorResult
{
	[JsonPropertyName("type")]
	public string Type { get; set; } = default!;

	[JsonPropertyName("message")]
	public string Message { get; set; } = default!;
}