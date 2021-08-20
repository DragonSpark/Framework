using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Scoped.Materialize.Specialized
{
	public class SumSelected<T> : SumSelected<T, T>
	{
		public SumSelected(IQueryable<T> queryable, Express<T, T> express, Expression<Func<T, decimal>> select)
			: base(queryable, express, select) {}
	}

	public class SumSelected<TKey, TEntity> : Sum<TKey>
	{
		protected SumSelected(IQueryable<TEntity> queryable, Express<TKey, TEntity> express,
		                      Expression<Func<TEntity, decimal>> select)
			: base(new WhereSelect<TKey, TEntity, decimal>(queryable, express, select)) {}
	}
}