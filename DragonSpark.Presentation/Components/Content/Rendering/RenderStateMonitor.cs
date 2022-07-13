using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Commands;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class RenderStateMonitor : ICommand, ICommand<RenderState>
{
	readonly CurrentRenderState     _session;
	readonly ContentIdentifierStore _store;

	public RenderStateMonitor(CurrentRenderState session, ContentIdentifierStore store)
	{
		_session = session;
		_store   = store;
	}

	public void Execute(None parameter)
	{
		switch (_session.Get())
		{
			case RenderState.Ready:
				_session.Execute(RenderState.Established);
				break;
			case RenderState.Established:
				_store.Execute();
				break;
		}
	}

	public void Execute(RenderState parameter)
	{
		_session.Execute(parameter);
	}
}