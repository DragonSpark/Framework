using System.Linq;

namespace DragonSpark.Application.Entities.Queries.Materialization.Specialized
{
	public class SingleOrDefault<TKey, TEntity> : Materialization.SingleOrDefault<TKey, TEntity?>
	{
		public SingleOrDefault(IQueryable<TEntity> queryable, Query<TKey, TEntity> query)
			: base(new Queries.Where<TKey, TEntity>(queryable, query)) {}
	}
}