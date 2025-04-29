using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS;

sealed class Registrations : ICommand<IServiceCollection>
{
    public static Registrations Default { get; } = new();

    Registrations() {}

    public void Execute(IServiceCollection parameter)
    {
        /*parameter.Start<INotifications>()
                 .Forward<Notifications.Notifications>()
                 .Decorate<PermissionAwareNotifications>()
                 .Singleton()
                 //
                 .Then.Start<IMessenger>()
                 .Forward<ActivityMessenger>()
                 .Decorate<PermissionAwareMessenger>()
                 .Include(x => x.Dependencies)
                 .Singleton();*/
    }
}