using DragonSpark.Model.Results;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Composition;

public class Selection<TFrom, TTo> : Instance<Expression<Func<IQueryable<TFrom>, IQueryable<TTo>>>>,
                                     ISelection<TFrom, TTo>
{
	protected Selection(Expression<Func<IQueryable<TFrom>, IQueryable<TTo>>> instance) : base(instance) {}
}