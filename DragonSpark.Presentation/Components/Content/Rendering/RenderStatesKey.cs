using DragonSpark.Application.Compose.Store;
using DragonSpark.Compose;
using System;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class RenderStatesKey : Key<Guid>
{
	public static RenderStatesKey Default { get; } = new();

	RenderStatesKey() : base(A.Type<RenderStatesKey>(), x => x.ToString()) {}
}