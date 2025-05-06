using DragonSpark.Application.Mobile.Maui.Run;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Mobile.Maui;

sealed class LocalRegistrations : ICommand<IServiceCollection>
{
    public static LocalRegistrations Default { get; } = new();

    LocalRegistrations() {}

    public void Execute(IServiceCollection parameter)
    {
        parameter.Start<IInitializeApplication>()
                 .Forward<DefaultInitializeApplication>()
                 .Singleton();
    }
}