using System.Threading.Tasks;
using DragonSpark.Application.AspNet.Entities;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Security.Tokens;

sealed class MarkUsed : IMarkUsed
{
    readonly INewContext           _context;
    readonly TypeAwareComposeQuery _query;

    public MarkUsed(INewContext context, TypeAwareComposeQuery query)
    {
        _context = context;
        _query   = query;
    }

    public async ValueTask<bool> Get(Stop<MarkUsedInput> parameter)
    {
        var ((identity, type), stop) = parameter;
        if (!identity.IsNullOrWhiteSpace())
        {
            await using var context = _context.Get();
            var (query, now) = _query.Get(new(context.Set<Nonce>(), identity, type));
            var rows = await query.ExecuteUpdateAsync(s => s.SetProperty(n => n.UsedAtUtc, _ => now), stop).Off();
            return rows == 1;
        }

        return false;
    }
}