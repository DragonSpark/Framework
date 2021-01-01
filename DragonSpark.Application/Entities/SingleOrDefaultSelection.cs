using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities
{
	public class SingleOrDefaultSelection<TKey, TEntity, T> : ISelecting<TKey, T?>
	{
		readonly IQueryable<TEntity>                      _queryable;
		readonly Query<TKey, TEntity>                     _query;
		readonly Func<IQueryable<TEntity>, IQueryable<T>> _selection;

		public SingleOrDefaultSelection(IQueryable<TEntity> queryable, Query<TKey, TEntity> query,
		                                Func<IQueryable<TEntity>, IQueryable<T>> selection)
		{
			_queryable = queryable;
			_query     = query;
			_selection = selection;
		}

		public async ValueTask<T?> Get(TKey parameter)
		{
			var query     = _queryable.Where(_query(parameter));
			var selection = _selection(query);
			var result    = await selection.SingleOrDefaultAsync().ConfigureAwait(false);
			return result;
		}
	}
}