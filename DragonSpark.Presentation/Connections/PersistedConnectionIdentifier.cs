using DragonSpark.Model.Results;
using DragonSpark.Presentation.Components.Content.Rendering;
using System;

namespace DragonSpark.Presentation.Connections;

sealed class PersistedConnectionIdentifier : IResult<Guid?>
{
	readonly RenderStateStore        _session;
	readonly ConnectionIdentifierStore _store;

	public PersistedConnectionIdentifier(RenderStateStore session, ConnectionIdentifierStore store)
	{
		_session = session;
		_store   = store;
	}

	public Guid? Get()
	{
		var store = _store.Get();
		if (store.Success)
		{
			_session.Execute(RenderState.Ready);
			return store.Value;
		}

		return null;
	}
}