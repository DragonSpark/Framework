using System.Linq;

namespace DragonSpark.Application.Entities.Queries.Materialization.Specialized
{
	public class Where<TKey, TEntity> : ToArray<TKey, TEntity>
	{
		protected Where(IQueryable<TEntity> queryable, Express<TKey, TEntity> express)
			: base(new Scoped.Where<TKey, TEntity>(queryable, express)) {}
	}
}