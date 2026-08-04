using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SerilogTracing;

namespace DragonSpark.Diagnostics;

sealed class ConfigureSerilog : ICommand<IServiceCollection>
{
	readonly Action<IServiceProvider, LoggerConfiguration> _configure;
	readonly ActivityListenerConfiguration                 _listener;
	readonly bool                                          _preserveExistingLogging;

	public ConfigureSerilog(Action<IServiceProvider, LoggerConfiguration> configure, bool preserveExistingLogging)
		: this(configure, new(), preserveExistingLogging) {}

	public ConfigureSerilog(Action<IServiceProvider, LoggerConfiguration> configure,
	                        ActivityListenerConfiguration listener, bool preserveExistingLogging)
	{
		_configure               = configure;
		_listener                = listener;
		_preserveExistingLogging = preserveExistingLogging;
	}

	public void Execute(IServiceCollection parameter)
	{
		parameter.AddSingleton(_listener).AddSingleton<IFlushLogging, FlushLogging>();
		parameter.AddSerilog(_configure, writeToProviders: _preserveExistingLogging);
	}
}