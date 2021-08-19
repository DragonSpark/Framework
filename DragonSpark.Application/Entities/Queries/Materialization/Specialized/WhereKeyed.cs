using System;
using System.Linq;

namespace DragonSpark.Application.Entities.Queries.Materialization.Specialized
{
	public class WhereKeyed<T, TEntity, TKey> : ToDictionary<T, TKey, TEntity> where TKey : notnull
	{
		protected WhereKeyed(IQueryable<TEntity> queryable, Express<T, TEntity> express, Func<TEntity, TKey> key)
			: base(new Scoped.Where<T, TEntity>(queryable, express), key) {}
	}
}