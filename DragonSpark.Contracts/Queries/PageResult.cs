namespace DragonSpark.Contracts.Queries;

public sealed record PageResult<T>(IReadOnlyCollection<T> Items, ulong? Count);