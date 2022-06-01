using DragonSpark.Model;
using DragonSpark.Model.Commands;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class RenderStateMonitor : ICommand
{
	readonly ConnectionRenderState _connection;
	readonly CurrentRenderState    _store;
	readonly SessionRenderState    _session;

	public RenderStateMonitor(ConnectionRenderState connection, CurrentRenderState store, SessionRenderState session)
	{
		_connection = connection;
		_store      = store;
		_session    = session;
	}

	public void Execute(None parameter)
	{
		if (_connection.Get().HasValue || _session.Get().HasValue)
		{
			switch (_store.Get())
			{
				case RenderState.Default:
					_store.Execute(RenderState.Ready);
					break;
				case RenderState.Ready:
					_store.Execute(RenderState.Established);
					break;
			}
		}
		else
		{
			_store.Execute(RenderState.Default);
		}
	}
}