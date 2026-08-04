using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace DragonSpark.Diagnostics;

sealed class ConfigureDeferredLogging : ICommand<IServiceCollection>
{
	public static ConfigureDeferredLogging Default { get; } = new();

	ConfigureDeferredLogging() {}

	public void Execute(IServiceCollection parameter)
	{
		var configuration = parameter.Configuration();
		var store         = new StoredLogger(configuration);
		parameter.TryDecorate<ILogger>(x => new Logger(store, x)); // TODO: Fix
	}
}