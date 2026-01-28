using System.Linq;
using System.Threading.Tasks;
using DragonSpark.Application.AspNet.Entities;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Server.Mobile.Security.Devices;

sealed class DeviceUsed : IDeviceUsed
{
    readonly INewContext _context;

    public DeviceUsed(INewContext context) => _context = context;

    public async ValueTask<bool> Get(Stop<DeviceUsedInput> parameter)
    {
        var ((deviceId, now), stop) = parameter;

        await using var db = _context.Get();
        var updated = await db.Set<DeviceKey>()
                              .Where(x => x.Identity == deviceId)
                              .ExecuteUpdateAsync(s => s.SetProperty(k => k.LastSeenAtUtc, _ => now), stop)
                              .Off();

        return updated == 1;
    }
}