using DragonSpark.Model.Results;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries
{
	public class Query<T> : Instance<IQueryable<T>>
	{
		protected Query(IQueryable<T> instance) : base(instance) {}
	}


	public delegate Expression<Func<TEntity, bool>> Query<in TKey, TEntity>(TKey parameter);
	public delegate Expression<Func<TFrom, TTo>> Query<in TKey, TFrom, TTo>(TKey parameter);
}