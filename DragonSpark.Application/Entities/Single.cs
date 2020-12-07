﻿using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities
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

		public async ValueTask<TEntity> Get(TKey parameter)
			=> await _queryable.SingleAsync(_query(parameter)).ConfigureAwait(false);
	}
}