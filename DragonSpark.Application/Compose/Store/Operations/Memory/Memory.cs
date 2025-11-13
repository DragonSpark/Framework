using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Compose.Store.Operations.Memory;

sealed class Memory<TIn, TOut> : ISelecting<TIn, TOut>
{
	readonly Getting                        _getting;
	readonly Await<EntryKey<TIn>, TOut> _store;
	readonly Func<TIn, object>          _key;

	public Memory(IMemoryCache memory, Await<EntryKey<TIn>, TOut> store, Func<TIn, object> key)
		: this(memory.TryGetValue, key, store) {}

	public Memory(Getting getting, Func<TIn, object> key, Await<EntryKey<TIn>, TOut> store)
	{
		_getting   = getting;
		_store = store;
		_key   = key;
	}

	public async ValueTask<TOut> Get(TIn parameter)
	{
		var key = _key(parameter);
		var result = _getting(key, out var stored) && stored is not null
			             ? stored.To<TOut>()
			             : await _store(new(parameter, key));
		return result;
	}
}