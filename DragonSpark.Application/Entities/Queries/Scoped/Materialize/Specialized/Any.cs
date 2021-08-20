using DragonSpark.Model.Operations;
using System.Linq;

namespace DragonSpark.Application.Entities.Queries.Scoped.Materialize.Specialized
{
	public class Any<TIn, TEntity> : Materialize.Any<TIn, TEntity>,
	                                 IDepending<TIn>
	{
		protected Any(IQueryable<TEntity> queryable, Express<TIn, TEntity> express)
			: base(new Scoped.Where<TIn, TEntity>(queryable, express)) {}
	}
}