using DragonSpark.Model.Commands;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class ClearComponentKey : ICommand<object>
{
	readonly ClearContentKey   _clear;
	readonly IRenderContentKey _key;

	public ClearComponentKey(ClearContentKey clear, IRenderContentKey key)
	{
		_clear = clear;
		_key   = key;
	}

	public void Execute(object parameter)
	{
		var key = _key.Get(parameter);
		_clear.Execute(key);
	}
}