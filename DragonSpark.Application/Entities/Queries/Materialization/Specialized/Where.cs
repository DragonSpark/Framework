using System.Linq;

namespace DragonSpark.Application.Entities.Queries.Materialization.Specialized
{
	public class Where<TKey, TEntity> : ToArray<TKey, TEntity>
	{
		protected Where(IQueryable<TEntity> queryable, Query<TKey, TEntity> query)
			: base(new Queries.Where<TKey, TEntity>(queryable, query)) {}
	}
}