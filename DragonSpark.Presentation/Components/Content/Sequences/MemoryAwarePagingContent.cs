using DragonSpark.Application.Entities.Queries.Runtime.Shape;
using DragonSpark.Presentation.Components.Content.Rendering;

namespace DragonSpark.Presentation.Components.Content.Sequences;

sealed class MemoryAwarePagingContent<T> : RenderStateContent<QueryInput, Current<T>>
{
	public MemoryAwarePagingContent(IPaging<T> previous, CurrentRenderState state, RenderAssignment<Current<T>> store)
		: base(previous, state, store) {}
}