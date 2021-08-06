using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Materialization.Specialized
{
	public class WhereMany<TKey, T> : WhereMany<TKey, T, T>
	{
		protected WhereMany(IQueryable<T> queryable, Query<TKey, T> query, Expression<Func<T, IEnumerable<T>>> select)
			: base(queryable, query, select) {}
	}

	public class WhereMany<TKey, TEntity, T> : ToArray<TKey, T>
	{
		protected WhereMany(IQueryable<TEntity> queryable, Query<TKey, TEntity> query,
		                    Expression<Func<TEntity, IEnumerable<T>>> select)
			: base(new Queries.WhereMany<TKey, TEntity, T>(queryable, query, select)) {}
	}
}