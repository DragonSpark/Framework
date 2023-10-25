using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Commands;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class RenderStateMonitor : ICommand, ICommand<RenderState>
{
	readonly RenderStateStore       _store;
	readonly ContentIdentifierStore _content;

	public RenderStateMonitor(RenderStateStore store, ContentIdentifierStore content)
	{
		_store   = store;
		_content = content;
	}

	public void Execute(None parameter)
	{
		switch (_store.Get())
		{
			case RenderState.Ready:
				_store.Execute(RenderState.Established);
				break;
			case RenderState.Established:
				_content.Execute();
				break;
		}
	}

	public void Execute(RenderState parameter)
	{
		_store.Execute(parameter);
	}
}