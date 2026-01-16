using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.AspNet.Security.Identity.Passkeys;

public class HasPasskeys<T> : IDepending<string> where T : class
{
    readonly IUsers<T> _users;

    protected HasPasskeys(IUsers<T> users) => _users = users;

    public async ValueTask<bool> Get(Stop<string> parameter)
    {
        using var session = _users.Get();
        var       user    = await session.Subject.FindByEmailAsync(parameter).Off();
        var       result  = user is not null && await session.Subject.GetPasskeysAsync(user).Off() is { Count: > 0 };
        return result;
    }
}