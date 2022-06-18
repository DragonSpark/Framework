using DragonSpark.Application.Entities.Queries.Runtime.Pagination;
using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content.Rendering.Sequences;

sealed class RenderAwareAny<T> : IAny<T>
{
	readonly IsPreRendering                 _condition;
	readonly RenderStateAwareAnyContents<T> _contents;
	readonly IAny<T>                        _previous;

	public RenderAwareAny(IsPreRendering condition, RenderStateAwareAnyContents<T> contents, IAny<T> previous)
	{
		_condition = condition;
		_contents  = contents;
		_previous  = previous;
	}

	public ValueTask<bool> Get(AnyInput<T> parameter)
	{
		var (owner, _) = parameter;
		var source = _condition.Get() ? _contents.Get(new(owner, _previous)) : _previous;
		var result = source.Get(parameter);
		return result;
	}
}