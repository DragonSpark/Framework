namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class RenderingAwareActiveContents<T> : IActiveContents<T>
{
	readonly IRenderState                      _state;
	readonly RenderStateAwareActiveContents<T> _contents;
	readonly IActiveContents<T>                _previous;

	public RenderingAwareActiveContents(IRenderState state, RenderStateAwareActiveContents<T> contents,
	                                    IActiveContents<T> previous)
	{
		_state = state;
		_contents  = contents;
		_previous  = previous;
	}

	public IActiveContent<T> Get(ActiveContentInput<T> parameter)
	{
		var source = _state.IsLoading() ? parameter with { Source = _contents.Get(parameter) } : parameter;
		var result = _previous.Get(source);
		return result;
	}
}