using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Selection
{
	public class Any<TKey, TEntity> : IDepending<TKey>
	{
		readonly IQueryable<TEntity>          _queryable;
		readonly Query<TKey, TEntity>         _query;

		public Any(IQueryable<TEntity> queryable, Query<TKey, TEntity> query)
		{
			_queryable = queryable;
			_query     = query;
		}

		public ValueTask<bool> Get(TKey parameter) => _queryable.Where(_query(parameter)).AnyAsync().ToOperation();
	}
}