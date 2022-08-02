using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Components;
using System;

namespace DragonSpark.Presentation.Components.State.Persistence;

public class Persist<T> : ICommand<T>, IResult<Pop<T>>, IDisposable
{
	readonly IMutable<PersistingComponentStateSubscription?> _store;
	readonly IMutable<Pop<T>?>                               _result;
	readonly ISubscription<T>                                _subscription;

	protected Persist(PersistentComponentState state, Type key) : this(state, key.AssemblyQualifiedName.Verify()) {}

	protected Persist(PersistentComponentState state, string key) : this(new Subscription<T>(state, key)) {}

	protected Persist(ISubscription<T> persist)
		: this(new Variable<PersistingComponentStateSubscription?>(), new Variable<Pop<T>?>(), persist) {}

	protected Persist(IMutable<PersistingComponentStateSubscription?> store, IMutable<Pop<T>?> result,
	                  ISubscription<T> subscription)
	{
		_store        = store;
		_result       = result;
		_subscription = subscription;
	}

	public void Execute(T parameter)
	{
		var store = _store.Get();
		if (store is not null)
		{
			throw new InvalidOperationException($"The {GetType().Namespace}'s value has already been set!");
		}

		var assignment = _subscription.Get(parameter);
		_store.Execute(assignment);
	}

	public void Dispose()
	{
		_store.Get()?.Dispose();
	}

	public Pop<T> Get()
	{
		var stored = _result.Get();
		if (stored is null)
		{
			var pop = _subscription.Get();
			_result.Execute(pop);
			return pop;
		}

		return stored.Value;
	}
}