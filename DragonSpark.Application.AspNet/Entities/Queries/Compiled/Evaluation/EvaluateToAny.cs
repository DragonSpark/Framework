using System.Linq.Expressions;
using DragonSpark.Model;
using DragonSpark.Model.Operations.Selection.Stop.Conditions;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Queries.Compiled.Evaluation;

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
	public EvaluateToAny(IScopes scopes, Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
		: this(new Reading<TIn, T>(scopes, (d, @in) => expression.Invoke(d, @in).Take(1))) {}

	protected EvaluateToAny(IReading<TIn, T> reading) : base(reading, ToAny<T>.Default) {}
}