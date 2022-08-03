using DragonSpark.Text;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace DragonSpark.Presentation.Environment;

sealed class GetRequestHost : IFormatter<HttpRequest>
{
	public static GetRequestHost Default { get; } = new();

	GetRequestHost() : this('.') {}

	readonly char _split;

	public GetRequestHost(char split) => _split = split;

	public string Get(HttpRequest parameter) => string.Join(_split, parameter.Host.Host.Split(_split).TakeLast(2));
}