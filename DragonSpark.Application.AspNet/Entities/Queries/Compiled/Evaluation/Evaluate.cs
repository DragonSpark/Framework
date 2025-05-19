using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.AspNet.Entities.Queries.Compiled.Evaluation;

public class Evaluate<TIn, T, TResult> : StopAdaptor<TIn, TResult>
{
	public Evaluate(IReading<TIn, T> reading, IEvaluate<T, TResult> evaluate)
		: base(new EvaluateDispatch<TIn, T, TResult>(reading, evaluate)) {}
}