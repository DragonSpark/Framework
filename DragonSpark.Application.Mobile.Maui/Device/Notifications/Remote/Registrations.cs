using DragonSpark.Application.Communication.Http.Security;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Hosting;

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
                 .Then.AddSingleton<IMauiInitializeService>(InitializeDeviceRegistration.Default)
                 .TryDecorate<ICompleteLogin, LoginAwareDeviceRegistration>();
    }
}