using DragonSpark.Model.Operations;
using System.Linq;

namespace DragonSpark.Application.Entities.Queries.Materialization.Specialized
{
	public class Any<TIn, TEntity> : DragonSpark.Application.Entities.Queries.Materialization.Any<TIn, TEntity>,
	                                 IDepending<TIn>
	{
		protected Any(IQueryable<TEntity> queryable, Query<TIn, TEntity> query)
			: base(new DragonSpark.Application.Entities.Queries.Where<TIn, TEntity>(queryable, query)) {}
	}
}