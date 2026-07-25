using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Additional;

sealed class ComposeUserData<T> : ISelecting<ComposeUserDataInput<T>, IReadOnlyDictionary<string, string>>
    where T : class
{
    readonly IProperties _properties;
    readonly string      _default;

    public ComposeUserData(IProperties properties) : this(properties, "null") {}

    public ComposeUserData(IProperties properties, string @default)
    {
        _properties = properties;
        _default    = @default;
    }

    public async ValueTask<IReadOnlyDictionary<string, string>> Get(ComposeUserDataInput<T> parameter)
    {
        var (users, u, _) = parameter;

        var user   = u.Verify();
        var result = new Dictionary<string, string>();
        foreach (var (name, value) in _properties.Get(user))
        {
            result.Add(name, value ?? _default);
        }

        var logins = await users.Subject.GetLoginsAsync(user).Off();
        foreach (var l in logins)
        {
            result.Add($"{l.LoginProvider} external login provider key", l.ProviderKey);
        }

        var key = await users.Subject.GetAuthenticatorKeyAsync(user).Off();

        result.Add("Authenticator Key", key!);

        return result;
    }
}