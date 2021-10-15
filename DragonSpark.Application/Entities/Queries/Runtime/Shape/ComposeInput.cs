using System.Linq;

namespace DragonSpark.Application.Entities.Queries.Runtime.Shape;

public readonly record struct ComposeInput<T>(QueryInput Input, IQueryable<T> Current);