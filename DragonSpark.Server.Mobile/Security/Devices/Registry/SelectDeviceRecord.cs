using DragonSpark.Application.AspNet.Entities.Queries.Composition;
using DragonSpark.Server.Mobile.Security.Devices.Claims;

namespace DragonSpark.Server.Mobile.Security.Devices.Registry;

sealed class SelectDeviceRecord : StartWhereSelect<string, DeviceKey, DeviceRecord>
{
    public static SelectDeviceRecord Default { get; } = new();

    SelectDeviceRecord()
        : base((p, x) => x.Identity == p,
               x => new(x.Identity, x.Kty, x.Crv, x.X, x.Y, x.IsBlocked, x.AttestedAtUtc, x.LastSeenAtUtc,
                        x.EvaluationType)) {}
}