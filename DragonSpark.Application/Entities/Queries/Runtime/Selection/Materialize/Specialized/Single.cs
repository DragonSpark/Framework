using System.Linq;

namespace DragonSpark.Application.Entities.Queries.Runtime.Selection.Materialize.Specialized
{
	public class Single<TKey, TEntity> : Materialize.Single<TKey, TEntity>
	{
		protected Single(IQueryable<TEntity> queryable, Express<TKey, TEntity> express)
			: base(new Selection.Where<TKey, TEntity>(queryable, express)) {}
	}
}