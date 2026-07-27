using DragonSpark.Application.Mobile.Maui.Device.Notifications.Remote;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Notifications.Remote;

sealed class LocalRegistrations : ICommand<IServiceCollection>
{
    public static LocalRegistrations Default { get; } = new();

    LocalRegistrations() {}

    public void Execute(IServiceCollection parameter)
    {
        parameter.Start<IProcessNotifications>()
                 .Forward<ProcessNotifications>()
                 .Decorate<DiagnosticAwareProcessNotifications>()
                 .Singleton()
                 //
                 .Then.Start<IDeviceIdentifier>()
                 .Forward<DeviceIdentifier>()
                 .Singleton()
                 //
                 .Then.Start<IInitialize>()
                 .Forward<Initialize>()
                 .Singleton();
    }
}