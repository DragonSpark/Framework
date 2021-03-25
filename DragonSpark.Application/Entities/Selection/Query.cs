using DragonSpark.Model.Results;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Selection
{
	public class Query<T> : Instance<IQueryable<T>>
	{
		public Query(IQueryable<T> instance) : base(instance) {}
	}

	public delegate Expression<Func<TEntity, bool>> Query<in TKey, TEntity>(TKey parameter);
}