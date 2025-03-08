using DragonSpark.Compose;
using DragonSpark.Model.Selection.Stores;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Selection;

public class Storing<TIn, TOut> : ISelecting<TIn, TOut>
{
	readonly ITable<TIn, TOut>          _store;
	readonly Func<TIn, ValueTask<TOut>> _source;

	protected Storing(ITable<TIn, TOut> store, Func<TIn, ValueTask<TOut>> source)
	{
		_store  = store;
		_source = source;
	}

	public async ValueTask<TOut> Get(TIn parameter)
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