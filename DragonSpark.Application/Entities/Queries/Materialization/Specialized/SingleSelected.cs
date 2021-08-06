using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Materialization.Specialized
{
	public class SingleSelected<TKey, T> : SingleSelected<TKey, TKey, T>
	{
		protected SingleSelected(IQueryable<TKey> queryable, Query<TKey, TKey> query, Expression<Func<TKey, T>> select)
			: base(queryable, query, select) {}
	}

	public class SingleSelected<TKey, TEntity, T> : Materialization.Single<TKey, T>
	{
		protected SingleSelected(IQueryable<TEntity> queryable, Query<TKey, TEntity> query,
		                         Expression<Func<TEntity, T>> select)
			: base(new WhereSelect<TKey, TEntity, T>(queryable, query, select)) {}
	}
}