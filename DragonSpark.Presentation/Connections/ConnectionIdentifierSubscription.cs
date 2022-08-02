using DragonSpark.Compose;
using DragonSpark.Presentation.Components.State.Persistence;
using Microsoft.AspNetCore.Components;
using System;

namespace DragonSpark.Presentation.Connections;

sealed class ConnectionIdentifierStore : Persist<Guid>
{
	public ConnectionIdentifierStore(PersistentComponentState state)
		: base(state, A.Type<ConnectionIdentifierStore>()) {}
}