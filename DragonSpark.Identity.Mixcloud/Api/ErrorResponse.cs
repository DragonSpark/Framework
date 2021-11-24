using System.Text.Json.Serialization;

namespace DragonSpark.Identity.Mixcloud.Api;

sealed class ErrorResponse : ApiResponse
{
	[JsonPropertyName("error")]
	public ErrorResult Error { get; set; } = default!;
}