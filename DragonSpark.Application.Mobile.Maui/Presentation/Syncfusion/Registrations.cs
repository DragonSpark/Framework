using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Hosting;

namespace DragonSpark.Application.Mobile.Maui.Presentation.Syncfusion;

public sealed class Registrations : ICommand<IServiceCollection>
{
    public static Registrations Default { get; } = new();

    Registrations() {}

    public void Execute(IServiceCollection parameter)
    {
        parameter.Start<RegisterLicense>()
                 .Singleton()
                 .Then.Start<IMauiInitializeService>()
                 .Forward<ConfigureApplication>()
                 .Singleton();
    }
}