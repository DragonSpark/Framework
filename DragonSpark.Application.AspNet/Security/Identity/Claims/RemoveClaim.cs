using DragonSpark.Application.AspNet.Security.Identity.Authentication;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.AspNet.Security.Identity.Claims;

sealed class RemoveClaim<T> : IRemoveClaim where T : IdentityUser
{
    readonly IAuthentications<T> _sessions;
    readonly IRefreshUser        _refresh;

    public RemoveClaim(IAuthentications<T> sessions, IRefreshUser refresh)
    {
        _sessions     = sessions;
        _refresh = refresh;
    }

    public async ValueTask<IdentityResult> Get(Stop<ClaimInput> parameter)
    {
        var ((owner, subject), _) = parameter;
        using var session = _sessions.Get();
        var       users   = session.Subject.UserManager;
        var       user    = await users.GetUserAsync(owner).Off();
        var       verify  = user.Verify();
        var       result  = await users.RemoveClaimAsync(verify, subject).Off();
        await _refresh.Off(owner);
        return result;
    }
}