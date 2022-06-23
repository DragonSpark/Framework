using DragonSpark.Model;
using DragonSpark.Model.Commands;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class RenderStateMonitor : ICommand, ICommand<RenderState>
{
	readonly CurrentRenderState _session;
	readonly AssignRenderState  _assign;

	public RenderStateMonitor(CurrentRenderState session, AssignRenderState assign)
	{
		_session     = session;
		_assign = assign;
	}

	public void Execute(None parameter)
	{
		var state = _session.Get();
		switch (state)
		{
			case RenderState.Ready:
				_assign.Execute(RenderState.Established);
				break;
		}	}

	public void Execute(RenderState parameter)
	{
		_assign.Execute(parameter);
	}
}