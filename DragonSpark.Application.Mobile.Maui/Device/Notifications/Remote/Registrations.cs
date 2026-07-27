using DragonSpark.Composition;
using DragonSpark.Model.Commands;

namespace DragonSpark.Application.Mobile.Maui.Device.Notifications.Remote;

sealed class Registrations : ICommand<IServiceCollection>
{
    public static Registrations Default { get; } = new();

    Registrations() {}

    public void Execute(IServiceCollection parameter)
    {
        parameter.Start<IRegisterDevice>()
                 .Forward<RegisterDevice>()
                 .Singleton()
                 .Then.Start<DeviceRegistration>()
                 .Singleton()
                 .Then.AddSingleton<IMauiInitializeService>(InitializeDeviceRegistration.Default);
    }
}