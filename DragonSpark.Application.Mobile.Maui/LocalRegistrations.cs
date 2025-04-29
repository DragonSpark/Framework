using DragonSpark.Application.Mobile.Maui.Run;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Mobile.Maui;

sealed class LocalRegistrations : ICommand<IServiceCollection>
{
    /*public static LocalRegistrations Default { get; } = new();

    LocalRegistrations()
        : this(x => CurrentServices.Default.GetRequiredService<IApplicationErrorHandler>().Execute(x)) {}

    readonly Action<Exception> _error;

    public LocalRegistrations(Action<Exception> error) => _error = error;*/
    public static LocalRegistrations Default { get; } = new();

    LocalRegistrations() {}

    public void Execute(IServiceCollection parameter)
    {
        parameter.Start<IInitializeApplication>()
                 .Forward<DefaultInitializeApplication>()
                 .Singleton()
            ;
        // Command.DefaultErrorHandler = _error;
    }
}