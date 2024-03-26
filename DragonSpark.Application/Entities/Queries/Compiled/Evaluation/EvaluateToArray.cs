using DragonSpark.Model;
using DragonSpark.Model.Operations.Results;
using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation;

public class EvaluateToArray<T> : EvaluateToArray<None, T>, IResulting<Array<T>>
{
	public EvaluateToArray(IContexts contexts, Expression<Func<DbContext, IQueryable<T>>> expression)
		: base(contexts, expression.Then()) {}

	public EvaluateToArray(IContexts contexts, Expression<Func<DbContext, None, IQueryable<T>>> expression)
		: base(contexts, expression) {}

	public EvaluateToArray(IReading<None, T> reading) : base(reading) {}

	public ValueTask<Array<T>> Get() => base.Get(None.Default);
}

public class EvaluateToArray<TIn, T> : Evaluate<TIn, T, Array<T>>
{
	public EvaluateToArray(IContexts contexts, Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
		: this(new Reading<TIn, T>(contexts, expression)) {}

	protected EvaluateToArray(IReading<TIn, T> reading) : base(reading, ToArray<T>.Default) {}
}