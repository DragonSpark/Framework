using System.Linq;

namespace DragonSpark.Application.Entities.Queries.Runtime.Selection.Materialize.Specialized
{
	public class Where<TKey, TEntity> : ToArray<TKey, TEntity>
	{
		protected Where(IQueryable<TEntity> queryable, Express<TKey, TEntity> express)
			: base(new Selection.Where<TKey, TEntity>(queryable, express)) {}
	}
}