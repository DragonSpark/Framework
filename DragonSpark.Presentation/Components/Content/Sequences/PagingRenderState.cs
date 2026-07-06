using System;

namespace DragonSpark.Presentation.Components.Content.Sequences;

public readonly record struct PagingRenderState(Exception? Error, bool Any, bool Loading, bool Ready)
{
	public PagingRenderState(Exception? Error, bool Any, bool Loading)
		: this(Error, Any, Loading, (Error is not null || Any) && !Loading) {}
}