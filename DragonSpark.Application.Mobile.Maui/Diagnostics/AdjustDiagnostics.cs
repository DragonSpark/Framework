using System;
using DragonSpark.Application.Mobile.Runtime.Initialization;
using DragonSpark.Compose;
using DragonSpark.Diagnostics;
using DragonSpark.Model.Commands;
using Microsoft.Maui.Hosting;
using Serilog;

namespace DragonSpark.Application.Mobile.Maui.Diagnostics;

sealed class AdjustDiagnostics : IMauiInitializeService
{
    public static AdjustDiagnostics Default { get; } = new();

    AdjustDiagnostics() : this(CurrentLogger.Default, RegisterInitialization.Default, InitializeLogger.Default) {}

    readonly ICommand<ILogger>          _current;
    readonly ICommand<Action>           _register;
    readonly ICommand<IServiceProvider> _initialize;

    public AdjustDiagnostics(ICommand<ILogger> current, ICommand<Action> register,
                             ICommand<IServiceProvider> initialize)
    {
        _current    = current;
        _register   = register;
        _initialize = initialize;
    }

    public void Initialize(IServiceProvider services)
    {
        _current.Execute(new MonitoredLogger());
        _register.Execute(_initialize.Then().Bind(services));
    }
}