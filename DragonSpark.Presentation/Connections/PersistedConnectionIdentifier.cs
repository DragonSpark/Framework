using DragonSpark.Model.Results;
using DragonSpark.Presentation.Components.Content.Rendering;
using Microsoft.AspNetCore.Components;
using System;

namespace DragonSpark.Presentation.Connections;

sealed class PersistedConnectionIdentifier : IResult<Guid?>
{
	readonly PersistentComponentState _state;
	readonly CurrentRenderState       _session;
	readonly string                   _key;

	public PersistedConnectionIdentifier(PersistentComponentState state, CurrentRenderState session)
		: this(state, session, ConnectionSessionKey.Default) {}

	public PersistedConnectionIdentifier(PersistentComponentState state, CurrentRenderState session, string key)
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