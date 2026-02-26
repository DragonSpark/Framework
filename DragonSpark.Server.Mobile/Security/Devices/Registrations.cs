using System;
using DragonSpark.Application.AspNet.Security.Tokens;
using DragonSpark.Application.Security.Tokens;
using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using DragonSpark.Server.Mobile.Security.Devices.Authentication;
using DragonSpark.Server.Mobile.Security.Devices.Registry;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Server.Mobile.Security.Devices;

sealed class Registrations : ICommand<IServiceCollection>
{
    readonly string                   _name;
    readonly Action<DevicePoPOptions> _configure;

    public Registrations(Action<DevicePoPOptions> configure) : this(SchemeName.Default, configure) {}

    public Registrations(string name, Action<DevicePoPOptions> configure)
    {
        _name      = name;
        _configure = configure;
    }

    public void Execute(IServiceCollection parameter)
    {
        parameter.Register<DevicePoPOptions>()
                 //
                 .Start<IDeviceRegistry>()
                 .Forward<DeviceRegistry>()
                 .Decorate<MemoryAwareDeviceRegistry>()
                 .Decorate<ProofAwareDeviceRegistry>()
                 .Include(x => x.Dependencies.Recursive())
                 .Singleton()
                 //
                 .Then.Start<IUpsertDevice>()
                 .Forward<UpsertDevice>()
                 .Include(x => x.Dependencies.Recursive())
                 .Singleton()
                 //
                 .Then.Start<IBlockDevice>()
                 .Forward<BlockDevice>()
                 .Decorate<MemoryAwareBlockDevice>()
                 .Include(x => x.Dependencies)
                 .Singleton()
                 //
                 .Then.Start<IDeviceSeen>()
                 .Forward<DeviceSeen>()
                 .Singleton()
                 //
                 .Then.Start<IAuthenticateDevice>()
                 .Forward<AuthenticateDevice>()
                 .Include(x => x.Dependencies.Recursive())
                 .Singleton()
                 //
                 .Then.TryDecorate<IMarkUsed, DeviceAwareMarkUsed>()
                 .Return(parameter)
                 //
                 .AddAuthentication()
                 .AddScheme<DevicePoPOptions, DevicePoPHandler>(_name, _configure)
                 //
                 .Services.AddAuthorization(x => x.AddPolicy(_name,
                                                             y =>
                                                             {
                                                                 y.AddAuthenticationSchemes(_name);
                                                                 y.RequireAuthenticatedUser();
                                                             }));
    }
}