using System;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
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
        var logger = new Logger(new StoredLogger(parameter.Configuration()));
        var services = parameter.AddSingleton(new ActivityListenerConfiguration())
                                .AddSingleton<IFlushLogging, FlushLogging>()
                                .AddScoped(_provider).AddSingleton<ILogger>(logger);
        if (_configure)
        {
            services.AddSerilog(logger);
        }
    }
}

public sealed class CurrentLogger : Variable<ILogger>
{
    public static CurrentLogger Default { get; } = new();

    CurrentLogger() {}
}

// TODO:

sealed class Logger : ILogger
{
    readonly IResult<ILogger?> _store;
    readonly ILogger           _default;

    public Logger(IResult<ILogger?> store) : this(store, Log.Logger) {}

    public Logger(IResult<ILogger?> store, ILogger @default)
    {
        _store   = store;
        _default = @default;
    }

    public void Write(LogEvent logEvent)
    {
        var logger = _store.Get() ?? _default;
        logger.Write(logEvent);
    }
}

sealed class StoredLogger : Stored<ILogger>
{
    public StoredLogger(IConfiguration configuration) : this(CurrentLogger.Default, configuration) {}

    public StoredLogger(IMutable<ILogger?> store, IConfiguration configuration)
        : base(store, new CreateLogger(configuration)) {}
}

sealed class CreateLogger : IResult<ILogger>
{
    readonly IConfiguration _configuration;

    public CreateLogger(IConfiguration configuration) => _configuration = configuration;

    public ILogger Get() => new LoggerConfiguration().ReadFrom.Configuration(_configuration).CreateLogger();
}