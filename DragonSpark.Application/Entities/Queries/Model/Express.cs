using System;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Model
{
	public delegate Expression<Func<TEntity, bool>> Express<in TKey, TEntity>(TKey parameter);
}