using DragonSpark.Compose;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

public class Reporting<TIn, TOut> : ISelecting<TIn, TOut>
{
	readonly ISelecting<TIn, TOut> _previous;
	readonly Action<Task<TOut>>    _report;

	protected Reporting(ISelecting<TIn, TOut> previous, Action<Task<TOut>> report)
	{
		_previous = previous;
		_report   = report;
	}

	public ValueTask<TOut> Get(TIn parameter)
	{
		var result = _previous.Get(parameter).AsTask();
		_report(result);
		return result.ToOperation();
	}
}