using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Queries.Compiled.Evaluation;

public class Evaluate<TIn, T, TResult> : StopAdaptor<TIn, TResult>
{
	public Evaluate(IReading<TIn, T> reading, IEvaluate<T, TResult> evaluate)
		: base(new StopAwareEvaluate<TIn, T, TResult>(reading, evaluate)) {}
}

// TODO

public class StopAwareEvaluate<TIn, T, TResult> : IStopAware<TIn, TResult>
{
	readonly IReading<TIn, T>      _reading;
	readonly IEvaluate<T, TResult> _evaluate;

	public StopAwareEvaluate(IReading<TIn, T> reading, IEvaluate<T, TResult> evaluate)
	{
		_reading  = reading;
		_evaluate = evaluate;
	}

	public async ValueTask<TResult> Get(Stop<TIn> parameter)
	{
		var (subject, token) = parameter;
		using var invocation = _reading.Get(subject);
		var       result     = await _evaluate.Off(new Stop<IAsyncEnumerable<T>>(invocation.Elements, token));
		return result;
	}
}