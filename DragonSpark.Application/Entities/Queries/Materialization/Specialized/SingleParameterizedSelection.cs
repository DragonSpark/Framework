using DragonSpark.Application.Entities.Queries.Scoped;
using System;
using System.Linq;

namespace DragonSpark.Application.Entities.Queries.Materialization.Specialized
{
	public class SingleParameterizedSelection<TKey, TEntity, T> : Materialization.Single<TKey, T>
	{
		protected SingleParameterizedSelection(IQueryable<TEntity> queryable, Express<TKey, TEntity> express,
		                                       Func<TKey, IQueryable<TEntity>, IQueryable<T>> selection)
			: base(new ParameterAwareWhereSelection<TKey, TEntity, T>(queryable, express, selection)) {}
	}
}