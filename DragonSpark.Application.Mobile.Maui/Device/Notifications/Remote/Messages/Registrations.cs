using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Mobile.Maui.Device.Notifications.Remote.Messages;

sealed class Registrations : ICommand<IServiceCollection>
{
    public static Registrations Default { get; } = new();

    Registrations() {}

    public void Execute(IServiceCollection parameter)
    {
        parameter.Start<IProcessNotification>()
                 .Forward<ProcessNotification>()
                 .Include(x => x.Dependencies)
                 .Singleton()
                 //
                 .Then.Start<IActionParser>()
                 .Forward<ActionParser>().Include(x => x.Dependencies.Recursive())
                 .Singleton()
            ;
    }
}