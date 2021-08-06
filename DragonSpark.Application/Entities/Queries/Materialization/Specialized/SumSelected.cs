using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Materialization.Specialized
{
	public class SumSelected<T> : SumSelected<T, T>
	{
		public SumSelected(IQueryable<T> queryable, Query<T, T> query, Expression<Func<T, decimal>> select)
			: base(queryable, query, select) {}
	}

	public class SumSelected<TKey, TEntity> : Sum<TKey>
	{
		protected SumSelected(IQueryable<TEntity> queryable, Query<TKey, TEntity> query,
		                      Expression<Func<TEntity, decimal>> select)
			: base(new WhereSelect<TKey, TEntity, decimal>(queryable, query, select)) {}
	}
}