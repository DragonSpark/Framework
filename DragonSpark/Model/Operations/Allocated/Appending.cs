using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Allocated;

public class Appending : IAllocated
{
	readonly Await _previous, _current;

	public Appending(IAllocated first, IAllocated second) : this(first.Await, second.Await) {}

	public Appending(Await previous, Await current)
	{
		_previous = previous;
		_current  = current;
	}

	public async Task Get()
	{
		await _previous();
		await _current();
	}
}

public class Appending<T> : IAllocated<T>
{
	readonly Await<T> _first;
	readonly Await<T> _second;

	public Appending(IAllocated<T> first, IAllocated<T> second) : this(first.Await, second.Await) {}

	public Appending(Await<T> first, Await<T> second)
	{
		_first  = first;
		_second = second;
	}

	public async Task Get(T parameter)
	{
		await _first(parameter);
		await _second(parameter);
	}
}