using DragonSpark.Model.Operations.Allocated;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.State.Persistence;

sealed class Assign<T> : IAllocated<T>
{
	readonly PersistentComponentState _state;
	readonly string                   _key;

	public Assign(PersistentComponentState state, string key)
	{
		_state = state;
		_key   = key;
	}

	public Task Get(T parameter)
	{
		_state.PersistAsJson(_key, parameter);
		return Task.CompletedTask;
	}
}