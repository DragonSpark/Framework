using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Materialization.Specialized
{
	public class WhereSelected<TKey, TEntity, T> : ToArray<TKey, T>
	{
		protected WhereSelected(IQueryable<TEntity> queryable, Query<TKey, TEntity> query,
		                        Expression<Func<TEntity, T>> select)
			: base(new WhereSelect<TKey, TEntity, T>(queryable, query, select)) {}
	}
}