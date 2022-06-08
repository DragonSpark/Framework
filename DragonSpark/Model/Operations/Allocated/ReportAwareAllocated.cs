using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Allocated;

public class ReportAwareAllocated<T> : IAllocated<T>
{
	readonly IAllocated<T> _previous;
	readonly Action        _report;

	public ReportAwareAllocated(IAllocated<T> previous, Action report)
	{
		_previous = previous;
		_report   = report;
	}

	public async Task Get(T parameter)
	{
		try
		{
			await _previous.Get(parameter);
		}
		finally
		{
			_report();
		}
	}
}