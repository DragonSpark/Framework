using System;
using System.Linq;
using System.Threading.Tasks;
using DragonSpark.Application.AspNet.Entities;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Runtime;
using DragonSpark.Server.Mobile.Security.Devices.Claims;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Server.Mobile.Security.Devices.Registry;

sealed class DeviceUsed : IDeviceUsed
{
    readonly INewContext _context;
    readonly ITime       _time;

    public DeviceUsed(INewContext context) : this(context, Time.Default) {}

    public DeviceUsed(INewContext context, ITime time)
    {
        _context = context;
        _time    = time;
    }

    public async ValueTask<bool> Get(Stop<string> parameter)
    {
        var (deviceId, stop) = parameter;

        var             time = _time.Get();
        await using var db   = _context.Get();
        var updated = await db.Set<DeviceKey>()
                              .Where(x => x.Identity == deviceId)
                              .ExecuteUpdateAsync(s => s.SetProperty(k => k.LastSeenAtUtc, _ => (DateTimeOffset?)time),
                                                  stop)
                              .Off();

        return updated == 1;
    }
}