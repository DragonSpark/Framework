using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences.Memory;
using Microsoft.AspNetCore.OutputCaching;

namespace DragonSpark.Server.Output.Compose;

public sealed class SelectedRegistration<T, TTo> where T : notnull
{
	readonly IOutputCacheStore   _store;
	readonly IOutputKey[]        _keys;
	readonly Func<T, TTo>        _select;
	readonly List<IRegistration> _registrations;

	public SelectedRegistration(IOutputCacheStore store, IOutputKey[] keys, Func<T, TTo> select)
		: this(store, keys, select, []) {}

	// ReSharper disable once TooManyDependencies
	public SelectedRegistration(IOutputCacheStore store, IOutputKey[] keys, Func<T, TTo> select,
	                            List<IRegistration> registrations)
	{
		_store         = store;
		_keys          = keys;
		_select        = select;
		_registrations = registrations;
	}

	public IStopAware<T> Build() => new Evict<T>(_store, new RegistrationAwareTags(_registrations), _keys);

	public SelectedRegistration<T, TTo> Register()
	{
		_registrations.Add(new Registration<T, TTo>(_select));
		return this;
	}

	public SelectedRegistration<T, TTo> Many<TOut>(IStopAware<TTo, TOut> select, ISelect<TOut, IEnumerable<TTo>> keys)
		=> Many(select, keys.Get);

	public SelectedRegistration<T, TTo> Many<TOut>(IStopAware<TTo, Leasing<TOut>> select,
	                                               ISelect<TOut, IEnumerable<TTo>> keys)
		=> Many(select, keys.Get);

	public SelectedRegistration<T, TTo> Many<TOut>(IStopAware<TTo, Leasing<TOut>> select,
	                                               Func<TOut, IEnumerable<TTo>> keys)
	{
		_registrations.Add(new ManyRegistrations<T, TOut, TTo>(_select, select, keys));
		return this;
	}

	public SelectedRegistration<T, TTo> Many<TOut>(IStopAware<TTo, TOut> select, Func<TOut, IEnumerable<TTo>> keys)
	{
		_registrations.Add(new Registrations<T, TOut, TTo>(_select, select, keys));
		return this;
	}

	public SelectedRegistration<T, TTo> Single(IStopAware<TTo, TTo> select)
	{
		_registrations.Add(new Single<T, TTo>(_select, select));
		return this;
	}
}