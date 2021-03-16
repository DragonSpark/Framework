using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities
{
	public class WhereSelection<TKey, T> : WhereSelection<TKey, T, T>
	{
		public WhereSelection(IQueryable<T> queryable, Query<TKey, T> query,
		                      Func<IQueryable<T>, IQueryable<T>> selection)
			: base(queryable, query, selection) {}
	}

	public class WhereSelection<TKey, TEntity, T> : ISelecting<TKey, List<T>>
	{
		readonly IQueryable<TEntity>                      _queryable;
		readonly Query<TKey, TEntity>                     _query;
		readonly Func<IQueryable<TEntity>, IQueryable<T>> _selection;

		public WhereSelection(IQueryable<TEntity> queryable, Query<TKey, TEntity> query,
		                      Func<IQueryable<TEntity>, IQueryable<T>> selection)
		{
			_queryable = queryable;
			_query     = query;
			_selection = selection;
		}

		public ValueTask<List<T>> Get(TKey parameter)
		{
			var query     = _queryable.Where(_query(parameter));
			var selection = _selection(query);
			var result    = selection.ToListAsync().ToOperation();
			return result;
		}
	}


}