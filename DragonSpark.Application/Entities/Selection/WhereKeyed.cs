using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Selection
{
	public class WhereKeyed<T, TEntity, TKey> : ISelecting<T, IReadOnlyDictionary<TKey, TEntity>> where TKey : notnull
	{
		readonly IQueryable<TEntity> _queryable;
		readonly Query<T, TEntity>   _query;
		readonly Func<TEntity, TKey> _key;

		public WhereKeyed(IQueryable<TEntity> queryable, Query<T, TEntity> query, Func<TEntity, TKey> key)
		{
			_queryable = queryable;
			_query     = query;
			_key       = key;
		}

		public async ValueTask<IReadOnlyDictionary<TKey, TEntity>> Get(T parameter)
			=> await _queryable.Where(_query(parameter)).ToDictionaryAsync(_key).ConfigureAwait(false);
	}
}