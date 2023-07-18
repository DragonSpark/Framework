using System.Text.Json.Serialization;

namespace DragonSpark.Identity.Twitter.Api;

public sealed class TwitterResponseData
{
	[JsonPropertyName("id")]
	public string Id { get; init; } = default!;
}