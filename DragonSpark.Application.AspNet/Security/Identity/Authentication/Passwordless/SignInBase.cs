using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Passwordless;

public class SignInBase<T> : ISignIn<T> where T : class
{
    readonly IDepending<ValidateSignInInput<T>> _validate;
    readonly IAuthentications<T>                _signin;

    protected SignInBase(IDepending<ValidateSignInInput<T>> validate, IAuthentications<T> signin)
    {
        _validate = validate;
        _signin   = signin;
    }

    public async ValueTask<bool> Get(Stop<SignInInput<T>> parameter)
    {
        var ((user, _, persistent, scheme), stop) = parameter;
        using var signin  = _signin.Get();
        var       subject = signin.Subject;
        subject.AuthenticationScheme = scheme;

        var id     = await signin.Users.GetUserIdAsync(user).Off();
        var local  = (await signin.Users.FindByIdAsync(id).Off()).Verify();
        var result = await _validate.Off(new(new(signin, parameter.Subject with { User = local }), stop));
        if (result)
        {
            await subject.SignInAsync(local, persistent).Off();
        }

        return result;
    }
}