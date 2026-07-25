using System.Security.Claims;
using DragonSpark.Application.Security.Identity;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results.Stop;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.AspNet.Security.Identity.Claims;

public class AddClaimToCurrent : IStopAware<IdentityResult>
{
    readonly ICurrentPrincipal            _current;
    readonly IAddClaim                    _add;
    readonly Func<ClaimsPrincipal, Claim> _claim;

    protected AddClaimToCurrent(ICurrentPrincipal current, IAddClaim add, string type)
        : this(current, add, new Claim(type, string.Empty).Accept) {}

    protected AddClaimToCurrent(ICurrentPrincipal current, IAddClaim add, Func<ClaimsPrincipal, Claim> claim)
    {
        _current = current;
        _add     = add;
        _claim   = claim;
    }

    public async ValueTask<IdentityResult> Get(CancellationToken parameter)
    {
        var current = _current.Get();
        var claim   = _claim(current);
        var result  = await _add.Off(new(new(current, claim), parameter));
        return result;
    }
}