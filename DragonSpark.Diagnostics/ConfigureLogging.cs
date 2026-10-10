using DragonSpark.Model.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;

namespace DragonSpark.Diagnostics;

sealed class ConfigureLogging : ICommand<(IServiceProvider, LoggerConfiguration)>
{
	readonly ICommand<ApplyConfigurationInput> _apply;
	readonly string[]                          _omitPolicySources;

	public ConfigureLogging(params string[] omitPolicySources) : this(ApplyConfiguration.Default, omitPolicySources) {}

	public ConfigureLogging(ICommand<ApplyConfigurationInput> apply, params string[] omitPolicySources)
	{
		_apply             = apply;
		_omitPolicySources = omitPolicySources;
	}

	public void Execute((IServiceProvider, LoggerConfiguration) parameter)
	{
		var (services, configuration) = parameter;

		_apply.Execute(new(configuration, services.GetRequiredService<IConfiguration>(), _omitPolicySources));

		var enrichers = services.GetServices<ILogEventEnricher>().ToArray();
		if (enrichers.Length > 0)
		{
			configuration.Enrich.With(enrichers);
		}
	}
}