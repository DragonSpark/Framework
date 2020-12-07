using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities
{
	public class SingleOrDefault<TKey, TEntity> : ISelecting<TKey, TEntity?> where TEntity : class
	{
		readonly IQueryable<TEntity>  _queryable;
		readonly Query<TKey, TEntity> _query;

		public SingleOrDefault(IQueryable<TEntity> queryable, Query<TKey, TEntity> query)
		{
			_queryable = queryable;
			_query     = query;
		}

		public async ValueTask<TEntity?> Get(TKey parameter)
		{
			var entity = await _queryable.SingleOrDefaultAsync(_query(parameter)).ConfigureAwait(false);
			var result = entity.Account();
			return result;
		}
	}
}