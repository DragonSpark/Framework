using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content.Rendering;

class RenderStateContent<TIn, TOut> : ISelecting<RenderStateInput<TIn>, TOut>
{
	readonly ISelecting<TIn, TOut> _previous;
	readonly IMemoryCache          _memory;

	protected RenderStateContent(ISelecting<TIn, TOut> previous, IMemoryCache memory)
	{
		_previous = previous;
		_memory   = memory;
	}

	public async ValueTask<TOut> Get(RenderStateInput<TIn> parameter)
	{
		var (input, key, state) = parameter;
		switch (state)
		{
			case RenderState.Default:
			{
				var result = await _previous.Await(input);
				_memory.Set(key, result, PreRenderingWindow.Default);
				return result;
			}
			case RenderState.Stored:
			{
				if (_memory.TryGetValue(key, out var existing))
				{
					_memory.Remove(key);
					return existing.To<TOut>();
				}

				break;
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
	readonly IMemoryCache      _memory;
	readonly string            _key;

	public RenderStateContent(IActiveContent<T> previous, IMemoryCache memory, string key)
	{
		_previous = previous;
		_memory   = memory;
		_key      = key;
	}

	public async ValueTask<T?> Get(RenderState parameter)
	{
		switch (parameter)
		{
			case RenderState.Default:
			{
				var result = await _previous.Await();
				_memory.Set(_key, result, PreRenderingWindow.Default);
				return result;
			}
			case RenderState.Stored:
			{
				if (_memory.TryGetValue(_key, out var existing))
				{
					_memory.Remove(_key);
					return (T?)existing;
				}

				break;
			}
		}

		{
			var result = await _previous.Await();
			return result;
		}
	}
}