using DragonSpark.Model.Commands;
using Microsoft.Extensions.Caching.Memory;

namespace DragonSpark.Presentation.Components.Content.Rendering;

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