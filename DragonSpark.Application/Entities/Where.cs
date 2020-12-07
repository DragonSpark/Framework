using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities
{
	public class Where<TKey, TEntity> : ISelecting<TKey, List<TEntity>>
	{
		readonly IQueryable<TEntity>  _queryable;
		readonly Query<TKey, TEntity> _query;

		public Where(IQueryable<TEntity> queryable, Query<TKey, TEntity> query)
		{
			_queryable = queryable;
			_query     = query;
		}

		public async ValueTask<List<TEntity>> Get(TKey parameter)
			=> await _queryable.Where(_query(parameter)).ToListAsync().ConfigureAwait(false);
	}
}