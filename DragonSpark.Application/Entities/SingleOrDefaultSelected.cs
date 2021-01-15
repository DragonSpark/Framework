﻿using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities
{
	public class SingleOrDefaultSelected<TKey, TEntity, T> : ISelecting<TKey, T?> where T : class
	{
		readonly IQueryable<TEntity>          _queryable;
		readonly Query<TKey, TEntity>         _query;
		readonly Expression<Func<TEntity, T>> _select;

		public SingleOrDefaultSelected(IQueryable<TEntity> queryable, Query<TKey, TEntity> query,
		                               Expression<Func<TEntity, T>> select)
		{
			_queryable = queryable;
			_query     = query;
			_select    = select;
		}

		public async ValueTask<T?> Get(TKey parameter) => await _queryable.Where(_query(parameter))
		                                                                  .Select(_select)
		                                                                  .SingleOrDefaultAsync()
		                                                                  .ConfigureAwait(false);
	}
}