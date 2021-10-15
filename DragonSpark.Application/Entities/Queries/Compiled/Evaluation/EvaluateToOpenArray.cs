using DragonSpark.Model;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation;

internal class EvaluateToOpenArray
{
}

public class EvaluateToOpenArray<T> : EvaluateToOpenArray<None, T>, IResulting<T[]>
{
	public EvaluateToOpenArray(IScopes scopes, Expression<Func<DbContext, IQueryable<T>>> expression)
		: base(scopes, expression.Then()) {}

	public EvaluateToOpenArray(IScopes scopes, Expression<Func<DbContext, None, IQueryable<T>>> expression)
		: base(scopes, expression) {}

	public EvaluateToOpenArray(IReading<None, T> reading) : base(reading) {}

	public ValueTask<T[]> Get() => base.Get(None.Default);
}

public class EvaluateToOpenArray<TIn, T> : Evaluate<TIn, T, T[]>
{
	public EvaluateToOpenArray(IScopes scopes, Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
		: this(new Reading<TIn, T>(scopes, expression)) {}

	protected EvaluateToOpenArray(IReading<TIn, T> reading) : base(reading, ToOpenArray<T>.Default) {}
}