using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Server.Output;

public class ConditionalOutputsAware<TIn, T> : IStopAware<TIn, T>
{
	readonly IStopAware<TIn, T> _previous;
	readonly Func<T, bool>      _when;
	readonly IStopAware<TIn>    _evict;

	protected ConditionalOutputsAware(IStopAware<TIn, T> previous, Func<T, bool> when, IStopAware<TIn> evict)
	{
		_previous = previous;
		_when     = when;
		_evict    = evict;
	}

	public async ValueTask<T> Get(Stop<TIn> parameter)
	{
		var result = await _previous.Off(parameter);
		if (_when(result))
		{
			await _evict.Off(parameter);
		}

		return result;
	}
}