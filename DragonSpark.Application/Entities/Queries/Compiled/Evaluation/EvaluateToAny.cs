using DragonSpark.Model;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation;

public class EvaluateToAny<T> : EvaluateToAny<None, T>
{
	public EvaluateToAny(IScopes scopes, Expression<Func<DbContext, IQueryable<T>>> expression)
		: base(scopes, expression.Then()) {}

	public EvaluateToAny(IScopes scopes, Expression<Func<DbContext, None, IQueryable<T>>> expression)
		: base(scopes, expression) {}

	public EvaluateToAny(IReading<None, T> reading) : base(reading) {}
}

public class EvaluateToAny<TIn, T> : Evaluate<TIn, T, bool>, IDepending<TIn>
{
	protected EvaluateToAny(IScopes scopes, Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
		: this(new Reading<TIn, T>(scopes, expression)) {}

	protected EvaluateToAny(IReading<TIn, T> reading) : base(reading, ToAny<T>.Default) {}
}