namespace DragonSpark.Presentation.Components.Content.Rendering;

/*sealed class StoreAwareClearRenderState : IClearRenderState
{
	readonly IClearRenderState     _previous;
	readonly ConnectionRenderState _state;
	readonly SessionRenderState    _session;

	public StoreAwareClearRenderState(IClearRenderState previous, ConnectionRenderState state,
	                                  SessionRenderState session)
	{
		_previous = previous;
		_state    = state;
		_session  = session;
	}

	public void Execute(None parameter)
	{
		_previous.Execute(parameter);
		_state.Remove();
		_session.Execute(RenderState.Established);
	}
}*/