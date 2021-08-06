using System;
using System.Linq;

namespace DragonSpark.Application.Entities.Queries.Materialization.Specialized
{
	public class SingleOrDefaultSelection<TKey, T> : SingleOrDefaultSelection<TKey, T, T>
	{
		protected SingleOrDefaultSelection(IQueryable<T> queryable, Query<TKey, T> query,
		                                   Func<IQueryable<T>, IQueryable<T>> selection)
			: base(queryable, query, selection) {}
	}

	public class SingleOrDefaultSelection<TKey, TEntity, T> : Materialization.SingleOrDefault<TKey, T?>
	{
		protected SingleOrDefaultSelection(IQueryable<TEntity> queryable, Query<TKey, TEntity> query,
		                                   Func<IQueryable<TEntity>, IQueryable<T>> selection)
			: base(new WhereSelection<TKey, TEntity, T>(queryable, query, selection)) {}
	}
}