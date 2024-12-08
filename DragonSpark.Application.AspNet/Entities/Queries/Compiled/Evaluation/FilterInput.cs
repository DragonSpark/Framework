using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Queries.Compiled.Evaluation;

public readonly record struct FilterInput<T>(IQueryable<T> Source, string Filter);