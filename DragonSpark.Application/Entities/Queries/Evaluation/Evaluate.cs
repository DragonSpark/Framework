using DragonSpark.Model;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Evaluation
{
	public class Evaluate<T, TResult> : Evaluate<None, T, TResult>
	{
		public Evaluate(IInvoke<None, T> invoke, IEvaluate<T, TResult> evaluate) : base(invoke, evaluate) {}
	}


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
			await using var invocation = _invoke.Get(parameter);
			var             result     = await _evaluate.Get(invocation.Elements);
			return result;
		}
	}
}