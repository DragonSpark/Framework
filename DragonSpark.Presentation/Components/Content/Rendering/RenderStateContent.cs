using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content.Rendering;

class RenderStateContent<TIn, TOut> : ISelecting<RenderStateInput<TIn>, TOut>
{
	readonly ISelecting<TIn, TOut>  _previous;
	readonly CurrentRenderState     _state;
	readonly RenderAssignment<TOut> _store;

	protected RenderStateContent(ISelecting<TIn, TOut> previous, CurrentRenderState state, RenderAssignment<TOut> store)
	{
		_previous   = previous;
		_state = state;
		_store      = store;
	}

	public async ValueTask<TOut> Get(RenderStateInput<TIn> parameter)
	{
		var (input, key) = parameter;
		var state = _state.Get();
		{
			switch (state)
			{
				case RenderState.Default:
				{
					var result = await _previous.Await(input);
					_store.Assign(key, result);
					return result;
				}
				case RenderState.Ready:
				{
					if (_store.Pop(key, out var result))
					{
						return result.Verify();
					}

					break;
				}
			}
		}

		{
			var result = await _previous.Await(input);
			return result;
		}
	}
}



sealed class RenderStateContent<T> : ISelecting<RenderState, T?>
{
	readonly IActiveContent<T> _previous;
	readonly RenderStore<T>    _store;

	public RenderStateContent(IActiveContent<T> previous, RenderStore<T> store)
	{
		_previous = previous;
		_store    = store;
	}

	public async ValueTask<T?> Get(RenderState parameter)
	{
		if (_previous.Monitor.Get())
		{
			_store.Remove();
		}
		else
		{
			switch (parameter)
			{
				case RenderState.Default:
				{
					var result = await _previous.Await();
					_store.Execute(result);
					return result;
				}
				case RenderState.Ready:
				{
					if (_store.Pop(out var result))
					{
						return result is not null ? _previous.Parameter(result) : result;
					}

					break;
				}
			}
		}

		{
			var result = await _previous.Await();
			return result;
		}
	}
}