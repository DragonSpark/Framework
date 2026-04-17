using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Server.Mobile.Notifications;

sealed class Registration : ICommand<IServiceCollection>
{
    public static Registration Default { get; } = new();

    Registration() {}

    public void Execute(IServiceCollection parameter)
    {
        parameter.Register<NotificationHubSettings>()
                 .Register<CleanUpSettings>()
                 //
                 .Start<NotificationHubClients>()
                 .Use<ComposeNotificationHubClients>()
                 .Singleton()
                 //
                 .Then.Start<IDeviceRegistration>()
                 .Forward<DeviceRegistration>()
                 .Singleton()
                 //
                 .Then.Start<DeleteInstallation>()
                 .Singleton()
                 //
                 .Then.Start<ExpiredInstallationsCleanupService>()
                 .Include(x => x.Dependencies)
                 .Singleton()
                 //
                 .Then.AddHostedService<ExpiredInstallationsCleanupService>();
    }
}