using Microsoft.AspNetCore.OutputCaching;

namespace DragonSpark.Server.Output.Compose;

public sealed class StartRegistration<T> where T : notnull
{
	readonly IOutputCacheStore _store;
	readonly IOutputKey[]      _keys;

	public StartRegistration(IOutputCacheStore store, IOutputKey[] keys)
	{
		_store = store;
		_keys  = keys;
	}

	public SelectedRegistration<T, TTo> Using<TTo>(Func<T, TTo> select) => new(_store, _keys, select);
}