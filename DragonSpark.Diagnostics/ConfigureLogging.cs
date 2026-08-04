using DragonSpark.Model.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;

namespace DragonSpark.Diagnostics;

sealed class ConfigureLogging : ICommand<(IServiceProvider, LoggerConfiguration)>
{
	public static ConfigureLogging Default { get; } = new();

	ConfigureLogging() : this(ApplyConfiguration.Default) {}

	readonly ICommand<ApplyConfigurationInput> _apply;

	public ConfigureLogging(ICommand<ApplyConfigurationInput> apply) => _apply = apply;

	public void Execute((IServiceProvider, LoggerConfiguration) parameter)
	{
		var (services, configuration) = parameter;
		
		_apply.Execute(new(configuration, services.GetRequiredService<IConfiguration>()));
		
		var enrichers = services.GetServices<ILogEventEnricher>().ToArray();
		if (enrichers.Length > 0)
		{
			configuration.Enrich.With(enrichers);
		}
	}
}