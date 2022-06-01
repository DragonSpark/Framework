using DragonSpark.Application.Compose.Store;
using DragonSpark.Compose;
using System;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class ConnectionRenderKey : Key<Guid>
{
	public static ConnectionRenderKey Default { get; } = new();

	ConnectionRenderKey() : base(A.Type<ConnectionRenderKey>(), x => x.ToString()) {}
}