using DragonSpark.Model;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Runtime.Execution;

public class FirstBase : ICondition
{
	readonly ICounter _counter;

	public FirstBase(ICounter counter) => _counter = counter;

	public bool Get(None parameter) => _counter.Get() == 0 && _counter.Count() == 1;
}