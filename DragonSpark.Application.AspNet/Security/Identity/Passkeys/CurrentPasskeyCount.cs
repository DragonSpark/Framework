using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results;

namespace DragonSpark.Application.AspNet.Security.Identity.Passkeys;

public sealed class CurrentPasskeyCount<T> : IResulting<byte> where T : class
{
    readonly ICurrentContext _context;
    readonly IUsers<T>       _users;

    public CurrentPasskeyCount(ICurrentContext context, IUsers<T> users)
    {
        _context = context;
        _users   = users;
    }

    public async ValueTask<byte> Get()
    {
        var       context = _context.Get();
        using var session = _users.Get();
        var       user    = await session.Subject.GetUserAsync(context.User).Off();
        var       keys    = await session.Subject.GetPasskeysAsync(user.Verify()).Off();
        return (byte)keys.Count;
    }
}