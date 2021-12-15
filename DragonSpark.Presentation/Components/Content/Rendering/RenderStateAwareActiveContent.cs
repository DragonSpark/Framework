using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class RenderStateAwareActiveContent<T> : IActiveContent<T>
{
	readonly IMutable<RenderState>       _state;
	readonly ISelecting<RenderState, T?> _content;

	public RenderStateAwareActiveContent(IMutable<RenderState> state, ISelecting<RenderState, T?> content)
	{
		_state   = state;
		_content = content;
	}

	public async ValueTask<T?> Get()
	{
		var state = _state.Get();
		switch (state)
		{
			case RenderState.Default:
				_state.Execute(RenderState.Stored);
				break;
			case RenderState.Stored:
				_state.Execute(RenderState.Ready);
				break;
		}

		var result = await _content.Await(state);
		return result;
	}
}