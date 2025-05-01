using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Mobile.Diagnostics;

sealed class Registrations : ICommand<IServiceCollection>
{
    public static Registrations Default { get; } = new();

    Registrations() {}

    public void Execute(IServiceCollection parameter)
    {
        parameter.Start<ILastChanceExceptionHandler>()
                 .Forward<LastChanceExceptionHandler>()
                 .Singleton()
                 //
                 .Then.Start<IApplicationErrorHandler>()
                 .Forward<ApplicationErrorHandler>()
                 .Singleton();
    }
}