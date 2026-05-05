using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Bearer;

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
        var ((user, _, scheme), stop) = parameter;
        using var signin  = _signin.Get();
        var       subject = signin.Subject;
        subject.AuthenticationScheme = scheme;

        var result = await _validate.Off(new(new(signin, parameter), stop));
        if (result)
        {
            await subject.SignInAsync(user, false).Off();
        }

        return result;
    }
}