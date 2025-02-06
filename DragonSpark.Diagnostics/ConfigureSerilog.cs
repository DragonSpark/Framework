using System;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using SerilogTracing;
using ILogger = Serilog.ILogger;

namespace DragonSpark.Diagnostics;

sealed class ConfigureSerilog : ICommand<IServiceCollection>
{
	readonly Func<IServiceProvider, ILoggerProvider> _provider;

	public ConfigureSerilog(Func<IServiceProvider, ILoggerProvider> provider) => _provider = provider;

	public void Execute(IServiceCollection parameter)
	{
		var configuration = new LoggerConfiguration().ReadFrom.Configuration(parameter.Configuration());
		var logger        = configuration.CreateLogger();
		parameter.AddSingleton(new ActivityListenerConfiguration())
		         .AddSingleton(configuration)
		         .AddSingleton<ILogger>(_ => logger)
		         .AddScoped(_provider)
		         .AddSerilog(logger);
	}
}