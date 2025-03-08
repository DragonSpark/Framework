using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Allocated;

public class Termination<TIn, TNext> : IAllocated<TIn>
{
	readonly Await<TIn, TNext> _first;
	readonly Await<TNext>      _second;

	public Termination(IAllocating<TIn, TNext> first, IAllocated<TNext> second) : this(first.Off, second.Off) {}

	public Termination(Await<TIn, TNext> first, Await<TNext> second)
	{
		_first  = first;
		_second = second;
	}

	public async Task Get(TIn parameter)
	{
		var first = await _first(parameter);
		await _second(first);
	}
}

public class Termination<T> : IAllocated<T>
{
	readonly Await<T> _first;
	readonly Await    _second;

	public Termination(IAllocated<T> first, IAllocated second) : this(first.Off, second.Off) {}

	public Termination(Await<T> first, Await second)
	{
		_first  = first;
		_second = second;
	}

	public async Task Get(T parameter)
	{
		await _first(parameter);
		await _second();
	}
}