using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Bearer;

sealed class ValidateCode<T> : IDepending<ValidateSignInInput<T>> where T : class
{
    public static ValidateCode<T> Default { get; } = new();

    ValidateCode() : this(TokenOptions.DefaultEmailProvider, Purpose.Default) {}

    readonly string _provider, _purpose;

    public ValidateCode(string provider, string purpose)
    {
        _provider = provider;
        _purpose  = purpose;
    }

    public ValueTask<bool> Get(Stop<ValidateSignInInput<T>> parameter)
    {
        var (((_, users), (user, input, _)), _) = parameter;
        return users.VerifyUserTokenAsync(user, _provider, _purpose, input).ToOperation();
    }
}