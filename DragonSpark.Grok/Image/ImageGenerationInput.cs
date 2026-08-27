using System.Text.Json.Serialization;

namespace DragonSpark.Grok.Image;

public readonly record struct ImageGenerationInput(
	string Prompt,

	string Model = "grok-imagine-image-2.0",

	[property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	string? Quality = null,

	[property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	string? Resolution = null,

	[property: JsonPropertyName("n")] 
	[property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	int? Count = null,

	[property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	string? Size = null
);