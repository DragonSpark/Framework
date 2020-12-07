using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities
{
	public class SingleSelected<TKey, TEntity, T> : ISelecting<TKey, T>
	{
		readonly IQueryable<TEntity>          _queryable;
		readonly Query<TKey, TEntity>         _query;
		readonly Expression<Func<TEntity, T>> _select;

		public SingleSelected(IQueryable<TEntity> queryable, Query<TKey, TEntity> query,
		                      Expression<Func<TEntity, T>> select)
		{
			_queryable = queryable;
			_query     = query;
			_select    = select;
		}

		public async ValueTask<T> Get(TKey parameter) => await _queryable.Where(_query(parameter))
		                                                                 .Select(_select)
		                                                                 .SingleAsync()
		                                                                 .ConfigureAwait(false);
	}
}