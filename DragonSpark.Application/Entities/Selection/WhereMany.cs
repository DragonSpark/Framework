using DragonSpark.Application.Entities.Queries;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Selection
{
	public class WhereMany<TKey, TEntity, T> : ISelecting<TKey, Array<T>>
	{
		readonly IQueryable<TEntity>                       _queryable;
		readonly Query<TKey, TEntity>                      _query;
		readonly Expression<Func<TEntity, IEnumerable<T>>> _select;

		protected WhereMany(IQueryable<TEntity> queryable, Query<TKey, TEntity> query,
		                    Expression<Func<TEntity, IEnumerable<T>>> select)
		{
			_queryable = queryable;
			_query     = query;
			_select    = select;
		}

		public async ValueTask<Array<T>> Get(TKey parameter)
			=> await _queryable.Where(_query(parameter)).SelectMany(_select).ToArrayAsync().ConfigureAwait(false);
	}

	public class WhereMany<TKey, T> : WhereMany<TKey, T, T>
	{
		public WhereMany(IQueryable<T> queryable, Query<TKey, T> query, Expression<Func<T, IEnumerable<T>>> select)
			: base(queryable, query, select) {}
	}
}