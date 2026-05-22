using DragonSpark.Model.Operations.Allocated.Stop;
using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Stop;

public class Reported<T> : IAllocated<T>
{
	readonly ISelect<Stop<T>, Task> _previous;
	readonly Action<Task>           _report;

	protected Reported(ISelect<Stop<T>, Task> previous, Action<Task> report)
	{
		_previous = previous;
		_report   = report;
	}

	public Task Get(Stop<T> parameter)
	{
		var result = _previous.Get(parameter);
		_report(result);
		return result;
	}
}