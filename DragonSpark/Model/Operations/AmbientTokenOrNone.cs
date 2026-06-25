using System.Threading;
using DragonSpark.Model.Results;

namespace DragonSpark.Model.Operations;

public sealed class AmbientTokenOrNone : IResult<CancellationToken?>
{
    public static AmbientTokenOrNone Default { get; } = new();

    AmbientTokenOrNone() : this(AmbientToken.Default) {}

    readonly IResult<CancellationToken> _previous;

    public AmbientTokenOrNone(IResult<CancellationToken> previous) => _previous = previous;

    public CancellationToken? Get()
    {
        var previous = _previous.Get();
        return previous == CancellationToken.None ? null : previous;
    }
}