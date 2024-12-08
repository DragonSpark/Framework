using DragonSpark.Model;
using DragonSpark.Model.Operations.Results;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Queries.Compiled.Evaluation;

public class EvaluateToOpenArray<T> : EvaluateToOpenArray<None, T>, IResulting<T[]>
{
	protected EvaluateToOpenArray(IScopes scopes, Expression<Func<DbContext, IQueryable<T>>> expression)
		: base(scopes, expression.Then()) {}

	protected EvaluateToOpenArray(IScopes scopes, Expression<Func<DbContext, None, IQueryable<T>>> expression)
		: base(scopes, expression) {}

	public EvaluateToOpenArray(IReading<None, T> reading) : base(reading) {}

	public ValueTask<T[]> Get() => base.Get(None.Default);
}

public class EvaluateToOpenArray<TIn, T> : Evaluate<TIn, T, T[]>
{
	protected EvaluateToOpenArray(IScopes scopes, Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
		: this(new Reading<TIn, T>(scopes, expression)) {}

	protected EvaluateToOpenArray(IReading<TIn, T> reading) : base(reading, ToOpenArray<T>.Default) {}
}