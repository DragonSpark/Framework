using System;
using DragonSpark.Compose;
using DragonSpark.Diagnostics;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace DragonSpark.Application.Mobile.Maui.Diagnostics;

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