using DragonSpark.Model.Selection;

namespace DragonSpark.Application.AspNet.Entities.Queries.Compiled.Evaluation;

public interface IFilter<T> : ISelect<FilterInput<T>, IQueryable<T>>;