using DragonSpark.Application.Mobile.Diagnostics;
using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Hosting;

namespace DragonSpark.Application.Mobile.Maui.Diagnostics;

sealed class Registrations : ICommand<IServiceCollection>
{
    public static Registrations Default { get; } = new();

    Registrations() {}

    public void Execute(IServiceCollection parameter)
    {
        parameter.TryDecorate<ILastChanceExceptionHandler, LastChanceExceptionHandler>()
                 .Return(parameter)
                 //
                 .Start<InitializeApplication>().Singleton()
                 //
                 .Then.Start<IConfigureExceptions>()
                 .Forward<DefaultConfigureExceptions>()
                 .Singleton()
                 //
                 .Then.Start<HandleApplicationException>()
                 .Singleton()
                 //
                 .Then.AddSingleton<IMauiInitializeService>(UnhandledExceptions.Default)
                 .AddSingleton<IMauiInitializeService>(AdjustDiagnostics.Default);
    }
}