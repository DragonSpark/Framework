using System.Net.Http;
using DragonSpark.Diagnostics;
using JetBrains.Annotations;

namespace DragonSpark.Application.Communication;

sealed class RetryPolicy : RetryPolicy<HttpResponseMessage>, ICommunicationsPolicy
{
	[UsedImplicitly]
	public static RetryPolicy Default { get; } = new();

	RetryPolicy() {}
}