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
    readonly bool                                    _configure;

    public ConfigureSerilog(Func<IServiceProvider, ILoggerProvider> provider, bool configure)
    {
        _provider  = provider;
        _configure = configure;
    }

    public void Execute(IServiceCollection parameter)
    {
        var configuration = new LoggerConfiguration().ReadFrom.Configuration(parameter.Configuration());
        var logger        = configuration.CreateLogger();
        var services = parameter.AddSingleton(new ActivityListenerConfiguration())
                                .AddSingleton(configuration)
                                .AddSingleton<IFlushLogging>(new FlushLogging(logger))
                                .AddSingleton<ILogger>(_ => logger)
                                .AddScoped(_provider);
        if (_configure)
        {
            services.AddSerilog(logger);
        }
    }
}