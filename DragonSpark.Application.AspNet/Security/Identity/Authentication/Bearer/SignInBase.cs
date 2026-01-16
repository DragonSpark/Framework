using System.Buffers;
using System.Security.Claims;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using Microsoft.AspNetCore.Identity;
using NetFabric.Hyperlinq;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Bearer;

public class SignInBase<T> : ISignIn<T> where T : class
{
    readonly IDepending<ValidateSignInInput<T>> _validate;
    readonly IAuthentications<T>                _signin;
    readonly IComposeClaims<T>                  _claims;

    protected SignInBase(IDepending<ValidateSignInInput<T>> validate, IAuthentications<T> signin,
                         IComposeClaims<T> claims)
    {
        _validate = validate;
        _signin   = signin;
        _claims   = claims;
    }

    public async ValueTask<bool> Get(Stop<SignInInput<T>> parameter)
    {
        var ((user, _), stop) = parameter;
        using var signin  = _signin.Get();
        using var claims  = _claims.Get(user).AsValueEnumerable().ToArray(ArrayPool<Claim>.Shared);
        var       subject = signin.Subject;
        subject.AuthenticationScheme = IdentityConstants.BearerScheme;

        var result = await _validate.Off(new(new(signin, parameter), stop));
        if (result)
        {
            await subject.SignInWithClaimsAsync(user, null, claims).Off();
        }

        return result;
    }
}