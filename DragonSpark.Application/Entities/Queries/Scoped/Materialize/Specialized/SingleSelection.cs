using System;
using System.Linq;

namespace DragonSpark.Application.Entities.Queries.Scoped.Materialize.Specialized
{
	public class SingleSelection<TKey, TEntity, T> : Materialize.Single<TKey, T>
	{
		protected SingleSelection(IQueryable<TEntity> queryable, Express<TKey, TEntity> express,
		                          Func<IQueryable<TEntity>, IQueryable<T>> selection)
			: base(new WhereSelection<TKey, TEntity, T>(queryable, express, selection)) {}
	}
}