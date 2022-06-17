using DragonSpark.Model.Operations.Allocated;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Connections;

sealed class Assignment<T> : IAllocated
{
	readonly T                        _instance;
	readonly PersistentComponentState _state;
	readonly string                   _key;

	public Assignment(T instance, PersistentComponentState state, string key)
	{
		_instance = instance;
		_state    = state;
		_key      = key;
	}

	public Task Get()
	{
		_state.PersistAsJson(_key, _instance);
		return Task.CompletedTask;
	}
}