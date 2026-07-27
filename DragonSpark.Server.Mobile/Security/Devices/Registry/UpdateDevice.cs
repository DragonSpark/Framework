using DragonSpark.Application.AspNet.Entities;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop.Conditions;
using DragonSpark.Server.Mobile.Security.Devices.Claims;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Server.Mobile.Security.Devices.Registry;

sealed class UpdateDevice : IDepending<DeviceRecord>
{
    readonly INewContext               _context;
    readonly ICommand<UpdateKeysInput> _update;

    public UpdateDevice(INewContext context) : this(context, UpdateKeys.Default) {}

    public UpdateDevice(INewContext context, ICommand<UpdateKeysInput> update)
    {
        _context = context;
        _update  = update;
    }

    public async ValueTask<bool> Get(Stop<DeviceRecord> parameter)
    {
        var (r, stop) = parameter;
        await using var db   = _context.Get();
        var             keys = db.Set<DeviceKey>();
        var updated = await keys.Where(d => d.Identity == r.DeviceId)
                                .ExecuteUpdateAsync(x => _update.Execute(new(r, x)), stop)
                                .Off();
        return updated > 0;
    }
}