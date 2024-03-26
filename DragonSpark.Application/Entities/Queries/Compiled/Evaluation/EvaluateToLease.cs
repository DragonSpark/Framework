using DragonSpark.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation;

public class EvaluateToLease<T> : EvaluateToLease<None, T>
{
	protected EvaluateToLease(IContexts contexts, Expression<Func<DbContext, IQueryable<T>>> expression)
		: base(contexts, expression.Then()) {}

	protected EvaluateToLease(IContexts contexts,
	                          Expression<Func<DbContext, None, IQueryable<T>>> expression)
		: base(contexts, expression) {}

	protected EvaluateToLease(IReading<None, T> reading) : base(reading) {}
}

public class EvaluateToLease<TIn, T> : Evaluate<TIn, T, DragonSpark.Model.Sequences.Memory.Leasing<T>>
{
	protected EvaluateToLease(IContexts contexts, Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
		: this(new Reading<TIn, T>(contexts, expression)) {}

	protected EvaluateToLease(IReading<TIn, T> reading) : base(reading, ToLease<T>.Default) {}
}