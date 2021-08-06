using System;
using System.Linq;

namespace DragonSpark.Application.Entities.Queries.Materialization.Specialized
{
	public class SingleParameterizedSelection<TKey, TEntity, T> : Materialization.Single<TKey, T>
	{
		protected SingleParameterizedSelection(IQueryable<TEntity> queryable, Query<TKey, TEntity> query,
		                                       Func<TKey, IQueryable<TEntity>, IQueryable<T>> selection)
			: base(new ParameterAwareWhereSelection<TKey, TEntity, T>(queryable, query, selection)) {}
	}
}