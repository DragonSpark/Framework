using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SerilogTracing;

namespace DragonSpark.Diagnostics;

sealed class ConfigureSerilog : ICommand<IServiceCollection>
{
	readonly Action<IServiceProvider, LoggerConfiguration> _configure;
	readonly ActivityListenerConfiguration                 _listener;
	readonly bool                                          _preserveOutputs;

	public ConfigureSerilog(Action<IServiceProvider, LoggerConfiguration> configure, bool preserveOutputs)
		: this(configure, new(), preserveOutputs) {}

	public ConfigureSerilog(Action<IServiceProvider, LoggerConfiguration> configure,
	                        ActivityListenerConfiguration listener, bool preserveOutputs)
	{
		_configure       = configure;
		_listener        = listener;
		_preserveOutputs = preserveOutputs;
	}

	public void Execute(IServiceCollection parameter)
	{
		parameter.AddSingleton(_listener).AddSerilog(_configure, writeToProviders: _preserveOutputs);
	}
}