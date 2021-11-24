using System.Text.Json.Serialization;

namespace DragonSpark.Identity.DeviantArt.Api;

class ApiResponse
{
	[JsonPropertyName("status")]
	public string Status { get; set; } = default!;

	[JsonPropertyName("error")]
	public string? Error { get; set; } = default!;

	[JsonPropertyName("error_description")]
	public string? ErrorMessage { get; set; } = default!;
}