using DragonSpark.Model.Commands;
using Polly;

namespace DragonSpark.Application.AspNet.Diagnostics;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using Polly;
using Polly.Timeout;
using System;
using System.Net;
using System.Net.Http;

public sealed class ConfigureHttpBuilder : ICommand<IHttpClientBuilder>
{
	public static ConfigureHttpBuilder Default { get; } = new();

	ConfigureHttpBuilder() : this(ShouldProcess.Default.Get) {}

	readonly Action<HttpStandardResilienceOptions> _options;

	public ConfigureHttpBuilder(Func<Exception, bool> condition)
		: this(x => x.Retry.ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
		                                   .Handle<TimeoutRejectedException>()
		                                   .Handle<HttpRequestException>()
		                                   .Handle(condition)
		                                   .HandleResult(y => (int)y.StatusCode >= 500 ||
		                                                      y.StatusCode == HttpStatusCode.RequestTimeout)) {}

	public ConfigureHttpBuilder(Action<HttpStandardResilienceOptions> options) => _options = options;

	public void Execute(IHttpClientBuilder parameter)
	{
		parameter.AddStandardResilienceHandler(x =>
		                                       {
			                                       // 1. Core timeouts
			                                       x.AttemptTimeout.Timeout          = TimeSpan.FromSeconds(20);
			                                       x.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(45);
			                                       x.TotalRequestTimeout.Timeout     = TimeSpan.FromMinutes(1);

			                                       // 2. Run initial options delegate (sets ShouldHandle from constructor)
			                                       _options(x);
		                                       });
	}
}