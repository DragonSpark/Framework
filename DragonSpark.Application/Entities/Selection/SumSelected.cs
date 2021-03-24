using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Selection
{
	public class SumSelected<T> : SumSelected<T, T>
	{
		public SumSelected(IQueryable<T> queryable, Query<T, T> query, Expression<Func<T, decimal>> select)
			: base(queryable, query, select) {}
	}

	public class SumSelected<TKey, TEntity> : ISelecting<TKey, decimal>
	{
		readonly IQueryable<TEntity>                _queryable;
		readonly Query<TKey, TEntity>               _query;
		readonly Expression<Func<TEntity, decimal>> _select;

		public SumSelected(IQueryable<TEntity> queryable, Query<TKey, TEntity> query,
		                   Expression<Func<TEntity, decimal>> select)
		{
			_queryable = queryable;
			_query     = query;
			_select    = select;
		}

		public async ValueTask<decimal> Get(TKey parameter)
			=> await _queryable.Where(_query(parameter)).Select(_select).SumAsync().ConfigureAwait(false);
	}
}