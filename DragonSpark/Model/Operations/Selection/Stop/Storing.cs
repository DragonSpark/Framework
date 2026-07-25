using DragonSpark.Compose;
using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Model.Operations.Selection.Stop;

public class Storing<TIn, TOut> : IStopAware<TIn, TOut>
{
	readonly ITable<TIn, TOut>                _store;
	readonly Func<Stop<TIn>, ValueTask<TOut>> _source;

	protected Storing(ITable<TIn, TOut> store, Func<Stop<TIn>, ValueTask<TOut>> source)
	{
		_store  = store;
		_source = source;
	}

	public async ValueTask<TOut> Get(Stop<TIn> parameter)
	{
		if (_store.TryGet(parameter, out var result))
		{
			return result;
		}

		var source = await _source(parameter).Off();
		_store.Assign(parameter, source);
		return source;
	}
}