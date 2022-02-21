using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using Microsoft.Extensions.Caching.Memory;

namespace DragonSpark.Presentation.Components.Content.Rendering;

public class RenderApply<T> : ICommand
{
	readonly IMemoryCache    _memory;
	readonly IResult<string> _key;
	readonly IMutable<T?>    _store;

	protected RenderApply(IMemoryCache memory, IResult<string> key, IMutable<T?> store)
	{
		_memory = memory;
		_key    = key;
		_store  = store;
	}

	public void Execute(None parameter)
	{
		var result = _store.Get();
		var key    = _key.Get();
		if (result is null && _memory.TryGetValue<T>(key, out var existing))
		{
			_store.Execute(existing);
			_memory.Remove(key);
		}
	}
}