using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;
using DragonSpark.Compose;

namespace DragonSpark.Presentation.Components.Content.Rendering.Sequences;

sealed class RenderAwarePaging<T> : IPaging<T>
{
	readonly IsPreRendering                    _condition;
	readonly RenderStateAwarePagingContents<T> _contents;
	readonly IPaging<T>                        _previous;

	public RenderAwarePaging(IsPreRendering condition, RenderStateAwarePagingContents<T> contents, IPaging<T> previous)
	{
		_condition = condition;
		_contents  = contents;
		_previous  = previous;
	}

	public IPages<T> Get(PagingInput<T> parameter)
	{
		var (owner, _, _) = parameter;
		var previous = _previous.Get(parameter);
		var result   = _condition.Get() ? _contents.Get(new(owner, previous)) : previous;
		return result;
	}
}