using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using Microsoft.Extensions.Caching.Memory;

namespace DragonSpark.Presentation.Components.Content.Rendering;

public class RenderInitialize<T> : ICommand<T>
{
	readonly IMemoryCache       _memory;
	readonly ISelect<T, string> _key;
	readonly IMutable<T?>       _store;

	protected RenderInitialize(IMemoryCache memory, ISelect<T, string> key, IMutable<T?> store)
	{
		_memory = memory;
		_key    = key;
		_store  = store;
	}

	public void Execute(T parameter)
	{
		_store.Execute(parameter);
		var key = _key.Get(parameter);
		_memory.Set(key, parameter, PreRenderingWindow.Default);
	}
}