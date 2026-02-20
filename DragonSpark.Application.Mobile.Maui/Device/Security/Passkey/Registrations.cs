using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Mobile.Maui.Device.Security.Passkey;

sealed class Registrations : ICommand<IServiceCollection>
{
    public static Registrations Default { get; } = new();

    Registrations() {}

    public void Execute(IServiceCollection parameter)
    {
        parameter.Register<PasskeyWorkflowSettings>()
                 //
                 .Start<ISupportsPasskey>()
                 .Forward<SupportsPasskey>()
                 //.Decorate<SimulatorAwareSupportsPasskey>()
                 .Singleton()
                 //
                 .Then.Start<ILaunchHostedRegistration>()
                 .Forward<LaunchHostedRegistration>()
                 .Include(x => x.Dependencies)
                 .Singleton()
                 //
                 .Then.Start<ILaunchHostedLogin>()
                 .Forward<LaunchHostedLogin>()
                 .Include(x => x.Dependencies)
                 .Singleton();
    }
}