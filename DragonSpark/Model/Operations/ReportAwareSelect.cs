using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

public class ReportAwareSelect<TIn, TOut> : ISelecting<TIn, TOut>
{
	readonly ISelecting<TIn, TOut> _previous;
	readonly Action                _report;

	public ReportAwareSelect(ISelecting<TIn, TOut> previous, Action report)
	{
		_previous = previous;
		_report   = report;
	}

	public ValueTask<TOut> Get(TIn parameter)
	{
		try
		{
			return _previous.Get(parameter);
		}
		finally
		{
			_report();
		}
	}
}