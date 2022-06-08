using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Allocated;

public class ReportedAllocated<T> : IAllocated<T>
{
	readonly IAllocated<T> _previous;
	readonly Action<Task>  _report;

	protected ReportedAllocated(IAllocated<T> previous, Action<Task> report)
	{
		_previous = previous;
		_report   = report;
	}

	public Task Get(T parameter)
	{
		var result = _previous.Get(parameter);
		_report(result);
		return result;
	}
}