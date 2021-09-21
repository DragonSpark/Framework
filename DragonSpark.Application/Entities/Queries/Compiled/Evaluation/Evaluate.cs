using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation
{
	public class Evaluate<TIn, T, TResult> : ISelecting<TIn, TResult>
	{
		readonly IInvoke<TIn, T>       _invoke;
		readonly IEvaluate<T, TResult> _evaluate;

		public Evaluate(IInvoke<TIn, T> invoke, IEvaluate<T, TResult> evaluate)
		{
			_invoke   = invoke;
			_evaluate = evaluate;
		}

		public async ValueTask<TResult> Get(TIn parameter)
		{
			using var invocation = await _invoke.Await(parameter);
			var       result     = await _evaluate.Await(invocation.Elements);
			return result;
		}
	}
}