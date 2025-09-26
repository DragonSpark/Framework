using System;
using System.Buffers;
using System.Collections.Generic;
using DragonSpark.Application.Mobile.Runtime.Initialization;
using DragonSpark.Compose;
using DragonSpark.Diagnostics;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Hosting;
using NetFabric.Hyperlinq;
using Serilog;
using Serilog.Events;

namespace DragonSpark.Application.Mobile.Maui.Diagnostics;

sealed class UnhandledExceptions : IMauiInitializeService
{
    public static UnhandledExceptions Default { get; } = new();

    UnhandledExceptions() {}

    // ReSharper disable once AvoidAsyncVoid
    public void Initialize(IServiceProvider services)
    {
        services.GetRequiredService<IConfigureExceptions>().Execute(new HandleExceptions(services).Execute);
    }
}

sealed class AdjustDiagnostics : IMauiInitializeService
{
    public static AdjustDiagnostics Default { get; } = new();

    AdjustDiagnostics() {}

    public void Initialize(IServiceProvider services)
    {
        CurrentLogger.Default.Execute(new MonitoredLogger());
        RegisterInitialization.Default.Execute(InitializeLogger.Default.Then().Bind(services));
    }
}

sealed class InitializeLogger : ICommand<IServiceProvider>
{
    public static InitializeLogger Default { get; } = new();

    InitializeLogger() : this(CurrentLogger.Default) {}

    readonly IMutable<ILogger?> _store;

    public InitializeLogger(IMutable<ILogger?> store) => _store = store;

    public void Execute(IServiceProvider parameter)
    {
        var monitored = _store.Get().Verify().To<MonitoredLogger>();
        _store.Execute(null);
        var logger = parameter.GetRequiredService<ILogger>();
        monitored.Execute(logger);
    }
}

public interface IMonitoredLogger : ILogger, ICommand<ILogger>;

sealed class MonitoredLogger : IMonitoredLogger
{
    readonly List<LogEvent> _history;

    public MonitoredLogger() : this([]) {}

    public MonitoredLogger(List<LogEvent> history) => _history = history;

    public void Write(LogEvent logEvent)
    {
        _history.Add(logEvent);
    }

    public void Execute(ILogger parameter)
    {
        using var lease = _history.AsValueEnumerable().ToArray(ArrayPool<LogEvent>.Shared);
        if (lease.Length > 0)
        {
            _history.Clear();
            parameter.Information("Initializing log with {Count} entries", lease.Length);
            foreach (var @event in lease)
            {
                parameter.Write(@event);
            }   
        }
    }
}

// TODO

sealed class HandleExceptions
{
    readonly IServiceProvider _services;

    public HandleExceptions(IServiceProvider services) => _services = services;

    public void Execute(object sender, UnhandledExceptionEventArgs e)
    {
        _services.GetRequiredService<HandleApplicationException>().Execute(sender, e);
    }
}