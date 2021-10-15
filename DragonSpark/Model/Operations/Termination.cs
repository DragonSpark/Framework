using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

public class Termination<TIn, TNext> : IOperation<TIn>
{
	readonly Await<TIn, TNext> _first;
	readonly Await<TNext>      _second;

	public Termination(ISelecting<TIn, TNext> first, IOperation<TNext> second) : this(first.Await, second.Await) {}

	public Termination(Await<TIn, TNext> first, Await<TNext> second)
	{
		_first  = first;
		_second = second;
	}

	public async ValueTask Get(TIn parameter)
	{
		var first = await _first(parameter);
		await _second(first);
	}
}

public class Termination<T> : IOperation<T>
{
	readonly Await<T> _first;
	readonly Await    _second;

	public Termination(IOperation<T> first, IOperation second) : this(first.Await, second.Await) {}

	public Termination(Await<T> first, Await second)
	{
		_first  = first;
		_second = second;
	}

	public async ValueTask Get(T parameter)
	{
		await _first(parameter);
		await _second();
	}
}