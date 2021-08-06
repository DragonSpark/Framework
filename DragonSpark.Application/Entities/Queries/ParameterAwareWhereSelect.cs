using DragonSpark.Application.Entities.Selection;
using System.Linq;

namespace DragonSpark.Application.Entities.Queries
{
	public class ParameterAwareWhereSelect<TKey, TEntity, T> : IQuery<TKey, T>
	{
		readonly IQueryable<TEntity>     _queryable;
		readonly Query<TKey, TEntity>    _query;
		readonly Query<TKey, TEntity, T> _selection;

		protected ParameterAwareWhereSelect(IQueryable<TEntity> queryable, Query<TKey, TEntity> query,
		                                    Query<TKey, TEntity, T> selection)
		{
			_queryable = queryable;
			_query     = query;
			_selection = selection;
		}

		public IQueryable<T> Get(TKey parameter) => _queryable.Where(_query(parameter)).Select(_selection(parameter));
	}
}