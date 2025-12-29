using DragonSpark.Contracts.Queries;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime.Shape;

public readonly record struct ComposeInput<T>(PageInput Input, IQueryable<T> Current);