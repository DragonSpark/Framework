using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Allocated;

public class ReportedAllocating<TIn, TOut> : IAllocating<TIn, TOut>
{
	readonly IAllocating<TIn, TOut> _previous;
	readonly Action<Task<TOut>>     _report;

	public ReportedAllocating(IAllocating<TIn, TOut> previous, Action<Task<TOut>> report)
	{
		_previous = previous;
		_report   = report;
	}

	public Task<TOut> Get(TIn parameter)
	{
		var result = _previous.Get(parameter);
		_report(result);
		return result;
	}
}