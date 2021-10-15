using DragonSpark.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation;

public class EvaluateToFirst<T> : EvaluateToFirst<None, T>
{
	protected EvaluateToFirst(IScopes scopes, Expression<Func<DbContext, IQueryable<T>>> expression)
		: base(scopes, expression.Then()) {}

	protected EvaluateToFirst(IScopes scopes, Expression<Func<DbContext, None, IQueryable<T>>> expression)
		: base(scopes, expression) {}

	protected EvaluateToFirst(IReading<None, T> reading) : base(reading) {}
}

public class EvaluateToFirst<TIn, T> : Evaluate<TIn, T, T>
{
	protected EvaluateToFirst(IScopes scopes, Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
		: this(new Reading<TIn, T>(scopes, expression)) {}

	protected EvaluateToFirst(IReading<TIn, T> reading) : base(reading, ToFirst<T>.Default) {}
}