using System;
using System.Linq;

namespace DragonSpark.Application.Entities.Queries
{
	public class ParameterAwareWhereSelection<TKey, TEntity, T> : IQuery<TKey, T>
	{
		readonly IQuery<TKey, TEntity>                          _query;
		readonly Query<TKey, TEntity>                           _where;
		readonly Func<TKey, IQueryable<TEntity>, IQueryable<T>> _selection;

		public ParameterAwareWhereSelection(IQueryable<TEntity> queryable, Query<TKey, TEntity> where,
		                                    Func<TKey, IQueryable<TEntity>, IQueryable<T>> selection)
			: this(new Accept<TKey, TEntity>(queryable), where, selection) {}

		public ParameterAwareWhereSelection(IQuery<TKey, TEntity> query, Query<TKey, TEntity> where,
		                                    Func<TKey, IQueryable<TEntity>, IQueryable<T>> selection)
		{
			_query     = query;
			_where     = where;
			_selection = selection;
		}

		public IQueryable<T> Get(TKey parameter)
		{
			var query  = _query.Get(parameter).Where(_where(parameter));
			var result = _selection(parameter, query);
			return result;
		}
	}
}