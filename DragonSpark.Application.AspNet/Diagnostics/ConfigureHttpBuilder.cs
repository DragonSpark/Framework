using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using Polly;

namespace DragonSpark.Application.AspNet.Diagnostics;

public sealed class ConfigureHttpBuilder : ICommand<IHttpClientBuilder>
{
	public static ConfigureHttpBuilder Default { get; } = new();

	ConfigureHttpBuilder() : this(ShouldProcess.Default.Get) {}

	readonly Action<HttpStandardResilienceOptions> _options;

	public ConfigureHttpBuilder(Func<Exception, bool> condition)
		: this(x => x.Retry.ShouldHandle = new PredicateBuilder<HttpResponseMessage>().Handle(condition)) {}

	public ConfigureHttpBuilder(Action<HttpStandardResilienceOptions> options) => _options = options;

	public void Execute(IHttpClientBuilder parameter)
	{
		parameter.AddStandardResilienceHandler(_options);
	}
}