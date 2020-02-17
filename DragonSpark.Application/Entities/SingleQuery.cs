using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities {
	public class SingleQuery<TKey, TEntity> : IOperationResult<TKey, TEntity>
	{
		readonly IQueryable<TEntity>   _queryable;
		readonly IQuery<TKey, TEntity> _query;

		public SingleQuery(IQueryable<TEntity> queryable, IQuery<TKey, TEntity> query)
		{
			_queryable = queryable;
			_query     = query;
		}

		public async ValueTask<TEntity> Get(TKey parameter)
			=> await _queryable.SingleOrDefaultAsync(_query.Get(parameter)).ConfigureAwait(false);
	}
}