using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Runtime.Execution;

public abstract class FirstBase : ICondition, ICommand
{
	readonly ICounter _counter;

	protected FirstBase(ICounter counter) => _counter = counter;

	public bool Get(None parameter) => _counter.Get() == 0 && _counter.Count() == 1;

	public void Execute(None parameter)
	{
		_counter.Execute(Clear.Default);
	}
}