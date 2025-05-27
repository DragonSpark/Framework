using System.Buffers;
using System.Security.Claims;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Identity;
using NetFabric.Hyperlinq;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Bearer;

public class SignIn<T> : ISignIn<T> where T : class
{
    readonly IAuthentications<T> _signin;
    readonly IComposeClaims<T>   _claims;

    public SignIn(IAuthentications<T> signin, IComposeClaims<T> claims)
    {
        _signin = signin;
        _claims = claims;
    }

    public async ValueTask<bool> Get(Stop<SignInInput<T>> parameter)
    {
        var ((user, _), _) = parameter;
        using var signin  = _signin.Get();
        using var claims  = _claims.Get(user).AsValueEnumerable().ToArray(ArrayPool<Claim>.Shared);
        var       subject = signin.Subject;
        subject.AuthenticationScheme = IdentityConstants.BearerScheme;
        await subject.SignInWithClaimsAsync(user, null, claims).Off();
        return true;
    }
}