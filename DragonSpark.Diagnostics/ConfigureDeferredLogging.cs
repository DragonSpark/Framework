using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SerilogTracing;

namespace DragonSpark.Diagnostics;

sealed class ConfigureDeferredLogging : ICommand<IServiceCollection>
{
	readonly ActivityListenerConfiguration _listener;
	readonly bool                          _preserveOutputs;

	public ConfigureDeferredLogging(bool preserveOutputs) : this(new(), preserveOutputs) {}

	public ConfigureDeferredLogging(ActivityListenerConfiguration listener, bool preserveOutputs)
	{
		_listener        = listener;
		_preserveOutputs = preserveOutputs;
	}

	public void Execute(IServiceCollection parameter)
	{
		var configuration = parameter.Configuration();
		var logger        = new Logger(new StoredLogger(configuration));
		parameter.AddSingleton<IFlushLogging, FlushLogging>()
		         .AddSingleton(_listener)
		         .AddSerilog(logger, providers: _preserveOutputs ? new() : null);
	}
}