using System;
using System.Linq;

namespace DragonSpark.Application.Entities.Queries
{
	public class ParameterAwareSelection<TKey, TEntity, T> : IQuery<TKey, T>
	{
		readonly IQuery<TKey, TEntity>                          _query;
		readonly Func<TKey, IQueryable<TEntity>, IQueryable<T>> _selection;

		public ParameterAwareSelection(IQueryable<TEntity> queryable,
		                               Func<TKey, IQueryable<TEntity>, IQueryable<T>> selection)
			: this(new Accept<TKey, TEntity>(queryable), selection) {}

		public ParameterAwareSelection(IQuery<TKey, TEntity> query,
		                               Func<TKey, IQueryable<TEntity>, IQueryable<T>> selection)
		{
			_query     = query;
			_selection = selection;
		}

		public IQueryable<T> Get(TKey parameter) => _selection(parameter, _query.Get(parameter));
	}
}