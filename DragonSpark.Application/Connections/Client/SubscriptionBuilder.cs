using DragonSpark.Diagnostics;
using System.Net.Http;
using Policy = Polly.Policy;

namespace DragonSpark.Application.Connections.Client;

sealed class SubscriptionBuilder : Builder
{
	public static SubscriptionBuilder Default { get; } = new();

	SubscriptionBuilder() : base(Policy.Handle<HttpRequestException>()) {}
}