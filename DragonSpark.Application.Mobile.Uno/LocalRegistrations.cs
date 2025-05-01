using System;
using DragonSpark.Application.Mobile.Diagnostics;
using DragonSpark.Application.Mobile.Uno.Presentation;
using DragonSpark.Application.Mobile.Uno.Run;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;
using Command = Uno.Extensions.Reactive.Command;

namespace DragonSpark.Application.Mobile.Uno;

sealed class LocalRegistrations : ICommand<IServiceCollection>
{
    public static LocalRegistrations Default { get; } = new();

    LocalRegistrations()
        : this(x => CurrentServices.Default.GetRequiredService<IApplicationErrorHandler>().Execute(x)) {}

    readonly Action<Exception> _error;

    public LocalRegistrations(Action<Exception> error) => _error = error;

    public void Execute(IServiceCollection parameter)
    {
        parameter.Start<IInitializeApplication>()
                 .Forward<DefaultInitializeApplication>()
                 .Singleton()
            ;
        Command.DefaultErrorHandler = _error;
    }
}
