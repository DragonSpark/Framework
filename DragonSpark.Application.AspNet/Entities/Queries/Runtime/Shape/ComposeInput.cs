using DragonSpark.Contracts.Queries;

namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime.Shape;

public readonly record struct ComposeInput<T>(PageInput Input, IQueryable<T> Current);