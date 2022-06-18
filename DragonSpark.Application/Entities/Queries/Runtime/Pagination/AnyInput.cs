namespace DragonSpark.Application.Entities.Queries.Runtime.Pagination;

public readonly record struct AnyInput<T>(object Owner, IQueries<T> Source);