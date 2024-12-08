using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime.Shape;

public readonly record struct Partition<T>(IQueryable<T> Subject, Partition Input);

public readonly record struct Partition(int? Skip, int? Top);