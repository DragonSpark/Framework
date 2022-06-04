using DragonSpark.Application.Compose.Store;
using DragonSpark.Compose;
using System;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class ConnectionRenderKey : Key<Guid>
{
	public ConnectionRenderKey(ConnectionLocationKey key) : base(A.Type<ConnectionRenderKey>(), key.Get) {}
}