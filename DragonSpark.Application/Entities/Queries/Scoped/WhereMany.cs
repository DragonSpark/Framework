using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Scoped
{
	public class WhereMany<TKey, T> : WhereMany<TKey, T, T>
	{
		public WhereMany(IQueryable<T> queryable, Express<TKey, T> express, Expression<Func<T, IEnumerable<T>>> select)
			: base(queryable, express, select) {}
	}

	public class WhereMany<TKey, TEntity, T> : IQuery<TKey, T>
	{
		readonly IQueryable<TEntity>                       _queryable;
		readonly Express<TKey, TEntity>                    _express;
		readonly Expression<Func<TEntity, IEnumerable<T>>> _select;

		public WhereMany(IQueryable<TEntity> queryable, Express<TKey, TEntity> express,
		                 Expression<Func<TEntity, IEnumerable<T>>> select)
		{
			_queryable = queryable;
			_express   = express;
			_select    = select;
		}

		public IQueryable<T> Get(TKey parameter) => _queryable.Where(_express(parameter)).SelectMany(_select);
	}
}