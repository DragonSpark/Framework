using DragonSpark.Model.Results;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class CurrentRenderState : IMutable<RenderState>
{
	readonly ConnectionRenderState _connection;
	readonly SessionRenderState    _session;

	public CurrentRenderState(ConnectionRenderState connection, SessionRenderState session)
	{
		_connection = connection;
		_session    = session;
	}

	public RenderState Get() => _session.Get() ?? _connection.Get() ?? RenderState.Default;

	public void Execute(RenderState parameter)
	{
		_session.Execute(parameter);
		switch (parameter)
		{
			case RenderState.Default:
				_connection.Execute(parameter);
				break;
			case RenderState.Ready:
			case RenderState.Established:
				_connection.Remove();
				break;
		}
	}
}