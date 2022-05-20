using DragonSpark.Model;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Runtime.Execution;

public sealed class HasCounted : ICondition
{
	readonly ICounter _counter;

	public HasCounted(ICounter counter) => _counter = counter;

	public bool Get(None parameter) => _counter.Get() > 0;
}