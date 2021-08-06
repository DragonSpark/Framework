using DragonSpark.Application.Entities.Queries;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Selection
{
	public class WhereSelected<TKey, T> : WhereSelected<TKey, T, T>
	{
		protected WhereSelected(IQueryable<T> queryable, Query<TKey, T> query, Expression<Func<T, T>> select)
			: base(queryable, query, select) {}
	}

	public class WhereSelected<TKey, TEntity, T> : ISelecting<TKey, Array<T>>
	{
		readonly IQueryable<TEntity>          _queryable;
		readonly Query<TKey, TEntity>         _query;
		readonly Expression<Func<TEntity, T>> _select;

		protected WhereSelected(IQueryable<TEntity> queryable, Query<TKey, TEntity> query,
		                        Expression<Func<TEntity, T>> select)
		{
			_queryable = queryable;
			_query     = query;
			_select    = select;
		}

		public async ValueTask<Array<T>> Get(TKey parameter)
			=> await _queryable.Where(_query(parameter)).Select(_select).ToArrayAsync().ConfigureAwait(false);
	}
}