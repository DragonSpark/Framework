using System.Threading;

namespace DragonSpark.Application.AspNet.Security;

sealed class CurrentStop : ICurrentStop
{
    readonly ICurrentContext _context;

    public CurrentStop(ICurrentContext context) => _context = context;

    public CancellationToken Get() => _context.Get().RequestAborted;
}