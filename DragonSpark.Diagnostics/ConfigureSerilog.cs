using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using SerilogTracing;
using System;
using ILogger = Serilog.ILogger;

namespace DragonSpark.Diagnostics;

sealed class ConfigureSerilog : ICommand<IServiceCollection>
{
	readonly Func<IServiceProvider, ILoggerProvider>        _provider;
	readonly Func<LoggerConfiguration, LoggerConfiguration> _configuration;
	readonly bool                                           _configure;

	public ConfigureSerilog(Func<IServiceProvider, ILoggerProvider> provider,
	                        Func<LoggerConfiguration, LoggerConfiguration> configuration, bool configure)
	{
		_provider      = provider;
		_configuration = configuration;
		_configure     = configure;
	}

	public void Execute(IServiceCollection parameter)
	{
		var logger = new Logger(new StoredLogger(parameter.Configuration(), _configuration));
		var services = parameter.AddSingleton(new ActivityListenerConfiguration())
		                        .AddSingleton<IFlushLogging, FlushLogging>()
		                        .AddScoped(_provider)
		                        .AddSingleton<ILogger>(logger);
		if (_configure)
		{
			services.AddSerilog(logger);
		}
	}
}