using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Commands;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class RenderStateMonitor : ICommand, ICommand<RenderState>
{
	readonly CurrentRenderState          _session;
	readonly IClearContentIdentification _clear;

	public RenderStateMonitor(CurrentRenderState session, IClearContentIdentification clear)
	{
		_session    = session;
		_clear = clear;
	}

	public void Execute(None parameter)
	{
		switch (_session.Get())
		{
			case RenderState.Ready:
				_session.Execute(RenderState.Established);
				break;
			case RenderState.Established:
				_clear.Execute();
				break;
		}
	}

	public void Execute(RenderState parameter)
	{
		_session.Execute(parameter);
	}
}