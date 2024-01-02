using DragonSpark.Diagnostics;
using System.Net.Http;
using Policy = Polly.Policy;

namespace DragonSpark.Application.Connections.Events;

sealed class RequestBuilder : Builder
{
	public static RequestBuilder Default { get; } = new();

	RequestBuilder() : base(Policy.Handle<HttpRequestException>()) {}
}