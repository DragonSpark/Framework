using DragonSpark.Application.AspNet.Entities;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace DragonSpark.Application.AspNet.Security.Tokens;

public sealed class ValidateNonce : IStopAware<string>
{
    readonly INewContext   _context;
    readonly IComposeQuery _query;

    public ValidateNonce(INewContext context) : this(context, ComposeQuery.Default) {}

    public ValidateNonce(INewContext context, IComposeQuery query)
    {
        _context = context;
        _query   = query;
    }

    public async ValueTask Get(Stop<string> parameter)
    {
        var (identity, stop) = parameter;
        if (!identity.IsNullOrWhiteSpace())
        {
            await using var context = _context.Get();
            var (query, _) = _query.Get(new(context.Set<Nonce>(), identity));
            var result = await query.AnyAsync(stop).Off();
            if (result)
            {
                return;
            }
        }

        throw new SecurityTokenException("The nonce is not valid or expired");
    }
}