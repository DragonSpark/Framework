using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.AspNet.Security.Identity.Passkeys;

public class PasskeyCount<T> : IStopAware<string, uint?> where T : class
{
    readonly IUsers<T> _users;

    protected PasskeyCount(IUsers<T> users) => _users = users;

    public async ValueTask<uint?> Get(Stop<string> parameter)
    {
        using var session = _users.Get();
        var       user    = await session.Subject.FindByEmailAsync(parameter).Off();
        if (user is not null)
        {
            var list = await session.Subject.GetPasskeysAsync(user).Off();
            return list.Count.Grade();
        }
        return null;
    }
}