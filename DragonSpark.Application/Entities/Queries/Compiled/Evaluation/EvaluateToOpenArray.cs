using DragonSpark.Model;
using DragonSpark.Model.Operations.Results;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation;

public class EvaluateToOpenArray<T> : EvaluateToOpenArray<None, T>, IResulting<T[]>
{
	protected EvaluateToOpenArray(IContexts contexts, Expression<Func<DbContext, IQueryable<T>>> expression)
		: base(contexts, expression.Then()) {}

	protected EvaluateToOpenArray(IContexts contexts, Expression<Func<DbContext, None, IQueryable<T>>> expression)
		: base(contexts, expression) {}

	public EvaluateToOpenArray(IReading<None, T> reading) : base(reading) {}

	public ValueTask<T[]> Get() => base.Get(None.Default);
}

public class EvaluateToOpenArray<TIn, T> : Evaluate<TIn, T, T[]>
{
	protected EvaluateToOpenArray(IContexts contexts, Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
		: this(new Reading<TIn, T>(contexts, expression)) {}

	protected EvaluateToOpenArray(IReading<TIn, T> reading) : base(reading, ToOpenArray<T>.Default) {}
}