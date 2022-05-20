using DragonSpark.Model;
using DragonSpark.Model.Results;

namespace DragonSpark.Runtime.Execution;

public sealed class Counter : ICounter
{
	readonly IMutable<int> _store;

	public Counter() : this(new Variable<int>()) {}

	public Counter(IMutable<int> store) => _store = store;

	public int Get() => _store.Get();

	public void Execute(None parameter)
	{
		_store.Execute(_store.Get() + 1);
	}
}

