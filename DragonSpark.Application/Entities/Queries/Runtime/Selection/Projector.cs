using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Runtime.Selection;

public class Projector<TIn, TOut> : Selector<IQueryable<TIn>, IQueryable<TOut>>
{
	protected Projector(Expression<Func<IQueryable<TIn>, IQueryable<TOut>>> instance) : base(instance) {}
}