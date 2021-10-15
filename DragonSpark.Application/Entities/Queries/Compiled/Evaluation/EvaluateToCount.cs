using DragonSpark.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation;

public class EvaluateToCount<T> : EvaluateToCount<None, T>
{
	public EvaluateToCount(IScopes scopes, Expression<Func<DbContext, IQueryable<T>>> expression)
		: base(scopes, expression.Then()) {}

	public EvaluateToCount(IScopes scopes, Expression<Func<DbContext, None, IQueryable<T>>> expression)
		: base(scopes, expression) {}

	public EvaluateToCount(IReading<None, T> reading) : base(reading) {}
}

public class EvaluateToCount<TIn, T> : Evaluate<TIn, T, uint>
{
	public EvaluateToCount(IScopes scopes, Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
		: this(new Reading<TIn, T>(scopes, expression)) {}

	public EvaluateToCount(IReading<TIn, T> reading) : base(reading, ToCount<T>.Default) {}
}