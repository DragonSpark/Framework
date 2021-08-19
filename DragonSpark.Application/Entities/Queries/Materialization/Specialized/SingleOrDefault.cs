using System.Linq;

namespace DragonSpark.Application.Entities.Queries.Materialization.Specialized
{
	public class SingleOrDefault<TKey, TEntity> : Materialization.SingleOrDefault<TKey, TEntity?>
	{
		public SingleOrDefault(IQueryable<TEntity> queryable, Express<TKey, TEntity> express)
			: base(new Scoped.Where<TKey, TEntity>(queryable, express)) {}
	}
}