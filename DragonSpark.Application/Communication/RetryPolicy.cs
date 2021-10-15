using DragonSpark.Diagnostics;
using System.Net.Http;

namespace DragonSpark.Application.Communication;

sealed class RetryPolicy : RetryPolicy<HttpResponseMessage>, ICommunicationsPolicy
{
	public static RetryPolicy Default { get; } = new RetryPolicy();

	RetryPolicy() {}
}