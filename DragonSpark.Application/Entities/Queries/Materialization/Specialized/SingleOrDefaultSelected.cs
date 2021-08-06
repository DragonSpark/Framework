using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Materialization.Specialized
{
	public class SingleOrDefaultSelected<TKey, T> : SingleOrDefaultSelected<TKey, TKey, T>
	{
		public SingleOrDefaultSelected(IQueryable<TKey> queryable, Query<TKey, TKey> query,
		                               Expression<Func<TKey, T>> @select) : base(queryable, query, @select) {}
	}

	public class SingleOrDefaultSelected<TKey, TEntity, T> : Materialization.SingleOrDefault<TKey, T?>
	{
		protected SingleOrDefaultSelected(IQueryable<TEntity> queryable, Query<TKey, TEntity> query,
		                                  Expression<Func<TEntity, T>> select)
			: base(new WhereSelect<TKey, TEntity, T>(queryable, query, select)) {}
	}
}