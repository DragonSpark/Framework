using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Presentation.Connections;

namespace DragonSpark.Presentation.Environment;

sealed class ContextAwareInitializeConnection : IInitializeConnection
{
	readonly IInitializeConnection _previous;
	readonly ContextStore          _store;
	readonly ContextMemory         _memory;

	public ContextAwareInitializeConnection(IInitializeConnection previous, ContextStore store, ContextMemory memory)
	{
		_previous = previous;
		_store    = store;
		_memory   = memory;
	}

	public void Execute(None parameter)
	{
		_previous.Execute();

		var store = _store.Get();
		if (store is null && _memory.Pop(out var context))
		{
			_store.Execute(context);
		}
		else
		{
			_memory.Execute(store);
		}
	}
}