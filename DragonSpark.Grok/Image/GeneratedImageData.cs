using System.Text.Json.Serialization;

namespace DragonSpark.Grok.Image;

public record GeneratedImageData(
	[property: JsonPropertyName("url")] Uri? Url,
	[property: JsonPropertyName("b64_json")]
	string? Base64Json,
	[property: JsonPropertyName("revised_prompt")]
	string? RevisedPrompt
);