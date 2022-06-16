namespace DragonSpark.Presentation.Components.Content.Rendering;

/*sealed class RenderStateAwareActiveContent<T> : IActiveContent<T>
{
	readonly IActiveContent<T>           _previous;
	readonly IResult<RenderState>        _state;
	readonly ISelecting<RenderState, T?> _content;

	public RenderStateAwareActiveContent(IActiveContent<T> previous, IResult<RenderState> state,
	                                     ISelecting<RenderState, T?> content)
	{
		_previous = previous;
		_state    = state;
		_content  = content;
	}

	public async ValueTask<T?> Get()
	{
		var state  = _state.Get();
		var result = await _content.Await(state);
		return result;
	}

	public IUpdateMonitor Monitor => _previous.Monitor;

	public void Execute(T parameter)
	{
		_previous.Execute(parameter);
	}
}*/