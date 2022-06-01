using DragonSpark.Model;
using DragonSpark.Model.Commands;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class ClearContentKeys : ICommand
{
	readonly RenderContentKeys   _keys;
	readonly ClearContentKey     _clear;

	public ClearContentKeys(RenderContentKeys keys, ClearContentKey clear)
	{
		_keys   = keys;
		_clear  = clear;
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