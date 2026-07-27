using DragonSpark.Application.AspNet.Entities;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Runtime;
using DragonSpark.Server.Mobile.Security.Devices.Claims;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Server.Mobile.Security.Devices.Registry;

sealed class DeviceSeen : IDeviceSeen
{
    readonly INewContext _context;
    readonly ITime       _time;

    public DeviceSeen(INewContext context) : this(context, Time.Default) {}

    public DeviceSeen(INewContext context, ITime time)
    {
        _context = context;
        _time    = time;
    }

    public async ValueTask<bool> Get(Stop<string> parameter)
    {
        var (deviceId, stop) = parameter;

        var             time = _time.Get().UtcDateTime;
        await using var db   = _context.Get();
        var updated = await db.Set<DeviceKey>()
                              .Where(x => x.Identity == deviceId)
                              .ExecuteUpdateAsync(s => s.SetProperty(k => k.LastSeenAtUtc, _ => time),
                                                  stop)
                              .Off();
        return updated == 1;
    }
}