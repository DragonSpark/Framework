using System.Security.Claims;
using DragonSpark.Application.Security.Identity;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results.Stop;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.AspNet.Security.Identity.Claims;

public class RemoveClaimFromCurrent : IStopAware<IdentityResult>
{
    readonly ICurrentPrincipal            _current;
    readonly IRemoveClaim                 _remove;
    readonly Func<ClaimsPrincipal, Claim> _claim;

    protected RemoveClaimFromCurrent(ICurrentPrincipal current, IRemoveClaim remove, string type)
        : this(current, remove, new Claim(type, string.Empty).Accept) {}

    protected RemoveClaimFromCurrent(ICurrentPrincipal current, IRemoveClaim remove, Func<ClaimsPrincipal, Claim> claim)
    {
        _current = current;
        _remove  = remove;
        _claim   = claim;
    }

    public async ValueTask<IdentityResult> Get(CancellationToken parameter)
    {
        var current = _current.Get();
        var claim   = _claim(current);
        var result  = await _remove.Off(new(new(current, claim), parameter));
        return result;
    }
}