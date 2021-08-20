using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Scoped.Materialize.Specialized
{
	public class SingleOrDefaultSelected<TKey, T> : SingleOrDefaultSelected<TKey, TKey, T>
	{
		public SingleOrDefaultSelected(IQueryable<TKey> queryable, Express<TKey, TKey> express,
		                               Expression<Func<TKey, T>> @select) : base(queryable, express, @select) {}
	}

	public class SingleOrDefaultSelected<TKey, TEntity, T> : Materialize.SingleOrDefault<TKey, T?>
	{
		protected SingleOrDefaultSelected(IQueryable<TEntity> queryable, Express<TKey, TEntity> express,
		                                  Expression<Func<TEntity, T>> select)
			: base(new WhereSelect<TKey, TEntity, T>(queryable, express, select)) {}
	}
}