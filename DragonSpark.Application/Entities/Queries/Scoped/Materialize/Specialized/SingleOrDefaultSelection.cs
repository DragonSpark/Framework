using System;
using System.Linq;

namespace DragonSpark.Application.Entities.Queries.Scoped.Materialize.Specialized
{
	public class SingleOrDefaultSelection<TKey, T> : SingleOrDefaultSelection<TKey, T, T>
	{
		protected SingleOrDefaultSelection(IQueryable<T> queryable, Express<TKey, T> express,
		                                   Func<IQueryable<T>, IQueryable<T>> selection)
			: base(queryable, express, selection) {}
	}

	public class SingleOrDefaultSelection<TKey, TEntity, T> : Materialize.SingleOrDefault<TKey, T?>
	{
		protected SingleOrDefaultSelection(IQueryable<TEntity> queryable, Express<TKey, TEntity> express,
		                                   Func<IQueryable<TEntity>, IQueryable<T>> selection)
			: base(new WhereSelection<TKey, TEntity, T>(queryable, express, selection)) {}
	}
}