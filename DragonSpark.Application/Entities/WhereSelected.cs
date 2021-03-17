using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities
{
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

		public ValueTask<List<T>> Get(TKey parameter)
			=> _queryable.Where(_query(parameter)).Select(_select).ToListAsync().ToOperation();
	}
}