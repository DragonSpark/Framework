using DragonSpark.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation;

public class EvaluateToSingle<T> : EvaluateToSingle<None, T>
{
	protected EvaluateToSingle(IScopes scopes, Expression<Func<DbContext, IQueryable<T>>> expression)
		: base(scopes, expression.Then()) {}

	protected EvaluateToSingle(IScopes scopes, Expression<Func<DbContext, None, IQueryable<T>>> expression)
		: base(scopes, expression) {}

	protected EvaluateToSingle(IReading<None, T> reading) : base(reading) {}
}

public class EvaluateToSingle<TIn, T> : Evaluate<TIn, T, T>
{
	protected EvaluateToSingle(IScopes scopes, Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
		: this(new Reading<TIn, T>(scopes, expression)) {}

	protected EvaluateToSingle(IReading<TIn, T> reading) : base(reading, ToSingle<T>.Default) {}
}