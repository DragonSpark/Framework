using System.Linq;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation;

public readonly record struct FilterInput<T>(IQueryable<T> Source, string Filter);