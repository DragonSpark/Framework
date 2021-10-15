using DragonSpark.Model.Selection;
using System.Linq;

namespace DragonSpark.Application.Entities.Queries.Runtime.Selection;

public sealed class Accept<TIn, T> : FixedResult<TIn, IQueryable<T>>, IQuery<TIn, T>
{
	public Accept(IQueryable<T> instance) : base(instance) {}
}