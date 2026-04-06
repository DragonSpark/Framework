using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Azure.NotificationHubs;
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
                 .Start<NotificationHubClient>()
                 .Use<NotificationHubInstance>()
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