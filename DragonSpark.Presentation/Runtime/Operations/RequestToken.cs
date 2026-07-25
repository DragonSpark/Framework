using DragonSpark.Application.AspNet.Security;

namespace DragonSpark.Presentation.Runtime.Operations;

sealed class RequestToken : IRequestToken
{
    readonly ICurrentContext _context;

    public RequestToken(ICurrentContext context) => _context = context;

    public CancellationToken Get() => _context.Get().RequestAborted;
}