using System.Text.Json.Serialization;

namespace DragonSpark.Identity.Twitter.Api;

public sealed class TwitterResponseDocument
{
	[JsonPropertyName("data")]
	public TwitterResponseData Data { get; init; } = null!;
}