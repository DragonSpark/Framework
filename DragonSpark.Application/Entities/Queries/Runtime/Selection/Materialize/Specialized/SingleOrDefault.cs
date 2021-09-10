using System.Linq;

namespace DragonSpark.Application.Entities.Queries.Runtime.Selection.Materialize.Specialized
{
	public class SingleOrDefault<TKey, TEntity> : Materialize.SingleOrDefault<TKey, TEntity?>
	{
		public SingleOrDefault(IQueryable<TEntity> queryable, Express<TKey, TEntity> express)
			: base(new Selection.Where<TKey, TEntity>(queryable, express)) {}
	}
}