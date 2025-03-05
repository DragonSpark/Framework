using System.Threading;
using DragonSpark.Compose;

namespace DragonSpark.Application.Runtime.Process;

/// <summary>
/// ATTRIBUTION: https://stackoverflow.com/a/60546519
/// </summary>
sealed class ExistingMarker<T> : ExistingMarker
{
    public static ExistingMarker<T> Default { get; } = new();

    ExistingMarker() : base(A.Type<T>().AssemblyQualifiedName.Verify()) {}
}

/// <summary>
/// ATTRIBUTION: https://stackoverflow.com/a/60546519
/// </summary>
class ExistingMarker : DragonSpark.Model.Results.Instance<Mutex>
{
    protected ExistingMarker(string name) : base(new(true, name)) {}
}