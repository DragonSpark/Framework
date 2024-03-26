using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation;

public class EvaluateToSingleOrDefault<TIn, T> : Evaluate<TIn, T, T?>
{
	public EvaluateToSingleOrDefault(IContexts contexts, Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
		: this(new Reading<TIn, T>(contexts, (d, @in) => expression.Invoke(d, @in))) {}

	protected EvaluateToSingleOrDefault(IReading<TIn, T> reading) : base(reading, ToSingleOrDefault<T>.Default) {}
}