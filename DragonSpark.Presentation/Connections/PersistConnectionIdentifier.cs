using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Components;
using System;

namespace DragonSpark.Presentation.Connections;

sealed class PersistConnectionIdentifier : ICommand<Guid>, IDisposable
{
	readonly IMutable<PersistingComponentStateSubscription?> _store;
	readonly PersistentComponentState                        _state;
	readonly string                                          _key;

	public PersistConnectionIdentifier(PersistentComponentState state)
		: this(new Variable<PersistingComponentStateSubscription?>(), state, ConnectionSessionKey.Default) {}

	public PersistConnectionIdentifier(IMutable<PersistingComponentStateSubscription?> store,
	                                   PersistentComponentState state, string key)
	{
		_store = store;
		_state = state;
		_key   = key;
	}

	public void Dispose()
	{
		_store.Get()?.Dispose();
	}

	public void Execute(Guid parameter)
	{
		var store = _store.Get();
		if (store is not null)
		{
			throw new InvalidOperationException("The ConnectionSession Identifier has already been set!");
		}

		var assignment = _state.RegisterOnPersisting(new Assignment<Guid>(parameter, _state, _key).Get);
		_store.Execute(assignment);
	}
}