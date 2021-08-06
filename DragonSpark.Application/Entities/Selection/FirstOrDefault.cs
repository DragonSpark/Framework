using DragonSpark.Application.Entities.Queries;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Selection
{
	public class FirstOrDefault<TKey, TEntity> : ISelecting<TKey, TEntity?> where TEntity : class
	{
		readonly IQueryable<TEntity>  _queryable;
		readonly Query<TKey, TEntity> _query;

		public FirstOrDefault(IQueryable<TEntity> queryable, Query<TKey, TEntity> query)
		{
			_queryable = queryable;
			_query     = query;
		}

		public async ValueTask<TEntity?> Get(TKey parameter)
		{
			var entity = await _queryable.FirstOrDefaultAsync(_query(parameter)).ConfigureAwait(false);
			var result = entity.Account();
			return result;
		}
	}
}