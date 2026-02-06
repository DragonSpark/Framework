using System.Threading;

namespace DragonSpark.Application.AspNet.Security.Tokens;

public static class Extensions
{
    public static CancellationToken Linked(this CancellationToken @this, CancellationToken other)
        => @this == other || other == CancellationToken.None
               ? other
               : CancellationTokenSource.CreateLinkedTokenSource(@this, other).Token;
}