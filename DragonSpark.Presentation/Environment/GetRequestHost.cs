using DragonSpark.Text;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Presentation.Environment;

sealed class GetRequestHost : IFormatter<HttpRequest>
{
	public static GetRequestHost Default { get; } = new();

	GetRequestHost() {}

	public string Get(HttpRequest parameter) => $"{parameter.Scheme}://{parameter.Host}";
}