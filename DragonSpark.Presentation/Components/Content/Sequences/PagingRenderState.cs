using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;

namespace DragonSpark.Presentation.Components.Content.Sequences;

public readonly record struct PagingRenderState<T>(IPages<T> Subject, bool Any, bool Loading, bool Ready)
{
	public PagingRenderState(IPages<T> Subject, bool Any, bool Loading) : this(Subject, Any, Loading, Any && !Loading) {}
}