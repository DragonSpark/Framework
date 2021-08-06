using System;
using System.Linq;

namespace DragonSpark.Application.Entities.Queries.Materialization.Specialized
{
	public class SingleSelection<TKey, TEntity, T> : Materialization.Single<TKey, T>
	{
		protected SingleSelection(IQueryable<TEntity> queryable, Query<TKey, TEntity> query,
		                          Func<IQueryable<TEntity>, IQueryable<T>> selection)
			: base(new WhereSelection<TKey, TEntity, T>(queryable, query, selection)) {}
	}
}