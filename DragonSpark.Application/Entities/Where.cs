using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities
{
	public class Where<TKey, TEntity> : ISelecting<TKey, List<TEntity>>
	{
		readonly IQueryable<TEntity>  _queryable;
		readonly Query<TKey, TEntity> _query;

		public Where(IQueryable<TEntity> queryable, Query<TKey, TEntity> query)
		{
			_queryable = queryable;
			_query     = query;
		}

		public async ValueTask<List<TEntity>> Get(TKey parameter)
			=> await _queryable.Where(_query(parameter)).ToListAsync().ConfigureAwait(false);
	}

	public class WhereSelected<TKey, T> : WhereSelected<TKey, T, T>
	{
		public WhereSelected(IQueryable<T> queryable, Query<TKey, T> query, Expression<Func<T, T>> select)
			: base(queryable, query, select) {}
	}

	public class WhereSelected<TKey, TEntity, T> : ISelecting<TKey, List<T>>
	{
		readonly IQueryable<TEntity>          _queryable;
		readonly Query<TKey, TEntity>         _query;
		readonly Expression<Func<TEntity, T>> _select;

		public WhereSelected(IQueryable<TEntity> queryable, Query<TKey, TEntity> query,
		                     Expression<Func<TEntity, T>> select)
		{
			_queryable = queryable;
			_query     = query;
			_select    = @select;
		}

		public async ValueTask<List<T>> Get(TKey parameter)
			=> await _queryable.Where(_query(parameter)).Select(_select).ToListAsync().ConfigureAwait(false);
	}

	public class WhereMany<TKey, T> : WhereMany<TKey, T, T>
	{
		public WhereMany(IQueryable<T> queryable, Query<TKey, T> query, Expression<Func<T, IEnumerable<T>>> select)
			: base(queryable, query, select) {}
	}

	public class WhereMany<TKey, TEntity, T> : ISelecting<TKey, List<T>>
	{
		readonly IQueryable<TEntity>                      _queryable;
		readonly Query<TKey, TEntity>                     _query;
		readonly Expression<Func<TEntity, IEnumerable<T>>> _select;

		public WhereMany(IQueryable<TEntity> queryable, Query<TKey, TEntity> query,
		                 Expression<Func<TEntity, IEnumerable<T>>> select)
		{
			_queryable = queryable;
			_query     = query;
			_select    = select;
		}

		public async ValueTask<List<T>> Get(TKey parameter)
			=> await _queryable.Where(_query(parameter)).SelectMany(_select).ToListAsync().ConfigureAwait(false);
	}
}