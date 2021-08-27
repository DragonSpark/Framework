using DragonSpark.Application.Entities.Queries.Model;
using System.Linq;

namespace DragonSpark.Application.Entities.Queries.Scoped.Materialize.Specialized
{
	public class SingleOrDefault<TKey, TEntity> : Materialize.SingleOrDefault<TKey, TEntity?>
	{
		public SingleOrDefault(IQueryable<TEntity> queryable, Express<TKey, TEntity> express)
			: base(new Scoped.Where<TKey, TEntity>(queryable, express)) {}
	}
}