using DragonSpark.Model;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class AdjustRenderState : IAdjustRenderState
{
	readonly ConnectionRenderState _state;

	public AdjustRenderState(ConnectionRenderState state) => _state = state;

	public void Execute(None parameter)
	{
		_state.Remove();
	}
}