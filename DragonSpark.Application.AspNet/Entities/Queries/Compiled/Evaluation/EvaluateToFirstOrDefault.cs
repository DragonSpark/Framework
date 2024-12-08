using DragonSpark.Model;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.AspNet.Entities.Queries.Compiled.Evaluation;

public class EvaluateToFirstOrDefault<T> : EvaluateToFirstOrDefault<None, T>
{
	protected EvaluateToFirstOrDefault(IScopes scopes,
	                                   Expression<Func<DbContext, IQueryable<T>>> expression)
		: base(scopes, expression.Then()) {}

	protected EvaluateToFirstOrDefault(IScopes scopes,
	                                   Expression<Func<DbContext, None, IQueryable<T>>> expression)
		: base(scopes, expression) {}

	protected EvaluateToFirstOrDefault(IReading<None, T> reading) : base(reading) {}
}

public class EvaluateToFirstOrDefault<TIn, T> : Evaluate<TIn, T, T?>
{
	public EvaluateToFirstOrDefault(IScopes scopes, Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
		: this(new Reading<TIn, T>(scopes, (d, @in) => expression.Invoke(d, @in).Take(1))) {}

	protected EvaluateToFirstOrDefault(IReading<TIn, T> reading) : base(reading, ToFirstOrDefault<T>.Default) {}
}