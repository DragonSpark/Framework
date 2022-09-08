using System.Threading;

namespace DragonSpark.Model.Operations.Allocated;

public readonly record struct TokenAware<T>(T Subject, CancellationToken Token);