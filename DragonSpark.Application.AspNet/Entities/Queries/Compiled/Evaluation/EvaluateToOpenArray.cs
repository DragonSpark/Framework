using DragonSpark.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.AspNet.Entities.Queries.Compiled.Evaluation;

public class EvaluateToOpenArray<T> : EvaluateToOpenArray<None, T>
{
	protected EvaluateToOpenArray(IScopes scopes, Expression<Func<DbContext, IQueryable<T>>> expression)
		: base(scopes, expression.Then()) {}

	protected EvaluateToOpenArray(IScopes scopes, Expression<Func<DbContext, None, IQueryable<T>>> expression)
		: base(scopes, expression) {}

	public EvaluateToOpenArray(IReading<None, T> reading) : base(reading) {}
}

public class EvaluateToOpenArray<TIn, T> : Evaluate<TIn, T, T[]>
{
	protected EvaluateToOpenArray(IScopes scopes, Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
		: this(new Reading<TIn, T>(scopes, expression)) {}

	protected EvaluateToOpenArray(IReading<TIn, T> reading) : base(reading, ToOpenArray<T>.Default) {}
}