using DragonSpark.Application.Entities.Queries.Model;
using System.Linq;

namespace DragonSpark.Application.Entities.Queries.Scoped.Materialize.Specialized
{
	public class Single<TKey, TEntity> : Materialize.Single<TKey, TEntity>
	{
		protected Single(IQueryable<TEntity> queryable, Express<TKey, TEntity> express)
			: base(new Scoped.Where<TKey, TEntity>(queryable, express)) {}
	}
}