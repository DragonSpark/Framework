using DragonSpark.Model;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation;

public class EvaluateToSingle<T> : EvaluateToSingle<None, T>
{
	public EvaluateToSingle(IContexts contexts, Expression<Func<DbContext, IQueryable<T>>> expression)
		: base(contexts, expression.Then()) {}

	public EvaluateToSingle(IContexts contexts, Expression<Func<DbContext, None, IQueryable<T>>> expression)
		: base(contexts, expression) {}

	protected EvaluateToSingle(IReading<None, T> reading) : base(reading) {}
}

public class EvaluateToSingle<TIn, T> : Evaluate<TIn, T, T>
{
	public EvaluateToSingle(IContexts contexts, Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
		: this(new Reading<TIn, T>(contexts, (d, @in) => expression.Invoke(d, @in))) {}

	protected EvaluateToSingle(IReading<TIn, T> reading) : base(reading, ToSingle<T>.Default) {}
}