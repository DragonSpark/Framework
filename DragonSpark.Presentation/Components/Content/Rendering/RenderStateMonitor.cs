using DragonSpark.Model;
using DragonSpark.Model.Commands;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class RenderStateMonitor : ICommand, ICommand<RenderState>
{
	readonly CurrentRenderState          _session;

	public RenderStateMonitor(CurrentRenderState session)
	{
		_session    = session;
	}

	public void Execute(None parameter)
	{
		switch (_session.Get())
		{
			case RenderState.Ready:
				_session.Execute(RenderState.Established);
				break;
		}
	}

	public void Execute(RenderState parameter)
	{
		_session.Execute(parameter);
	}
}