using DragonSpark.Model;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation;

public class EvaluateToFirst<T> : EvaluateToFirst<None, T>
{
	protected EvaluateToFirst(IContexts contexts, Expression<Func<DbContext, IQueryable<T>>> expression)
		: base(contexts, expression.Then()) {}

	protected EvaluateToFirst(IContexts contexts, Expression<Func<DbContext, None, IQueryable<T>>> expression)
		: base(contexts, expression) {}

	protected EvaluateToFirst(IReading<None, T> reading) : base(reading) {}
}

public class EvaluateToFirst<TIn, T> : Evaluate<TIn, T, T>
{
	public EvaluateToFirst(IContexts contexts, Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
		: this(new Reading<TIn, T>(contexts, (d, @in) => expression.Invoke(d, @in))) {}

	protected EvaluateToFirst(IReading<TIn, T> reading) : base(reading, ToFirst<T>.Default) {}
}