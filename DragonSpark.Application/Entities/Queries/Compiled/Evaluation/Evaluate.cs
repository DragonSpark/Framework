using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation;

public class Evaluate<TIn, T, TResult> : ISelecting<TIn, TResult>
{
	readonly IReading<TIn, T>      _reading;
	readonly IEvaluate<T, TResult> _evaluate;

	public Evaluate(IReading<TIn, T> reading, IEvaluate<T, TResult> evaluate)
	{
		_reading  = reading;
		_evaluate = evaluate;
	}

	public async ValueTask<TResult> Get(TIn parameter)
	{
		using var invocation = await _reading.Await(parameter);
		var       result     = await _evaluate.Await(invocation.Elements);
		return result;
	}
}