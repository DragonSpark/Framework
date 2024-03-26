using DragonSpark.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation;

public class EvaluateToCount<T> : EvaluateToCount<None, T>
{
	protected EvaluateToCount(IContexts contexts, Expression<Func<DbContext, IQueryable<T>>> expression)
		: base(contexts, expression.Then()) {}

	protected EvaluateToCount(IContexts contexts, Expression<Func<DbContext, None, IQueryable<T>>> expression)
		: base(contexts, expression) {}

	public EvaluateToCount(IReading<None, T> reading) : base(reading) {}
}

public class EvaluateToCount<TIn, T> : Evaluate<TIn, T, uint>
{
	protected EvaluateToCount(IContexts contexts, Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
		: this(new Reading<TIn, T>(contexts, expression)) {}

	protected EvaluateToCount(IReading<TIn, T> reading) : base(reading, ToCount<T>.Default) {}
}