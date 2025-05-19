using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Queries.Compiled.Evaluation;

sealed class EvaluateDispatch<TIn, T, TResult> : IStopAware<TIn, TResult>
{
	readonly IReading<TIn, T>      _reading;
	readonly IEvaluate<T, TResult> _evaluate;

	public EvaluateDispatch(IReading<TIn, T> reading, IEvaluate<T, TResult> evaluate)
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