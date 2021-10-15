using DragonSpark.Model;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation;

public class EvaluateToArray<T> : EvaluateToArray<None, T>, IResulting<Array<T>>
{
	public EvaluateToArray(IScopes scopes, Expression<Func<DbContext, IQueryable<T>>> expression)
		: base(scopes, expression.Then()) {}

	public EvaluateToArray(IScopes scopes, Expression<Func<DbContext, None, IQueryable<T>>> expression)
		: base(scopes, expression) {}

	public EvaluateToArray(IReading<None, T> reading) : base(reading) {}

	public ValueTask<Array<T>> Get() => base.Get(None.Default);
}

public class EvaluateToArray<TIn, T> : Evaluate<TIn, T, Array<T>>
{
	public EvaluateToArray(IScopes scopes, Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
		: this(new Reading<TIn, T>(scopes, expression)) {}

	protected EvaluateToArray(IReading<TIn, T> reading) : base(reading, ToArray<T>.Default) {}
}