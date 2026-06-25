using System.Threading;
using DragonSpark.Runtime.Execution;

namespace DragonSpark.Model.Operations;

public sealed class AmbientToken : Logical<CancellationToken>
{
    public static AmbientToken Default { get; } = new();

    AmbientToken() {}
}