using DragonSpark.Application.Mobile.Attestation;
using DragonSpark.Application.Security.Tokens;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Security.Tokens;

sealed class LocalRegistrations : ICommand<IServiceCollection>
{
    public static LocalRegistrations Default { get; } = new();

    LocalRegistrations() {}

    public void Execute(IServiceCollection parameter)
    {
        parameter.Start<IDeviceKeyProvider>()
                 .Forward<DeviceKeyProvider>()
                 .Singleton()
                 //
                 .Then.Start<IDeviceSigner>()
                 .Forward<DeviceSigner>()
                 .Singleton()
                 //
                 .Then.Start<IClearDeviceKey>()
                 .Forward<ClearDeviceKey>()
                 .Singleton()
                 //
                 .Then.TryDecorate<IClearClientKey, DeviceAwareClearClientKey>();
    }
}