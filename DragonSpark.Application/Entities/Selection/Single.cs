using DragonSpark.Application.Entities.Queries;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Selection
{
	public class Single<TKey, TEntity> : ISelecting<TKey, TEntity>
	{
		readonly IQueryable<TEntity>  _queryable;
		readonly Query<TKey, TEntity> _query;

		public Single(IQueryable<TEntity> queryable, Query<TKey, TEntity> query)
		{
			_queryable = queryable;
			_query     = query;
		}

		public ValueTask<TEntity> Get(TKey parameter) => _queryable.SingleAsync(_query(parameter)).ToOperation();
	}
}