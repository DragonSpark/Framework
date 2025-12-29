using System.Collections.Immutable;

namespace DragonSpark.Contracts.Queries;

public sealed record PageResult<T>(ImmutableArray<T> Page, ulong? Total);