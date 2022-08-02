using DragonSpark.Compose;
using DragonSpark.Model.Operations.Allocated;
using Microsoft.AspNetCore.Components;
using System;

namespace DragonSpark.Presentation.Components.State.Persistence;

sealed class Subscription<T> : ISubscription<T>
{
	readonly PersistentComponentState _state;
	readonly IAllocated<T>            _persist;
	readonly string                   _key;

	public Subscription(PersistentComponentState state, Type key) : this(state, key.AssemblyQualifiedName.Verify()) {}

	public Subscription(PersistentComponentState state, string key) : this(state, new Assign<T>(state, key), key) {}

	public Subscription(PersistentComponentState state, IAllocated<T> persist, string key)
	{
		_state   = state;
		_persist = persist;
		_key     = key;
	}

	public PersistingComponentStateSubscription Get(T parameter)
		=> _state.RegisterOnPersisting(_persist.Then().Bind(parameter));

	public Pop<T> Get()
	{
		var take = _state.TryTakeFromJson<T>(_key, out var stored);
		return new(take, stored);
	}
}