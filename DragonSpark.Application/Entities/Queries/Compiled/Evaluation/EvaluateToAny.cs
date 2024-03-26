using DragonSpark.Model.Operations.Selection.Conditions;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation;

public class EvaluateToAny<TIn, T> : Evaluate<TIn, T, bool>, IDepending<TIn>
{
	public EvaluateToAny(IContexts contexts, Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
		: this(new Reading<TIn, T>(contexts, (d, @in) => expression.Invoke(d, @in))) {}

	protected EvaluateToAny(IReading<TIn, T> reading) : base(reading, ToAny<T>.Default) {}
}