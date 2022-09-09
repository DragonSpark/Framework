using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;

namespace DragonSpark.Runtime.Execution;

public sealed class First : FirstBase, ICommand
{
	readonly IMutable<int> _store;

	public First() : this(new Variable<int>()) {}

	public First(IMutable<int> store) : base(new Counter(store)) => _store = store;

	public void Execute(None parameter)
	{
		_store.Execute(0);
	}
}