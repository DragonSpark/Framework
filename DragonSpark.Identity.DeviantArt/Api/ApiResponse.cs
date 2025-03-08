using System.Text.Json.Serialization;

namespace DragonSpark.Identity.DeviantArt.Api;

class ApiResponse
{
	[JsonPropertyName("status")]
	public string Status { get; set; } = null!;

	[JsonPropertyName("error")]
	public string? Error { get; set; } = null!;

	[JsonPropertyName("error_description")]
	public string? ErrorMessage { get; set; } = null!;
}