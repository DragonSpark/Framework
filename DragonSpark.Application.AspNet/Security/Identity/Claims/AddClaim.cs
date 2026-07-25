using DragonSpark.Application.AspNet.Security.Identity.Authentication;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.AspNet.Security.Identity.Claims;

sealed class AddClaim<T> : IAddClaim where T : IdentityUser
{
    readonly IAuthentications<T> _sessions;
    readonly IRefreshUser        _refresh;

    public AddClaim(IAuthentications<T> sessions, IRefreshUser refresh)
    {
        _sessions = sessions;
        _refresh  = refresh;
    }

    public async ValueTask<IdentityResult> Get(Stop<ClaimInput> parameter)
    {
        var ((owner, claim), _) = parameter;
        using var session = _sessions.Get();
        var       users   = session.Subject.UserManager;
        var       user    = await users.GetUserAsync(owner).Off();
        var       verify  = user.Verify();

        var remove = await users.RemoveClaimAsync(verify, claim).Off();
        if (remove.Succeeded)
        {
            var result = await users.AddClaimAsync(verify, claim).Off();
            if (result.Succeeded)
            {
                await _refresh.Off(owner);
            }
            return result;
        }

        return remove;
    }
}