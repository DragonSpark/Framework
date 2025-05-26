using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Stop;

public class Termination<T> : IStopAware<T>
{
	readonly Await<Stop<T>>           _first;
	readonly Await<CancellationToken> _second;

	public Termination(IOperation<Stop<T>> first, IOperation<CancellationToken> second) : this(first.Off, second.Off) {}

	public Termination(Await<Stop<T>> first, Await<CancellationToken> second)
	{
		_first  = first;
		_second = second;
	}

	public async ValueTask Get(Stop<T> parameter)
	{
		await _first(parameter);
		await _second(parameter);
	}
}

public class Termination<TIn, TNext> : IStopAware<TIn>
{
	readonly Await<Stop<TIn>, TNext> _first;
	readonly Await<Stop<TNext>>      _second;

	public Termination(ISelecting<Stop<TIn>, TNext> first, IOperation<Stop<TNext>> second)
		: this(first.Off, second.Off) {}

	public Termination(Await<Stop<TIn>, TNext> first, Await<Stop<TNext>> second)
	{
		_first  = first;
		_second = second;
	}

	public async ValueTask Get(Stop<TIn> parameter)
	{
		var first = await _first(parameter);
		await _second(new(first, parameter));
	}
}