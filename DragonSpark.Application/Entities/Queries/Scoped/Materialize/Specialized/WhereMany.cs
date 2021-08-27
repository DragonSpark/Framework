using DragonSpark.Application.Entities.Queries.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Scoped.Materialize.Specialized
{
	public class WhereMany<TKey, T> : WhereMany<TKey, T, T>
	{
		protected WhereMany(IQueryable<T> queryable, Express<TKey, T> express, Expression<Func<T, IEnumerable<T>>> select)
			: base(queryable, express, select) {}
	}

	public class WhereMany<TKey, TEntity, T> : ToArray<TKey, T>
	{
		protected WhereMany(IQueryable<TEntity> queryable, Express<TKey, TEntity> express,
		                    Expression<Func<TEntity, IEnumerable<T>>> select)
			: base(new Scoped.WhereMany<TKey, TEntity, T>(queryable, express, select)) {}
	}
}