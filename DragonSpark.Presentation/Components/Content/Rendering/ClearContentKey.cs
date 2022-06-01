using DragonSpark.Model.Commands;
using Microsoft.Extensions.Caching.Memory;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class ClearContentKey : ICommand<string>
{
	readonly IMemoryCache        _memory;

	public ClearContentKey(IMemoryCache memory) => _memory = memory;

	public void Execute(string parameter)
	{
		_memory.Remove(parameter);
	}
}