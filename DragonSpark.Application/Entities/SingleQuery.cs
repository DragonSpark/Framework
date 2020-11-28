using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities
{
	public class SingleQuery<TKey, TEntity> : ISelecting<TKey, TEntity?> where TEntity : class
	{
		readonly IQueryable<TEntity>  _queryable;
		readonly Query<TKey, TEntity> _query;

		public SingleQuery(IQueryable<TEntity> queryable, Query<TKey, TEntity> query)
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

	public delegate Expression<Func<TEntity, bool>> Query<in TKey, TEntity>(TKey parameter);
}