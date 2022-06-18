using DragonSpark.Application.Entities.Queries.Runtime.Pagination;
using System.Linq;

namespace DragonSpark.Application.Entities.Queries.Runtime.Shape;

public readonly record struct ComposeInput<T>(PageInput Input, IQueryable<T> Current);