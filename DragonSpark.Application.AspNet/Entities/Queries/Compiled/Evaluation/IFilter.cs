using DragonSpark.Model.Selection;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Queries.Compiled.Evaluation;

public interface IFilter<T> : ISelect<FilterInput<T>, IQueryable<T>>;