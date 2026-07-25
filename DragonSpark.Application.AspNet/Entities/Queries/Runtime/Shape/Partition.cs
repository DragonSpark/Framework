using DragonSpark.Contracts.Queries;

namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime.Shape;

public readonly record struct Partition<T>(IQueryable<T> Subject, Partition Input);