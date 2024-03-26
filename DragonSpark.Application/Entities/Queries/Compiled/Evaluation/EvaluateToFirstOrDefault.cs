using DragonSpark.Model;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation;

public class EvaluateToFirstOrDefault<T> : EvaluateToFirstOrDefault<None, T>
{
	protected EvaluateToFirstOrDefault(IContexts contexts,
	                                   Expression<Func<DbContext, IQueryable<T>>> expression)
		: base(contexts, expression.Then()) {}

	protected EvaluateToFirstOrDefault(IContexts contexts,
	                                   Expression<Func<DbContext, None, IQueryable<T>>> expression)
		: base(contexts, expression) {}

	protected EvaluateToFirstOrDefault(IReading<None, T> reading) : base(reading) {}
}

public class EvaluateToFirstOrDefault<TIn, T> : Evaluate<TIn, T, T?>
{
	public EvaluateToFirstOrDefault(IContexts contexts, Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
		: this(new Reading<TIn, T>(contexts, (d, @in) => expression.Invoke(d, @in))) {}

	protected EvaluateToFirstOrDefault(IReading<TIn, T> reading) : base(reading, ToFirstOrDefault<T>.Default) {}
}