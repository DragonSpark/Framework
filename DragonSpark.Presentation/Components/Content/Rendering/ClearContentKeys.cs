using DragonSpark.Model;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.Caching.Memory;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class ClearContentKeys : ICommand
{
	readonly RenderContentKeys _keys;
	readonly ClearContentKey   _clear;

	public ClearContentKeys(RenderContentKeys keys, ClearContentKey clear)
	{
		_keys  = keys;
		_clear = clear;
	}

	public void Execute(None parameter)
	{
		if (_keys.Count > 0)
		{
			foreach (var key in _keys)
			{
				_clear.Execute(key);
			}

			_keys.Clear();
		}
	}
}

// TODO

sealed class ClearComponentKey : ICommand<object>
{
	readonly ClearContentKey   _clear;
	readonly IRenderContentKey _key;

	public ClearComponentKey(ClearContentKey clear, IRenderContentKey key)
	{
		_clear    = clear;
		_key = key;
	}

	public void Execute(object parameter)
	{
		var key = _key.Get(parameter);
		_clear.Execute(key);
	}
}

sealed class ClearContentKey : ICommand<string>
{
	readonly IMemoryCache        _memory;
	readonly CurrentRenderStates _states;

	public ClearContentKey(IMemoryCache memory, CurrentRenderStates states)
	{
		_memory = memory;
		_states = states;
	}

	public void Execute(string parameter)
	{
		_memory.Remove(parameter);
		_states.Get().Remove(parameter);
	}
}