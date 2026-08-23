using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;

namespace DragonSpark.Presentation.Components.Content.Rendering.Sequences;

sealed class RenderAwarePaging<T> : IPaging<T>
{
	readonly IRenderState                      _state;
	readonly RenderStateAwarePagingContents<T> _contents;
	readonly IPaging<T>                        _previous;

	public RenderAwarePaging(IRenderState state, RenderStateAwarePagingContents<T> contents, IPaging<T> previous)
	{
		_state = state;
		_contents  = contents;
		_previous  = previous;
	}

	public IPages<T> Get(PagingInput<T> parameter)
	{
		var (owner, _, _) = parameter;
		var previous = _previous.Get(parameter);
		var result   = _state.IsLoading() ? _contents.Get(new(owner, previous)) : previous;
		return result;
	}
}