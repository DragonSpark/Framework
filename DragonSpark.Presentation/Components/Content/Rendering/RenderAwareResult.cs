using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class RenderAwareResult<T> : IResulting<T?>
{
	readonly RenderStateStore _state;
	readonly RenderVariable<T>  _variable;
	readonly IResulting<T?>     _previous;

	public RenderAwareResult(RenderStateStore state, RenderVariable<T> variable, IResulting<T?> previous)
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
				var result = _variable.Pop(out var stored) ? stored : await _previous.Off();
				_variable.Execute(result);
				return result;
			}
			case RenderState.Ready:
			{
				return _variable.Pop(out var stored) ? stored : await _previous.Off();
			}
			default:
			{
				return await _previous.Off();
			}
		}
	}
}