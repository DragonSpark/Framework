using DragonSpark.Compose;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class RenderingAwareActiveContents<T> : IActiveContents<T>
{
	readonly IsPreRendering                    _condition;
	readonly RenderStateAwareActiveContents<T> _contents;
	readonly IActiveContents<T>                _previous;

	public RenderingAwareActiveContents(IsPreRendering condition, RenderStateAwareActiveContents<T> contents,
	                                    IActiveContents<T> previous)
	{
		_condition = condition;
		_contents  = contents;
		_previous  = previous;
	}

	public IActiveContent<T> Get(ActiveContentInput<T> parameter)
	{
		var source = _condition.Get() ? parameter with { Source = _contents.Get(parameter) } : parameter;
		var result = _previous.Get(source);
		return result;
	}
}