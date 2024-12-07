using DragonSpark.Diagnostics;
using JetBrains.Annotations;
using System.Net.Http;

namespace DragonSpark.Application.Communication;

sealed class RetryPolicy : RetryPolicy<HttpResponseMessage>, ICommunicationsPolicy
{
	[UsedImplicitly]
	public static RetryPolicy Default { get; } = new();

	RetryPolicy() {}
}