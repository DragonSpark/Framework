using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

public class Coalesce<TIn, TOut> : ISelecting<TIn, TOut>
{
	readonly Await<TIn, TOut?> _first;
	readonly Await<TIn, TOut>  _second;

	public Coalesce(ISelecting<TIn, TOut?> first, ISelecting<TIn, TOut> second) : this(first.Await, second.Await) {}

	public Coalesce(Await<TIn, TOut?> first, Await<TIn, TOut> second)
	{
		_first  = first;
		_second = second;
	}

	public async ValueTask<TOut> Get(TIn parameter) => await _first(parameter) ?? await _second(parameter);
}

public class Coalesce<T> : IResulting<T>
{
	readonly IResulting<T?> _first;
	readonly IResulting<T>  _second;

	protected Coalesce(IResulting<T?> first, IResulting<T> second)
	{
		_first  = first;
		_second = second;
	}

	public async ValueTask<T> Get() => await _first.Await() ?? await _second.Await();
}