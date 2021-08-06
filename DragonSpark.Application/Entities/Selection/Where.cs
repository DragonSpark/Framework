using DragonSpark.Application.Entities.Queries;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Selection
{
	public class Where<TKey, TEntity> : ISelecting<TKey, Array<TEntity>>
	{
		readonly IQueryable<TEntity>  _queryable;
		readonly Query<TKey, TEntity> _query;

		public Where(IQueryable<TEntity> queryable, Query<TKey, TEntity> query)
		{
			_queryable = queryable;
			_query     = query;
		}

		public async ValueTask<Array<TEntity>> Get(TKey parameter)
			=> await _queryable.Where(_query(parameter)).ToArrayAsync().ConfigureAwait(false);
	}
}