using DragonSpark.Application.Mobile.Uno.Device.Messaging;
using DragonSpark.Application.Mobile.Uno.Device.Notifications;
using DragonSpark.Application.Mobile.Uno.Platforms.Android.Messages;
using DragonSpark.Application.Mobile.Uno.Platforms.Android.Notifications;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Mobile.Uno.Platforms.Android;

sealed class Registrations : ICommand<IServiceCollection>
{
    public static Registrations Default { get; } = new();

    Registrations() {}

    public void Execute(IServiceCollection parameter)
    {
        parameter.Start<INotifications>()
                 .Forward<Notifications.Notifications>()
                 .Decorate<PermissionAwareNotifications>()
                 .Singleton()
                 //
                 .Then.Start<IMessenger>()
                 .Forward<ActivityMessenger>()
                 .Decorate<PermissionAwareMessenger>()
                 .Include(x => x.Dependencies)
                 .Singleton();
    }
}