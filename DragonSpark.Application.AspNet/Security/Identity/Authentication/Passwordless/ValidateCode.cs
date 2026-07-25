using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop.Conditions;
using DragonSpark.Runtime;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Passwordless;

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

    public async ValueTask<bool> Get(Stop<ValidateSignInInput<T>> parameter)
    {
        var (((_, users), (user, input, _, _)), _) = parameter;
        var result = await users.VerifyUserTokenAsync(user, _provider, _purpose, input).Off();
        if (result && user is IdentityUser { EmailConfirmed: false } u)
        {
            u.EmailConfirmed = true;
            u.Modified       = Time.Default;
            await users.UpdateAsync(user).Off();
        }

        return result;
    }
}