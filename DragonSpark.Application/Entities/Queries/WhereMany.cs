using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries
{
	public class WhereMany<TKey, T> : WhereMany<TKey, T, T>
	{
		public WhereMany(IQueryable<T> queryable, Query<TKey, T> query, Expression<Func<T, IEnumerable<T>>> select)
			: base(queryable, query, select) {}
	}

	public class WhereMany<TKey, TEntity, T> : IQuery<TKey, T>
	{
		readonly IQueryable<TEntity>                       _queryable;
		readonly Query<TKey, TEntity>                      _query;
		readonly Expression<Func<TEntity, IEnumerable<T>>> _select;

		public WhereMany(IQueryable<TEntity> queryable, Query<TKey, TEntity> query,
		                 Expression<Func<TEntity, IEnumerable<T>>> select)
		{
			_queryable = queryable;
			_query     = query;
			_select    = select;
		}

		public IQueryable<T> Get(TKey parameter) => _queryable.Where(_query(parameter)).SelectMany(_select);
	}

}