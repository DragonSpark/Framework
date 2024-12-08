using DragonSpark.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.AspNet.Entities.Queries.Compiled.Evaluation;

public class EvaluateToCount<T> : EvaluateToCount<None, T>
{
	protected EvaluateToCount(IScopes scopes, Expression<Func<DbContext, IQueryable<T>>> expression)
		: base(scopes, expression.Then()) {}

	protected EvaluateToCount(IScopes scopes, Expression<Func<DbContext, None, IQueryable<T>>> expression)
		: base(scopes, expression) {}

	public EvaluateToCount(IReading<None, T> reading) : base(reading) {}
}

public class EvaluateToCount<TIn, T> : Evaluate<TIn, T, uint>
{
	protected EvaluateToCount(IScopes scopes, Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
		: this(new Reading<TIn, T>(scopes, expression)) {}

	protected EvaluateToCount(IReading<TIn, T> reading) : base(reading, ToCount<T>.Default) {}
}