using System.Linq;
using System.Threading.Tasks;
using DragonSpark.Application.AspNet.Entities;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Server.Mobile.Security.Devices;

sealed class BlockDevice : IBlockDevice
{
    readonly INewContext _context;

    public BlockDevice(INewContext context) => _context = context;

    public async ValueTask<bool> Get(Stop<BlockInput> parameter)
    {
        var ((deviceId, blocked), stop) = parameter;

        await using var db   = _context.Get();
        var             keys = db.Set<DeviceKey>();

        var updated = await keys.Where(x => x.Identity == deviceId)
                                .ExecuteUpdateAsync(s => s.SetProperty(k => k.IsBlocked, _ => blocked), stop)
                                .Off();

        return updated == 1;
    }
}