using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Model.Operations.Selection;

public class Reporting<TIn, TOut> : IStopAware<TIn, TOut>
{
	readonly IStopAware<TIn, TOut> _previous;
	readonly Action<Task<TOut>>    _report;

	protected Reporting(IStopAware<TIn, TOut> previous, Action<Task<TOut>> report)
	{
		_previous = previous;
		_report   = report;
	}

	public ValueTask<TOut> Get(Stop<TIn> parameter)
	{
		var result = _previous.Allocate(parameter);
		_report(result);
		return result.ToOperation();
	}
}