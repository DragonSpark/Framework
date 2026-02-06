using System.Security.Claims;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.AspNet.Security.Tokens;

public sealed class MarkCurrentNonceUsed : IDepending
{
    readonly ICurrentContext _context;
    readonly IMarkUsed       _used;
    readonly string          _name;

    public MarkCurrentNonceUsed(ICurrentContext context, IMarkUsed used) : this(context, used, NonceClaim.Default) {}

    public MarkCurrentNonceUsed(ICurrentContext context, IMarkUsed used, string name)
    {
        _context = context;
        _used    = used;
        _name    = name;
    }

    public ValueTask<bool> Get(Stop<None> parameter)
    {
        var context = _context.Get();
        var nonce   = context.User.FindFirstValue(_name).Verify();
        var stop    = parameter.Token.Linked(context.RequestAborted);
        return _used.Get(new(nonce, stop));
    }
}