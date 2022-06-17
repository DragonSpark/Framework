using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Results;
using DragonSpark.Presentation.Components.Content.Rendering;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Connections;

sealed class InitializeConnection : IInitializeConnection
{
	readonly IConnectionIdentifier    _identifier;

	public InitializeConnection(IConnectionIdentifier identifier) => _identifier = identifier;

	public void Execute(HttpContext parameter)
	{
		_identifier.Get();
	}
}

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

sealed class Assignment<T> : IAllocated
{
	readonly T                        _instance;
	readonly PersistentComponentState _state;
	readonly string                   _key;

	public Assignment(T instance, PersistentComponentState state, string key)
	{
		_instance = instance;
		_state    = state;
		_key      = key;
	}

	public Task Get()
	{
		_state.PersistAsJson(_key, _instance);
		return Task.CompletedTask;
	}
}

sealed class ConnectionSessionKey : DragonSpark.Text.Text
{
	public static ConnectionSessionKey Default { get; } = new();

	ConnectionSessionKey() : base(A.Type<ConnectionSessionKey>().AssemblyQualifiedName.Verify()) {}
}

sealed class PersistedConnectionIdentifier : IResult<Guid?>
{
	readonly PersistentComponentState _state;
	readonly SessionRenderState       _session;
	readonly string                   _key;

	public PersistedConnectionIdentifier(PersistentComponentState state, SessionRenderState session)
		: this(state, session, ConnectionSessionKey.Default) {}

	public PersistedConnectionIdentifier(PersistentComponentState state, SessionRenderState session, string key)
	{
		_state   = state;
		_session = session;
		_key     = key;
	}

	public Guid? Get()
	{
		if (_state.TryTakeFromJson<Guid>(_key, out var restored))
		{
			_session.Execute(RenderState.Ready);
			return restored;
		}

		return null;
	}
}

sealed class SetConnectionIdentifier : IResult<Guid>
{
	readonly SessionIdentifier           _identifier;
	readonly PersistConnectionIdentifier _persist;

	public SetConnectionIdentifier(SessionIdentifier identifier, PersistConnectionIdentifier persist)
	{
		_identifier = identifier;
		_persist    = persist;
	}

	public Guid Get()
	{
		var result = _identifier.Get();
		_persist.Execute(result);
		return result;
	}
}

public interface IConnectionIdentifier : IResult<Guid> {}

sealed class ConnectionIdentifier : StoredStructure<Guid>, IConnectionIdentifier
{
	public ConnectionIdentifier(DetermineConnectionIdentifier result) : base(result) {}
}

sealed class DetermineConnectionIdentifier : CoalesceStructure<Guid>
{
	public DetermineConnectionIdentifier(PersistedConnectionIdentifier persisted, SetConnectionIdentifier set)
		: base(persisted, set) {}
}

sealed class SessionIdentifier : Instance<Guid>
{
	public SessionIdentifier() : base(Guid.NewGuid()) {}
}