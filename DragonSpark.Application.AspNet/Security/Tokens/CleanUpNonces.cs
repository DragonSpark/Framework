using DragonSpark.Application.AspNet.Entities;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results.Stop;
using DragonSpark.Runtime;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Security.Tokens;

sealed class CleanUpNonces : IStopAware<uint>
{
    readonly INewContext _context;
    readonly ITime       _time;

    public CleanUpNonces(INewContext context) : this(context, Time.Default) {}

    public CleanUpNonces(INewContext context, ITime time)
    {
        _context = context;
        _time    = time;
    }

    public async ValueTask<uint> Get(CancellationToken parameter)
    {
        await using var db  = _context.Get();
        var             now = _time.Get().UtcDateTime;
        return (uint)await db.Set<Nonce>().Where(x => x.ExpiresAtUtc < now).ExecuteDeleteAsync(parameter).Off();
    }
}