using DragonSpark.Model.Operations;
using System.Linq;

namespace DragonSpark.Application.Entities.Queries.Materialization.Specialized
{
	public class Any<TIn, TEntity> : DragonSpark.Application.Entities.Queries.Materialization.Any<TIn, TEntity>,
	                                 IDepending<TIn>
	{
		protected Any(IQueryable<TEntity> queryable, Express<TIn, TEntity> express)
			: base(new Scoped.Where<TIn, TEntity>(queryable, express)) {}
	}
}