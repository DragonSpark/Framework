using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Selection
{
	public class SingleSelected<TKey, T> : SingleSelected<TKey, TKey, T>
	{
		protected SingleSelected(IQueryable<TKey> queryable, Query<TKey, TKey> query, Expression<Func<TKey, T>> select)
			: base(queryable, query, select) {}
	}

	public class SingleSelected<TKey, TEntity, T> : ISelecting<TKey, T>
	{
		readonly IQueryable<TEntity>          _queryable;
		readonly Query<TKey, TEntity>         _query;
		readonly Expression<Func<TEntity, T>> _select;

		protected SingleSelected(IQueryable<TEntity> queryable, Query<TKey, TEntity> query,
		                         Expression<Func<TEntity, T>> select)
		{
			_queryable = queryable;
			_query     = query;
			_select    = select;
		}

		public ValueTask<T> Get(TKey parameter) => _queryable.Where(_query(parameter))
		                                                     .Select(_select)
		                                                     .SingleAsync()
		                                                     .ToOperation();
	}
}