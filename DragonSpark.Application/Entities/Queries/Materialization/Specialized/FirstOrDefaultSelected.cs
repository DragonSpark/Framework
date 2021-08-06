using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Materialization.Specialized
{
	public class FirstOrDefaultSelected<TKey, TEntity, T> : FirstOrDefault<TKey, T?>
	{
		protected FirstOrDefaultSelected(IQueryable<TEntity> queryable, Query<TKey, TEntity> query,
		                                 Expression<Func<TEntity, T>> select)
			: base(new WhereSelect<TKey, TEntity, T>(queryable, query, select)) {}
	}
}