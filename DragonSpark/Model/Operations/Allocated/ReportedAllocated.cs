using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Allocated;

public class ReportedAllocated<T> : IAllocated<T>
{
	readonly ISelect<T, Task> _previous;
	readonly Action<Task>     _report;

	protected ReportedAllocated(ISelect<T, Task> previous, Action<Task> report)
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