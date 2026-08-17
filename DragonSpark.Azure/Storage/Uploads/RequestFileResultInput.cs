using Microsoft.Net.Http.Headers;

namespace DragonSpark.Azure.Storage.Uploads;

public readonly record struct RequestFileResultInput(
	Stream Stream,
	string ContentType,
	string Name,
	DateTimeOffset Modified,
	EntityTagHeaderValue Tag,
	bool IsStreamable);