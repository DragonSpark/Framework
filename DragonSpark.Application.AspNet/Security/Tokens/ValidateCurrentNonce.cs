using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Application.Security.Tokens;
using DragonSpark.Compose;

namespace DragonSpark.Application.AspNet.Security.Tokens;

public sealed class ValidateCurrentNonce : DragonSpark.Model.Operations.Results.Stop.IStopAware<string>
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

    public async ValueTask<string> Get(CancellationToken parameter)
    {
        var context = _context.Get();
        var result   = context.User.FindFirstValue(_name).Verify();
        var stop    = parameter.Linked(context.RequestAborted);
        await _valid.Off(new(result, stop));
        return result;
    }
}