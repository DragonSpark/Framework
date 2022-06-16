using DragonSpark.Model;
using DragonSpark.Model.Commands;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class RenderStateMonitor : ICommand, ICommand<RenderState>
{
	readonly SessionRenderState _session;

	public RenderStateMonitor(SessionRenderState session)
		=> _session = session;

	public void Execute(None parameter)
	{
		var state = _session.Get();
		switch (state)
		{
			case RenderState.Ready:
				_session.Execute(RenderState.Established);
				break;
		}	}

	public void Execute(RenderState parameter)
	{
		_session.Execute(parameter);
	}
}