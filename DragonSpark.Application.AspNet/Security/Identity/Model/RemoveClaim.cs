using System;
using System.Security.Claims;
using System.Threading.Tasks;
using DragonSpark.Application.AspNet.Security.Identity.Authentication;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.AspNet.Security.Identity.Model;

public class AddClaim<T> : IStopAware<T, IdentityResult> where T : IdentityUser
{
    readonly IAuthentications<T> _sessions;
    readonly Func<T, Claim>      _claim;

    public AddClaim(IAuthentications<T> sessions, string type) : this(sessions, new Claim(type, string.Empty).Accept) {}

    protected AddClaim(IAuthentications<T> sessions, Func<T, Claim> claim)
    {
        _sessions = sessions;
        _claim    = claim;
    }

    public async ValueTask<IdentityResult> Get(Stop<T> parameter)
    {
        var (subject, _) = parameter;
        using var session = _sessions.Get();
        var       claim   = _claim(parameter);
        var       users   = session.Subject.UserManager;
        var       user    = await users.FindByIdAsync(subject.Id.ToString()).Off();
        var       verify  = user.Verify();
        var       result  = await users.AddClaimAsync(verify, claim).Off();
        await session.Subject.RefreshSignInAsync(verify).Off();
        return result;
    }
}

public class RemoveClaim<T> : IStopAware<T, IdentityResult> where T : IdentityUser
{
    readonly IAuthentications<T> _sessions;
    readonly Func<T, Claim>      _claim;

    public RemoveClaim(IAuthentications<T> sessions, string type)
        : this(sessions, new Claim(type, string.Empty).Accept) {}

    protected RemoveClaim(IAuthentications<T> sessions, Func<T, Claim> claim)
    {
        _sessions = sessions;
        _claim    = claim;
    }

    public async ValueTask<IdentityResult> Get(Stop<T> parameter)
    {
        var (subject, _) = parameter;
        using var session = _sessions.Get();
        var       claim   = _claim(parameter);
        var       users   = session.Subject.UserManager;
        var       user    = await users.FindByIdAsync(subject.Id.ToString()).Off();
        var       verify  = user.Verify();
        var       result  = await users.RemoveClaimAsync(verify, claim).Off();
        await session.Users.UpdateSecurityStampAsync(verify).Off();
        return result;
    }
}