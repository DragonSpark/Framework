using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Allocated;

public class AllocatedAppended : IAllocated
{
	readonly Func<Task> _previous, _current;

	public AllocatedAppended(Func<Task> previous, Func<Task> current)
	{
		_previous = previous;
		_current  = current;
	}

	public async Task Get()
	{
		await _previous();
		await _current().ConfigureAwait(false);
	}
}