using System.Linq;

namespace DragonSpark.Application.Entities.Queries.Materialization.Specialized
{
	public class Single<TKey, TEntity> : Materialization.Single<TKey, TEntity>
	{
		protected Single(IQueryable<TEntity> queryable, Query<TKey, TEntity> query)
			: base(new Queries.Where<TKey, TEntity>(queryable, query)) {}
	}
}