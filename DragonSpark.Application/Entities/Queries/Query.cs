using System;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries
{
	public delegate Expression<Func<TEntity, bool>> Query<in TKey, TEntity>(TKey parameter);
	public delegate Expression<Func<TFrom, TTo>> Query<in TKey, TFrom, TTo>(TKey parameter);
}