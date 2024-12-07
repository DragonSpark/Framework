using DragonSpark.Model.Selection;
using System.Linq;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation;

public interface IFilter<T> : ISelect<FilterInput<T>, IQueryable<T>>;