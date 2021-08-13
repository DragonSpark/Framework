using System;
using System.Linq;

namespace DragonSpark.Application.Entities.Queries.Materialization.Specialized
{
	public class WhereKeyed<T, TEntity, TKey> : ToDictionary<T, TKey, TEntity> where TKey : notnull
	{
		protected WhereKeyed(IQueryable<TEntity> queryable, Query<T, TEntity> query, Func<TEntity, TKey> key)
			: base(new Queries.Where<T, TEntity>(queryable, query), key) {}
	}
}