using DragonSpark.Application.Entities.Selection;
using System;
using System.Linq;

namespace DragonSpark.Application.Entities.Queries
{
	public class ParameterAwareWhereSelection<TKey, TEntity, T> : IQuery<TKey, T>
	{
		readonly IQueryable<TEntity>                            _queryable;
		readonly Query<TKey, TEntity>                           _query;
		readonly Func<TKey, IQueryable<TEntity>, IQueryable<T>> _selection;

		protected ParameterAwareWhereSelection(IQueryable<TEntity> queryable, Query<TKey, TEntity> query,
		                                       Func<TKey, IQueryable<TEntity>, IQueryable<T>> selection)
		{
			_queryable = queryable;
			_query     = query;
			_selection = selection;
		}

		public IQueryable<T> Get(TKey parameter)
		{
			var query  = _queryable.Where(_query(parameter));
			var result = _selection(parameter, query);
			return result;
		}
	}
}