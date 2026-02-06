using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.AspNet.Security.Tokens;

public sealed class ValidateCurrentNonce : IStopAware
{
    readonly ICurrentContext _context;
    readonly ValidateNonce   _valid;
    readonly string          _name;

    public ValidateCurrentNonce(ICurrentContext context, ValidateNonce valid)
        : this(context, valid, NonceClaim.Default) {}

    public ValidateCurrentNonce(ICurrentContext context, ValidateNonce valid, string name)
    {
        _context = context;
        _valid   = valid;
        _name    = name;
    }

    public ValueTask Get(CancellationToken parameter)
    {
        var context = _context.Get();
        var nonce   = context.User.FindFirstValue(_name).Verify();
        var stop    = parameter.Linked(context.RequestAborted);
        return _valid.Get(new(nonce, stop));
    }
}