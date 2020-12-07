using System;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities
{
	public delegate Expression<Func<TEntity, bool>> Query<in TKey, TEntity>(TKey parameter);
}