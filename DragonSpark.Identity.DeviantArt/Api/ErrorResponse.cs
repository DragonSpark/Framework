using System.Text.Json.Serialization;

namespace DragonSpark.Identity.DeviantArt.Api;

sealed class ErrorResponse : ApiResponse
{
	[JsonPropertyName("error_code")]
	public byte Number { get; set; }
}