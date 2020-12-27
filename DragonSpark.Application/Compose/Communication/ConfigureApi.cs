using DragonSpark.Application.Communication;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Refit;
using System;
using System.Linq;
using System.Net.Http;

namespace DragonSpark.Application.Compose.Communication
{
	sealed class ConfigureApi<T> : ICommand<IServiceCollection> where T : class
	{
		readonly Action<IServiceProvider, HttpClient> _configure;
		readonly RefitSettings?                       _settings;
		readonly IAsyncPolicy<HttpResponseMessage>[]  _policies;

		public ConfigureApi(Action<IServiceProvider, HttpClient> configure, RefitSettings? settings = null)
			: this(configure, settings,
			       LocationAwareConnectionPolicy.Default.Then()
			                                    .Select(RetryPolicy.Default)
			                                    .Instance(),
			       ConnectionPolicy.Default.Then().Select(CircuitBreakerPolicy.Default).Instance()) {}

		public ConfigureApi(Action<IServiceProvider, HttpClient> configure, RefitSettings? settings = null,
		                    params IAsyncPolicy<HttpResponseMessage>[] policies)
		{
			_configure = configure;
			_settings  = settings;
			_policies  = policies;
		}

		public void Execute(IServiceCollection parameter)
		{
			var seed = parameter.AddRefitClient<T>(_settings)
			                    .ConfigureHttpClient(_configure);
			_policies.Aggregate(seed, (current, policy) => current.AddPolicyHandler(policy));
		}
	}
}