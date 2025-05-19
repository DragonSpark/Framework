using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Stop;

public class StopAwareAppending<T> : IStopAware<T>
{
	readonly Await<Stop<T>> _first, _second;

	public StopAwareAppending(IStopAware<T> first, IStopAware<T> second) : this(first.Off, second.Off) {}

	public StopAwareAppending(Await<Stop<T>> first, Await<Stop<T>> second)
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