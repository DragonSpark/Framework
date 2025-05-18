using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Selection.Stop;

public class StopAwareCoalesce<TIn, TOut> : IStopAware<TIn, TOut>
{
	readonly Await<Stop<TIn>, TOut?> _first;
	readonly Await<Stop<TIn>, TOut>  _second;

	public StopAwareCoalesce(ISelecting<Stop<TIn>, TOut?> first, ISelecting<Stop<TIn>, TOut> second)
		: this(first.Off, second.Off) {}

	public StopAwareCoalesce(Await<Stop<TIn>, TOut?> first, Await<Stop<TIn>, TOut> second)
	{
		_first  = first;
		_second = second;
	}

	public async ValueTask<TOut> Get(Stop<TIn> parameter) => await _first(parameter) ?? await _second(parameter);
}