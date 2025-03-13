using DragonSpark.Application.Mobile.Android.Notifications;
using DragonSpark.Application.Mobile.Device.Notifications;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Mobile.Android;

sealed class Registrations : ICommand<IServiceCollection>
{
    public static Registrations Default { get; } = new();

    Registrations() {}

    public void Execute(IServiceCollection parameter)
    {
        parameter.Start<INotificationManagerService>()
                 .Forward<NotificationManagerService>()
                 .Decorate<RegisteredNotificationManagerService>()
                 .Singleton();
    }
}