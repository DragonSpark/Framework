using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content.Rendering;

class RenderAwareSelection<TIn, TOut> : ISelecting<TIn, TOut>
{
	readonly RenderStateStore    _state;
	readonly RenderVariable<TOut>  _variable;
	readonly ISelecting<TIn, TOut> _previous;

	protected RenderAwareSelection(ISelecting<TIn, TOut> previous, RenderStateStore state,
	                               RenderVariable<TOut> variable)
	{
		_state    = state;
		_variable = variable;
		_previous = previous;
	}

	public async ValueTask<TOut> Get(TIn parameter)
	{
		switch (_state.Get())
		{
			case RenderState.Default:
			{
				var result = _variable.Pop(out var stored) ? stored.Verify() : await _previous.Await(parameter);
				_variable.Execute(result);
				return result;
			}
			default:
			{
				var pop = _variable.Pop(out var stored);
				return pop ? stored.Verify() : await _previous.Await(parameter);
			}
		}
	}
}