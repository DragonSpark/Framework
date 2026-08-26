using System.Text.Json.Serialization;

namespace DragonSpark.Grok.Image;

public record ImageGenerationResponsePayload(
	[property: JsonPropertyName("created")]
	long Created,
	[property: JsonPropertyName("data")] IReadOnlyList<GeneratedImageData> Data
);