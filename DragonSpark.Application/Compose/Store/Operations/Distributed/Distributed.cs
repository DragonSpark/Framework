using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Compose.Store.Operations.Distributed;

sealed class Distributed<TIn, TOut> : ISelecting<TIn, TOut>
{
	readonly ISelecting<string, TOut?>  _read;
	readonly Func<TIn, string>          _key;
	readonly Await<EntryKey<TIn>, TOut> _store;

	public Distributed(IDistributedCache read, Func<TIn, string> key, Await<EntryKey<TIn>, TOut> store)
		: this(new Read<TOut>(read), key, store) {}

	public Distributed(ISelecting<string, TOut?> read, Func<TIn, string> key, Await<EntryKey<TIn>, TOut> store)
	{
		_read  = read;
		_store = store;
		_key   = key;
	}

	public async ValueTask<TOut> Get(TIn parameter)
	{
		var key      = _key(parameter);
		var existing = await _read.Off(key);
		var result   = existing ?? await _store(new(parameter, key));
		return result;
	}
}