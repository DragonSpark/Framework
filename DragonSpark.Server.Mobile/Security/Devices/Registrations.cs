using System;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Server.Mobile.Security.Devices;

sealed class Registrations : ICommand<IServiceCollection>
{
    readonly Action<DevicePoPOptions> _configure;

    public Registrations(Action<DevicePoPOptions> configure) => _configure = configure;

    public void Execute(IServiceCollection parameter)
    {
        parameter.Register<DevicePoPOptions>()
                 //
                 .Start<IDeviceRegistry>()
                 .Forward<DeviceRegistry>()
                 .Singleton()
                 //
                 .Then.Start<IUpsertDevice>()
                 .Forward<UpsertDevice>()
                 .Include(x => x.Dependencies.Recursive())
                 .Singleton()
                 //
                 .Then.Start<IBlockDevice>()
                 .Forward<BlockDevice>()
                 .Singleton()
                 //
                 .Then.Start<IDeviceUsed>()
                 .Forward<DeviceUsed>()
                 .Singleton()
                 //
                 .Then.Start<IAuthenticateDevice>()
                 .Forward<AuthenticateDevice>()
                 .Include(x => x.Dependencies.Recursive())
                 .Singleton()
                 //
                 .Then.AddAuthentication()
                 .AddScheme<DevicePoPOptions, DevicePoPHandler>("DevicePoP", _configure);
    }
}