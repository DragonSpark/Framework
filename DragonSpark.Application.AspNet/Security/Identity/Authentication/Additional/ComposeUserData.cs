using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;
using DragonSpark.Model.Sequences;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Additional;

sealed class ComposeUserData<T> : ISelecting<ComposeUserDataInput<T>, IReadOnlyDictionary<string, string>>
    where T : class
{
    public static ComposeUserData<T> Default { get; } = new();

    ComposeUserData()
        : this(A.Type<T>().GetProperties().Where(x => Attribute.IsDefined(x, typeof(PersonalDataAttribute))).ToArray(),
               "null") {}

    readonly Array<PropertyInfo> _properties;
    readonly string              _unassigned;

    public ComposeUserData(Array<PropertyInfo> properties, string unassigned)
    {
        _properties = properties;
        _unassigned = unassigned;
    }

    public async ValueTask<IReadOnlyDictionary<string, string>> Get(ComposeUserDataInput<T> parameter)
    {
        var (users, user, _) = parameter;

        var result = new Dictionary<string, string>();
        foreach (var p in _properties)
        {
            result.Add(p.Name, p.GetValue(parameter)?.ToString() ?? _unassigned);
        }

        var verify = user.Verify();
        var logins = await users.Subject.GetLoginsAsync(verify).Off();
        foreach (var l in logins)
        {
            result.Add($"{l.LoginProvider} external login provider key", l.ProviderKey);
        }

        var key = await users.Subject.GetAuthenticatorKeyAsync(verify).Off();

        result.Add("Authenticator Key", key!);

        return result;
    }
}