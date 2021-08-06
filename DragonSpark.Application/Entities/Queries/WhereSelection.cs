using System;
using System.Linq;

namespace DragonSpark.Application.Entities.Queries
{
	public class WhereSelection<TKey, TEntity, T> : IQuery<TKey, T>
	{
		readonly IQueryable<TEntity>                      _source;
		readonly Query<TKey, TEntity>                     _query;
		readonly Func<IQueryable<TEntity>, IQueryable<T>> _selection;

		public WhereSelection(IQueryable<TEntity> source, Query<TKey, TEntity> query,
		                      Func<IQueryable<TEntity>, IQueryable<T>> selection)
		{
			_source    = source;
			_query     = query;
			_selection = selection;
		}

		public IQueryable<T> Get(TKey parameter) => _selection(_source.Where(_query(parameter)));
	}
}