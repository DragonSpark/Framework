using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class RenderAwareResult<T> : IResulting<T?>
{
	readonly CurrentRenderState _state;
	readonly RenderVariable<T>  _variable;
	readonly IResulting<T?>     _previous;

	public RenderAwareResult(CurrentRenderState state, RenderVariable<T> variable, IResulting<T?> previous)
	{
		_state    = state;
		_variable = variable;
		_previous = previous;
	}

	public async ValueTask<T?> Get()
	{
		switch (_state.Get())
		{
			case RenderState.Default:
			{
				var result = _variable.Pop(out var stored) ? stored : await _previous.Await();
				_variable.Execute(result);
				return result;
			}
			case RenderState.Ready:
			{
				var pop = _variable.Pop(out var stored);
				return pop ? stored : await _previous.Await();
			}
			default:
			{
				return await _previous.Await();
			}
		}
	}
}