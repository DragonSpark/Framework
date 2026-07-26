using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences.Memory;
using Microsoft.AspNetCore.OutputCaching;

namespace DragonSpark.Server.Output.Compose;

public sealed class SelectedRegistration<T, TTo> where T : notnull
{
	readonly IOutputCacheStore _store;
	readonly IOutputKey[]      _keys;
	readonly Func<T, TTo>      _select;

	public SelectedRegistration(IOutputCacheStore store, IOutputKey[] keys, Func<T, TTo> select)
	{
		_store  = store;
		_keys   = keys;
		_select = select;
	}

	public IStopAware<T> Build()
		=> new Evict<T>(_store, new RegistrationAwareTags(new Registration<T, TTo>(_select)), _keys);

	public IStopAware<T> Many<TOut>(IStopAware<TTo, TOut> select, ISelect<TOut, IEnumerable<TTo>> keys)
		=> Many(select, keys.Get);

	public IStopAware<T> Many<TOut>(IStopAware<TTo, Leasing<TOut>> select, ISelect<TOut, IEnumerable<TTo>> keys)
		=> Many(select, keys.Get);

	public IStopAware<T> Many<TOut>(IStopAware<TTo, Leasing<TOut>> select, Func<TOut, IEnumerable<TTo>> keys)
		=> new Evict<T>(_store, new RegistrationAwareTags(new ManyRegistrations<T, TOut, TTo>(_select, select, keys)),
		                _keys);
	public IStopAware<T> Many<TOut>(IStopAware<TTo, TOut> select, Func<TOut, IEnumerable<TTo>> keys)
		=> new Evict<T>(_store, new RegistrationAwareTags(new Registrations<T, TOut, TTo>(_select, select, keys)),
		                _keys);

	public IStopAware<T> Single(IStopAware<TTo, TTo> select)
		=> new Evict<T>(_store, new RegistrationAwareTags(new Single<T, TTo>(_select, select)), _keys);
}