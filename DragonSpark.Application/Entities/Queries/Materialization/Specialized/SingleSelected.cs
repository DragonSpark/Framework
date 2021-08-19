using DragonSpark.Application.Entities.Queries.Scoped;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Materialization.Specialized
{
	public class SingleSelected<TKey, T> : SingleSelected<TKey, TKey, T>
	{
		protected SingleSelected(IQueryable<TKey> queryable, Express<TKey, TKey> express, Expression<Func<TKey, T>> select)
			: base(queryable, express, select) {}
	}

	public class SingleSelected<TKey, TEntity, T> : Materialization.Single<TKey, T>
	{
		protected SingleSelected(IQueryable<TEntity> queryable, Express<TKey, TEntity> express,
		                         Expression<Func<TEntity, T>> select)
			: base(new WhereSelect<TKey, TEntity, T>(queryable, express, select)) {}
	}
}