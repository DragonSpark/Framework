using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

public class Appending : IOperation
{
	readonly Await _previous, _current;

	public Appending(IOperation first, IOperation second) : this(first.Await, second.Await) {}

	public Appending(Await previous, Await current)
	{
		_previous = previous;
		_current  = current;
	}

	public async ValueTask Get()
	{
		await _previous();
		await _current();
	}
}

public class Appending<T> : IOperation<T>
{
	readonly Await<T> _first;
	readonly Await<T> _second;

	public Appending(IOperation<T> first, IOperation<T> second) : this(first.Await, second.Await) {}

	public Appending(Await<T> first, Await<T> second)
	{
		_first  = first;
		_second = second;
	}

	public async ValueTask Get(T parameter)
	{
		await _first(parameter);
		await _second(parameter);
	}
}