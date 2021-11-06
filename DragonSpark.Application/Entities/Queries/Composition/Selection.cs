using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Composition;

public class Selection<TFrom, TTo> : Projection<IQueryable<TFrom>, IQueryable<TTo>>, ISelection<TFrom, TTo>
{
	protected Selection(Expression<Func<IQueryable<TFrom>, IQueryable<TTo>>> instance) : base(instance) {}
}