using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Runtime.Selection.Materialize.Specialized
{
	public class FirstOrDefaultSelected<TKey, TEntity, T> : FirstOrDefault<TKey, T?>
	{
		protected FirstOrDefaultSelected(IQueryable<TEntity> queryable, Express<TKey, TEntity> express,
		                                 Expression<Func<TEntity, T>> select)
			: base(new WhereSelect<TKey, TEntity, T>(queryable, express, select)) {}
	}
}